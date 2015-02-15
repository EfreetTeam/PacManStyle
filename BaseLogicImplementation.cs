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
            int matrixWidth = 20;
            int matrixHeight = 20;
            int botsCount = 4;
            Console.BufferHeight = Console.WindowHeight;
            Position Player = new Position(0, 0,0);
            Console.SetCursorPosition(Player.X, Player.Y);
            Console.Write("(0)");
            Random RandomIntGenerator = new Random();
            List<Position> Bots = new List<Position>();
            //generate bots.
            for (int i = 0; i < botsCount; i++)
            {
                Bots.Add(new Position(RandomIntGenerator.Next(0, matrixWidth), RandomIntGenerator.Next(0, matrixHeight),RandomIntGenerator.Next(0,3)));
            }
            // draw bots
            
           
            for (int i = 0; i < Bots.Count; i++)
            {
                Console.SetCursorPosition(Bots[i].X, Bots[i].Y);
                Console.Write(bots[i]);
            }
            //game loop logic.
            while (true)
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
                    Player = new Position(Player.X + Directions[Direction].X, Player.Y - Directions[Direction].Y,0);
                }
                //test
                
                int randomDirection=0;
                //move bots.
                List<Position> CopyBots = new List<Position>(Bots);
                Bots.Clear();
                for (int i = 0; i < botsCount; i++)
                {
                    randomDirection=CopyBots[i].CurrentDirectionBot;
                    Bots.Add(new Position(CopyBots[i].X + Directions[randomDirection].X, CopyBots[i].Y - Directions[randomDirection].Y, randomDirection));
                }
                    

                //draw stuff
                
                Console.Clear();
                Console.SetCursorPosition(Player.X, Player.Y);
                Console.Write("(0)");
                for (int i = 0; i < botsCount; i++)
                {
                    Console.SetCursorPosition(Bots[i].X, Bots[i].Y);
                    Console.Write(bots[i]);
                }
                Thread.Sleep(700);
                 
            }







        }
    }
}
