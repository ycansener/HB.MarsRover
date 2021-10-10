using System;
using System.Collections.Generic;
using System.Text;

namespace HB.MarsRover.Domain
{
    // Enums are domain specific. They should not change by the implementation approach. So correct place to put them is in Domain.
    public class Enums
    {
        public enum MovementCommand
        {
            TurnLeft,
            TurnRight,
            MoveForward,
            Invalid
        }

        public enum CompassFacing
        {
            North,
            East,
            South,
            West
        }
    }
}
