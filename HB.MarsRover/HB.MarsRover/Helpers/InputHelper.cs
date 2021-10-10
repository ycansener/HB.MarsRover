using System;
using System.Collections.Generic;
using System.Text;
using HB.MarsRover.Domain;
using HB.MarsRover.Models;

namespace HB.MarsRover.Helpers
{

    public class InputHelper
    {
        /// <summary>
        /// Populates the environment from the line of string that contains the environment information
        /// </summary>
        /// <param name="environmentLine">line of string that contains the environment information</param>
        /// <returns>Environment object initialized via information in the environmentLine</returns>
        public MarsEnvironment PopulateEnvironment(string environmentLine)
        {
            string[] dimensions = environmentLine.Split(" ");
            int sizeX, sizeY;

            if (dimensions == null || dimensions.Length < 2 || !Int32.TryParse(dimensions[0], out sizeX) || !Int32.TryParse(dimensions[1], out sizeY))
            {
                throw new Exception("The dimensions for the environment are not valid!");
            }

            return new MarsEnvironment(sizeX + 1, sizeY + 1);
        }

        /// <summary>
        /// Populates the robot from the line of string that contains the robot information
        /// </summary>
        /// <param name="id">robot id</param>
        /// <param name="robotLine">line of string that contains the robot information</param>
        /// <returns>Robot object initialized via information in the robotLine and the id</returns>
        public Robot PopulateRobot(int id, string robotLine)
        {
            string[] information = robotLine.Split(" ");
            int locationX, locationY;
            char facing;

            if (information == null || information.Length < 3 || !Int32.TryParse(information[0], out locationX) || !Int32.TryParse(information[1], out locationY) || !char.TryParse(information[2], out facing))
            {
                throw new Exception("The dimensions for the environment are not valid!");
            }

            Enums.CompassFacing compassFacing = FacingHelper.ConvertToCompassFacing(facing);
            return new Robot(id, locationX, locationY, compassFacing);
        }

        /// <summary>
        /// Populates the commands from the line of string that contains the information of the commands
        /// </summary>
        /// <param name="movementCommandsLine">line of string that contains the information of the commands</param>
        /// <returns>MovementCommand list that contains the command objects that initialized via information in the movementCommandsLine</returns>
        public IEnumerable<Enums.MovementCommand> PopulateCommandList(string movementCommandsLine)
        {
            List<Enums.MovementCommand> commands = new List<Enums.MovementCommand>();

            if (string.IsNullOrEmpty(movementCommandsLine))
            {
                throw new Exception($"Movement command line cannot be empty!");
            }

            foreach (char commandChar in movementCommandsLine)
            {
                Enums.MovementCommand command = CommandHelper.ParseCommandMessage(commandChar);

                if (command == Enums.MovementCommand.Invalid)
                {
                    throw new Exception($"Movement command is not valid: {commandChar}");
                }

                commands.Add(command);
            }

            return commands;
        }
    }
}
