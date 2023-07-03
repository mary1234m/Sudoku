using System;

namespace SudokuApp
{
    public class SudokuBoard
    {
        private const int BoardSize = 9;
        private const int SubGridSize = 3;

        private int[,] board;
        private int[,] hideBoard;

        public SudokuBoard()
        {
            board = new int[BoardSize, BoardSize];
        }

        public void GenerateRandomBoard()
        {
            // Clear the board
            ClearBoard();

            // Fill the board with random valid values
            FillBoard();

            //HideBoardNumbers();
        }

        public void SolveBoard()
        {
            // Clone the board to keep the original state
            int[,] clonedBoard = (int[,])board.Clone();

            // Solve the cloned board
            bool isSolved = SolveSudoku(clonedBoard);

            if (isSolved)
            {
                board = clonedBoard;
            }
            else
            {
                Console.WriteLine("Invalid Sudoku puzzle! No solution exists.");
            }
        }

        private void ClearBoard()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    board[row, col] = 0;
                }
            }
        }

        private void FillBoard()
        {
            Random random = new Random();

            // Fill each sub-grid with random valid values
            for (int subGridRow = 0; subGridRow < SubGridSize; subGridRow++)
            {
                for (int subGridCol = 0; subGridCol < SubGridSize; subGridCol++)
                {
                    int startRow = subGridRow * SubGridSize;
                    int startCol = subGridCol * SubGridSize;

                    FillSubGrid(startRow, startCol, random);
                }
            }
            hideBoard = board;

        }

        private void HideBoardNumbers()
        {
            Random random = new Random();



            for (int row = 0; row < BoardSize; row++)

            {

                for (int col = 0; col < BoardSize; col++)

                {


                    if (random.Next(2) == 0)

                    {

                        hideBoard[row, col] = 0;

                    }

                }

            }

        }

        private void FillSubGrid(int startRow, int startCol, Random random)
        {
            int[] values = GenerateRandomValues();

            for (int row = startRow; row < startRow + SubGridSize; row++)
            {
                for (int col = startCol; col < startCol + SubGridSize; col++)
                {
                    int valueIndex = row % SubGridSize * SubGridSize + col % SubGridSize;
                    board[row, col] = values[valueIndex];
                }
            }
        }

        private int[] GenerateRandomValues()
        {
            int[] values = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // Shuffle the values using Fisher-Yates algorithm
            Random random = new Random();
            for (int i = values.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                int temp = values[i];
                values[i] = values[j];
                values[j] = temp;
            }

            return values;
        }

        private bool SolveSudoku(int[,] board)
        {
            return SolveSudokuHelper(board, 0, 0);
        }

        private bool SolveSudokuHelper(int[,] board, int row, int col)
        {
            if (row == BoardSize)
            {
                return true; // Reached the end of the board
            }

            int nextRow = col == BoardSize - 1 ? row + 1 : row;
            int nextCol = (col + 1) % BoardSize;

            if (board[row, col] != 0)
            {
                return SolveSudokuHelper(board, nextRow, nextCol); // Skip the cell with a pre-filled value
            }

            for (int value = 1; value <= BoardSize; value++)
            {
                if (IsValidMove(board, row, col, value))
                {
                    board[row, col] = value;

                    if (SolveSudokuHelper(board, nextRow, nextCol))
                    {
                        return true;
                    }

                    board[row, col] = 0; // Backtrack
                }
            }

            return false; // No valid value found
        }

        private bool IsValidMove(int[,] board, int row, int col, int value)
        {
            for (int i = 0; i < BoardSize; i++)
            {
                if (board[row, i] == value || board[i, col] == value)
                {
                    return false; // Check row and column
                }
            }

            int gridRow = row - row % SubGridSize;
            int gridCol = col - col % SubGridSize;

            for (int i = 0; i < SubGridSize; i++)
            {
                for (int j = 0; j < SubGridSize; j++)
                {
                    if (board[gridRow + i, gridCol + j] == value)
                    {
                        return false; // Check sub-grid
                    }
                }
            }

            return true;
        }

        public void PrintBoard()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    Console.Write(board[row, col] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void PrintHideBoard()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    Random random = new Random();
                    if (random.Next(2) == 0)

                    {

                        Console.Write("0 ");

                    }
                    else
                    {
                        Console.Write(board[row, col] + " ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    public class Program
    {
        static void Main()
        {
            SudokuBoard sudokuBoard = new SudokuBoard();

            Console.WriteLine("Generated Sudoku Board:");
            sudokuBoard.GenerateRandomBoard();
            sudokuBoard.PrintHideBoard();

            Console.WriteLine("Solved Sudoku Board:");
            sudokuBoard.SolveBoard();
            sudokuBoard.PrintBoard();
        }
    }
}
