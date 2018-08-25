using System;
using System.Collections.Generic;
using System.Linq;
using chess;

namespace program 
{
	class CLI 
	{
		static game game1;
		static movingEngine engine1;
		static void Main()
		{
			while (true)
			{
				Console.Clear();
				Console.Write("Black or White? (b/w): ");
				string bw = Console.ReadLine();
				team playerColour;
				if (bw == "b")
				{
					playerColour = team.black;
				}
				else if (bw == "w")
				{
					playerColour = team.white;
				}
				else 
				{
					Console.WriteLine("That wasn't an option...");
					continue;
				}
				game1 = new game(playerColour);
				engine1 = new movingEngine(game1);
				Console.Clear();
				Console.WriteLine("The Board:");
				printBoard(game1);
				Console.ReadLine();
			}
		}

		//Print the board line by line, adding axes and coordinate labelling
		static void printBoard(game toPrint)
		{
			for (int y=0; y<8; y++)
			{
				Console.Write(8-y);
				Console.Write("| ");
				for (int x=0; y<8; y++)
				{
					Console.Write(toPrint.board[x,y]);
					Console.Write(" ");
				}
				Console.Write("\n");
			}
			Console.WriteLine(" |________________");
			Console.WriteLine("  A B C D E F G H");
		}

		//The command prompt
		//Only has getMove for now but should be easy to add commands
		static void commandPrompt()
		{
			Console.Write(">");
			string command = Console.ReadLine();
			switch (command)
			{
				case "getmove":
					getMove(game1);
					break;
				default:
					Console.WriteLine("Command not recognised.");
					break;
			}
		}

		public static void getMove(game toAnalyze)
		{
			movingEngine.coord piecePos;
			while (true)
			{
				//Array to convert the x letter to a number
				char[,] convertArray = {{'A','B','C','D','E','F','G','H'},{'1','2','3','4','5','6','7','8'}};
				//Prompt the user
				printBoard(toAnalyze);
				Console.WriteLine();
				Console.Write("Enter the coordinates of the piece you want the moves for:");
				string userRequest= Console.ReadLine().ToUpper();
				piecePos = new movingEngine.coord();
				//Check the y coordinate is valid
				if (!Int32.TryParse(Convert.ToString(userRequest[1]), out piecePos.y))
				{
					Console.WriteLine("Bad coordinate formatting!");
					continue;
				}
				bool found = false;
				for (int i=0; i<8; i++)
				{
					if (userRequest[0] == convertArray[0,i])
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
				//Decrement for 0-based arrays
				piecePos.x--;
				piecePos.y--;
				movingEngine.coord[] moves = engine1.getAllMoves();
				//Print the moves
			}
		}
	}
}

