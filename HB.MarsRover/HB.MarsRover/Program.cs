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
            InputHelper inputHelper = new InputHelper();
            IMobilityService mobilityService = new MobilityService();

            char? selection = PrintInitializationOptionsAndGetUserInput();

            if (selection != null && selection.Value == 'f')
            {
                InitializeFromFile(inputHelper, mobilityService);
            }
            else
            {
                InteractiveInitialization(inputHelper, mobilityService);
            }

            System.Console.ReadLine();
        }

        private static char? PrintInitializationOptionsAndGetUserInput()
        {
            Console.WriteLine($"Initialization Options:\nF: Initialize from input.txt file\nAny other input: Interactive Initialization\nPlease select: ");
            string selection = Console.ReadLine();

            return string.IsNullOrEmpty(selection) ? (char?)null : selection.ToLower().Trim()[0];
        }

        #region Interactive Initialization
        private static void InteractiveInitialization(InputHelper inputHelper, IMobilityService mobilityService)
        {
            IEnvironment environment = null;
            IRobot robot = null;
            IEnumerable<Enums.MovementCommand> commands = null;
            int robotCount = 1;

            Console.WriteLine("INFO: Send 'X' to terminate the input sequence.");

            while (true)
            {
                bool terminate;
                if (environment == null)
                {
                    string environmentInfo = GetInfoFromUser("Environment Info (Ex. '5 5'): ", out terminate);

                    if (terminate) break;

                    try
                    {
                        environment = inputHelper.PopulateEnvironment(environmentInfo);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        environment = null;
                    }

                    if (environment == null)
                    {
                        System.Console.WriteLine(
                            "Something wrong with the environment information. Please fill it again.");
                    }
                }
                else if (robot == null)
                {
                    string robotInfo = GetInfoFromUser($"Robot {robotCount} Info (Ex. '1 2 N'): ", out terminate);

                    if (terminate) break;

                    try
                    {
                        robot = inputHelper.PopulateRobot(robotCount, robotInfo);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        robot = null;
                    }

                    if (robot == null)
                    {
                        System.Console.WriteLine(
                            "Something wrong with the robot information. Please fill it again.");
                    }
                    else
                    {
                        robotCount++;
                    }
                }
                else if (commands == null)
                {
                    string commandsInfo = GetInfoFromUser("Commands Info (Ex. 'LMLMLMLMM'): ", out terminate);

                    if (terminate) break;

                    try
                    {
                        commands = inputHelper.PopulateCommandList(commandsInfo);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        commands = null;
                    }

                    if (commands == null)
                    {
                        System.Console.WriteLine(
                            "Something wrong with the commands information. Please fill it again.");
                    }
                }
                else
                {
                    ExecuteCommands(commands, mobilityService, environment, robot);

                    // Print latest location of the robot
                    System.Console.WriteLine("Discovery has been completed.");
                    Console.WriteLine($"{robot.PrintShortRobotInfo()}");
                    commands = null;
                    robot = null;
                }
            }

            System.Console.WriteLine("Terminated by user intervention.");
        }

        private static string GetInfoFromUser(string message, out bool terminationRequested)
        {
            terminationRequested = false;
            System.Console.WriteLine(message);
            string input = Console.ReadLine();
            input = input?.ToUpper();

            if (input == "X")
            {
                terminationRequested = true;
            }

            return input;
        }

        #endregion

        #region Initialize From File
        private static void InitializeFromFile(InputHelper inputHelper, IMobilityService mobilityService)
        {
            IEnvironment environment = null;
            IRobot robot = null;
            IEnumerable<Enums.MovementCommand> commands = null;
            int robotCount = 0;
            int lineNumber = -1;
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
                    environment = inputHelper.PopulateEnvironment(line);
                }
                else
                {
                    if (lineNumber % 2 == 1)
                    {
                        robotCount++;
                        robot = HandleRobotLine(inputHelper, line, mobilityService, environment, robotCount);
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
                            commands = inputHelper.PopulateCommandList(line);
                        }
                        catch (Exception e)
                        {
                            // If something goes wrong while parsing the commands, then it means there will be no commands to execute by the robot.
                            // Continue to move forward with the next robot
                            // Even if one of the commands is not valid, terminate the execution to not end up at the wrong destination.
                            Console.WriteLine($"Some of the commands are not valid for {robot.PrintRobotInfo()} : {e.Message}");
                            continue;
                        }

                        ExecuteCommands(commands, mobilityService, environment, robot);

                        // Print latest location of the robot
                        Console.WriteLine($"{robot.PrintShortRobotInfo()}");
                    }
                }

            }
        }

        #endregion

        #region Common functions
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
        #endregion
    }












}
