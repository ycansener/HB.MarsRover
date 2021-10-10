using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HB.MarsRover.Domain;
using HB.MarsRover.Domain.Models;
using HB.MarsRover.Domain.Services;
using HB.MarsRover.Helpers;
using HB.MarsRover.Models;
using HB.MarsRover.Services;

namespace HB.MarsRover
{
    class Program
    {
        /// <summary>
        /// All dependencies under the Main function can be also injected since for most of the dependencies contracts have been used.
        /// The ones that does not have a contract are Helper methods which are not belongs to the Domain since they are implementation-specific helpers.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            int lineNumber = -1;
            int robotCount = 0;
            IEnvironment environment = null;
            IRobot robot = null;
            IEnumerable<Enums.MovementCommand> commands = null;

            InputHelper _inputHelper = new InputHelper();
            IMobilityService _mobilityService = new MobilityService();

            string inputFileName = "input.txt";
            string filePath = Environment.CurrentDirectory + @"\" + inputFileName;


            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Input file not found!", inputFileName);
            }

            foreach (string line in System.IO.File.ReadLines(filePath))
            {
                lineNumber++;
                if (lineNumber == 0)
                {
                    // No need to handle errors while populating the environment since if something is wrong with the environment, we should terminate the execution anyways.
                    environment = _inputHelper.PopulateEnvironment(line);
                }
                else
                {
                    if (lineNumber % 2 == 1)
                    {
                        robotCount++;
                        robot = HandleRobotLine(_inputHelper, line, _mobilityService, environment, robotCount);
                    }
                    else
                    {
                        // If robot is null, it means we couldn't populate the robot with the information from the previous line.
                        // We can't execute the commands even if we can parse them.
                        if (robot == null)
                        {
                            continue; // Continue to read next line which should be a robot definition line
                        }

                        // If robot is not null, then it means we have a robot to execute the commands on
                        // Green light to populate commands from the line
                        try
                        {
                            commands = _inputHelper.PopulateCommandList(line);
                        }
                        catch (Exception e)
                        {
                            // If something goes wrong while parsing the commands, then it means there will be no commands to execute by the robot.
                            // Continue to move forward with the next robot
                            // Even if one of the commands is not valid, terminate the execution to not end up at the wrong destination.
                            Console.WriteLine($"Some of the commands are not valid for {robot.PrintRobotInfo()} : {e.Message}");
                            continue;
                        }

                        ExecuteCommands(commands, _mobilityService, environment, robot);

                        // Print latest location of the robot
                        Console.WriteLine($"{robot.PrintShortRobotInfo()}");
                    }
                }

            }

            System.Console.WriteLine("Discovery has been completed.");
            System.Console.ReadLine();
        }

        private static void ExecuteCommands(IEnumerable<Enums.MovementCommand> commands, IMobilityService _mobilityService, IEnvironment environment,
            IRobot robot)
        {
            foreach (var command in commands)
            {
                try
                {
                    _mobilityService.ProcessMovementCommand(environment, robot, command);
                }
                catch (Exception e)
                {
                    // If there is an exception has been thrown, that means the movement command is not valid - like an attempt to move outside of the environment
                    // In that case, don't leave the robot, but instead skip this command and continue with the next one (if exists).
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static IRobot HandleRobotLine(InputHelper _inputHelper, string line, IMobilityService _mobilityService,
            IEnvironment environment, int robotCount)
        {
            IRobot robot;
            // If something goes wrong while populating the robot, handle the error and give other robot(s) a chance.
            try
            {
                robot = _inputHelper.PopulateRobot(robotCount, line);
                _mobilityService.DeployRobotToEnvironment(environment, robot);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                robot = null;
            }

            return robot;
        }
    }












}
