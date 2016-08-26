using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.CustomObjects
{

    /// <summary>
    /// Represents the grid in which the game of Connect Four is played.
    /// </summary>
    class Grid
    {

        #region Properties

        /// <summary>
        /// The slots belonging to the grid.
        /// </summary>
        public char[,] Slots { get; set; }

        #endregion

        #region Constants

        /// <summary>
        /// The number of rows the grid has.
        /// </summary>
        public const int RowCount = 6;

        /// <summary>
        /// The number of columns the grid has.
        /// </summary>
        public const int ColumnCount = 7;

        #endregion

        #region Constructors

        /// <summary>
        /// Basic constructor.
        /// </summary>
        public Grid()
        {
            this.Slots = new char[RowCount, ColumnCount];
        }

        #endregion

        #region Methods        

        /// <summary>
        /// Tries to inserts a disc into the column supplied.
        /// </summary>
        /// <param name="columnIndex">The column in which to try the disc insert.</param>
        /// <param name="diskMarker">The marker that will be shown to represent the disc within the grid.</param>
        /// <returns>Returns true if a disc was inserted, or false if there was no room to insert the disc in the column supplied.</returns>
        public bool TryInsertDisc(int columnIndex, char diskMarker)
        {
            try
            {
                bool isInserted = false;

                // call private method to try and find an empty row in the column supplied
                int emptyColumnRowIndex = FindColumnEmptyRowIndex(columnIndex);

                // if this column has an empty slot
                if (emptyColumnRowIndex > -1)
                {
                    // assign the disk marker to the empty column row
                    this.Slots[emptyColumnRowIndex, columnIndex] = diskMarker;

                    // set flag to denote that the disc has been inserted
                    isInserted = true;
                }                

                // return success status of disc insert
                return isInserted;
            }
            catch(Exception ex)
            {
                // throw exception up the chain
                throw ex;
            }
        }

        /// <summary>
        /// Determines if the grid is full.
        /// </summary>
        /// <returns>Returns true if the grid is full, or false if it has space.</returns>
        public bool IsGridFull()
        {
            try
            {
                bool isFull = true;

                // iterate every slot in the top row of the columns from left-to-right, looking for an empty slot
                // no need to check slots not in the top row
                for (int iteratedColumnIndex = 0; iteratedColumnIndex <= (ColumnCount - 1); iteratedColumnIndex++)
                {
                    // is this slot empty?
                    if (this.Slots[0, iteratedColumnIndex] == new char())
                    {
                        // the grid cannot be full because an empty slot was found
                        isFull = false;

                        // exit the foor loop
                        break;
                    }
                }

                // return the true or false verdict as to whether the grid is full
                return isFull;
            }
            catch(Exception ex)
            {
                // throw exception up the chain
                throw ex;
            }
        }

        /// <summary>
        /// Checks if the supplied disc marker has connected to other discs of the same type, the required amount of times.
        /// </summary>
        /// <param name="discMarker">The disc marker to check.</param>
        /// <returns>Returns true if a win condition has been met, or false if no win conditions met.</returns>
        /// <remarks>Checks the horizontal, vertical, and diagonal directions for connected discs.</remarks>
        public bool DoesMarkerMeetWinCondition(char discMarker)
        {
            try
            {                
                bool isConditionMet = false;

                // iterate each row in the grid
                for (int iteratedRowIndex = 5; iteratedRowIndex >= 1; --iteratedRowIndex)
                {
                    // only continue on to iterate the grid columns if the win condition hasn't already been mets
                    if (!isConditionMet)
                    {
                        // iterate the grid columns
                        for (int iteratedColumnIndex = 6; iteratedColumnIndex >= 1; --iteratedColumnIndex)
                        {
                            // check to prevent out of array bounds exception
                            if (iteratedColumnIndex >= 3)
                            {
                                // check for a horizontal win left (<--)
                                if (this.Slots[iteratedRowIndex, iteratedColumnIndex] == discMarker &&
                                    this.Slots[iteratedRowIndex, iteratedColumnIndex - 1] == discMarker &&
                                    this.Slots[iteratedRowIndex, iteratedColumnIndex - 2] == discMarker &&
                                    this.Slots[iteratedRowIndex, iteratedColumnIndex - 3] == discMarker)
                                {
                                    // win condition met, break out of inner for loop
                                    isConditionMet = true;
                                    break;
                                }
                            }

                            // check to prevent out of array bounds exception
                            if (iteratedColumnIndex <= (ColumnCount - 4))
                            {
                                // check for a horzontal win right (-->)
                                if (this.Slots[iteratedRowIndex, iteratedColumnIndex] == discMarker &&
                                        this.Slots[iteratedRowIndex, iteratedColumnIndex + 1] == discMarker &&
                                        this.Slots[iteratedRowIndex, iteratedColumnIndex + 2] == discMarker &&
                                        this.Slots[iteratedRowIndex, iteratedColumnIndex + 3] == discMarker)
                                {
                                    // win condition met, break out of inner for loop
                                    isConditionMet = true;
                                    break;
                                }
                            }

                            // check to prevent out of array bounds exception
                            if (iteratedRowIndex >= 3)
                            {
                                // check for a vertical win
                                if (this.Slots[iteratedRowIndex, iteratedColumnIndex] == discMarker &&
                                    this.Slots[iteratedRowIndex - 1, iteratedColumnIndex] == discMarker &&
                                    this.Slots[iteratedRowIndex - 2, iteratedColumnIndex] == discMarker &&
                                    this.Slots[iteratedRowIndex - 3, iteratedColumnIndex] == discMarker)
                                {
                                    // win condition met, break out of inner for loop
                                    isConditionMet = true;
                                    break;
                                }
                            }

                            // check to prevent out of array bounds exception
                            if (iteratedRowIndex >= 3 & iteratedColumnIndex >= 3)
                            {
                                // check for a diagonal win (\)
                                if (this.Slots[iteratedRowIndex, iteratedColumnIndex] == discMarker &&
                                this.Slots[iteratedRowIndex - 1, iteratedColumnIndex - 1] == discMarker &&
                                this.Slots[iteratedRowIndex - 2, iteratedColumnIndex - 2] == discMarker &&
                                this.Slots[iteratedRowIndex - 3, iteratedColumnIndex - 3] == discMarker)
                                {
                                    // win condition met, break out of inner for loop
                                    isConditionMet = true;
                                    break;
                                }
                            }

                            // check to prevent out of array bounds exception
                            if (iteratedRowIndex <= (RowCount - 4) & iteratedColumnIndex >= 3)
                            {
                                // check for a diagonal win (/)
                                if (this.Slots[iteratedRowIndex, iteratedColumnIndex] == discMarker &&
                                this.Slots[iteratedRowIndex + 1, iteratedColumnIndex - 1] == discMarker &&
                                this.Slots[iteratedRowIndex + 2, iteratedColumnIndex - 2] == discMarker &&
                                this.Slots[iteratedRowIndex + 3, iteratedColumnIndex - 3] == discMarker)
                                {
                                    // win condition met, break out of inner for loop
                                    isConditionMet = true;
                                    break;
                                }
                            }

                        }
                    }
                    else
                    {
                        break;
                    }
                }

                // return condition success status
                return isConditionMet;
            }
            catch (Exception ex)
            {
                // throw exception up the chain
                throw ex;
            }
        }

        /// <summary>
        /// Finds an empty row index within the column supplied.
        /// </summary>
        /// <param name="columnIndex">The 0-based index of the column to check.</param>
        /// <returns>If the column supplied is at capacity then -1 will be returned. If the column supplied has space to receive a disc,
        /// then the row index of the empty slot will be returned.</returns>
        private int FindColumnEmptyRowIndex(int columnIndex)
        {
            try
            {
                // the empty column row index should be set to -1 until a free row space is found
                int emptyColumnRowIndex = -1;

                // iterate every row in the column, starting with the bottom one
                for (int iteratedRowIndex = (RowCount - 1); iteratedRowIndex >= 0; iteratedRowIndex--)
                {
                    // if this row does not contain a disc already
                    if (this.Slots[iteratedRowIndex, columnIndex] == new char())
                    {
                        // this row index has capacity to receive a disc so assign the row index to the return variable
                        emptyColumnRowIndex = iteratedRowIndex;

                        // now exit the for loop to save time
                        break;
                    }
                }

                // this be returning either -1 (no capacity) or row index
                return emptyColumnRowIndex;
            }
            catch (Exception ex)
            {
                // throw exception up the chain
                throw ex;
            }
        }

        #endregion
    }
}
