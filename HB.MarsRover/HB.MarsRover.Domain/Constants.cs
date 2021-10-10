using System;
using System.Collections.Generic;
using System.Text;

namespace HB.MarsRover.Domain
{
    // Constants are domain specific. They should not change by the implementation approach. So correct place to put them is in Domain.
    public class Constants
    {
        public static readonly Enums.CompassFacing[] FACES = { Enums.CompassFacing.North, Enums.CompassFacing.East, Enums.CompassFacing.South, Enums.CompassFacing.West };

        public static readonly char[] MOVEMENT_COMMANDS = new[] { 'L', 'R', 'M' };
        public static readonly char[] COMPASS_POINTS = new[] { 'N', 'S', 'W', 'E' };
    }
}
