using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using PageObjects.Attributes;

namespace PageObjects.Context.ContextElements
{
    [Flags]
    [ContextElement((uint)ANY)]
    public enum PageTechnology
    { 
        Asp, SL, 
        ANY = Asp | SL
    }

    

    public static class CurrentContext
    {
        [Export("CurrentContextElement", typeof(ContextElement))]
        public static ContextElement CurrentPageTechnology 
        {
            get
            {
                return new ContextElement(typeof(PageTechnology), (uint)PageTechnology.Asp, (uint)PageTechnology.ANY);
            }
        }
    }

    
}
