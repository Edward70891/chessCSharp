using System;
using System.Collections.Generic;
namespace program 
{
	class chess 
	{
		public static char[,] board;
		public static bool black;

		static void Main()
		{
			while (true)
			{
				Console.Clear();
				Console.Write("Black or White? (b/w): ");
				string bw = Console.ReadLine();
				if (bw == "b")
				{
					black = true;
				}
				else if (bw == "w")
				{
					black = false;
				}
				else 
				{
					Console.WriteLine("That wasn't an option...");
					continue;
				}
				setDefaultBoard(black);
				Console.Clear();
				Console.WriteLine("The Board:");
				printBoard();
				Console.ReadLine();
			}
		}

		static void setDefaultBoard(bool black)
		{
			//Manually set the default starting section, needs to be replace with unicode chars for chess pieces if at all possible
			board = new char[8,8] {
				{'r','n','b','q','k','b','n','r'},
				{'p','p','p','p','p','p','p','p'},
				{' ',' ',' ',' ',' ',' ',' ',' '},
				{' ',' ',' ',' ',' ',' ',' ',' '},
				{' ',' ',' ',' ',' ',' ',' ',' '},
				{' ',' ',' ',' ',' ',' ',' ',' '},
				{'P','P','P','P','P','P','P','P'},
				{'R','K','B','Q','K','B','N','R'}
			};
			//Swap the sides if the user is playing black; this doesn't seem to work for some reason
			if (black)
			{
				char[,] newBoard = new char[8,8];
				for (int x=0; x<8; x++)
				{
					for (int y=0; y<8; y++)
					{
						newBoard[x,7-y] = board[x,y];
					}
				}
				board = newBoard;
			}
		}

		public static void printBoard()
		{
			for (int x=0; x<8; x++)
			{
				Console.Write(8-x);
				Console.Write("| ");
				for (int y=0; y<8; y++)
				{
					Console.Write(board[x,y]);
					Console.Write(" ");
				}
				Console.Write("\n");
			}
			Console.WriteLine(" |________________");
			Console.WriteLine("  A B C D E F G H");
		}

		static void commandPrompt()
		{
			Console.Write(">");
			string command = Console.ReadLine();
			switch (command)
			{
				case "getmove":
					movingEngine.getMove();
					break;
				default:
					Console.WriteLine("Command not recognised.");
					break;
			}
		}
	}

	static class movingEngine
	{
		private static List<int[]> movesList;
		public static void getMove()
		{
			while (true)
			{
				Console.Clear();
				chess.printBoard();
				Console.Write("Enter the coordinates of the piece you want the moves for:");
				string coords = Console.ReadLine().ToUpper();
				int x = 0;
				int y = 0;
				if (!Int32.TryParse(Convert.ToString(coords[1]), out y))
				{
					Console.WriteLine("Bad coordinate formatting!");
					continue;
				}
				char[,] convertArray = {{'A','B','C','D','E','F','G','H'},{'1','2','3','4','5','6','7','8'}};
				bool found = false;
				for (int i=0; i<8; i++)
				{
					if (coords[0] == convertArray[0,i])
					{
						x = Convert.ToInt32(convertArray[1,i]);
						found = true;
						break;
					}
				}
				if (!found)
				{
					Console.WriteLine("Bad coordinate formatting");
					continue;
				}
				x -= 1;
				y -= 1;
				int[,] moves = getAllMoves(x, y);
				//Print the moves
			}
		}

		public static int[,] getAllMoves(int x, int y)
		{
			char pieceChar = chess.board[x,y];
			if (Char.ToLower(pieceChar) == 'p')
			{
				if ((chess.black && pieceChar == 'p') || (!chess.black && pieceChar == 'P'))
				{
					upPawn(x,y);
				}
				else
				{
					downPawn(x,y);
				}
			}
			switch (Char.ToLower(pieceChar)){
				case 'r':
					rook(x,y);
					break;
				case 'n':
					knight(x,y);
					break;
				case 'b':
					bishop(x,y);
					break;
				case 'q':
					queen(x,y);
					break;
				case 'k':
					king(x,y);
					break;
			}

			//Code to return a list of the movesList stored as a 2D array?
		}

		private static void upPawn(int x, int y)
		{

		}

		private static void downPawn(int x, int y)
		{

		}

		private static void rook(int x, int y)
		{
			//A temporary array to store the current valid move because I've forgotten how to assign whole arrays in c#
			int[] currentMove = new int[2];
			//Checking all the tiles "east" of the rook
			for (int i=x+1; i<8; i++)
			{
				if (chess.board[y,i] == ' ')
				{
					currentMove[0] = y;
					currentMove[1] = i;
					movesList.Add(currentMove);
				}
				else
				{
					break;
				}
			}
			//Checking all the tiles "south" of the rook
			for (int i=y+1; i<8; i++)
			{
				if (chess.board[i,x] == ' ')
				{
					currentMove[0] = i;
					currentMove[1] = x;
					movesList.Add(currentMove);
				}
				else
				{
					break;
				}
			}
			//Checking all the tiles "north" of the rook
			for (int i=y-1; i>-1; i--)
			{
				if (chess.board[i,x] == ' ')
				{
					currentMove[0] = i;
					currentMove[1] = x;
					movesList.Add(currentMove);
				}
				else
				{
					break;
				}
			}
			//Checking all the tiles "west" of the rook
			for (int i=x-1; i>-1; i++)
			{
				if (chess.board[y,i] == ' ')
				{
					currentMove[0] = y;
					currentMove[1] = i;
					movesList.Add(currentMove);
				}
				else
				{
					break;
				}
			}
			//Done!
			
		}

		private static void knight(int x, int y)
		{

		}

		private static void bishop(int x, int y)
		{

		}

		private static void queen(int x, int y)
		{

		}

		private static void king(int x, int y)
		{

		}
	}
}

