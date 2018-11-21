using System;
using System.Collections.Generic;

using StratumFive.Core;

namespace StratumFive
{
    public class NavigationService
    {
        public NavigationService()
        {
            LostShipWarnings = new List<Tuple<Position, Heading>>();
        }

        // List of last known position/heading of lost ships
        public List<Tuple<Position, Heading>> LostShipWarnings { get; set; }

        // Determine whether a position coordinate falls within a grid
        public bool IsPositionWithinGrid(Position newPosition, Grid grid)
        {
            if (newPosition.X > grid.X || newPosition.Y > grid.Y)
            {
                return false;
            }
            if (newPosition.X < 0 || newPosition.Y < 0)
            {
                return false;
            }

            return true;
        }

        // Calculates new position of ship based on current position and heading
        public Position CalculcateNewPosition(Position currentPosition, Heading heading)
        {
            if (heading == Heading.N)
            {
                return new Position(currentPosition.X, currentPosition.Y + 1);
            }
            else if (heading == Heading.E)
            {
                return new Position(currentPosition.X + 1, currentPosition.Y);
            }
            else if (heading == Heading.S)
            {
                return new Position(currentPosition.X, currentPosition.Y -1);
            }
            else if (heading == Heading.W)
            {
                return new Position(currentPosition.X - 1, currentPosition.Y);
            }
            else
            {
                throw new InvalidOperationException("Not a valid heading");
            }
        }

        // Return new heading based on the current heading & instruction
        public Heading ChangeHeading(Instruction instruction, Heading currentHeading)
        {
            if (instruction == Instruction.L)
            {
                if (currentHeading == Heading.N)
                {
                    return Heading.W;
                }
                else if (currentHeading == Heading.E)
                {
                    return Heading.N;
                }
                else if (currentHeading == Heading.S)
                {
                    return Heading.E;
                }
                else if (currentHeading == Heading.W)
                {
                    return Heading.S;
                }

                throw new InvalidOperationException("Invalid heading");
            }
            else if (instruction == Instruction.R)
            {
                if (currentHeading == Heading.N)
                {
                    return Heading.E;
                }
                else if (currentHeading == Heading.E)
                {
                    return Heading.S;
                }
                else if (currentHeading == Heading.S)
                {
                    return Heading.W;
                }
                else if (currentHeading == Heading.W)
                {
                    return Heading.N;
                }

                throw new InvalidOperationException("Invalid heading");
            }

            throw new InvalidOperationException("Invalid instruction");
        }

        public bool WillShipBeLost(Position position, Heading heading)
        {
            return LostShipWarnings.Contains(new Tuple<Position, Heading>(position, heading));
        }
    }
}
