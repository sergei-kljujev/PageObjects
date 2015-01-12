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

        public AmbiguousControlsException(Type controlInterface, IEnumerable<Type> implementations)
            : base(string.Format("{0} - Multiple suitable implementations found: ({1}).  Please restrict context so that only one would suite for current context.",
            controlInterface.FullName,
            string.Join("; ", implementations)))
        {
        }

    }
}
