using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PageObjects.Attributes;
using PageObjects.Context.ContextElements;

namespace PageObjects.Controls.TreeView
{
    [WebControlExport(typeof(ITreeView), PageTechnology.Asp)]
    class TreeViewAsp : ITreeView
    {
    }
}
