using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackWPF.Model
{
    public class Card
    {
        #region Static members definition
        public enum POSITIONS
        {
            Blank,
            Unknown,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King,
            Ace,
        };

        public enum SUITS
        {
            Blank,
            Unknown,
            Clubs,
            Spades,
            Hearts,
            Diamonds,
        }
        #endregion

        #region Members definition
        private POSITIONS pos;
        public POSITIONS Pos
        {
            get { return pos; }
        }

        private SUITS suit;
        public SUITS Suit
        {
            get { return suit; }
        }
        #endregion

        #region Constructors definition
        public Card(POSITIONS pos, SUITS suit)
        {
            this.pos = pos;
            this.suit = suit;
        }
        #endregion

        #region Methods definition
        public string GetName()
        {
            if (this.Pos == POSITIONS.Blank || this.Suit == SUITS.Blank)
            {
                return "Blank";
            }
            else if (this.Pos == POSITIONS.Unknown || this.Suit == SUITS.Unknown)
            {
                return "Unknown";
            }

            return (this.Pos.ToString() + " of " + this.Suit.ToString());
        }
        #endregion
    }
}
