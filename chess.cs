using System;
using System.Collections.Generic;
using System.Linq;

namespace chess
{
	public enum team {white, black};

	public class game
	{
		public char[,] board;
		public team attacker;

		public struct coord
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
			public char getPiece(game target)
			{
				//Check if the coordinate is valid
				if (x>7 || y>7 || x<0 || y<0)
				{
					return 'I';
				}
				else
				{
					return target.board[x,y];
				}
			}
		}

		private static char[,] defaultWhite = new char[8,8];
		private static char[,] defaultBlack = new char[8,8];

		public static char[] blackPieces = {'p','r','n','b','q','k'};
		public static char[] whitePieces = {'P','R','N','B','Q','K'};

		//Initialize the board with a default board layout
		public game(team bottom)
		{
			switch (bottom)
			{
				case team.white:
					board = defaultWhite;
					break;
				case team.black:
					board = defaultBlack;
					break;
			}
			attacker = bottom;
			defineDefaults();
		}

		//Define the default boards because it's easier to do programmatically
		private void defineDefaults()
		{
			for (int i=0; i<8; i++)
			{
				//Set the pawns
				defaultWhite[i,1] = whitePieces[0];
				defaultWhite[i,6] = blackPieces[0];
				defaultBlack[i,1] = blackPieces[0];
				defaultBlack[i,6] = whitePieces[0];

				//Add the spaces between the rows
				for (int o=0; o<8; o++)
				{
					defaultWhite[i,o] = ' ';
					defaultBlack[i,o] = ' ';
				}
			}

			//Create the "home rows"
			defaultWhite[0,0] = whitePieces[1];
			defaultWhite[0,1] = whitePieces[2];
			defaultWhite[0,2] = whitePieces[3];
			defaultWhite[0,3] = whitePieces[4];
			defaultWhite[0,4] = whitePieces[5];
			defaultWhite[0,5] = whitePieces[3];
			defaultWhite[0,6] = whitePieces[2];
			defaultWhite[0,7] = whitePieces[1];

			defaultBlack[0,0] = blackPieces[1];
			defaultBlack[0,1] = blackPieces[2];
			defaultBlack[0,2] = blackPieces[3];
			defaultBlack[0,3] = blackPieces[4];
			defaultBlack[0,4] = blackPieces[5];
			defaultBlack[0,5] = blackPieces[3];
			defaultBlack[0,6] = blackPieces[2];
			defaultBlack[0,7] = blackPieces[1];
		}
	}

	public class movingEngine
	{
		private game toAnalyze;

		public movingEngine(game target)
		{
			toAnalyze=target;
		}

		

		private game.coord piecePos;
		
		//Return a list of all the coordinates that a single piece could move to, including taking
		public game.coord[] getAllMoves()
		{
			//The list containing valid moves that the piece can make
			List<game.coord> valid = new List<game.coord>();
			//The array to contain the "enemy" pieces to simplify code
			char[] enemies;
			//The current piece's character (ie. type)
			char pieceChar = piecePos.getPiece(toAnalyze);
			//Set the correct set of enemies
			if (game.whitePieces.Contains(pieceChar))
			{
				enemies = game.blackPieces;
			}
			else
			{
				enemies = game.whitePieces;
			}

			//Pawns
			if (Char.ToLower(pieceChar) == game.blackPieces[0])
			{
				game.coord ahead;
				game.coord left;
				game.coord right;
				if ((toAnalyze.attacker == team.black && pieceChar == 'p') || (!(toAnalyze.attacker == team.white) && pieceChar == 'P'))
				{
					//Pawn moving up
					int aheadY = piecePos.y+1;
					ahead = new game.coord(piecePos.x, aheadY);
					left = new game.coord(piecePos.x+1, aheadY);
					right = new game.coord(piecePos.x-1, aheadY);
				}
				else
				{
					//Pawn moving down
					int aheadY = piecePos.y-1;
					ahead = new game.coord(piecePos.x, aheadY);
					left = new game.coord(piecePos.x+1, aheadY);
					right = new game.coord(piecePos.x-1, aheadY);
				}
				//Check if ahead is clear
				if (ahead.getPiece(toAnalyze) == ' ')
				{
					valid.Add(ahead);
				}
				//Check if sides are occupied by enemies
				//Left
				if (enemies.Contains(left.getPiece(toAnalyze)))
				{
					valid.Add(left);
				}
				//Right
				if (enemies.Contains(right.getPiece(toAnalyze)))
				{
					valid.Add(right);
				}
			}
			else
			{
			switch (Char.ToLower(pieceChar)){
				case game.blackPieces[1]:
					//Rooks
					break;
				case game.blackPieces[2]:
					//Knights
					//Define the array of moves knights can make
					game.coord[] knightMoves = {
						//Bottom left
						new game.coord(piecePos.x-2,piecePos.x-1),
						new game.coord(piecePos.x-1,piecePos.y-2),
						//Bottom right
						new game.coord(piecePos.x+2,piecePos.y-1),
						new game.coord(piecePos.x+1,piecePos.y-2),
						//Top right
						new game.coord(piecePos.x+2,piecePos.y+1),
						new game.coord(piecePos.x+1,piecePos.y+2),
						//Top left
						new game.coord(piecePos.x-2,piecePos.y+1),
						new game.coord(piecePos.x-1,piecePos.y+2)
					};
					//Check them all for free spaces and enemies
					foreach (game.coord current in knightMoves)
					{
						if (current.getPiece(toAnalyze) == ' ' || enemies.Contains(current.getPiece(toAnalyze)))
						{
							valid.Add(current);
						}
					}
					break;
				case game.blackPieces[3]:
					//Bishops
					break;
				case game.blackPieces[4]:
					//Queens
					break;
				case game.blackPieces[5]:
					//Kings
					break;
				}
			}
			return valid.ToArray();
		}
	}
}
