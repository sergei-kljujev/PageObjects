using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjects.Exceptions
{
    public class MissingControlException : Exception
    {
        public MissingControlException(string message) : base (message)
        {
        }

        public MissingControlException(Type controlInterface) :
            base(string.Format("{0} - Unable to find suitable implementation.", controlInterface.FullName))
        {
        }

        public MissingControlException(Type controlInterface, Exception inner) :
            base(string.Format("{0} - Unable to find suitable implementation.", controlInterface.FullName), inner)
        {
            
        }

    }
}
