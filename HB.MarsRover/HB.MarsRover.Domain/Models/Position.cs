using System;
using System.Collections.Generic;
using System.Text;
using HB.MarsRover.Domain;

namespace HB.MarsRover.Domain.Models
{
    // Since position is a generic class, implementation of it can be a member of the domain
    public class Position
    {
        public Tuple<int, int> Coordinates { get; set; }
        public Enums.CompassFacing Facing { get; set; }
        public Position(Enums.CompassFacing facing, Tuple<int, int> coordinates)
        {
            Facing = facing;
            Coordinates = coordinates;
        }
    }
}
