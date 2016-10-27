using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackWPF.Model
{
    public abstract class Game
    {
        #region Members definition
        protected Deck cardDeck;
        public Deck CardDeck
        {
            get { return cardDeck; }
        }
        #endregion
    }
}
