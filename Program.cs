using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConnectFour.CustomObjects;

/// <summary>
/// Connect Four console application!
/// 
/// This program allows a human player to play a game of Connect Four with an AI opponent.
/// 
/// The requirements:
/// Must be a console app
/// Must be written in C#
/// Must implement connect four rules
/// The grid must be seven columns and six rows
/// Must be able to play versus a computer opponent
/// Concentrate on writing quality code, don’t spend time creating a fancy UI.The computer opponent doesn't have to clever, just keep it very simple.
/// </summary>
/// <remarks>Created by Julian Willing on 25/08/2016</remarks>
namespace ConnectFour
{
    class Program
    {
        #region Methods

        /// <summary>
        /// The entry point for the Connect Four game.
        /// </summary>        
        static void Main()
        {
            try
            {
                List<Player> players = null;

                // the game is starting now, so write the game instructions to the console
                WriteGameInstructions();

                // create the players
                players = CreatePlayers();

                // start a new round of Connect Four
                StartNewRound(players);
            }
            catch (Exception ex)
            {
                // write the exception out to the console
                Console.WriteLine(ex.Message.ToString());
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Starts a new round of Connect Four.
        /// </summary>
        /// <param name="players">Players for this round.</param>
        /// <remarks>Call this function again to start a new round of Connect Four.</remarks>
        static void StartNewRound(List<Player> players)
        {
            try
            {
                Grid playingGrid = null;                
                bool isGameAlive = true;                

                // player to start the round is always the 1st player in the list
                Player currentPlayer = players[0];

                // create a new playing grid
                playingGrid = CreateNewPlayingGrid();

                Console.WriteLine("New round started! \n");          

                // write the playing grid out to the console
                WriteGrid(playingGrid);

                // loop while the game is still alive
                do
                {
                    bool isTurnOver = false;

                    // if the current player is human, perform all the tasks required to facilitate the human turn
                    if (currentPlayer.Type == CustomEnums.PlayerType.Human)
                    {
                        string userInput = string.Empty;
                        int requestedColumnNumber = 0;

                        // write the instructions for this turn
                        Console.WriteLine(string.Format("{0} - Enter a number between 1 and 7:", currentPlayer.Name));

                        // await the user input (e.g. which column to drop the disc into)
                        userInput = Console.ReadLine();

                        // try parsing the inputted string as a number
                        if (int.TryParse(userInput, out requestedColumnNumber))
                        {
                            // the user input was a numeric value, now check if it was between 1-7
                            if(requestedColumnNumber >= 1 & requestedColumnNumber <= 7)
                            {                                
                                // user input was a number between 1-7, now check if this column is accepting discs
                                if (playingGrid.TryInsertDisc((requestedColumnNumber - 1), currentPlayer.DiscMarker))
                                {    
                                    // set flag to indicate that this player's turn is over
                                    isTurnOver = true;                                    
                                }
                                else
                                {
                                    // no room in this column, ask player to re-choose
                                    Console.WriteLine("Column is full, choose another column.");
                                }
                            }
                            else
                            {
                                // column number supplied was out of range, issue round instructions
                                Console.WriteLine("Column supplied is invalid.");
                            }
                        }
                        else
                        {
                            // input value supplied was non-numeric, issue round instructions
                            Console.WriteLine("Column supplied is invalid.");
                        }
                    }
                    else
                    {
                        // player this turn is the AI opponent
                        // as per the specification allows, the AI is very dumb in this version of Connect Four, and will keep placing discs in the furthest left column that contains a free space
                        // therefore keep trying to insert a disc into each column from left-to-right until succesfully inserted, at which point the isTurnOver flag will be set to true
                        
                        // loop the columns from left-to-right
                        for (int iteratedColumn = 1; iteratedColumn <= Grid.ColumnCount; iteratedColumn++)
                        {
                            // try inserting a disc in the currently iterated column
                            if (playingGrid.TryInsertDisc((iteratedColumn - 1), currentPlayer.DiscMarker))
                            {
                                // disc succesfully placed by the AI, their turn is over
                                isTurnOver = true;

                                // write a message to the console to inform user that the AI has placed
                                Console.WriteLine(string.Format("{0} has placed their disc.", currentPlayer.Name));

                                // exit the for loop to save time
                                break;
                            }
                        }
                    }

                    // alternate the player for the next go if the current turn is over                    
                    if (isTurnOver)
                    {                  
                        // write the updated playing grid to the console
                        WriteGrid(playingGrid);

                        // we need to check if either player has won the game
                        if(playingGrid.DoesMarkerMeetWinCondition(currentPlayer.DiscMarker))
                        {
                            // current player has won the round!
                            // write game won message
                            Console.WriteLine(string.Format("Game won by {0}!", currentPlayer.Name));

                            // set game over flag
                            isGameAlive = false;
                        }
                        else
                        {
                            // nobody has won yet
                            // check if the current player whose turn is about to end is human
                            if (currentPlayer.Type == CustomEnums.PlayerType.Human)
                            {
                                // make the player press any key to invoke the AI's turn
                                Console.WriteLine("Press any key to place the AI disc...");
                                Console.ReadKey();
                            }
                            else
                            {
                                // the last go in a round will always be that of the AI opponent (as the AI always goes second and there is an even number of slots)
                                // is the playing grid now full?
                                if (playingGrid.IsGridFull())
                                {
                                    Console.WriteLine("The game ends in a draw as the board is full.");

                                    // so we need to set a flag to say that the game is no longer alive because there are no more empty slots on the grid
                                    isGameAlive = false;

                                }
                            }

                            // we know that there is 1 human and 1 AI player, so just choose the other type to the current player        
                            currentPlayer = players.First(x => x.Type != currentPlayer.Type);
                        }
                        
                    }                    

                }
                // loop again if the game is still alive
                while (isGameAlive);

                // write a message to the console that the game is over
                Console.WriteLine("Game over. Type '-r' to start a new round, or press any other key to close this application... \n");

                // awaits user input, if the input is "-r" then restart a new round, else just exit this method
                if (Console.ReadLine() == "-r")
                {
                    StartNewRound(players);
                }                           
            }
            catch(Exception ex)
            {
                // throw exception up the chain
                throw ex;
            }
        }

        /// <summary>
        /// Writes the instructions for the Connect Four game to the console.
        /// </summary>
        static void WriteGameInstructions()
        {
            try
            {
                Console.WriteLine("************************************************************************************************");
                Console.WriteLine("Welcome to Connect Four! \n");
                Console.WriteLine("You will be facing an AI opponenet.");
                Console.WriteLine("Human discs will marked with \"H\".");
                Console.WriteLine("AI discs will marked with \"A\".");
                Console.WriteLine("Drop a disc into a column by entering a number between 1 and 7, then press the return key.");
                Console.WriteLine("************************************************************************************************ \n");
            }
            catch (Exception ex)
            {
                // throw exception up the chain
                throw ex;
            }
        }

        /// <summary>
        /// Creates the Connect Four players. 
        /// </summary>
        /// <returns>Returns 1 x human player and 1 x AI player.</returns>
        static List<Player> CreatePlayers()
        {
            try
            {
                // create return object - list of players
                List<Player> players = new List<Player>();

                // create 1 human player, and 1 AI player
                players.Add(new Player("Player One", CustomEnums.PlayerType.Human, Convert.ToChar("H")));
                players.Add(new Player("Player Two", CustomEnums.PlayerType.AI, Convert.ToChar("A")));

                // return the created players
                return players;
            }
            catch (Exception ex)
            {
                // throw exception up the chain
                throw ex;
            }
        }

        /// <summary>
        /// Creates a new Connect Four playing grid.
        /// </summary>
        /// <returns>Returns a new playing grid.</returns>
        static Grid CreateNewPlayingGrid()
        {
            try
            {
                // define the grid that we'll return
                Grid playingGrid = new Grid();

                // define the slots for the grid
                playingGrid.Slots = new char[Grid.RowCount, Grid.ColumnCount];

                // return the created grid
                return playingGrid;
            }
            catch (Exception ex)
            {
                // throw exception up the chain
                throw ex;
            }
        }

        /// <summary>
        /// Writes the playing grid to the console.
        /// </summary>
        /// <param name="playingGrid">The playing grid to write to the console.</param>
        static void WriteGrid(Grid playingGrid)
        {
            try
            {
                // leave an empty line above the grid that we're writing to the console
                Console.WriteLine("");

                // iterate the row dimension in the slots
                for(int iteratedRow = 0; iteratedRow < Grid.RowCount; iteratedRow++)
                {
                    // draw the left grid boundary for the row
                    Console.Write("|");

                    // iterate the column dimension in the slots
                    for(int iteratedColumn = 0; iteratedColumn < Grid.ColumnCount; iteratedColumn++)
                    {
                        
                        // assess the currently iterated slot to determine if it is empty
                        if(playingGrid.Slots[iteratedRow, iteratedColumn] == new char())
                        {
                            // this slot has not been populated with a player's disc yet
                            // so write a value to the slot in the console which represents emptyness
                            Console.Write("-");
                        }
                        else
                        {
                            // the slot is populated with a player disc
                            // write the character within the slot to the slot display
                            Console.Write(playingGrid.Slots[iteratedRow, iteratedColumn]);
                        }
                    }

                    // draw the right grid boundary for the row
                    Console.WriteLine("|");
                }                
            }
            catch (Exception ex)
            {
                // write the exception out to the console
                throw ex;
            }            

            #endregion
        }
    }
}
