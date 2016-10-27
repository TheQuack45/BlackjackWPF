using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackWPF.ViewModel
{
    public class VMCard
    {
        #region Static members definition
        private static readonly Dictionary<Model.Card.POSITIONS, string> POSITIONS_DICTIONARY = new Dictionary<Model.Card.POSITIONS, string>()
        {
            { Model.Card.POSITIONS.Ace, "ace" },
            { Model.Card.POSITIONS.Two, "2" },
            { Model.Card.POSITIONS.Three, "3" },
            { Model.Card.POSITIONS.Four, "4" },
            { Model.Card.POSITIONS.Five, "5" },
            { Model.Card.POSITIONS.Six, "6" },
            { Model.Card.POSITIONS.Seven, "7" },
            { Model.Card.POSITIONS.Eight, "8" },
            { Model.Card.POSITIONS.Nine, "9" },
            { Model.Card.POSITIONS.Ten, "10" },
            { Model.Card.POSITIONS.Jack, "jack" },
            { Model.Card.POSITIONS.Queen, "queen" },
            { Model.Card.POSITIONS.King, "king" },
        };

        private static readonly Dictionary<Model.Card.SUITS, string> SUITS_DICTIONARY = new Dictionary<Model.Card.SUITS, string>()
        {
            { Model.Card.SUITS.Clubs, "clubs" },
            { Model.Card.SUITS.Diamonds, "diamonds" },
            { Model.Card.SUITS.Hearts, "hearts" },
            { Model.Card.SUITS.Spades, "spades" },
        };
        
        private const string BLANK_CARD_PATH = "blank.png";
        private const string BACKSIDE_CARD_PATH = "backside.png";
        private const string EXTENSION = ".png";
        private const string POS_SUIT_SPLIT = "_of_";
        #endregion

        #region Members definition
        private Uri imageSource;
        public Uri ImageSource
        {
            get { return imageSource; }
            set { imageSource = value; }
        }

        public string CardName
        {
            get
            {
                if (Position == Model.Card.POSITIONS.Blank || Suit == Model.Card.SUITS.Blank || Position == Model.Card.POSITIONS.Unknown || Suit == Model.Card.SUITS.Unknown)
                {
                    return null;
                }
                return (Position.ToString() + " of " + Suit.ToString());
            }
        }

        private Model.Card.POSITIONS position;
        public Model.Card.POSITIONS Position
        {
            get { return position; }
        }

        private Model.Card.SUITS suit;
        public Model.Card.SUITS Suit
        {
            get { return suit; }
        }
        #endregion

        #region Constructors definition
        /// <summary>
        /// Creates a VMCard instance with the default, blank image.
        /// </summary>
        public VMCard()
        {
            this.ImageSource = new Uri(Environment.CurrentDirectory + Properties.Settings.Default.cardImagesDirectory + BLANK_CARD_PATH);
            this.position = Model.Card.POSITIONS.Blank;
            this.suit = Model.Card.SUITS.Blank;
        }

        public VMCard(Model.Card.POSITIONS position, Model.Card.SUITS suit)
        {
            if (position == Model.Card.POSITIONS.Blank || suit == Model.Card.SUITS.Blank)
            {
                this.ImageSource = new Uri(Environment.CurrentDirectory + Properties.Settings.Default.cardImagesDirectory + BLANK_CARD_PATH);
            }
            else if (position == Model.Card.POSITIONS.Unknown || suit == Model.Card.SUITS.Unknown)
            {
                this.ImageSource = new Uri(Environment.CurrentDirectory + Properties.Settings.Default.cardImagesDirectory + BACKSIDE_CARD_PATH);
            }
            else
            {
                this.ImageSource = new Uri(Environment.CurrentDirectory + Properties.Settings.Default.cardImagesDirectory + POSITIONS_DICTIONARY[position] + POS_SUIT_SPLIT + SUITS_DICTIONARY[suit] + EXTENSION);
            }
            this.position = position;
            this.suit = suit;
        }
        #endregion
    }
}
