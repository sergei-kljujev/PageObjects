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
    internal class CurrentContext : WebContext, ICurrentContext
    {
        /// <summary>
        /// Default Constructor, used by MEF
        /// </summary>
        public CurrentContext()
        {
        }

        /// <summary>
        /// Constructor for Unit Tests 
        /// </summary>
        /// <param name="contextElements"></param>
        internal CurrentContext(IEnumerable<ContextElement> contextElements) : base(contextElements)
        {
        }


//        [ImportMany("CurrentContextElement")]
//        public IEnumerable<ContextElement> ContextElements { get; private set; }

        public void AssertCurrentContextValid()
        {
            Validate(AllContextElements());
        }

        internal void Validate(IEnumerable<Type> AllElements)
        {
            if (!AllElements.All(ac => ContextElements.Any(ce => ac == ce.Type)))
            {
                var elements = string.Empty;

                foreach (var el in AllElements.Where(ac => ContextElements.All(ce => ac != ce.Type)))
                {
                    elements += string.IsNullOrEmpty(elements) ? "(" : ", "; 
                    elements += el.FullName;
                }
                elements += ")";

                throw new InvalidOperationException("Current Context is not defined for some Attributed Context Element types: " + elements);
            }
            else if (!AllElements.Any())
            {
                throw new InvalidOperationException("No Context Elements defined in the project.");
            }
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
