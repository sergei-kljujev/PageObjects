﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace PageObjects.Controls
{

    [InheritedExport(typeof(IWebControl))]
    public interface IWebControl
    {
    }
}
