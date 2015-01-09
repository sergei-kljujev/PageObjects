using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PageObjects.Attributes;

namespace PageObjects.Context.ContextElements
{

    [ContextElement(ANY)]
    public class PageTechnology
    {
        public const uint Asp = 1;
        public const uint SL = 2;
        
        
        public const uint ANY = Asp | SL;

        public static uint GetCurrentContext()
        {
            return SL;
        }
    }
}
