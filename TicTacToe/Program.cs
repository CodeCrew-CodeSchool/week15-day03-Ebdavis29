using System;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }

    public class GameBoard
    {
        private string[] board;

        public GameBoard()
        {
            board = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        }

        public void DisplayBoard()
        {
            Console.WriteLine($"{board[0]} | {board[1]} | {board[2]}");
            Console.WriteLine("---------");
            Console.WriteLine($"{board[3]} | {board[4]} | {board[5]}");
            Console.WriteLine("---------");
            Console.WriteLine($"{board[6]} | {board[7]} | {board[8]}");
        }

        public void UpdateBoard(int position, string marker)
        {
            if (board[position - 1] == "X" || board[position - 1] == "O")
            {
                Console.WriteLine("This position is already taken.");
                return;
            }
            board[position - 1] = marker;
        }

        public void CheckForWinner()
        {
            string[,] winningCombinations = new string[,]
            {
                { board[0], board[1], board[2] },
                { board[3], board[4], board[5] },
                { board[6], board[7], board[8] },
                { board[0], board[3], board[6] },
                { board[1], board[4], board[7] },
                { board[2], board[5], board[8] },
                { board[0], board[4], board[8] },
                { board[2], board[4], board[6] }
            };

            for (int i = 0; i < winningCombinations.GetLength(0); i++)
            {
                if (winningCombinations[i, 0] == winningCombinations[i, 1] && winningCombinations[i, 1] == winningCombinations[i, 2])
                {
                    throw new GameWonException($"{winningCombinations[i, 0]} wins!");
                }
            }
        }

        public void CheckIfBoardIsFull()
        {
            foreach (string position in board)
            {
                if (position != "X" && position != "O")
                {
                    return;
                }
            }
            throw new GameDrawException();
        }
    }

    public class GameWonException : Exception
    {
        public GameWonException(string message) : base(message) { }
    }

    public class GameDrawException : Exception
    {
        public GameDrawException() : base("The game is a draw!") { }
    }

    public class Player
    {
        public string Name { get; set; }
        public string Marker { get; set; }

        public Player(string name, string marker)
        {
            Name = name;
            Marker = marker;
        }
    }

    public class Game
    {
        private GameBoard board;
        private Player player1;
        private Player player2;
        private Player currentPlayer;

        public Game()
        {
            board = new GameBoard();
            player1 = new Player("Player 1", "X");
            player2 = new Player("Player 2", "O");
            currentPlayer = player1;
        }

        public void Start()
        {
            while (true)
            {
                board.DisplayBoard();
                Console.WriteLine($"{currentPlayer.Name}'s turn ({currentPlayer.Marker}). Choose a position:");

                int position;
                while (!int.TryParse(Console.ReadLine(), out position) || position < 1 || position > 9)
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 9.");
                }

                board.UpdateBoard(position, currentPlayer.Marker);

                try
                {
                    board.CheckForWinner();
                    board.CheckIfBoardIsFull();
                    SwitchPlayer();
                }
                catch (GameWonException ex)
                {
                    board.DisplayBoard();
                    Console.WriteLine(ex.Message);
                    break;
                }
                catch (GameDrawException ex)
                {
                    board.DisplayBoard();
                    Console.WriteLine(ex.Message);
                    break;
                }
            }
        }

        private void SwitchPlayer()
        {
            currentPlayer = currentPlayer == player1 ? player2 : player1;
        }
    }
}