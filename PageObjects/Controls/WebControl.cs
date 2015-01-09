using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using PageObjects.Factory;

namespace PageObjects.Controls
{
    public class WebControl : IWebControl
    {
        [Import(typeof(IFactory))]
        protected IFactory Factory;
    }
}
