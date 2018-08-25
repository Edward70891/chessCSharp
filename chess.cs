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

		public struct coord : IEquatable<coord>
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

			public bool Equals(coord comparison)
			{
				if (x == comparison.x && y == comparison.y)
				{
					return true;
				}
				else
				{
					return false;
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

		public struct moveset
		{
			public game.coord target{get;}
			public game.coord[] passiveMoves{get;}
			public game.coord[] activeMoves{get;}

			//Make a new moveset from a target and two lists of moves
			public moveset(game.coord pieceTarget, game.coord[] nonTaking, game.coord[] taking)
			{
				activeMoves = taking;
				passiveMoves = nonTaking;
				target = pieceTarget;
			}
			//Combine two movesets, this is here basically for the sake of the queen
			public moveset(moveset a, moveset b)
			{
				if (!(a.Equals(b)))
				{
					throw new Exception("The targets do not match!");
				}
				target = a.target;
				//Define new lists to concatenate arrays
				List<game.coord> validPass = new List<game.coord>();
				List<game.coord> validActi = new List<game.coord>();
				
				//Add the contents of a to each array
				foreach (game.coord current in a.passiveMoves)
				{
					validPass.Add(current);
				}
				foreach (game.coord current in a.activeMoves)
				{
					validActi.Add(current);
				}

				//Add the contents of b which are not already in a to each array
				foreach (game.coord current in b.passiveMoves)
				{
					if (!(a.passiveMoves.Contains(current)))
					{
						validPass.Add(current);
					}
				}
				foreach (game.coord current in b.activeMoves)
				{
					if (!(a.activeMoves.Contains(current)))
					{
						validActi.Add(current);
					}
				}

				//Set the arrays
				activeMoves = validActi.ToArray();
				passiveMoves = validPass.ToArray();
			}
		}

		private game.coord piecePos;
		
		//Return a list of all the coordinates that a single piece could move to, including taking
		public moveset getAllMoves(game.coord target)
		{
			piecePos = target;
			//The list containing valid moves that the piece can make
			List<game.coord> valid = new List<game.coord>();
			List<game.coord> validTaking = new List<game.coord>();
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
					validTaking.Add(left);
				}
				//Right
				if (enemies.Contains(right.getPiece(toAnalyze)))
				{
					valid.Add(right);
					validTaking.Add(left);
				}
			}
			else
			{
			switch (Char.ToLower(pieceChar)){
				case game.blackPieces[1]:
					//Rooks
					return straightMoves(piecePos);
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
							validTaking.Add(current);
						}
					}
					return new moveset(piecePos, valid.ToArray(), validTaking.ToArray());
				case game.blackPieces[3]:
					//Bishops
					return diagonalMoves(piecePos);
				case game.blackPieces[4]:
					//Queens
					return new moveset(diagonalMoves(piecePos), straightMoves(piecePos));
				case game.blackPieces[5]:
					//Kings
					for (int i=-1; i<2; i++)
					{
						for (int o =-1; o<2; o++)
						{
							if (toAnalyze.board[i,o] == ' ' || enemies.Contains(toAnalyze.board[i,o]))
							{
								valid.Add(new game.coord(i,o));
								validTaking.Add(new game.coord(i,o));
							}
						}
					}
					return new moveset(piecePos, valid.ToArray(), validTaking.ToArray());
				}
			}
		}

		private moveset straightMoves(game.coord target)
		{

		}

		private moveset diagonalMoves(game.coord target)
		{

		}
	}
}
