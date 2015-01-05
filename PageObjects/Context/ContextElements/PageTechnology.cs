using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PageObjects.Attributes;

namespace PageObjects.Context.ContextElements
{
    [Flags]
    [ContextElement((uint)PageTechnology.ANY)]
    public enum PageTechnology
    {
        Asp,
        SL,
        ANY = Asp | SL
    }
}
