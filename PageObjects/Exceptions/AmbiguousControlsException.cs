using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjects.Exceptions
{
    public class AmbiguousControlsException : Exception
    {
        public AmbiguousControlsException(string Message) : base(Message)
        {
        }

        public AmbiguousControlsException(Type controlInterface) :
            base(
            string.Format(
                "{0} - Multiple suitable implementations found. Please restrict context so that only one would suite for current context.", controlInterface.FullName)
            )
        {
        }

    }
}
