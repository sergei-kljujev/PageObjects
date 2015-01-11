using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageObjects.Context;

namespace PageObjects.Attributes
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CurrentContextAttribute : ExportAttribute
    {
        public CurrentContextAttribute() :
            base("CurrentContextElement", typeof(ContextElement))
        { }
    }
}
