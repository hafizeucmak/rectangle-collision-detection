using NfiCodeAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NfiCodeAssignment.Services
{
    public class RobotScheduleValidationService
    {
        /// <summary>
        /// Validates whether there are no collisions between any of the robots in the schedule.
        /// </summary>
        /// <param name="schedule">The schedule, as read from a file. See the "Readme.pdf" file in the root folder of
        /// this project for more information about the structure/contents of this string. </param>
        /// <returns>Whether the schedule is valid. The schedule is valid if there are no collisions between any of the
        /// robots.</returns>
        public bool ValidateSchedule(string schedule)
        {
            var lines = ParseScheduleInputLines(schedule);

            var robots = ConvertScheduleInputLines(lines);

            foreach (var movement in robots.Movements)
            {
                // NOTE: I prioritize code readability over performance optimization by selecting all `RobotInformations` directly according to selecting all `RectangleCoordinates`,
                // ignoring the performance concern associated with retrieving specific elements at the selected movement time (line numbers : 139,140),
                // as the data is already in memory.

                var robotInfosInEachMovementTime = robots.RobotInformations.Where(x => x.RectangleCoordinates.Any(y => y.Movement.Equals(movement)));

                if (CheckAnyRectanglesCollideInEachMovementTime(robotInfosInEachMovementTime, movement))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Extracts the lines from the schedule string.
        /// Removes all comments and empty lines.
        /// </summary>
        private List<string> ParseScheduleInputLines(string schedule)
        {
            return schedule
                .Replace("\r\n", "\n")
                .Split('\n')
                .Where(x => !string.IsNullOrWhiteSpace(x) && !x.StartsWith("#"))
                .ToList();
        }

        private RobotsInputDTO ConvertScheduleInputLines(List<string> lines)
        {
            // NOTE: I have assumed that the input format is always correct, so I have not implemented input validation methods.
            var robots = new RobotsInputDTO();

            foreach (var line in lines)
            {
                var partialList = line.Split(',');

                if (partialList.Count() == 1)
                {
                    robots.RobotsCount = Convert.ToInt32(lines[0].Trim());
                }
                else if (partialList.Count() == 3) // set initial position
                {
                    var robotName = partialList[0].Trim();
                    var xPoint = Convert.ToInt32(partialList[1].Trim());
                    var yPoint = Convert.ToInt32(partialList[2].Trim());

                    if (!robots.RobotInformations.Any(x => x.RobotName.Equals(robotName)))
                    {
                        var robotInformation = new RobotInformationDTO
                        {
                            RobotName = robotName,
                            InitialPosition = new Position(xPoint, yPoint)
                        };

                        robots.RobotInformations.Add(robotInformation);
                    }
                }
                else if (partialList.Count() == 4) // set movement and rectangle positions for the given movement time
                {
                    var movement = Convert.ToInt32(partialList[0].Trim());

                    if (!robots.Movements.Any(x => x.Equals(movement)))
                    {
                        robots.Movements.Add(movement);
                    }

                    var robotName = partialList[1].Trim();
                    var nextXPoint = Convert.ToInt32(partialList[2].Trim());
                    var nextYPoint = Convert.ToInt32(partialList[3].Trim());

                    var robotInformation = robots.RobotInformations.FirstOrDefault(x => x.RobotName.Equals(robotName));
                    robotInformation.CalculateRectangleCoordinates(nextXPoint, nextYPoint, movement);
                }
                else
                {
                    throw new FormatException("The format is not correct. Please check and provide valid data.");
                }
            }

            return robots;
        }

        /// <summary>
        /// Checks if any pair of robots in the given collection collide at the specified movement time.
        /// </summary>
        /// <param name="robotInformations">Collection of RobotInformationDTO objects.</param>
        /// <param name="movementTime">The specified movement time for collision check.</param>
        /// <returns>True if any pair of robots collide, otherwise false.</returns>
        private bool CheckAnyRectanglesCollideInEachMovementTime(IEnumerable<RobotInformationDTO> robotInformations, int movementTime)
        {
            for (int i = 0; i < robotInformations.Count(); i++)
            {
                for (int j = i + 1; j < robotInformations.Count(); j++)
                {
                    var isCollide = IsRectanglesCollide(robotInformations.ElementAt(i), robotInformations.ElementAt(j), movementTime);

                    Console.WriteLine($"{robotInformations.ElementAt(i).RobotName} vs {robotInformations.ElementAt(j).RobotName} at movement time is  => {movementTime} and isCollide = {isCollide}");

                    if (isCollide)
                        return true; // Optimize performance by returning immediately upon finding the first true condition.
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if two robots' rectangles collide at the specified movement time.
        /// </summary>
        /// <param name="robot1">First RobotInformationDTO object.</param>
        /// <param name="robot2">Second RobotInformationDTO object.</param>
        /// <param name="movementTime">The specified movement time for collision check.</param>
        /// <returns>True if rectangles collide, otherwise false.</returns>
        private bool IsRectanglesCollide(RobotInformationDTO robot1, RobotInformationDTO robot2, int movementTime)
        {
            var robot1Coordinates = robot1.RectangleCoordinates.FirstOrDefault(x => x.Movement.Equals(movementTime));
            var robot2Coordinates = robot2.RectangleCoordinates.FirstOrDefault(x => x.Movement.Equals(movementTime));

            if (robot2Coordinates.XPointLeft <= robot1Coordinates.XPointRight
                && robot1Coordinates.XPointLeft <= robot2Coordinates.XPointRight
                && robot2Coordinates.YPointBottom <= robot1Coordinates.YPointTop
                && robot1Coordinates.YPointBottom <= robot2Coordinates.YPointTop)
            {
                return true;
            }

            return false;
        }
    }
}