using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using PageObjects.Context;
using PageObjects.Controls;

namespace PageObjects.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class WebControlExportAttribute : Attribute
    {
        public IWebContext SupportedContext { get; set; }

        public Type ControlType { get; private set; }

        public WebControlExportAttribute(Type WebControlType, params object[] RequiredContext)
        {
            SupportedContext = new WebContext(RequiredContext);
            ControlType = WebControlType;
        }

        public WebControlExportAttribute(Type WebControlType)
        {
            SupportedContext = new WebContext();
            ControlType = WebControlType;
        }
    }
}
