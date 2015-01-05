using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PageObjects.Factory
{
    public interface IFactory
    {
        T Generate<T>();
    }
}
