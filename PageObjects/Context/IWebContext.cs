using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PageObjects.Context
{
    public interface IWebContext
    {
        IEnumerable<ContextElement> ContextElements { get; }

        uint ContextElementMask(string contextType);

        double ContextPrecision(string contextType);

        bool ContainsElement(string contextType);

        bool Match(IWebContext other);

        int MaxPrecisionMatch(Dictionary<string, double> maxPrecision);

        T ContextValue<T>()
            where T : struct, IConvertible;
    }
}
