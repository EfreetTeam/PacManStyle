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
        static void Main(string[] args)
        {
            Position[] Directions = new Position[] 
            {
                new Position(1,0,0), //right;
                new Position(-1,0,0),  //left
                new Position(0,1,0 ), //up
                new Position(0,-1,0) //down
            };
            int Direction = 0;
            string[] bots = new string[]
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
            Random RandomIntGenerator = new Random();
            List<Position> Bots = new List<Position>();
            //generate bots.
            for (int i = 0; i < botsCount; i++)
            {
                Bots.Add(new Position(RandomIntGenerator.Next(3, matrixWidth), RandomIntGenerator.Next(3, matrixHeight), RandomIntGenerator.Next(0, 3)));
            }
            //generate target
            Position Target = new Position(RandomIntGenerator.Next(5, matrixWidth - 5), RandomIntGenerator.Next(4, matrixHeight - 4), 0);
            bool targetAcquired = false;
            // draw bots


            for (int i = 0; i < Bots.Count; i++)
            {
                Console.SetCursorPosition(Bots[i].X, Bots[i].Y);
                Console.Write(bots[i]);
            }
            //game loop logic.
            bool gamerunning = true;

            while (gamerunning)
            {
                //move player
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();
                    if (userInput.Key == ConsoleKey.RightArrow)
                    {
                        Direction = 0;

                    }
                    else if (userInput.Key == ConsoleKey.LeftArrow)
                    {
                        Direction = 1;

                    }
                    else if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        Direction = 2;
                    }
                    else
                    {
                        Direction = 3;
                    }
                    Player = new Position(Player.X + Directions[Direction].X, Player.Y - Directions[Direction].Y, 0);
                }
                //test

                int randomDirection = 0;
                //crash tests

                int crashedBotCoordinateX = 0;
                int crashedBotCoordinateY = 0;
                int crashedBotCurrentDirection = 0;
                for (int bot = 0; bot < botsCount; bot++)
                {
                    if (Bots[bot].X <= 3

                        )
                    {
                        crashedBotCoordinateX = Bots[bot].X;
                        crashedBotCoordinateY = Bots[bot].Y;
                        crashedBotCurrentDirection = 0;
                        Bots.RemoveAt(bot);
                        Bots.Add(new Position(crashedBotCoordinateX, crashedBotCoordinateY, crashedBotCurrentDirection));

                    }
                    if (Bots[bot].X > matrixWidth - 3)
                    {
                        crashedBotCoordinateX = Bots[bot].X;
                        crashedBotCoordinateY = Bots[bot].Y;
                        crashedBotCurrentDirection = 1;
                        Bots.RemoveAt(bot);
                        Bots.Add(new Position(crashedBotCoordinateX, crashedBotCoordinateY, crashedBotCurrentDirection));
                    }
                    if (Bots[bot].Y < 3)
                    {
                        crashedBotCoordinateX = Bots[bot].X;
                        crashedBotCoordinateY = Bots[bot].Y;
                        crashedBotCurrentDirection = 3;
                        Bots.RemoveAt(bot);
                        Bots.Add(new Position(crashedBotCoordinateX, crashedBotCoordinateY, crashedBotCurrentDirection));
                    }
                    if (Bots[bot].Y > matrixHeight - 3)
                    {
                        crashedBotCoordinateX = Bots[bot].X;
                        crashedBotCoordinateY = Bots[bot].Y;
                        crashedBotCurrentDirection = 2;
                        Bots.RemoveAt(bot);
                        Bots.Add(new Position(crashedBotCoordinateX, crashedBotCoordinateY, crashedBotCurrentDirection));
                    }
                }
                //check if dead.



                //move bots.
                List<Position> CopyBots = new List<Position>(Bots);
                Bots.Clear();
                for (int i = 0; i < botsCount; i++)
                {
                    randomDirection = CopyBots[i].CurrentDirectionBot;
                    Bots.Add(new Position(CopyBots[i].X + Directions[randomDirection].X, CopyBots[i].Y - Directions[randomDirection].Y, randomDirection));
                }

                //check if target is acquired.
                if (Player.X == Target.X && Player.Y == Target.Y)
                {
                    targetAcquired = true;

                }
                if (targetAcquired)
                {
                    Target = new Position(RandomIntGenerator.Next(5, matrixWidth - 5), RandomIntGenerator.Next(4, matrixHeight - 4), 0);
                    targetAcquired = false;
                    score++;
                }



                //draw stuff

                Console.Clear();
                //player
                Console.SetCursorPosition(Player.X, Player.Y);
                Console.Write("(0)");
                //score
                Console.SetCursorPosition(50, 10);
                Console.Write("Your score is ... : {0}", score);
                //target
                Console.SetCursorPosition(Target.X, Target.Y);
                Console.Write("T");


                //bots
                for (int i = 0; i < botsCount; i++)
                {
                    Console.SetCursorPosition(Bots[i].X, Bots[i].Y);
                    Console.Write(bots[i]);
                }
                Thread.Sleep(200);
                //check if dead.
                for (int i = 0; i < Bots.Count; i++)
                {
                    if (Bots[i].X == Player.X && Bots[i].Y == Player.Y)
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
