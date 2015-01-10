using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PageObjects.Context
{
    public interface IWebContext
    {
        IEnumerable<ContextElement> ContextElements { get; }

        uint ContextElementMask(Type contextType);

        double ContextPrecision(Type contextType);

        bool ContainsElement(Type contextType);

        bool Match(IWebContext other);

        int MaxPrecisionMatch(Dictionary<Type, double> maxPrecision);

        //T ContextValue<T>()
        //    where T : struct, IConvertible;
    }
}
