using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PageObjects.Attributes;
[assembly: InternalsVisibleTo("PageObjects.tests")]

namespace PageObjects.Context
{
    [Export("CurrentContext", typeof(ICurrentContext))]
    public class CurrentContext : WebContext, ICurrentContext
    {
        [ImportMany("CurrentContextElement")]
        public new IEnumerable<ContextElement> ContextElements { get; private set; }

        public void AssertCurrentContextValid()
        {
            Validate(AllContextElements());
        }

        internal void Validate(IEnumerable<Type> AllElements)
        {
            if (!AllElements.All(ac => ContextElements.Any(ce => ac == ce.Type)))
            {
                var elements = string.Empty;

                AllElements
                    .Where(ac => ContextElements.Any(ce => ac == ce.Type))
                    .Select(ac => elements += string.IsNullOrEmpty(elements) ? "(" : ", " + ac.FullName);
                elements += string.IsNullOrEmpty(elements) ? "No Context Defined" : ")";

                throw new InvalidOperationException("Current Context is not defined for some Attributed Context Element types: " + elements);
            }
        }

        internal CurrentContext(IEnumerable<ContextElement> contextElements) 
        {
            ContextElements = contextElements;
        }

        private IEnumerable<Type> AllContextElements() 
        {
            return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(t => t.IsEnum)
            .Where(t => Attribute.IsDefined(t, typeof(ContextElementAttribute)));
        }

    }
}
