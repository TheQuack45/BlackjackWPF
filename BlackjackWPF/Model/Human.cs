using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackWPF.Model
{
    public class Human : Player
    {
        #region Constructors definition
        public Human()
        {
            this.currentHand = new Card[BlackjackGame.CARD_LIMIT];
            this.BlankHand();
            this.Wins = 0;
            this.Ties = 0;
            this.Losses = 0;
        }
        #endregion

        #region Methods definition
        /// <summary>
        /// Unused method that I really should get rid of, but haven't because it's an abstract method...
        /// </summary>
        /// <returns>Will always return BlackjackGame.ACTIONS.Hit</returns>
        public override BlackjackGame.ACTIONS GetAction()
        {
            return BlackjackGame.ACTIONS.Hit;
        }

        public Card[] GetHand()
        {
            return this.CurrentHand;
        }
        #endregion
    }
}
