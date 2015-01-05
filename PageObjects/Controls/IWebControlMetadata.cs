using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using PageObjects.Context;

namespace PageObjects.Controls
{
    public interface IWebControlMetadata
    {
        string SupportedContext { get; }

        Type ControlType { get; }
    }

    public static class MetadataHelper 
    { 
        public static IWebContext GetContext(this IWebControlMetadata m)
        {
            var json = m.SupportedContext as String;
            var context = new JavaScriptSerializer().Deserialize<IEnumerable<ContextElement>>(json);
            return new WebContext(context);
        }
    }
}
