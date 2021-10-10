using System;
using System.Collections.Generic;
using System.Text;

namespace HB.MarsRover.Domain.Models
{
    // For robot, domain should only knows the contract of the Robot, since the specs of the robot are not set in stone
    public interface IRobot
    {
        /// <summary>
        /// Updates the coordinate partition of the Position
        /// </summary>
        /// <param name="newCoordinates">New coordinates that represents where the robot is</param>
        void UpdatePosition(Tuple<int,int> newCoordinates);

        /// <summary>
        /// Updates the facing partition of the Position
        /// </summary>
        /// <param name="facing">New facing of the robot</param>
        void UpdatePosition(Enums.CompassFacing facing);

        /// <summary>
        /// Gets the position of the robot
        /// </summary>
        /// <returns>Position of the robot that contains coordinates and facing</returns>
        Position GetPosition();

        /// <summary>
        /// Prints detailed robot information
        /// </summary>
        /// <returns>String that represents the detailed robot information</returns>
        string PrintRobotInfo();

        /// <summary>
        /// Prints short robot information
        /// </summary>
        /// <returns>String that represents the short version of the robot information</returns>
        string PrintShortRobotInfo();
    }
}
