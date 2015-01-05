using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PageObjects.Controls
{
    public interface IPageObject : IWebControl
    {
        void AssertIsVisible();
    }
}
