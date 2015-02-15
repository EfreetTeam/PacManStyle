using System;
using System.IO;

class PacManStyle
{
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
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                Console.Write(board[i, j] == 1 ? ((char)1).ToString() : " ");    // 1's (☺) = Walls -> not allowed to go through | 0's -> maze path free to go through
            }
            Console.WriteLine();
        }
    }

    static void Main()
    {
        int currentBoardNumber = 1; // initialized for board No.1
        int[,] board = LoadBoard(currentBoardNumber); // Load 1st board from file board1.txt

        // Alternatively for testing purposes, instead of reading from external file, can be used following board:

        //int[,] board = {                                      // 0's = Path - can go through
        //{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},          // 1's = Walls - not allowed to go through
        //{1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1},
        //{1,0,1,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,1,0,1},
        //{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //{1,0,1,1,1,0,1,0,1,1,1,1,1,0,1,0,1,1,1,0,1},
        //{1,0,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,0,0,1},
        //{1,1,1,0,1,0,1,1,1,0,1,0,1,1,1,0,1,0,1,1,1},
        //{1,0,0,0,1,0,1,0,0,0,0,0,0,0,1,0,1,0,0,0,1},
        //{1,0,1,1,1,0,1,0,1,1,0,1,1,0,1,0,1,1,1,0,1},
        //{1,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,1},
        //{1,0,1,1,1,0,1,0,1,1,1,1,1,0,1,0,1,1,1,0,1},
        //{1,0,0,0,1,0,1,0,0,0,0,0,0,0,1,0,1,0,0,0,1},
        //{1,1,1,0,1,0,1,0,1,1,1,1,1,0,1,0,1,0,1,1,1},
        //{1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1},
        //{1,0,1,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,1,0,1},
        //{1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1},
        //{1,1,1,0,1,0,1,0,1,1,1,1,1,0,1,0,1,0,1,1,1},
        //{1,0,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,0,0,1},
        //{1,0,1,1,0,1,1,1,1,0,1,0,1,1,1,1,0,1,1,0,1},
        //{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
        //};

        PrintBoard(board);                          // Print 1st board

        Console.WriteLine();
        board = LoadBoard(++currentBoardNumber);    // Load 2nd board from file board2.txt
        PrintBoard(board);                          // Print 2nd board

        Console.WriteLine();
        board = LoadBoard(++currentBoardNumber);    // Load 3rd board from file board3.txt
        PrintBoard(board);                          // Print 3rd board
    }
}