using System;
using System.Collections.Generic;
using System.Text;
using HB.MarsRover.Domain;

namespace HB.MarsRover.Helpers
{

    public class CommandHelper
    {
        /// <summary>
        /// Converts command message to MovementCommand
        /// </summary>
        /// <param name="commandMessage">Command message character that comes from the input line</param>
        /// <returns>MovementCommand equivalent of the command character</returns>
        public static Enums.MovementCommand ParseCommandMessage(char commandMessage)
        {
            switch (commandMessage)
            {
                case 'L':
                    return Enums.MovementCommand.TurnLeft;
                case 'R':
                    return Enums.MovementCommand.TurnRight;
                case 'M':
                    return Enums.MovementCommand.MoveForward;
                default:
                    return Enums.MovementCommand.Invalid;
            }
        }
    }
}
