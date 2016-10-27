using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackWPF.Model
{
    public class Deck
    {
        #region Static members definition
        public enum DeckStyles { Standard52 };
        #endregion

        #region Members definition
        private Stack<Card> cardStack;
        public Stack<Card> CardStack
        {
            get { return cardStack; }
        }
        #endregion

        #region Constructors definition
        public Deck(DeckStyles style = DeckStyles.Standard52)
        {
            switch (style)
            {
                case DeckStyles.Standard52:
                    this.cardStack = new Stack<Card>();
                    this.cardStack = PopulateStandardDeck(this.CardStack);
                    break;
            }
        }
        #endregion

        #region Methods definition
        /// <summary>
        /// Populates this object's card deck with a standard 52 French set, unshuffled.
        /// </summary>
        private static Stack<Card> PopulateStandardDeck(Stack<Card> stack)
        {
            stack.Push(new Model.Card(Card.POSITIONS.Ace, Card.SUITS.Clubs));
            stack.Push(new Model.Card(Card.POSITIONS.Ace, Card.SUITS.Diamonds));
            stack.Push(new Model.Card(Card.POSITIONS.Ace, Card.SUITS.Hearts));
            stack.Push(new Model.Card(Card.POSITIONS.Ace, Card.SUITS.Spades));

            stack.Push(new Model.Card(Card.POSITIONS.Two, Card.SUITS.Clubs));
            stack.Push(new Model.Card(Card.POSITIONS.Two, Card.SUITS.Diamonds));
            stack.Push(new Model.Card(Card.POSITIONS.Two, Card.SUITS.Hearts));
            stack.Push(new Model.Card(Card.POSITIONS.Two, Card.SUITS.Spades));

            stack.Push(new Model.Card(Card.POSITIONS.Three, Card.SUITS.Clubs));
            stack.Push(new Model.Card(Card.POSITIONS.Three, Card.SUITS.Diamonds));
            stack.Push(new Model.Card(Card.POSITIONS.Three, Card.SUITS.Hearts));
            stack.Push(new Model.Card(Card.POSITIONS.Three, Card.SUITS.Spades));

            stack.Push(new Model.Card(Card.POSITIONS.Four, Card.SUITS.Clubs));
            stack.Push(new Model.Card(Card.POSITIONS.Four, Card.SUITS.Diamonds));
            stack.Push(new Model.Card(Card.POSITIONS.Four, Card.SUITS.Hearts));
            stack.Push(new Model.Card(Card.POSITIONS.Four, Card.SUITS.Spades));

            stack.Push(new Model.Card(Card.POSITIONS.Five, Card.SUITS.Clubs));
            stack.Push(new Model.Card(Card.POSITIONS.Five, Card.SUITS.Diamonds));
            stack.Push(new Model.Card(Card.POSITIONS.Five, Card.SUITS.Hearts));
            stack.Push(new Model.Card(Card.POSITIONS.Five, Card.SUITS.Spades));

            stack.Push(new Model.Card(Card.POSITIONS.Six, Card.SUITS.Clubs));
            stack.Push(new Model.Card(Card.POSITIONS.Six, Card.SUITS.Diamonds));
            stack.Push(new Model.Card(Card.POSITIONS.Six, Card.SUITS.Hearts));
            stack.Push(new Model.Card(Card.POSITIONS.Six, Card.SUITS.Spades));

            stack.Push(new Model.Card(Card.POSITIONS.Seven, Card.SUITS.Clubs));
            stack.Push(new Model.Card(Card.POSITIONS.Seven, Card.SUITS.Diamonds));
            stack.Push(new Model.Card(Card.POSITIONS.Seven, Card.SUITS.Hearts));
            stack.Push(new Model.Card(Card.POSITIONS.Seven, Card.SUITS.Spades));

            stack.Push(new Model.Card(Card.POSITIONS.Eight, Card.SUITS.Clubs));
            stack.Push(new Model.Card(Card.POSITIONS.Eight, Card.SUITS.Diamonds));
            stack.Push(new Model.Card(Card.POSITIONS.Eight, Card.SUITS.Hearts));
            stack.Push(new Model.Card(Card.POSITIONS.Eight, Card.SUITS.Spades));

            stack.Push(new Model.Card(Card.POSITIONS.Nine, Card.SUITS.Clubs));
            stack.Push(new Model.Card(Card.POSITIONS.Nine, Card.SUITS.Diamonds));
            stack.Push(new Model.Card(Card.POSITIONS.Nine, Card.SUITS.Hearts));
            stack.Push(new Model.Card(Card.POSITIONS.Nine, Card.SUITS.Spades));

            stack.Push(new Model.Card(Card.POSITIONS.Ten, Card.SUITS.Clubs));
            stack.Push(new Model.Card(Card.POSITIONS.Ten, Card.SUITS.Diamonds));
            stack.Push(new Model.Card(Card.POSITIONS.Ten, Card.SUITS.Hearts));
            stack.Push(new Model.Card(Card.POSITIONS.Ten, Card.SUITS.Spades));

            stack.Push(new Model.Card(Card.POSITIONS.Jack, Card.SUITS.Clubs));
            stack.Push(new Model.Card(Card.POSITIONS.Jack, Card.SUITS.Diamonds));
            stack.Push(new Model.Card(Card.POSITIONS.Jack, Card.SUITS.Hearts));
            stack.Push(new Model.Card(Card.POSITIONS.Jack, Card.SUITS.Spades));

            stack.Push(new Model.Card(Card.POSITIONS.Queen, Card.SUITS.Clubs));
            stack.Push(new Model.Card(Card.POSITIONS.Queen, Card.SUITS.Diamonds));
            stack.Push(new Model.Card(Card.POSITIONS.Queen, Card.SUITS.Hearts));
            stack.Push(new Model.Card(Card.POSITIONS.Queen, Card.SUITS.Spades));

            stack.Push(new Model.Card(Card.POSITIONS.King, Card.SUITS.Clubs));
            stack.Push(new Model.Card(Card.POSITIONS.King, Card.SUITS.Diamonds));
            stack.Push(new Model.Card(Card.POSITIONS.King, Card.SUITS.Hearts));
            stack.Push(new Model.Card(Card.POSITIONS.King, Card.SUITS.Spades));

            return stack;
        }

        /// <summary>
        /// Returns the shuffled version of the given deck using Richard Durstenfeld's implementation of the Fisher-Yates shuffle.
        /// </summary>
        /// <param name="deck"></param>
        /// <returns></returns>
        protected static Stack<Card> Shuffle(Stack<Card> deck)
        {
            Random rdm = new Random();
            Card[] cardArr = deck.ToArray<Card>();
            Card[] outputCardArr = new Card[cardArr.Length];

            for (int i = cardArr.Length - 1; i >= 0; i--)
            {
                do
                {
                    int j = rdm.Next(cardArr.Length);
                    if (!outputCardArr.Contains<Card>(cardArr[j]))
                        { outputCardArr[i] = cardArr[j]; }
                } while (outputCardArr[i] == null);
            }

            return BuildStack(outputCardArr);
        }

        /// <summary>
        /// Shuffles the current object's CardStack.
        /// </summary>
        public void ShuffleDeck()
        {
            this.cardStack = Shuffle(this.cardStack);
        }

        /// <summary>
        /// Converts an array of Card objects to a Stack built from the top down.
        /// </summary>
        /// <param name="inputArr">Array of Card objects.</param>
        /// <returns>Populated Stack of Card objects.</returns>
        private static Stack<Card> BuildStack(Card[] inputArr)
        {
            Stack<Card> outputStack = new Stack<Card>();

            for (int i = inputArr.Length - 1; i >= 0; i--)
            {
                outputStack.Push(inputArr[i]);
            }

            return outputStack;
        }

        /// <summary>
        /// Draws a Card from the top of CardStack and returns it.
        /// </summary>
        /// <returns>Card object that was drawn.</returns>
        public Card DrawCard()
        {
            if (CardStack.Count > 0)
            {
                return CardStack.Pop();
            }

            throw new DeckEmptyException("A Card cannot be drawn from the Deck because the Deck is empty.");
        }
        #endregion
    }
}
