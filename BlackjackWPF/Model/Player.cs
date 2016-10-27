using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackWPF.Model
{
    public abstract class Player
    {
        #region Members definition
        protected Card[] currentHand;
        public Card[] CurrentHand
        {
            get { return currentHand; }
        }

        private int wins;
        public int Wins
        {
            get { return wins; }
            set { wins = value; }
        }

        private int ties;
        public int Ties
        {
            get { return ties; }
            set { ties = value; }
        }

        private int losses;
        public int Losses
        {
            get { return losses; }
            set { losses = value; }
        }

        private bool isBusted;
        public bool IsBusted
        {
            get { return isBusted; }
            set { isBusted = value; }
        }

        public delegate void HandChangedEventHandler(object sender, HandChangedEventArgs args);
        public event HandChangedEventHandler HandChanged;
        #endregion

        #region Methods definition
        /// <summary>
        /// Add the given Card to this Player's hand. Throws HandFullException if there is no space in the hand.
        /// </summary>
        /// <param name="cCard">The Card to add to the Player's hand.</param>
        public void AddCardToHand(Card cCard)
        {
            if (cCard == null)
            {
                throw new ArgumentNullException(nameof(cCard), "cCard argument cannot be null.");
            }

            int blanksCount = (from Card checkCard in this.CurrentHand
                               where checkCard.Pos == Card.POSITIONS.Blank ||
                               checkCard.Suit == Card.SUITS.Blank
                               select checkCard).Count();

            if (blanksCount >= 1 || !(this.CurrentHand.Count() == this.CurrentHand.Length - blanksCount))
            {
                if (cCard.Pos == Card.POSITIONS.Blank || cCard.Suit == Card.SUITS.Blank || !this.CurrentHand.Contains<Card>(cCard))
                {
                    for (int i = 0; i < this.CurrentHand.Length; i++)
                    {
                        if (this.CurrentHand[i].Pos == Card.POSITIONS.Blank || this.CurrentHand[i].Suit == Card.SUITS.Unknown)
                        {
                            this.CurrentHand[i] = cCard;
                            // TODO: Check if this sorting is an issue
                            //this.currentHand = BlackjackGame.SortByPosition(this.CurrentHand);
                            OnHandChanged(new HandChangedEventArgs(i, cCard));
                            return;
                        }
                    }
                }
                else
                {
                    throw new HandFullException("The given Card is already in this object's hand.");
                }
            }
            else
            {
                throw new HandFullException("This object's hand is full.");
            }
        }

        /// <summary>
        /// Gets the total value of this object's hand.
        /// </summary>
        /// <returns>Integer of the summed total value</returns>
        public int GetHandTotal()
        {
            // TODO: If this method counts an earlier Ace as an 11, but a later Ace causes it to go over 21, it will not back up and change the first Ace to a 1.
            //       Make it do this.
            int total = 0;
            foreach (Card cCard in BlackjackGame.SortByPosition(this.CurrentHand))
            {
                if (cCard.Pos == Card.POSITIONS.Ace)
                {
                    // Current card is an ace, so it can be 11 or 1 depending on the hand's current value.
                    if ((total + 11) > 21)
                    {
                        // Counting the ace as 11 would go over the limit, so we will count it as 1.
                        total += 1;
                    }
                    else
                    {
                        // Counting the ace as 11 would not go over the limit, so we will count it as 11.
                        total += 11;
                    }
                }
                else if (cCard.Pos == Card.POSITIONS.Blank || cCard.Pos == Card.POSITIONS.Unknown || cCard.Suit == Card.SUITS.Blank || cCard.Suit == Card.SUITS.Unknown)
                {
                    total += 0;
                }
                else
                {
                    total += BlackjackGame.CardRanks[cCard.Pos];
                }
            }

            return total;
        }

        /// <summary>
        /// Adds one to this object's win count. ('Wins')
        /// </summary>
        public void AddWin()
        {
            this.Wins++;
        }

        /// <summary>
        /// Adds one to this object's loss count. ('Losses')
        /// </summary>
        public void AddLoss()
        {
            this.Losses++;
        }

        /// <summary>
        /// Returns this object's current hand and resets the member. Intended for returning the hand to a Game's Deck.
        /// </summary>
        /// <returns>List&lt;Card&gt; of this object's hand prior to reset.</returns>
        public List<Card> ReturnCards()
        {
            List<Card> initialList = this.CurrentHand.ToList<Card>();
            List<Card> returnList = new List<Card>();
            foreach (Card c in initialList)
            {
                if (!(c.Pos == Card.POSITIONS.Unknown || c.Pos == Card.POSITIONS.Blank || c.Suit == Card.SUITS.Unknown || c.Suit == Card.SUITS.Blank))
                {
                    returnList.Add(c);
                }
            }
            this.BlankHand();

            return returnList;
        }
        
        /// <summary>
        /// Set each Card in this object's hand to a blank card.
        /// </summary>
        public void BlankHand()
        {
            for (int i = 0; i < this.CurrentHand.Length; i++)
            {
                Card blankCard = new Model.Card(Card.POSITIONS.Blank, Card.SUITS.Blank);
                this.CurrentHand[i] = blankCard;
                OnHandChanged(new HandChangedEventArgs(i, blankCard));
            }
        }

        /// <summary>
        /// Method for HandChanged event triggering.
        /// </summary>
        /// <param name="args">HandChangedEventArgs object.</param>
        protected virtual void OnHandChanged(HandChangedEventArgs args)
        {
            // Uses null-conditional operator.
            HandChanged?.Invoke(this, args);
        }
        #endregion

        #region Abstract methods definition
        public abstract BlackjackGame.ACTIONS GetAction();
        #endregion
    }

    public class HandChangedEventArgs : EventArgs
    {
        private int index;
        public int Index
        {
            get { return index; }
        }

        private Card card;
        public Card Card
        {
            get { return card; }
        }

        public HandChangedEventArgs(int index, Card card)
        {
            this.index = index;
            this.card = card;
        }
    }
}
