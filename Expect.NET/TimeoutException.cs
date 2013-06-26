using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    [Serializable()]
    public class TimeoutException : Exception
    {
        public TimeoutException() : base() { }
        public TimeoutException(string message) : base(message) { }
        public TimeoutException(string message, Exception innerException) : base(message, innerException) { }

        protected TimeoutException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
