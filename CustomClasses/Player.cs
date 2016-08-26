using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConnectFour.CustomEnums;

namespace ConnectFour.CustomObjects
{

    /// <summary>
    /// Represents a Player of Connect Four.
    /// </summary>
    class Player
    {

        #region Properties

        public string Name{ get; set; }

        public PlayerType Type { get; set; }

        public char DiscMarker { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Basic constructor.
        /// </summary>
        public Player()
        {
            this.Name = string.Empty;
            this.Type = new PlayerType();
            this.DiscMarker = new char();
        }

        /// <summary>
        /// Fully populated constructor.
        /// </summary>
        /// <param name="name">The name to identify the player.</param>
        /// <param name="playerType">The player type.</param>
        /// <param name="diskMarker">The marker character to apply to the disc so that the disc is identifiable.</param>
        public Player(string name, PlayerType playerType, char discMarker): this()
        {
            this.Name = name;
            this.Type = playerType;
            this.DiscMarker = discMarker;
        }

        #endregion
    }
}
