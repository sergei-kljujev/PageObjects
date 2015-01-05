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
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class WebControlExportAttribute : ExportAttribute, IWebControlMetadata
    {
        public string SupportedContext { get; set; }

        public IWebContext GetSupportedContext() 
        {
            return new WebContext();
        }

        public Type ControlType { get; private set; }


        public WebControlExportAttribute(Type WebControlType, params object[] RequiredContext)
            : base(typeof(IWebControl))
        {
            var c = new WebContext(RequiredContext);
            SupportedContext = new JavaScriptSerializer().Serialize(c.ContextElements);// (object)new WebContext(RequiredContext);
            ControlType = WebControlType;
        }


        
        public WebControlExportAttribute(Type WebControlType)
            : base(typeof(IWebControl))
        {
            var c = new WebContext();
            SupportedContext = new JavaScriptSerializer().Serialize(c.ContextElements);
            ControlType = WebControlType;
        }
    }
}
