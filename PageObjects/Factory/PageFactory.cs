
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
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

        [ImportMany(typeof(IWebControl))]
        internal IEnumerable<Lazy<Type, IWebControlMetadata>> Controls {get; set;}


        public ControlFactory() 
        { 
        }




        public T Generate<T>() where T : IWebControl
        {
            var controlType = GetImplementation<T>();

            return (T) Activator.CreateInstance(controlType);
        }

        public Type GetImplementation<T>() where T : IWebControl
        {
            var controlType = typeof(T);

            var supportedControls = Controls.Where(m => m.Metadata.ControlType == controlType &&
                                        CurrentContext.Match(m.Metadata.GetContext()));

            var currentContextElements = CurrentContext.ContextElements.Select(ce => ce.Type);
            
            // Combined max context precision
            Dictionary<string, double> maxContextPrecision;
            try
            {
                maxContextPrecision = currentContextElements.
                    Select(t => new KeyValuePair<string, double>(t,
                        supportedControls.Min(cm => (cm.Metadata.GetContext()).ContextPrecision(t)))).
                    ToDictionary(x => x.Key, x => x.Value);
            }
            catch (InvalidOperationException ex)
            {
                throw new MissingControlException(controlType, ex);
            }


            var maxPrecisionElementsCount = supportedControls.Max(cm => (cm.Metadata.GetContext()).MaxPrecisionMatch(maxContextPrecision));

            var maxPrecisionElements = supportedControls.Where(cm => (cm.Metadata.GetContext()).MaxPrecisionMatch(maxContextPrecision) == maxPrecisionElementsCount);

            var numControlsFound = maxPrecisionElements.Count();

            if (numControlsFound == 1)
            {
                return maxPrecisionElements.FirstOrDefault().Value;
            }

            if (numControlsFound > 1)
            {
                throw new AmbiguousControlsException(controlType);
            }
            
            throw new InvalidOperationException("Unable to get implementation for control "+controlType.FullName);
        }
    }
}
