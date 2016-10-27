using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackWPF.Model
{
    public class BlackjackGame : Game
    {
        #region Static members definition
        public enum ACTIONS { Hit, Stand };

        public static readonly Dictionary<Card.POSITIONS, int> CardRanks = new Dictionary<Card.POSITIONS, int>()
        {
            { Card.POSITIONS.Two, 2 },
            { Card.POSITIONS.Three, 3 },
            { Card.POSITIONS.Four, 4 },
            { Card.POSITIONS.Five, 5 },
            { Card.POSITIONS.Six, 6 },
            { Card.POSITIONS.Seven, 7 },
            { Card.POSITIONS.Eight, 8 },
            { Card.POSITIONS.Nine, 9 },
            { Card.POSITIONS.Ten, 10 },
            { Card.POSITIONS.Jack, 10 },
            { Card.POSITIONS.Queen, 10 },
            { Card.POSITIONS.King, 10 },
            { Card.POSITIONS.Ace, 11 },
            { Card.POSITIONS.Blank, 12 },
            { Card.POSITIONS.Unknown, 12 },
        };

        public const int LIMIT = 21;
        public const int CARD_LIMIT = 7;
        #endregion

        #region Members definition
        private Dealer dealer;
        public Dealer Dealer
        {
            get { return dealer; }
            set { dealer = value; }
        }

        private Human human;
        public Human Human
        {
            get { return human; }
            set { human = value; }
        }

        private Player actingPlayer;
        public Player ActingPlayer
        {
            get { return actingPlayer; }
            set { actingPlayer = value; }
        }

        private Stack<Card> burnDeck = new Stack<Card>();
        public Stack<Card> BurnDeck
        {
            get { return burnDeck; }
            set { burnDeck = value;}
        }
        #endregion

        #region Constructors definition
        public BlackjackGame()
        {
            this.cardDeck = new Deck(Deck.DeckStyles.Standard52);
            this.CardDeck.ShuffleDeck();
        }
        #endregion

        #region Methods definition
        public void ResetGame()
        {
            List<Card> dealerCards = this.Dealer.ReturnCards();
            foreach (Card cCard in dealerCards)
            {
                this.BurnDeck.Push(cCard);
            }

            List<Card> humanCards = this.Human.ReturnCards();
            foreach (Card cCard in humanCards)
            {
                this.BurnDeck.Push(cCard);
            }

            this.Human.IsBusted = false;
            this.Dealer.IsBusted = false;
        }

        public Player GetWinner()
        {
            int dealerScore = this.Dealer.GetHandTotal();
            int humanScore = this.Human.GetHandTotal();

            if (dealerScore == humanScore)
            {
                return null;
            }
            if (dealerScore > humanScore)
            {
                return this.Dealer;
            }
            else
            {
                return this.Human;
            }
        }

        public void DoPlayerAction(ACTIONS action)
        {
            if (action == ACTIONS.Hit)
            {
                try
                {
                    Human.AddCardToHand(this.CardDeck.DrawCard());
                }
                catch (DeckEmptyException)
                {
                    for (int i = 0; i < this.BurnDeck.Count; i++)
                    {
                        this.CardDeck.CardStack.Push(this.BurnDeck.Pop());
                    }

                    this.CardDeck.ShuffleDeck();
                }
            }
            else
            {
                ActingPlayer = Dealer;
            }

            if (IsBusted(Human))
            {
                Human.IsBusted = true;
            }
        }

        public void DoDealerAction()
        {
            ACTIONS dealerAction = Dealer.GetAction();
            if (dealerAction == ACTIONS.Hit)
            {
                try
                {
                    Dealer.AddCardToHand(this.CardDeck.DrawCard());
                }
                catch (DeckEmptyException)
                {
                    for (int i = 0; i < this.BurnDeck.Count; i++)
                    {
                        this.CardDeck.CardStack.Push(this.BurnDeck.Pop());
                    }

                    this.CardDeck.ShuffleDeck();
                }
            }
            else
            {
                ActingPlayer = Human;
            }

            if (IsBusted(Dealer))
            {
                Dealer.IsBusted = true;
                ActingPlayer = Human;
            }
        }

        public bool IsBusted(Player player)
        {
            if (player.GetHandTotal() > LIMIT)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Distributes two cards to each of the players.
        /// </summary>
        public void DistributeHands()
        {
            if (this.CardDeck.CardStack.Count < 9)
            {
                // Deck is out of cards.
                for (int i = 0; i < this.BurnDeck.Count; i++)
                {
                    this.CardDeck.CardStack.Push(this.BurnDeck.Pop());
                }

                this.CardDeck.ShuffleDeck();
            }

            // I did this really simply.
            // I'm sorry.
            Human.AddCardToHand(this.CardDeck.DrawCard());
            Dealer.AddCardToHand(this.CardDeck.DrawCard());
            Human.AddCardToHand(this.CardDeck.DrawCard());
            Dealer.AddCardToHand(this.CardDeck.DrawCard());

            this.ActingPlayer = Human;
        }

        /// <summary>
        /// Sets this object's Dealer to the given Dealer object.
        /// </summary>
        /// <param name="dealer">The Dealer object to set the Dealer member to.</param>
        public void SetDealer(Dealer dealer)
        {
            this.Dealer = dealer;
        }

        /// <summary>
        /// Sets this object's Human to the given Player object.
        /// </summary>
        /// <param name="human">The Human object to set the Human member to.</param>
        public void SetHuman(Human human)
        {
            this.Human = human;
        }

        /// <summary>
        /// Sorts the given List&lt;Card&gt; by card position ASCENDING, and then returns the sorted List.
        /// </summary>
        /// <param name="list">List&lt;Card&gt; to sort and return.</param>
        /// <returns>Sorted List&lt;Card&gt;.</returns>
        public static Card[] SortByPosition(Card[] list)
        {
            return list.OrderBy(cCard => CardRanks[cCard.Pos]).ToArray<Card>();
        }
        #endregion
    }
}
