using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using PageObjects.Context;
using PageObjects.Controls;

namespace PageObjects.Factory
{
    [Export(typeof(IFactory))]
    public class ControlFactory : IFactory
    {
        private static ControlFactory _instance;
        
        [Import("CurrentContext", RequiredCreationPolicy=CreationPolicy.Shared)]
        private IWebContext CurrentContext;

        [Import("FullContext", RequiredCreationPolicy = CreationPolicy.Shared)]
        private IWebContext FullContext;

        [ImportMany(typeof(IWebControl))]
        public IEnumerable<Lazy<Type, IWebControlMetadata>> Controls {get;set;}

        public ControlFactory Instance {
            get {
                if (_instance == null)
                    _instance = new ControlFactory();

                return _instance;
            }
        }

        public ControlFactory() 
        { 
        }


        public ControlFactory(IEnumerable<Lazy<Type, IWebControlMetadata>> _controls) 
        {
            Controls = _controls;
        }


        public T Generate<T>()
        {
            var controlType = typeof(T);

            var supportedControls = Controls.Where(m => m.Metadata.ControlType == controlType && 
                                        CurrentContext.Match(m.Metadata.GetContext()));

            var currentContextElements = CurrentContext.ContextElements.Select(ce => ce.Type);
            
            // Combined max context precision
            var maxContextPrecision = currentContextElements.
                Select(t => new KeyValuePair<string, double>(t,
                    supportedControls.Min(cm => (cm.Metadata.GetContext()).ContextPrecision(t)))).
                    ToDictionary(x => x.Key, x => x.Value);

            
            var maxPrecisionElementsCount = supportedControls.Max(cm => (cm.Metadata.GetContext()).MaxPrecisionMatch(maxContextPrecision));

            var maxPrecisionElements = supportedControls.Where(cm => (cm.Metadata.GetContext()).MaxPrecisionMatch(maxContextPrecision) == maxPrecisionElementsCount);

            if (maxPrecisionElements.Count() == 1)
            {
                var type = maxPrecisionElements.FirstOrDefault().Value;

                return (T) Activator.CreateInstance(type);
            }

            throw new InvalidOperationException("Unable to generate Web Control");
        }
    }
}
