
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using PageObjects.Attributes;
using PageObjects.Context;
using PageObjects.Controls;
using PageObjects.Exceptions;
[assembly: InternalsVisibleTo("PageObjects.tests")]

namespace PageObjects.Factory
{
    
    [Export(typeof(IFactory))]
    public class ControlFactory : IFactory
    {
        private static ControlFactory _instance;
        
        [Import("CurrentContext", RequiredCreationPolicy=CreationPolicy.Shared)]
        private IWebContext CurrentContext;

        public ControlFactory() 
        { 
        }

        private WebControlExportAttribute GetControlAttribute(Type t)
        {
            var attr = (WebControlExportAttribute)Attribute.GetCustomAttribute(t, typeof(WebControlExportAttribute));

            return attr;
        }

        public Type[] GetAllMatchedImplementations<T>()
            where T : IWebControl
        {
            var interfaceType = typeof(T);

            var allImplementations = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => interfaceType.IsAssignableFrom(t) && interfaceType != t).ToArray();

            if (!allImplementations.Any())
                return null;

            var exports = allImplementations
                .Where(impl => GetControlAttribute(impl).ControlType == interfaceType).ToArray();

            var matched = exports
                .Where(impl => CurrentContext.Match(GetControlAttribute(impl).SupportedContext));

            return matched.ToArray();
        }

        public Type GetImplementation<T>(IDictionary<Type, IWebContext> matched) 
            where T : IWebControl
        {
            

            if (matched == null ||
                matched.Count == 0)
                throw new MissingControlException(typeof(T));

            if (matched.Count == 1)
                return matched.First().Key;

            var currentContextElements = CurrentContext.ContextElements.Select(ce => ce.Type);

            // Combined max context precision
            Dictionary<string, double> maxContextPrecision;
                maxContextPrecision = currentContextElements.
                    Select(contextType => new {
                        Context = contextType,
                        Precision = matched.Min(cm => cm.Value.ContextPrecision(contextType))})
                        .ToDictionary(x => x.Context, x => x.Precision);

            // Maximum number of Context Elements, specified with Max Precision..
            var maxPrecisionElementsCount = matched.Max(cm => cm.Value.MaxPrecisionMatch(maxContextPrecision));

            // Number of Most precise Controls
            var finalControls = matched.Where(t => t.Value.MaxPrecisionMatch(maxContextPrecision) == maxPrecisionElementsCount).ToArray();

            if (finalControls.Length > 1)
                throw new AmbiguousControlsException(typeof(T));

            if (finalControls.Length == 1)
                return finalControls.First().Key;

            throw new InvalidOperationException("Unable to find suitable control implementation for " + typeof(T).FullName);
        }

        public T Generate<T>() where T : IWebControl
        {
            var matched = GetAllMatchedImplementations<T>().ToDictionary(t => t, t => GetControlAttribute(t).SupportedContext);

            var controlType = GetImplementation<T>(matched);

            return (T) Activator.CreateInstance(controlType);
        }
    }
}
