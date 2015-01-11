using System;
using NUnit.Framework;
using PageObjects.Context;

namespace PageObjects.Tests
{
    [TestFixture]
    public class CurrentContextTests
    {
        [Test]
        public void When_Current_Context_Defined_For_All_ContextElements_Then_No_Exception()
        {
            var cc = new CurrentContext(new ContextElement[] {
                new ContextElement(typeof(string), 0, 0)
            });

            cc.Validate(new Type[] { typeof(string) });
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage="Int32", MatchType=MessageMatch.Contains)]
        public void When_Current_Context_Not_Defined_For_Any_ContextElement_Then_Exception()
        {
            var cc = new CurrentContext(new ContextElement[] {
                new ContextElement(typeof(string), 0, 0)
            });

            cc.Validate(new Type[] { typeof(string),
                                     typeof(Int32),
                                     typeof(Int64)});
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void When_No_Context_Specified_Then_Exception()
        {
            var cc = new CurrentContext(new ContextElement[] {
                new ContextElement(typeof(string), 0, 0)
            });

            cc.Validate(new Type[] {});

        }


    }
}
