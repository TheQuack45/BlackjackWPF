using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackWPF.Model
{
    [Serializable]
    public class HandFullException : Exception
    {
        public HandFullException(string message) : base(message) { }

        protected HandFullException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }
    }
}
