using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

namespace TankWars
{
    struct Position
    {
        public int X;
        public int Y;
        public int CurrentDirectionBot;
        public Position(int X, int Y, int CurrentDirectionBot)
        {
            this.X = X;
            this.Y = Y;
            this.CurrentDirectionBot = CurrentDirectionBot;
        }
    }
    class Program
    {
        static Position MovePlayer(Position player, Position[] directions)
        {
            int direction = 0;

                ConsoleKeyInfo userInput = Console.ReadKey();
                if (userInput.Key == ConsoleKey.RightArrow)
                {
                    direction = 0;

                }
                else if (userInput.Key == ConsoleKey.LeftArrow)
                {
                    direction = 1;

                }
                else if (userInput.Key == ConsoleKey.UpArrow)
                {
                    direction = 2;
                }
                else
                {
                    direction = 3;
                }
                player = new Position(player.X + directions[direction].X, player.Y - directions[direction].Y, 0);

                return player;
        }

        public static List<Position> BotCrashTests(List<Position> bots, int matrixWidth, int matrixHeight, int botsCount = 4)
        {
              int crashedBotCoordinateX = 0;
                int crashedBotCoordinateY = 0;
                int crashedBotCurrentDirection = 0;
                for (int bot = 0; bot < botsCount; bot++)
                {
                    if (bots[bot].X <= 3

                        )
                    {
                        crashedBotCoordinateX = bots[bot].X;
                        crashedBotCoordinateY = bots[bot].Y;
                        crashedBotCurrentDirection = 0;
                        bots.RemoveAt(bot);
                        bots.Add(new Position(crashedBotCoordinateX, crashedBotCoordinateY, crashedBotCurrentDirection));

                    }
                    if (bots[bot].X > matrixWidth - 3)
                    {
                        crashedBotCoordinateX = bots[bot].X;
                        crashedBotCoordinateY = bots[bot].Y;
                        crashedBotCurrentDirection = 1;
                        bots.RemoveAt(bot);
                        bots.Add(new Position(crashedBotCoordinateX, crashedBotCoordinateY, crashedBotCurrentDirection));
                    }
                    if (bots[bot].Y < 3)
                    {
                        crashedBotCoordinateX = bots[bot].X;
                        crashedBotCoordinateY = bots[bot].Y;
                        crashedBotCurrentDirection = 3;
                        bots.RemoveAt(bot);
                        bots.Add(new Position(crashedBotCoordinateX, crashedBotCoordinateY, crashedBotCurrentDirection));
                    }
                    if (bots[bot].Y > matrixHeight - 3)
                    {
                        crashedBotCoordinateX = bots[bot].X;
                        crashedBotCoordinateY = bots[bot].Y;
                        crashedBotCurrentDirection = 2;
                        bots.RemoveAt(bot);
                        bots.Add(new Position(crashedBotCoordinateX, crashedBotCoordinateY, crashedBotCurrentDirection));
                    }
                }

                return bots;
        }

        public static List<Position> MoveBotsPosition(Position[] directions, List<Position> bots, int botsCount = 4)
        {
            int randomDirection = 0;

            List<Position> CopyBots = new List<Position>(bots);
            bots.Clear();
            for (int i = 0; i < botsCount; i++)
            {
                randomDirection = CopyBots[i].CurrentDirectionBot;
                bots.Add(new Position(CopyBots[i].X + directions[randomDirection].X, CopyBots[i].Y - directions[randomDirection].Y, randomDirection));
            }

            return bots;
        }

        public static Position[] GetDirections()
        {

            Position[] directions = new Position[] 
            {
                new Position(1,0,0), //right;
                new Position(-1,0,0),  //left
                new Position(0,1,0 ), //up
                new Position(0,-1,0) //down
            };

            return directions;
        }

        public static List<Position> GenerateBots(Random randomIntGenerator,int matrixWidth, int matrixHeight, int botsCount = 4)
        {
            List<Position> bots = new List<Position>();

            for (int i = 0; i < botsCount; i++)
            {
                bots.Add(new Position(randomIntGenerator.Next(3, matrixWidth), randomIntGenerator.Next(3, matrixHeight), randomIntGenerator.Next(0, 3)));
            }

            return bots;
        }

        public static Position GenerateTarget(Random randomIntGenerator, int matrixWidth, int matrixHeight)
        {
            Position target = new Position(randomIntGenerator.Next(5, matrixWidth - 5), randomIntGenerator.Next(4, matrixHeight - 4), 0);
            return target;
        }

        public static void DrawBots(List<Position> bots, string[] botsSymbols)
        {
            for (int i = 0; i < bots.Count; i++)
            {
                Console.SetCursorPosition(bots[i].X, bots[i].Y);
                Console.Write(botsSymbols[i]);
            }
        }

        static void Main(string[] args)
        {
            Position[] directions = GetDirections();

            string[] botsSymbols = new string[]
            {
                "@",
                "$",
                "*",
                "#"
            };
            int score = 0;
            int matrixWidth = 20;
            int matrixHeight = 20;
            int botsCount = 4;
            Console.BufferHeight = Console.WindowHeight;
            Position Player = new Position(0, 0, 0);
            Console.SetCursorPosition(Player.X, Player.Y);
            Console.Write("(0)");
            Random randomIntGenerator = new Random();
            
            //generate bots.
            List<Position> bots = GenerateBots(randomIntGenerator, matrixWidth, matrixHeight);
            //generate target
            Position target = GenerateTarget(randomIntGenerator, matrixWidth, matrixHeight);
            bool targetAcquired = false;
            // draw bots
            DrawBots(bots, botsSymbols);
            //game loop logic.
            bool gamerunning = true;

            while (gamerunning)
            {
                //move player
                if (Console.KeyAvailable)
                {
                    Player = MovePlayer(Player, directions);
                }
                //crash tests
                bots = BotCrashTests(bots, matrixWidth, matrixHeight);
              
                //check if dead.

                //move bots.
                bots = MoveBotsPosition(directions, bots);

                //check if target is acquired.
                if (Player.X == target.X && Player.Y == target.Y)
                {
                    targetAcquired = true;

                }
                if (targetAcquired)
                {
                    target = new Position(randomIntGenerator.Next(5, matrixWidth - 5), randomIntGenerator.Next(4, matrixHeight - 4), 0);
                    targetAcquired = false;
                    score++;
                }



                //draw stuff

                Console.Clear();
                Console.BackgroundColor = ConsoleColor.White;
                //player
                Console.SetCursorPosition(Player.X, Player.Y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("(0)");
                //score
                Console.SetCursorPosition(50, 10);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Your score is ... : {0}", score);
                //target
                Console.SetCursorPosition(target.X, target.Y);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("T");


                //bots
                Console.ForegroundColor = ConsoleColor.Cyan;
                for (int i = 0; i < botsCount; i++)
                {
                    Console.SetCursorPosition(bots[i].X, bots[i].Y);
                    Console.Write(botsSymbols[i]);
                }
                Thread.Sleep(200);
                //check if dead.
                for (int i = 0; i < bots.Count; i++)
                {
                    if (bots[i].X == Player.X && bots[i].Y == Player.Y)
                    {
                        Console.Clear();
                        Console.SetCursorPosition(0, 0);
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("Game OVER ! Your score is ... {0} ", score);
                        gamerunning = false;



                    }
                }

            }







        }
    }
}
