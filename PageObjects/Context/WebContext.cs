using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using PageObjects.Attributes;

namespace PageObjects.Context
{
    public class WebContext : IWebContext
    {
        private IEnumerable<ContextElement> values;

        public WebContext(IEnumerable<ContextElement> elements)
        {
            values = elements;
        }

        public IEnumerable<ContextElement> ContextElements
        {
            get
            {
                return values;
            }
        }

        public bool Match(IWebContext other)
        {
            if (!other.ContextElements.Any())
                return true;

            foreach (var ce in ContextElements)
            {

                if (other.ContainsElement(ce.Type) &&
                    (other.ContextElementMask(ce.Type) & ce.Mask) == 0)
                    return false;
            }
            return true;
        }

        //public T ContextValue<T>() 
        //    where T: struct, IConvertible
        //{
        //    var type = typeof(T).FullName;

        //    return (T)(object)ContextElementMask(type);
        //}

        public WebContext(object[] contextElements)
        {
            values = new List<ContextElement>();

            foreach (var ce in contextElements)
            {
                var t = ce.GetType();

                if (!t.IsEnum)
                    throw new InvalidOperationException(string.Format("Unable to generate context from element {0} with type {1}.", ce, ce.GetType()));
                if (ContainsElement(t))
                    throw new InvalidOperationException(string.Format("Unable to add Dublicated Context element {0} {1}",t, ce));
                
                var attr = Attribute.GetCustomAttribute(t, typeof(ContextElementAttribute)) as ContextElementAttribute;
                if (attr == null ||
                    attr.FullContext == 0)
                {
                    throw new InvalidOperationException("Please specify FullContext at the ContextElement attribute of enum - "+t.FullName);
                }
                
                (values as List<ContextElement>).Add(new ContextElement(t, (uint)(int)ce, (uint)attr.FullContext));
            }

        }

        public int MaxPrecisionMatch(Dictionary<Type, double> maxPrecision) {
            var ret = 0;

            foreach (var t in maxPrecision.Keys)
            {
                if (ContextPrecision(t) <= maxPrecision[t])
                    ret++;
            }
            ret += values.Select(v => v.Type).Count(k => !maxPrecision.ContainsKey(k));
            return ret;
        }

        public WebContext() 
        {
            values = new List<ContextElement>();
        }

        public double ContextPrecision(Type contextElement) 
        {
            if (!ContainsElement(contextElement))
                return 1;


            return Helper.NumberOfOnes(ContextElementMask(contextElement)) / (double)Helper.NumberOfOnes(ContextElementOfType(contextElement).FullContext);
        }


        public bool ContainsElement(Type contextType)
        {
            return values.Any(v => v.Type == contextType);
        }

        public uint ContextElementMask(Type contextType)
        {
            return ContextElementOfType(contextType).Mask;
        }

        private ContextElement ContextElementOfType(Type type)
        {
            return values.FirstOrDefault(v => v.Type == type);
        }
        //public Dictionary<Type, double> ContextPrecision
        //{
         //   get; private set;
       //}

    }
}
