using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjects.Context
{
    public interface ICurrentContext : IWebContext
    {
        void AssertCurrentContextValid();
    }
}
