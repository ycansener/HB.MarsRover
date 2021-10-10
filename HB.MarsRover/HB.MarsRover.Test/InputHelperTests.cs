using AutoFixture;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HB.MarsRover.Domain;
using HB.MarsRover.Helpers;
using HB.MarsRover.Models;
using Xunit;

namespace HB.MarsRover.Test
{
    public class InputHelperTests
    {
        private Fixture _fixture;
        private Random _random;

        public InputHelperTests()
        {
            _fixture = new Fixture();
            _random = new Random();
        }

        #region Helpers
        private InputHelper CreateSut()
        {
            return new InputHelper();
        }

        private string GenerateRandomMovementCommandString(char[] movementCommands)
        {
            string commandString = string.Empty;
            int length = _random.Next(20);
            for (int i = 0; i < length; i++)
            {
                char currentCommand = movementCommands[_random.Next(1, movementCommands.Length)];
                commandString += currentCommand;
            }

            return commandString;
        }

        private char GenerateRandomFacing(char[] compassPoints)
        {
            return compassPoints[_random.Next(compassPoints.Length)];
        }
        #endregion

        #region Populate Environment
        [Fact]
        public void IfInputTextIsValid_CreateEnvironmentProperly()
        {
            var inputHelper = CreateSut();

            int sizeX = Math.Abs(_fixture.Create<int>());
            int sizeY = Math.Abs(_fixture.Create<int>());
            string environmentLine = $"{sizeX} {sizeY}";

            MarsEnvironment environment = inputHelper.PopulateEnvironment(environmentLine);

            Assert.NotNull(environment);
            Assert.Equal(environment.GetSizeX(), sizeX + 1);
            Assert.Equal(environment.GetSizeY(), sizeY + 1);
        }

        [Fact]
        public void IfInputTextIsInvalid_ThrowAnException()
        {
            var inputHelper = CreateSut();

            int sizeX = _fixture.Create<int>();
            int sizeY = _fixture.Create<int>();
            string environmentLine = $"{sizeX} X {sizeY}";

            Assert.Throws<Exception>(() => inputHelper.PopulateEnvironment(environmentLine));
        }
        #endregion

        #region Populate Robot
        [Fact]
        public void IfInputTextIsValid_CreateRobotProperly()
        {
            var inputHelper = CreateSut();

            int sizeX = _fixture.Create<int>();
            int sizeY = _fixture.Create<int>();
            char facing = GenerateRandomFacing(Constants.COMPASS_POINTS);
            string robotLine = $"{sizeX} {sizeY} {facing}";

            Robot robot = inputHelper.PopulateRobot(1, robotLine);

            Assert.NotNull(robot);
            Assert.Equal(robot.GetPosition()?.Coordinates?.Item1, sizeX);
            Assert.Equal(robot.GetPosition()?.Coordinates?.Item2, sizeY);
            Assert.Equal(robot.GetPosition().Facing, FacingHelper.ConvertToCompassFacing(facing));
        }

        [Fact]
        public void IfFacingIsNotValid_ThrowException()
        {
            var inputHelper = CreateSut();

            int sizeX = _fixture.Create<int>();
            int sizeY = _fixture.Create<int>();
            char facing = 'X';
            string robotLine = $"{sizeX} {sizeY} {facing}";

            Assert.Throws<Exception>(() => inputHelper.PopulateRobot(1, robotLine));
        }

        [Fact]
        public void IfCoordinatesIsNotValid_ThrowException()
        {
            var inputHelper = CreateSut();

            int sizeX = _fixture.Create<int>();
            int sizeY = _fixture.Create<int>();
            char facing = GenerateRandomFacing(Constants.COMPASS_POINTS);
            string robotLine = $"{sizeX} X {facing}";

            Assert.Throws<Exception>(() => inputHelper.PopulateRobot(1, robotLine));
        }
        #endregion

        #region Populate Movement Commands
        [Fact]
        public void IfInputTextIsValid_CreateCommandListProperly()
        {
            var inputHelper = CreateSut();
            string movementCommandLine = GenerateRandomMovementCommandString(Constants.MOVEMENT_COMMANDS);

            IEnumerable<Enums.MovementCommand> commands = inputHelper.PopulateCommandList(movementCommandLine);

            Assert.NotNull(commands);
            for(int i = 0; i < movementCommandLine.Length; i++)
            {
                char currentChar = movementCommandLine[i];
                Enums.MovementCommand convertedCommand = CommandHelper.ParseCommandMessage(currentChar);
                Enums.MovementCommand currentCommand = commands.ElementAt(i);

                Assert.Equal(convertedCommand, currentCommand);
            }
        }

        [Fact]
        public void IfInputTextIsEmpty_ThrowException()
        {
            var inputHelper = CreateSut();
            string movementCommandLine = string.Empty;

            Assert.Throws<Exception>(() => inputHelper.PopulateRobot(1, movementCommandLine));
        }

        [Fact]
        public void IfInputTextIsNotValid_ThrowException()
        {
            var inputHelper = CreateSut();
            string movementCommandLine = "X" + GenerateRandomMovementCommandString(Constants.MOVEMENT_COMMANDS);

            Assert.Throws<Exception>(() => inputHelper.PopulateCommandList(movementCommandLine));
        }
        #endregion
    }
}
