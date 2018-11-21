using System;
using System.Collections.Generic;
using System.Linq;
using StratumFive.Core;

namespace StratumFive
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read instructions
            const string inputFileLocation = @"input.txt";
            var inputs = getInput(inputFileLocation);            
            
            var grid = inputs[0].Grid;
            var navigationService = new NavigationService();

            bool lost;

            // Loop over ships
            foreach (Input shipInput in inputs)
            {
                var ship = new Ship { CurrentPosition = shipInput.Position, Heading = shipInput.Heading };
                lost = false;

                foreach (Instruction instruction in shipInput.Instructions)
                {
                    if (lost)
                    {
                        break;
                    }

                    // Parse instruction
                    // if heading change, change heading
                    if (instruction == Instruction.L || instruction == Instruction.R)
                    {
                        ship.Heading = navigationService.ChangeHeading(instruction, ship.Heading);
                    }
                    // if forward, check lost ship warnings
                    else if (instruction == Instruction.F)
                    {
                        if (navigationService.WillShipBeLost(ship.CurrentPosition, ship.Heading))
                        {
                            // Skip instruction
                        }
                        else
                        {
                            var newPosition = navigationService.CalculcateNewPosition(ship.CurrentPosition, ship.Heading);

                            // Check if vessel is lost
                            if (!navigationService.IsPositionWithinGrid(newPosition, grid))
                            {
                                // if lost, record last position
                                navigationService.LostShipWarnings.Add(new Tuple<Position, Heading>(ship.CurrentPosition, ship.Heading));
                                lost = true;
                            }
                            else
                            {
                                // if not lost, set new current position
                                ship.CurrentPosition = newPosition;
                            }
                        }
                    }
                }
                if (!lost)
                {
                    Console.WriteLine("{0} {1}", ship.CurrentPosition, ship.Heading);
                }
                else
                {
                    Console.WriteLine("{0} {1} LOST", ship.CurrentPosition, ship.Heading);
                }
            }                        
        }

        // read the input file and parse it
        private static List<Input> getInput(string inputFileLocation)
        {
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            var inputFileLines = System.IO.File.ReadAllLines(currentDirectory + "//" + inputFileLocation);

            // initialise grid
            // First line of file should define the grid
            var gridSize = inputFileLines[0].Split(' ');

            if (!int.TryParse(gridSize[0], out var gridWidth))
            {
                throw new InvalidOperationException("Expected int for grid width");
            }

            if (!int.TryParse(gridSize[1], out var gridHeight))
            {
                throw new InvalidOperationException("Expected int for grid height");
            }

            var grid = new Grid { X = gridWidth, Y = gridHeight };

            // Initialise ships/instructions

            //var instructionLines = new string[][] { };
            var instructionLines = new List<string[]>();
            var instructionCounter = 0;
            for (var counter = 1; counter < inputFileLines.Length; counter++)
            {
                if (string.IsNullOrEmpty(inputFileLines[counter]))
                {
                    continue;
                }

                if ("FLR".Contains(inputFileLines[counter][0]))
                {
                    instructionLines[instructionCounter][1] = inputFileLines[counter];

                    instructionCounter++;
                }
                else
                {
                    instructionLines.Add(new string[2] { inputFileLines[counter], "" });
                }
            }

            var inputs = new List<Input>();

            foreach (string[] inputLine in instructionLines)
            {
                var heading = inputLine[0][4];
                var xpos = char.GetNumericValue(inputLine[0][0]);
                var ypos = char.GetNumericValue(inputLine[0][2]);
                var instructions = inputLine[1].ToCharArray().Select(i => (Instruction)Enum.Parse(typeof(Instruction), i.ToString())).ToList();

                var headingEnum = (Heading)Enum.Parse(typeof(Heading), heading.ToString());

                inputs.Add(new Input { Grid = grid, Heading = headingEnum, Instructions = instructions, Position = new Position((int)xpos, (int)ypos) });
            }

            return inputs;
        }
    }
}
