using System;
using System.Collections.Generic;
using System.Text;
using HB.MarsRover.Domain.Models;

namespace HB.MarsRover.Domain.Services
{
    // Mobility service is one of the key members of the implementation layer. Since the approach to implement it can be change by the requirements, domain should only knows the contract of it
    public interface IMobilityService
    {
        /// <summary>
        /// Simulation of the deployment of the robot to the specified location to check if the location is valid or not
        /// </summary>
        /// <param name="environment">The environment that the robot will be deployed</param>
        /// <param name="robot">The robot that will be deployed to the environment, that also contains the initial location of the robot aka deployment location</param>
        /// <exception cref="Exception">Throws an exception if the deployment location is not valid</exception>
        void DeployRobotToEnvironment(IEnvironment environment, IRobot robot);

        /// <summary>
        /// Process the movement command in the environment for the specified robot
        /// </summary>
        /// <param name="environment">The environment that robot will move on</param>
        /// <param name="robot">The robot that will be moved</param>
        /// <param name="movementCommand">Type of command that will either rotate the robot or move it forward</param>
        void ProcessMovementCommand(IEnvironment environment, IRobot robot, Enums.MovementCommand movementCommand);
    }
}
