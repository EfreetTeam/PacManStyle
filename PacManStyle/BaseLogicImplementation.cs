﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.IO;

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
        static int matrixHeight, matrixWidth;
        static int botsCount = 4;
        static Random randomIntGenerator = new Random();
        static Position[] directions = GetDirections();

        static void Main()
        {
            string[] botsSymbols = new string[]
            {
                "@",
                "$",
                "*",
                "#",
                "\u15e4",   // left
                "\u15e7",   // right
                "\u15e3",   // down
                "\u15e2"    // up
            };

            int score = 0;
            int currentBoardNumber = 1;                         // initialized for board No.1
            int[,] board = LoadBoard(currentBoardNumber);       // Load 1st board from file board1.txt
            int lives = 3;
            matrixHeight = board.GetLength(0);
            matrixWidth = board.GetLength(1);

            Console.BufferHeight = Console.WindowHeight;
            DrawIntroScreen(); 
            DrawConsoleLayout();
            PrintBoard(board);
            Position player = InitializePlayer();
            List<Position> bots = GenerateBots(board);
            Position target = GenerateTarget();

            DrawBots(bots, botsSymbols);
            bool gamerunning = true;

            while (gamerunning)
            {
                ClearConsole(player, target, bots);
                bool targetAcquired = false;

                if (Console.KeyAvailable)
                {
                    player = MovePlayer(player, board);
                }

                bots = BotCrashTests(bots);
                bots = MoveBotsPosition(bots, board);
                targetAcquired = IsTargetAcquired(player, target);

                if (targetAcquired)
                {
                    target = GenerateTarget();
                    score++;
                }

                DrawPlayer(player);
                DrawPlayerInfo(score, lives);
                DrawTarget(target);
                DrawBots(bots, botsSymbols);

                Thread.Sleep(200);
                //check if dead.
                gamerunning = CheckIfBotHitPlayer(bots, player, score);

                if (!gamerunning)
                {
                    lives--;
                    if (lives > 0)
                    {
                        player = InitializePlayer();
                        bots = GenerateBots(board);
                        gamerunning = true;
                    }
                    else
                    {
                        DrawEndScreen(score);
                    }
                }

                // if (currentLive < previousLive)
                //  {
                //      player = InitializePlayer();
                //      bots = GenerateBots();
                //      target = GenerateTarget();
                //  }

            }
        }

        static void ClearConsole(Position player, Position target, List<Position> bots)
        {
            Console.SetCursorPosition(player.Y, player.X);
            Console.Write(" ");
            Console.SetCursorPosition(50, 10);
            Console.Write(" ");
            Console.SetCursorPosition(target.Y, target.X);
            Console.Write(" ");
            for (int i = 0; i < bots.Count; i++)
            {
                Console.SetCursorPosition(bots[i].Y, bots[i].X);
                Console.Write(" ");
            }
        }

        static void DrawEndScreen(int score)
        {
            //throw new NotImplementedException();
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Game OVER ! Your score is ... {0} ", score);
        }

        static void DrawBossScreen()                    // this method should be called just before Boss fight starts
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            StringBuilder sbBoss = new StringBuilder();
            sbBoss.Append("\n\n\n\n\n\n\n\n");
            sbBoss.Append("\t" + @"  ____   ____   _____ _____   ______ _____ _____ _    _ _______ " + "\n");
            sbBoss.Append("\t" + @" |  _ \ / __ \ / ____/ ____| |  ____|_   _/ ____| |  | |__   __|" + "\n");
            sbBoss.Append("\t" + @" | |_) | |  | | (___| (___   | |__    | || |  __| |__| |  | |   " + "\n");
            sbBoss.Append("\t" + @" |  _ <| |  | |\___ \\___ \  |  __|   | || | |_ |  __  |  | |   " + "\n");
            sbBoss.Append("\t" + @" | |_) | |__| |____) |___) | | |     _| || |__| | |  | |  | |   " + "\n");
            sbBoss.Append("\t" + @" |____/ \____/ \____/\____/  |_|    |_____\_____|_|  |_|  |_|   " + "\n");

            Console.WriteLine(sbBoss);
        }

        static void DrawIntroScreen()
        {
            Console.SetBufferSize(80, 33);
            Console.SetWindowSize(80, 33);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.OutputEncoding = System.Text.Encoding.ASCII;
            string path = "../../IntroScreen.txt";
            StreamReader reader = new StreamReader(path);
            string text = reader.ReadToEnd();
            reader.Close();
            Console.WriteLine(text);
            Console.Beep(2000, 110); Console.Beep(2000, 110); Console.Beep(2000, 110); Console.Beep(2000, 500);    
        }

        static Position InitializePlayer()
        {
            Position player = new Position(1, 1, 0);
            Console.SetCursorPosition(player.Y, player.X);
            Console.Write("" + (char)1);

            return player;
        }

        static Position MovePlayer(Position player, int[,] board)
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

            // SHOULD BE ADDED check if player is attempting to move outside board
            //if ((player.X == 1 && direction == 2) || (player.Y == 1 && direction == 1) || (player.X == matrixHeight - 2 && direction == 3) || (player.Y == matrixWidth - 2 && direction == 0))
            //{
            //    return player;
            //}

            if (board[player.X + directions[direction].X, player.Y + directions[direction].Y] == 0)
            {
                player = new Position(player.X + directions[direction].X, player.Y + directions[direction].Y, 0);
            }

            return player;
        }

        static List<Position> BotCrashTests(List<Position> bots)
        {
            int crashedBotCoordinateX = 0;
            int crashedBotCoordinateY = 0;

            int crashedBotCurrentDirection = 0;
            int currentCordinateX = 0;
            int currentCordinateY = 0;
            for (int bot = 0; bot < botsCount; bot++)
            {
                if (bots[bot].X <= 3) // to extract in method
                {
                    crashedBotCoordinateX = bots[bot].X;
                    crashedBotCoordinateY = bots[bot].Y;
                    //crashedBotCurrentDirection = 0;
                    bool correctDirection = false;
                    while (correctDirection == false)
                    {
                        crashedBotCurrentDirection = randomIntGenerator.Next(0, 4);
                        if (crashedBotCurrentDirection != 1)
                        {
                            currentCordinateX = crashedBotCoordinateX + directions[crashedBotCurrentDirection].X;
                            currentCordinateY = crashedBotCoordinateY - directions[crashedBotCurrentDirection].Y;

                            correctDirection = true;
                        }
                    }

                    bots[bot] = new Position(crashedBotCoordinateX, crashedBotCoordinateY, crashedBotCurrentDirection);
                }

                if (bots[bot].X > matrixWidth - 3)
                {
                    crashedBotCoordinateX = bots[bot].X;
                    crashedBotCoordinateY = bots[bot].Y;
                    //crashedBotCurrentDirection = 0;
                    bool correctDirection = false;
                    while (correctDirection == false)
                    {
                        crashedBotCurrentDirection = randomIntGenerator.Next(0, 4);
                        if (crashedBotCurrentDirection != 0)
                        {
                            currentCordinateX = crashedBotCoordinateX + directions[crashedBotCurrentDirection].X;
                            currentCordinateY = crashedBotCoordinateY - directions[crashedBotCurrentDirection].Y;

                            correctDirection = true;
                        }
                    }

                    bots[bot] = new Position(crashedBotCoordinateX, crashedBotCoordinateY, crashedBotCurrentDirection);
                }

                if (bots[bot].Y < 2)
                {
                    crashedBotCoordinateX = bots[bot].X;
                    crashedBotCoordinateY = bots[bot].Y;
                    //crashedBotCurrentDirection = 0;
                    bool correctDirection = false;
                    while (correctDirection == false)
                    {
                        crashedBotCurrentDirection = randomIntGenerator.Next(0, 4);
                        if (crashedBotCurrentDirection != 3)
                        {
                            currentCordinateX = crashedBotCoordinateX + directions[crashedBotCurrentDirection].X;
                            currentCordinateY = crashedBotCoordinateY - directions[crashedBotCurrentDirection].Y;

                            correctDirection = true;
                        }
                    }

                    bots[bot] = new Position(crashedBotCoordinateX, crashedBotCoordinateY, crashedBotCurrentDirection);
                }

                if (bots[bot].Y > matrixHeight - 3)
                {
                    crashedBotCoordinateX = bots[bot].X;
                    crashedBotCoordinateY = bots[bot].Y;
                    //crashedBotCurrentDirection = 0;
                    bool correctDirection = false;
                    while (correctDirection == false)
                    {
                        crashedBotCurrentDirection = randomIntGenerator.Next(0, 4);
                        if (crashedBotCurrentDirection != 3)
                        {
                            currentCordinateX = crashedBotCoordinateX + directions[crashedBotCurrentDirection].X;
                            currentCordinateY = crashedBotCoordinateY + directions[crashedBotCurrentDirection].Y;

                            correctDirection = true;
                        }
                    }

                    bots[bot] = new Position(crashedBotCoordinateX, crashedBotCoordinateY, crashedBotCurrentDirection);
                }
            }

            return bots;
        }

        static List<Position> SetBotPosition(List<Position> bots, int crashedBotCurrentDirection, int currentBot)
        {
            int crashedBotCoordinateX = 0;
            int crashedBotCoordinateY = 0;

            crashedBotCoordinateX = bots[currentBot].X;
            crashedBotCoordinateY = bots[currentBot].Y;
            bots.RemoveAt(currentBot);
            bots.Add(new Position(crashedBotCoordinateX, crashedBotCoordinateY, crashedBotCurrentDirection));

            return bots;
        }

        static List<Position> MoveBotsPosition(List<Position> bots, int[,] board)
        {
            int randomDirection = 0;
            int currCordinateX = 0;
            int currCordinateY = 0;
            int currDirection = 0;
            bool CorrectDirection = false;
            int nextPosX = 0;
            int nextPosY = 0;
            List<Position> CopyBots = new List<Position>(bots);
            bots.Clear();
            for (int i = 0; i < botsCount; i++)
            {
                randomDirection = CopyBots[i].CurrentDirectionBot;
                nextPosX = CopyBots[i].X + directions[randomDirection].X;
                nextPosY = CopyBots[i].Y - directions[randomDirection].Y;
                currCordinateX = CopyBots[i].X;
                currCordinateY = CopyBots[i].Y;
                CorrectDirection = false;
                if (board[nextPosX, nextPosY] == 1)
                {
                    CorrectDirection = false;
                    while (CorrectDirection == false)
                    {
                        currDirection = randomIntGenerator.Next(0, 4);
                        if (currDirection != randomDirection &&
                            board[currCordinateX + directions[currDirection].X, currCordinateY - directions[currDirection].Y] == 0)
                        {
                            CorrectDirection = true;
                            CopyBots[i] = new Position(currCordinateX, currCordinateY, currDirection);
                        }
                    }


                }
                else
                {
                    CorrectDirection = true;
                }

            }
            if (CorrectDirection)
            {
                for (int i = 0; i < botsCount; i++)
                {

                    randomDirection = CopyBots[i].CurrentDirectionBot;
                    bots.Add(new Position(CopyBots[i].X + directions[randomDirection].X, CopyBots[i].Y - directions[randomDirection].Y, randomDirection));
                }

            }
            return bots;

        }

        static Position[] GetDirections()
        {
            Position[] directions = new Position[] 
            {
                 new Position(0,1,0),    //right
                 new Position(0,-1,0),    //left
                 new Position(-1,0,0),   //up
                 new Position(1,0, 0)    //down
                
               
               
            };

            return directions;
        }

        static List<Position> GenerateBots(int[,] board)
        {
            List<Position> bots = new List<Position>();
            bool AllRightPosition;
            for (int i = 0; i < botsCount; i++)
            {
                AllRightPosition = false;
                int randomX = randomIntGenerator.Next(1, matrixWidth - 1);
                int randomY = randomIntGenerator.Next(1, matrixHeight - 1);
                while (AllRightPosition == false)
                {
                    randomY = randomIntGenerator.Next(1, matrixHeight - 1);
                    randomX = randomIntGenerator.Next(1, matrixWidth - 1);
                    if (board[randomX, randomY] == 0)
                    {
                        AllRightPosition = true;
                    }
                }
                bots.Add(new Position(randomX, randomY, randomIntGenerator.Next(0, 4)));

            }

            return bots;
        }

        static Position GenerateTarget()
        {
            Position target = new Position(randomIntGenerator.Next(5, matrixWidth - 5), randomIntGenerator.Next(4, matrixHeight - 4), 0);
            return target;
        }

        static void DrawBots(List<Position> bots, string[] botsSymbols)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int i = 0; i < bots.Count; i++)
            {
                //set symbol according to direction.
                // X,Y,Z x,y coordinate Z currentdirection 0,1,2,3 4 < > ^ 

                Console.SetCursorPosition(bots[i].Y, bots[i].X);
                // Console.Write(botsSymbols[bots[bot].currentDirection]);
                Console.Write(botsSymbols[i]);
            }
        }

        static void DrawConsoleLayout()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.White;
        }

        static void DrawPlayer(Position player)
        {
            Console.SetCursorPosition(player.Y, player.X);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("" + (char)1);
        }

        static void DrawPlayerInfo(int score, int lives)             //<==
        {
            Console.SetCursorPosition(50, 10);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Your score is ... : {0}", score);
            Console.SetCursorPosition(50, 11);
            Console.Write("Lives: {0} x ♥", lives);
        }

        static void DrawTarget(Position target)
        {
            Console.SetCursorPosition(target.Y, target.X);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write((char)3);
        }

        static bool CheckIfBotHitPlayer(List<Position> bots, Position player, int score)
        {
            bool gamerunning = true;

            for (int i = 0; i < bots.Count; i++)
            {
                if (bots[i].X == player.X && bots[i].Y == player.Y)
                {

                    //Console.Clear();
                    //Console.SetCursorPosition(0, 0);
                    //Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    //Console.ForegroundColor = ConsoleColor.Green;
                    //Console.Write("Game OVER ! Your score is ... {0} ", score);
                    gamerunning = false;
                    return gamerunning;
                }
            }

            return gamerunning;
        }

        static bool IsTargetAcquired(Position player, Position target)
        {
            bool targetAcquired = false;
            if (player.X == target.X && player.Y == target.Y)
            {
                targetAcquired = true;
            }

            return targetAcquired;
        }
        static void CheckIfOnWall(List<Position> bots, int[,] board)
        {
            int currentPosX = 0;
            int currentPosY = 0;
            int oldDirection = 0;
            bool correctDirection = false;
            int currDirection = 0;
            for (int i = 0; i < bots.Count; i++)
            {
                currentPosX = bots[i].X;
                currentPosY = bots[i].Y;
                oldDirection = bots[i].CurrentDirectionBot;
                if (board[currentPosX, currentPosY] == 1
            || board[currentPosX + directions[bots[i].CurrentDirectionBot].X, currentPosY - directions[bots[i].CurrentDirectionBot].Y] == 0
                    )
                {
                    while (correctDirection == false)
                    {
                        currDirection = randomIntGenerator.Next(0, 4);
                        if (board[currentPosX + directions[currDirection].X, currentPosY - directions[currDirection].Y] == 0)
                        {
                            correctDirection = true;
                        }
                    }

                    correctDirection = false;
                    bots.RemoveAt(i);
                    bots.Add(new Position(currentPosX, currentPosY, currDirection));
                }
            }
        }

        // maze
        static int[,] LoadBoard(int currentBoardNumber)     // This method can call any of the preset 3 boards, each in separate file, respectively named board1.txt, board2.txt and board3.txt
        {
            string path = "../../Boards/board" + currentBoardNumber + ".txt";
            StreamReader reader = new StreamReader(path);

            using (reader)
            {
                int rows = int.Parse(reader.ReadLine());
                string line = reader.ReadLine();
                int[,] boardToLoad = new int[rows, line.Length];
                int lineNumber = 0;

                while (line != null)
                {
                    for (int j = 0; j < line.Length; j++) boardToLoad[lineNumber, j] = (line[j]) - '0';
                    lineNumber++;
                    line = reader.ReadLine();
                }
                return boardToLoad;
            }
        }
        static void PrintBoard(int[,] board)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    sb.Append(board[i, j] == 1 ? ((char)2).ToString() : " ");    // 1's => Walls -> not allowed to go through | 0's -> maze path free to go through
                }
                sb.Append("\n");
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(sb);
        }

    }
}
