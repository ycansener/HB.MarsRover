using AutoFixture;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using HB.MarsRover.Domain;
using HB.MarsRover.Helpers;
using HB.MarsRover.Models;
using HB.MarsRover.Services;
using Xunit;

namespace HB.MarsRover.Test
{
    public class MobilityServiceTests
    {
        private Fixture _fixture;
        private Random _random;
        private MarsEnvironment _environment;
        private Robot _robot;

        public MobilityServiceTests()
        {
            _fixture = new Fixture();
            _random = new Random();

            int sizeX = _fixture.Create<int>();
            int sizeY = _fixture.Create<int>();

            _environment = new MarsEnvironment(sizeX, sizeY);

            int locationX = _random.Next(0, sizeX);
            int locationY = _random.Next(0, sizeY);


            _robot = new Robot(1, locationX, locationY, GenerateRandomFacing());
        }


        #region Helpers
        private MobilityService CreateSut()
        {
            return new MobilityService();
        }

        private Enums.CompassFacing GenerateRandomFacing()
        {
            return Constants.FACES[_random.Next(Constants.FACES.Length)];
        }
        #endregion

        #region Movement Control

        [Theory]
        [InlineData(6, 6, 3, 4, Enums.CompassFacing.East, Enums.MovementCommand.MoveForward, true)]
        [InlineData(6, 6, 3, 5, Enums.CompassFacing.North, Enums.MovementCommand.MoveForward, false)]
        [InlineData(6, 6, 3, 0, Enums.CompassFacing.South, Enums.MovementCommand.MoveForward, false)]
        [InlineData(6, 6, 5, 4, Enums.CompassFacing.East, Enums.MovementCommand.MoveForward, false)]
        [InlineData(6, 6, 0, 3, Enums.CompassFacing.West, Enums.MovementCommand.MoveForward, false)]
        public void IfMovementValid_ExecuteMovementCommand(int environmentSizeX, int environmentSizeY, int robotPositionX, int robotPositionY, Enums.CompassFacing robotFacing, Enums.MovementCommand command, bool executeMovement)
        {
            _environment = new MarsEnvironment(environmentSizeX, environmentSizeY);
            _robot.UpdatePosition(new Tuple<int, int>(robotPositionX,robotPositionY));
            _robot.UpdatePosition(robotFacing);
            bool result = true;
            try
            {
                var sut = CreateSut();
                sut.ProcessMovementCommand(_environment, _robot, command);
            }
            catch (Exception e)
            {
                result = false;
            }

            Assert.Equal(executeMovement, result);
        }

        #endregion
    }
}
