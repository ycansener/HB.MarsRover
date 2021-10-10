using System;
using System.Collections.Generic;
using System.Text;
using HB.MarsRover.Domain;

namespace HB.MarsRover.Helpers
{
    public class FacingHelper
    {
        /// <summary>
        /// Converts facing character to the CompassFacing
        /// </summary>
        /// <param name="facing">Facing character that comes from the input line</param>
        /// <returns>CompassFacing equivalent of the facing character</returns>
        public static Enums.CompassFacing ConvertToCompassFacing(char facing)
        {
            switch (facing)
            {
                case 'N':
                    return Enums.CompassFacing.North;
                case 'E':
                    return Enums.CompassFacing.East;
                case 'S':
                    return Enums.CompassFacing.South;
                case 'W':
                    return Enums.CompassFacing.West;
                default:
                    throw new Exception("Compass facing is not valid!");
            }

        }
    }
}
