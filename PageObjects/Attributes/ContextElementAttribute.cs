using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PageObjects.Attributes
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class ContextElementAttribute : Attribute
    {
        public uint FullContext;

        public ContextElementAttribute(uint FullContextMask) {
            FullContext = FullContextMask;
        }
    }
}
