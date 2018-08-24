using System;
using System.Collections.Generic;
using System.Linq;

namespace program 
{
	class chess 
	{
		public static char[,] board;
		public static bool black;

		public static char[] whitePieces = {'P','R','N','B','Q','K'};
		public static char[] blackPieces = {'p','r','n','b','q','k'};

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
			for (int y=0; y<8; y++)
			{
				Console.Write(8-y);
				Console.Write("| ");
				for (int x=0; y<8; y++)
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
		private struct coord
		{
			public int x;
			public int y;

			//Constructor to initialize with coordinates already present
			public coord(int newX, int newY)
			{
				x = newX;
				y = newY;
			}
			//Return the character representing the piece currently on the coordinate; returns I if the coordinate is invalid
			public char getPiece()
			{
				//Check if the coordinate is valid
				if (x>7 || y>7 || x<0 || y<0)
				{
					return 'I';
				}
				else
				{
					return chess.board[x,y];
				}
			}
		}

		private static coord piecePos;
		public static void getMove()
		{
			while (true)
			{
				Console.Clear();
				chess.printBoard();
				Console.Write("Enter the coordinates of the piece you want the moves for:");
				string coords = Console.ReadLine().ToUpper();
				piecePos = new coord();
				if (!Int32.TryParse(Convert.ToString(coords[1]), out piecePos.y))
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
						piecePos.x = Convert.ToInt32(convertArray[1,i]);
						found = true;
						break;
					}
				}
				if (!found)
				{
					Console.WriteLine("Bad coordinate formatting");
					continue;
				}
				piecePos.x -= 1;
				piecePos.y -= 1;
				coord[] moves = getAllMoves();
				//Print the moves
			}
		}

		private static coord[] getAllMoves()
		{
			//The list containing valid moves
			List<coord> valid = new List<coord>();
			//The array to contain the "enemy" pieces to simplify code
			char[] enemies;
			//The current piece's character (ie. type)
			char pieceChar = piecePos.getPiece();
			//Set the correct set of enemies
			if (chess.whitePieces.Contains(pieceChar))
			{
				enemies = chess.blackPieces;
			}
			else
			{
				enemies = chess.whitePieces;
			}
			//Pawns
			if (Char.ToLower(pieceChar) == 'p')
			{
				coord ahead;
				coord left;
				coord right;
				if ((chess.black && pieceChar == 'p') || (!chess.black && pieceChar == 'P'))
				{
					//Pawn moving up
					int aheadY = piecePos.y+1;
					ahead = new coord(piecePos.x, aheadY);
					left = new coord(piecePos.x+1, aheadY);
					right = new coord(piecePos.x-1, aheadY);
				}
				else
				{
					//Pawn moving down
					int aheadY = piecePos.y-1;
					ahead = new coord(piecePos.x, aheadY);
					left = new coord(piecePos.x+1, aheadY);
					right = new coord(piecePos.x-1, aheadY);
				}
				//Check if ahead is clear
				if (ahead.getPiece() == ' ')
				{
					valid.Add(ahead);
				}
				//Check if sides are occupied by enemies
				//Left
				if (enemies.Contains(left.getPiece()))
				{
					valid.Add(left);
				}
				//Right
				if (enemies.Contains(right.getPiece()))
				{
					valid.Add(right);
				}
			}
			else
			{
			switch (Char.ToLower(pieceChar)){
				case 'r':
					//Rooks
					break;
				case 'n':
					//Knights
					//Define the array of moves knights can make
					coord[] knightMoves = {new coord(piecePos.x-2,piecePos.x-1),new coord(piecePos.x-1,piecePos.y-2),new coord(piecePos.x+2,piecePos.y-1),new coord(piecePos.x+1,piecePos.y-2),new coord(piecePos.x+2,piecePos.y+1),new coord(piecePos.x+1,piecePos.y+2),new coord(piecePos.x-2,piecePos.y+1),new coord(piecePos.x-1,piecePos.y+2)};
					//Check them all for free spaces and enemies
					foreach (coord current in knightMoves)
					{
						if (current.getPiece() == ' ' || enemies.Contains(current.getPiece()))
						{
							valid.Add(current);
						}
					}
					break;
				case 'b':
					//Bishops
					break;
				case 'q':
					//Queens
					break;
				case 'k':
					//Kings
					break;
				}
			}
			return valid.ToArray();
		}
	}
}

