using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackWPF
{
    [Serializable]
    public class DeckEmptyException : Exception
    {
        public DeckEmptyException(string message) : base(message) { }

        public DeckEmptyException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }
    }
}