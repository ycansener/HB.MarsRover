using System;
using HB.MarsRover.Domain;
using HB.MarsRover.Domain.Models;

namespace HB.MarsRover.Models
{
    public class Robot : IRobot
    {
        private readonly int _id;
        private readonly Position _position;

        public Robot(int id, int locationX, int locationY, Enums.CompassFacing facing)
        {
            _id = id;
            _position = new Position(facing, new Tuple<int, int>(locationX, locationY));
        }

        public void UpdatePosition(Tuple<int,int> newCoordinates)
        {
            _position.Coordinates = newCoordinates;
        }

        public void UpdatePosition(Enums.CompassFacing facing)
        {
            _position.Facing = facing;
        }

        public Position GetPosition()
        {
            return this._position;
        }

        public string PrintRobotInfo()
        {
            return $"Robot {_id} <Location: x={_position.Coordinates.Item1}-y={_position.Coordinates.Item2}, Facing: {_position.Facing}>";
        }

        public string PrintShortRobotInfo()
        {
            return $"{_position.Coordinates.Item1} {_position.Coordinates.Item2} {_position.Facing.ToString().ToUpper()[0]}";
        }
    }
}
