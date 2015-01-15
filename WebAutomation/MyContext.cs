using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageObjects.Attributes;
using PageObjects.Context;

namespace WebAutomation
{

    [Flags]
    [ContextElement((uint)ANY)]
    public enum MyContext
    {
        test1 = 1,
        test2 = 2,
        test3 = 4,
        test4 = 8,
        ANY = test1 | test2 | test3 | test4,
        NEW = test3 | test4
    }
    
    [Flags]
    [ContextElement((uint)ANY)]
    public enum MyContext2
    {
        test1 = 1,
        test2 = 2,
        test3 = 4,
        test4 = 8,
        ANY = test1 | test2 | test3 | test4,
        NEW = test3 | test4
    }



    public static class ContextHelper
    {
        [CurrentContext]
        public static ContextElement CurrentMyContext
        {
            get
            {
                return new ContextElement(typeof(MyContext), (uint)MyContext.test3, (uint)MyContext.ANY);
            }
        }

        [CurrentContext]
        public static ContextElement CurrentMyContext2
        {
            get
            {
                return new ContextElement(typeof(MyContext2), (uint)MyContext2.test2, (uint)MyContext2.ANY);
            }
        }
    }
}
