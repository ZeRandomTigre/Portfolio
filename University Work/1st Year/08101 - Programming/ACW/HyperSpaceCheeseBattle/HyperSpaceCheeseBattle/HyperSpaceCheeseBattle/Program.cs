using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HyperSpaceCheeseBattle
{
    class Program
    {
        struct Player
        {
            public string Name;
            public int X;
            public int Y;
        }

        static int noOfPlayers;
        const int NUMBER_OF_PLAYERS = 4;

        static Player[] players = new Player[NUMBER_OF_PLAYERS];        
        static Random diceRandom = new Random();

        static bool gameOver = false;
        static bool testMode = false; // allows user to input dice value
        
        /*
        Game board array
        0 = up
        1 = right
        2 = down
        3 = left
        4 = end
        5 = cheese up
        6 = cheese right
        7 = cheese down
        8 = cheese left
        */

        static int[,] board = new int[,] 
        { 
            {0,0,0,0,0,0,0,0}, // row 0 
            {1,1,0,2,5,0,3,3}, // row 1 
            {1,1,0,3,3,3,3,3}, // row 2 
            {6,1,0,1,0,0,3,3}, // row 3 
            {1,1,1,1,0,0,8,3}, // row 4 
            {1,1,1,6,2,2,3,3}, // row 5 
            {1,1,0,2,0,3,3,3}, // row 6
            {2,1,1,1,1,1,3,4}  // row 7
        };
        
        /// <summary>
        /// Gets number of players & names - resets the player position
        /// </summary>
        /// <param name="min">min players</param>
        /// <param name="max">max players</param>
        static void ResetGame(int min, int max)
        {
            Console.Write("Would you like to enable test mode? (type 'Y' for yes and 'N' for no) ");
            string testModeString = Console.ReadLine();
            if (testModeString == "Y")
                testMode = true;

            string noOfPlayersString;            
            do
            {
                Console.Write("How many players are taking part? (enter a value between " + min + " and " + max + "): ");
                noOfPlayersString = Console.ReadLine();
                try
                {
                    noOfPlayers = int.Parse(noOfPlayersString);
                }
                catch
                {
                }
                if ((noOfPlayers > max) || (noOfPlayers < min))
                    Console.WriteLine("Invalid value - Please enter a number in the range " + min + " to " + max);
                else
                    break;
            }
            while (true);

            for (int i = 0; i < noOfPlayers; i = i + 1)
            {
                do
                {
                    bool resetLoop = false;
                    int playerNo = i + 1;
                    Console.Write("Please enter the name of player " + playerNo + ": ");
                    players[i].Name = Console.ReadLine();
                    for (int j = 0; j < noOfPlayers; j = j + 1)
                    {
                        if (i == j)
                        {
                        }
                        else
                            if ((players[i].Name == players[j].Name) || (string.IsNullOrEmpty(players[i].Name))) // checks to make there is not another player with the same name and that the name is not empty
                            {
                                Console.WriteLine("Invalid name - please enter a name that has not been taken or that is not empty");
                                resetLoop = true;
                                break;
                            }

                    }
                    if (resetLoop == false)
                        break;
                    players[i].X = 0;
                    players[i].Y = 0;
                }
                while (true);
            }
        }

        /// <summary>
        /// Randomly gets a dice value between 1 - 6
        /// 
        /// if testmode = true
        ///     then asks the user to input a dice value
        /// </summary>
        /// <param name="playerNo"></param>
        /// <returns>returns the dice value</returns>
        static int RandomDiceThrow(int playerNo)
        {
            int spots;
            if (testMode == true) // allows user to input dice value               
            {
                do
                {                    
                    Console.WriteLine();
                    Console.Write("Enter the dice value : ");
                    string spotsString = Console.ReadLine();
                    try
                    {
                        spots = int.Parse(spotsString);
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid Value");
                    }
                }
                while (true);
            }
            else // random dice roll
            {
                spots = diceRandom.Next(1, 7);
                Console.WriteLine();
                Thread.Sleep(300);
                Console.WriteLine(players[playerNo].Name + " throws the dice...");
                Thread.Sleep(1000);
                Console.Write("The Dice lands on");
                Thread.Sleep(300);
                Console.Write(".");
                Thread.Sleep(300);
                Console.Write(".");
                Thread.Sleep(300);
                Console.Write(". ");
                Thread.Sleep(300);
                Console.WriteLine(spots);
                Thread.Sleep(300);
            }
            return spots;
        }
        
        /// <summary>
        /// moves the player along the board
        /// </summary>
        /// <param name="playerNo">current player</param>
        static void PlayerTurn(int playerNo)
        {
            bool noCheeseAgain = false;

            do
            {
                int movementDir = board[players[playerNo].Y, players[playerNo].X]; // get movement direction from board array
                int movementNo = RandomDiceThrow(playerNo); // get movement number from dice throw method               
                
                switch (movementDir)
                {
                    case 0: // UP
                        players[playerNo].Y = players[playerNo].Y + movementNo;
                        break;
                    case 1: // RIGHT
                        players[playerNo].X = players[playerNo].X + movementNo;
                        break;
                    case 2: // DOWN
                        players[playerNo].Y = players[playerNo].Y - movementNo;
                        break;
                    case 3: // LEFT
                        players[playerNo].X = players[playerNo].X - movementNo;
                        break;
                    case 4: // WIN
                        break;
                    case 5: // CHEESE UP
                        players[playerNo].X = players[playerNo].X + movementNo;
                        break;
                    case 6: // CHEESE RIGHT
                        players[playerNo].X = players[playerNo].X + movementNo;
                        break;
                    case 7: // CHEESE DOWN
                        players[playerNo].X = players[playerNo].X - movementNo;
                        break;
                    case 8: // CHEESE LEFT
                        players[playerNo].X = players[playerNo].X - movementNo;
                        break;
                    default:
                        break;
                }

                if ((players[playerNo].Y > 7) || (players[playerNo].Y < 0) && (movementDir == 0)) // Check if player is off the board in UP direction
                {
                    players[playerNo].Y = players[playerNo].Y - movementNo;
                    Console.WriteLine("You are about to move off the board");
                    Console.WriteLine("you have been returned to your original position");
                    noCheeseAgain = true;
                }

                if ((players[playerNo].X > 7) || (players[playerNo].X < 0) && (movementDir == 1)) // Check if player is off the board in RIGHT direction
                {
                    players[playerNo].X = players[playerNo].X - movementNo;                    
                    Console.WriteLine("You are about to move off the board");
                    Console.WriteLine("you have been returned to your original position");
                    noCheeseAgain = true;
                }

                if ((players[playerNo].Y > 7) || (players[playerNo].Y < 0) && (movementDir == 2)) // Check if player is off the board in DOWN direction
                {
                    players[playerNo].Y = players[playerNo].Y + movementNo;
                    Console.WriteLine("You are about to move off the board");
                    Console.WriteLine("you have been returned to your original position");
                    noCheeseAgain = true;
                }

                if ((players[playerNo].X > 7) || (players[playerNo].X < 0) && (movementDir == 3)) // Check if player is off the board in LEFT direction
                {
                    players[playerNo].X = players[playerNo].X + movementNo;
                    Console.WriteLine("You are about to move off the board");
                    Console.WriteLine("you have been returned to your original position");
                    noCheeseAgain = true;
                }

                Console.WriteLine(players[playerNo].Name + " has thrown a " + movementNo + " and lands on " + "(" + players[playerNo].X + "," + players[playerNo].Y + ")");
                Thread.Sleep(1000);
                                 
            } while (RocketInSquare(playerNo, players[playerNo].X, players[playerNo].Y) == true); // checks for rocket collision
            
            // Checks movement direction of square rocket has moved to

            int currentMovementDir = board[players[playerNo].Y, players[playerNo].X]; // grabs movement direction of player AFTER movement from board array
            if (noCheeseAgain == false) 
                // Makes sure that cheese power is not absorbed again after 
                //player has been returned to same posistion after moving out side bounds of array
            {
                switch (currentMovementDir)
                {
                    case 4: // WIN
                        gameOver = true;
                        break;
                    case 5: // CHEESE UP
                        CheesePower(playerNo);
                        break;
                    case 6: // CHEESE RIGHT
                        CheesePower(playerNo);
                        break;
                    case 7: // CHEESE DOWN
                        CheesePower(playerNo);
                        break;
                    case 8: // CHEESE LEFT
                        CheesePower(playerNo);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// go through each player and does the PlayerTurn method
        /// 
        /// when all players turns are over prints the status report
        /// 
        /// if a player has won
        ///     then breaks the loop
        /// </summary>
        static void MakeMoves()
        {
            int i;
            for (i = 0; i < noOfPlayers; i = i + 1)
            {
                Console.Clear();
                DrawBoard();
                Console.WriteLine("It's " + players[i].Name + "'s turn...");
                PlayerTurn(i);
                Thread.Sleep(1000);
                if (gameOver == true)
                {
                    Console.Clear();
                    Console.WriteLine(players[i].Name + " has won!");
                    break;
                }
                if (i == noOfPlayers - 1) // shows status report when all players have had thier turns
                    ShowStatus();
                Console.WriteLine("Press return for next turn");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Go through all players to check if another player occupies the current players square
        /// </summary>
        /// <param name="currentPlayer">current player</param>
        /// <param name="X">X coord</param>
        /// <param name="Y">Y coord</param>
        /// <returns>returns true if another player is in current players square</returns>
        static bool RocketInSquare(int currentPlayer, int X, int Y)
        {
            bool returnTrue;
            returnTrue = false;
            for (int i = 0; i < noOfPlayers; i = i + 1)
            {
                if (i == currentPlayer) // ignores the current player
                    break;
                if ((players[i].X == X) && (players[i].Y == Y))
                {
                    Console.WriteLine("There is another rocket in this tile - rolling the dice again");
                    returnTrue = true;
                    break;
                }
            }
            if (returnTrue == true)
                return true;
            else
                return false;
        }

        /// <summary>
        /// prints a status report
        /// showing player current position
        /// </summary>
        static void ShowStatus()
        {
            Console.Clear();            
            DrawBoard();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Hyperspace Cheese Battle Space Report");
            Console.WriteLine();
            Console.WriteLine("=====================================");
            Console.WriteLine();
            Console.WriteLine("There are currently " + noOfPlayers + " playing");
            for (int i = 0; i < noOfPlayers; i = i + 1)
            {
                int playerNo = i + 1;
                Console.WriteLine(players[i].Name + " (P" + playerNo + ")" + " is on square " + "(" + players[i].X + "," + players[i].Y + ")");                
            }
        }
        
        /// <summary>
        /// Draws the board array showing the arrow directions, players locations and cheese locations
        /// </summary>
        static void DrawBoard()
        {
            int Row;
            int Column;

            for (Column = 7; Column > -1; Column = Column - 1)
            {                
                Console.WriteLine();
                Console.Write(Column + " "); // prints Y coordinates
                for (Row = 0; Row < 8; Row = Row + 1)
                {
                    string P = "  "; 
                    //Displaying player's current location 
                    // replaces space with P1, P2 e.t.c when player occupies the space
                    for (int i = 0; i < noOfPlayers; i = i + 1)
                    {
                        if (players[i].X == Row && players[i].Y == Column)
                        {
                            P = ("P" + (i = i + 1));
                            break;
                        }
                    }

                    int Plot = board[Column, Row];
                    switch (Plot)
                    {
                        case 0:
                            Console.Write(" ↑ " + P);
                            break;
                        case 1:
                            Console.Write(" → " + P);
                            break;
                        case 2:
                            Console.Write(" ↓ " + P);
                            break;
                        case 3:
                            Console.Write(" ← " + P);
                            break;
                        case 4:
                            Console.Write(" WIN ");
                            break;
                        case 5:
                            Console.Write(" ↑C" + P);
                            break;
                        case 6:
                            Console.Write(" →C" + P);
                            break;
                        case 7:
                            Console.Write(" ↓C" + P);
                            break;
                        case 8:
                            Console.Write(" ←C" + P);
                            break;
                    }                
                }   
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("   0    1    2    3    4    5    6    7  "); // prints X coordinates
            Console.WriteLine();
        }
        

        /// <summary>
        /// User picks whether to use cheese power for deathray or refuel
        /// </summary>
        /// <param name="playerNo">current player</param>
        static void CheesePower(int playerNo)
        {
            Console.WriteLine();
            Console.WriteLine(players[playerNo].Name + " has landed on a cheese square!");
            Console.WriteLine(players[playerNo].Name + " has absorbed the cheese power what would you like to do with it?");
            Console.Write("(type 'D' for cheese deathray or type 'R' for rocket fuel) ");
            do
            {
                bool invalidEntry = false;
                string cheesePowerString = Console.ReadLine(); 
                switch (cheesePowerString)
                {
                    case "R":
                        CheeseRocketFuel(playerNo);
                        break;
                    case "D":
                        CheeseDeathray(playerNo);
                        break;
                    default :
                        invalidEntry = true;
                        break;
                }
                if (invalidEntry == true)
                    Console.WriteLine("Invalid entry - please type 'D' for cheese deathray or type 'R' for rocket fuel");
                else
                    break;
            } 
            while (true);
        }

        /// <summary>
        /// Re-rolls dice for current player
        /// </summary>
        /// <param name="playerNo">current player</param>
        static void CheeseRocketFuel(int playerNo)
        {
            Console.WriteLine();
            Console.WriteLine(players[playerNo].Name + " has chosen to refuel their engines!");
            PlayerTurn(playerNo);
        }

        /// <summary>
        /// Current player uses cheese deathray
        /// when used on another player
        ///     that player picks x coord to land on
        /// </summary>
        /// <param name="playerNo">current player</param>
        static void CheeseDeathray(int playerNo)
        {
            Console.WriteLine();
            Console.WriteLine(players[playerNo].Name + " has chosen to use the cheese deathray!");
            Console.Write("Name your target: ");
            Console.WriteLine();

            for (int i = 0; i < noOfPlayers; i = i + 1)
            {
                if (i == playerNo)
                {
                }
                else
                    Console.WriteLine("type '" + (i + 1) + "' for " + players[i].Name); // prints out targets names - except player who is using cheese deathray
            }

            string userTargetString;
            int userTarget = 0;

            do
            {
                userTargetString = Console.ReadLine();
                try
                {
                    userTarget = int.Parse(userTargetString);
                }
                catch
                {
                }
                if ((userTarget > noOfPlayers) || (userTarget < 1) || (userTarget == (playerNo + 1))) // cannot target player that does not exisit or player that is using cheese deathray
                    Console.WriteLine("Invalid entry - please type the player number you wish to target");
                else
                    break;
            }
            while (true);

            userTarget = userTarget - 1;
            CheeseDeathrayAction(playerNo, userTarget);
        }

        /// <summary>
        /// Sets player targeted by deathray Y coordinate to 0
        /// and allows that player to choose their X coordinate
        /// </summary>
        /// <param name="playerNo">Player using the deathray</param>
        /// <param name="userTarget">Player being targeted by the deathray</param>
        static void CheeseDeathrayAction(int playerNo, int userTarget)
        {
            string xCoordString;
            int xCoord = -1;
            players[userTarget].Y = 0;
            Console.WriteLine(players[playerNo].Name + " used the cheese deathray on " + players[userTarget].Name + ".");
            Console.WriteLine(players[userTarget].Name + " engines have been exploded.");
            Console.WriteLine();
            Console.Write(players[userTarget].Name + ", type the X coordinate you would like to land on: ");
            do
            {
                xCoordString = Console.ReadLine();
                try
                {
                    xCoord = int.Parse(xCoordString);
                }
                catch
                {
                }
                if ((xCoord > 7) || (xCoord < 0))
                    Console.WriteLine("Invalid value - Please enter a number in the range 0 to 7");
                else
                    break;
            }
            while (true);
            players[userTarget].X = xCoord;
            Console.WriteLine(players[userTarget].Name + " is on square " + "(" + players[userTarget].X + "," + players[userTarget].Y + ")");
        }
       
        static void Main(string[] args)
        {
            string playAgain;

            do
            {
                Console.Clear();
                ResetGame(2,4);

                while (true)
                {
                    Console.Clear();
                    MakeMoves();
                    if (gameOver == true)
                        break;
                }
                Console.Write("Would you like to play again? (type 'Y' for yes and 'N' for no)");
                playAgain = Console.ReadLine();
            } while (playAgain == "Y");
        }
    }
}
