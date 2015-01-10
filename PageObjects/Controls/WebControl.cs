using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using PageObjects.Factory;

namespace PageObjects.Controls
{
    public abstract class WebControl : IWebControl
    {
        protected IFactory Factory;

        public WebControl() 
        {
            Factory = ControlFactory.Instance;
        }

    }
}
