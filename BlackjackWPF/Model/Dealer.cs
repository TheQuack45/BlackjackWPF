using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackWPF.Model
{
    public class Dealer : Player
    {
        #region Constructors definition
        public Dealer()
        {
            this.currentHand = new Card[BlackjackGame.CARD_LIMIT];
            this.BlankHand();
            this.Wins = 0;
            this.Ties = 0;
            this.Losses = 0;
        }
        #endregion

        #region Methods definition
        public override BlackjackGame.ACTIONS GetAction()
        {
            if (this.GetHandTotal() <= 16)
            {
                // Action is hit.
                return BlackjackGame.ACTIONS.Hit;
            }
            else
            {
                // Action is stand.
                return BlackjackGame.ACTIONS.Stand;
            }
        }

        public List<Card> GetHand()
        {
            List<Card> outputList = new List<Card>();
            for (int i = 0; i < this.CurrentHand.Length; i++)
            {
                if (i == 0)
                {
                    outputList[i] = new Card(Card.POSITIONS.Unknown, Card.SUITS.Unknown);
                }
                else
                {
                    outputList[i] = CurrentHand[i];
                }
            }

            return outputList;
        }

        public Card GetCard(int index, bool isHidden = true)
        {
            if (isHidden && index == 0)
            {
                return new Card(Card.POSITIONS.Unknown, Card.SUITS.Unknown);
            }

            return this.CurrentHand[index];
        }
        #endregion
    }
}
