using System;
using System.Collections.Generic;
using HB.MarsRover.Domain;
using HB.MarsRover.Domain.Models;
using HB.MarsRover.Domain.Services;

namespace HB.MarsRover.Services
{
    public class MobilityService : IMobilityService
    {
        public void DeployRobotToEnvironment(IEnvironment environment, IRobot robot)
        {
            bool deploymentSuccessfull = IsCoordinatesValid(
                robot.GetPosition().Coordinates.Item1, 
                robot.GetPosition().Coordinates.Item2,
                environment.GetSizeX(), 
                environment.GetSizeY());

            if (!deploymentSuccessfull)
            {
                throw new Exception($"Deployment failed! Coordinates are invalid: {robot.PrintRobotInfo()}");
            }
        }

        public void ProcessMovementCommand(IEnvironment environment, IRobot robot, Enums.MovementCommand movementCommand)
        {
            if (movementCommand == Enums.MovementCommand.Invalid)
            {
                throw new Exception($"Movement command is not valid: {robot.PrintRobotInfo()}");
            }

            if (movementCommand == Enums.MovementCommand.MoveForward)
            {
                ProcessMoveForwardCommand(environment, robot);
            }
            else
            {
                ProcessRotationCommand(robot, movementCommand == Enums.MovementCommand.TurnRight ? true : false);
            }
        }

        private void ProcessRotationCommand(IRobot robot, bool rotateClockwise)
        {
            List<Enums.CompassFacing> compassClockwise = new List<Enums.CompassFacing>()
            {
                Enums.CompassFacing.North, Enums.CompassFacing.East, Enums.CompassFacing.South, Enums.CompassFacing.West
            };

            int indexIncrementValue = rotateClockwise ? 1 : -1;

            int newIndex = (compassClockwise.IndexOf(robot.GetPosition().Facing) + indexIncrementValue) % compassClockwise.Count;

            if (newIndex == -1)
            {
                newIndex = compassClockwise.Count - 1;
            }

            robot.UpdatePosition(compassClockwise[newIndex]);
        }

        private void ProcessMoveForwardCommand(IEnvironment environment, IRobot robot)
        {
            Tuple<int, int> movementInfo = PopulateMovementTuple(robot.GetPosition().Facing);
            bool validMovement = IsMoveForwardPossible(environment, robot.GetPosition(), movementInfo);

            if (!validMovement)
            {
                throw new Exception($"Invalid movement to {robot.GetPosition().Facing} from location: x={robot.GetPosition().Coordinates.Item1}-y={robot.GetPosition().Coordinates.Item2}");
            }

            Tuple<int, int> newLocation = CalculateNewLocation(robot.GetPosition(), movementInfo);
            robot.UpdatePosition(newLocation);
        }

        private Tuple<int, int> PopulateMovementTuple(Enums.CompassFacing facing)
        {
            Tuple<int, int> movementInfo = null;

            switch (facing)
            {
                case Enums.CompassFacing.North:
                    movementInfo = new Tuple<int, int>(0, 1);
                    break;
                case Enums.CompassFacing.East:
                    movementInfo = new Tuple<int, int>(1, 0);
                    break;
                case Enums.CompassFacing.South:
                    movementInfo = new Tuple<int, int>(0, -1);
                    break;
                case Enums.CompassFacing.West:
                    movementInfo = new Tuple<int, int>(-1, 0);
                    break;
            }

            return movementInfo;
        }

        private bool IsMoveForwardPossible(IEnvironment marsEnvironment, Position currentLocation, Tuple<int, int> movementInfo)
        {
            return IsCoordinatesValid(currentLocation.Coordinates.Item1 + movementInfo.Item1,
                currentLocation.Coordinates.Item2 + movementInfo.Item2, marsEnvironment.GetSizeX(),
                marsEnvironment.GetSizeY());
        }

        private bool IsCoordinatesValid(int currentX, int currentY, int sizeX, int sizeY)
        {
            if (currentX >= sizeX
                || currentX < 0
                || currentY >= sizeY
                || currentY < 0)
            {
                return false;
            }

            return true;
        }

        private Tuple<int, int> CalculateNewLocation(Position currentLocation, Tuple<int, int> movementInfo)
        {
            return new Tuple<int, int>(currentLocation.Coordinates.Item1 + movementInfo.Item1, currentLocation.Coordinates.Item2 + movementInfo.Item2);
        }
    }
}
