using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PageObjects.Context;
using PageObjects.Controls;

namespace PageObjects.Factory
{
    public interface IFactory
    {
        T Generate<T>() where T : IWebControl;

        Type GetImplementation<T>(IDictionary<Type, IWebContext> matched) where T : IWebControl;
    }
}
