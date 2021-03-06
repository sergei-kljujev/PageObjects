﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjects.Context
{
    public class ContextElement
    {
        public Type Type { get;  set; }

        public uint Mask { get; set; }

        public uint FullContext { get; set; }

        public ContextElement()
        {
        }

        public ContextElement(Type contextType, uint mask, uint fullContext) 
        {
            Type = contextType;
            Mask = mask;
            FullContext = fullContext;
        }
    }
}
