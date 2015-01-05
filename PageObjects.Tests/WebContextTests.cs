using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Moq;
using NUnit.Framework;
using PageObjects.Attributes;
using PageObjects.Context;

namespace PageObjects.Tests
{
    [TestFixture]
    public class WebContextTests
    {
        [Flags]
        [ContextElement((int)ALL)]
        private enum test1
        { 
            A1 = 1,
            B1 = 2,
            ALL = 3
        }
        
        [Flags]
        [ContextElement((int)ALL)]
        private enum test2
        {
            A2 = 1,
            B2 = 2,
            ALL = 3
        }

        private Mock<IWebContext> FullContext = new Mock<IWebContext>();

        [Flags]
        [ContextElement((int)ALL)]
        private enum test3
        {
            A3 = 1,
            B3 = 2,
            C3 = 4,
            ALL = 7
        }

        [TestFixtureSetUp]
        public void Compose() 
        {
        //    var fullContextDictionary = new Dictionary<Type, uint>();
        //    fullContextDictionary.Add(typeof(test1), (uint)test1.ALL);
        //    fullContextDictionary.Add(typeof(test2), (uint)test2.ALL);
        //    fullContextDictionary.Add(typeof(test3), (uint)test3.ALL);

        //    FullContext.Setup(x => x.ContextElements).Returns(fullContextDictionary);
        }


        [Test]
        public void When_WebContext_Constructor_Empty_Then_ContextElements_Is_Empty()
        {
            var WebContext = new WebContext();
            
            var actual = WebContext.ContextElements;

            CollectionAssert.IsEmpty(actual);
        }

        [Test]
        public void When_WebContext_Constructor_NonEmpty_ContextElements_Has_Specified_Elements()
        {
            var WebContext = new WebContext(new object[] { test1.A1, test2.ALL });

            var actual = WebContext.ContextElements.Select(c => c.Type);

            Assert.That(actual.Count(), Is.EqualTo(2));
        }

        [Test]
        public void When_WebContext_Constructed_with_OR_Then_Element_Has_Correct_Value()
        {
            var WebContext = new WebContext(new object[] { test1.A1 | test1.B1 });


            Assert.That(WebContext.ContextElementMask(typeof(test1).FullName), Is.EqualTo((uint)(int)(test1.A1 | test1.B1)));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void When_Constructor_Has_WrongType_Then_Exception()
        {
               var WebContext = new WebContext( new object [] {test1.A1, test2.ALL, "wrong type"});

        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void When_Constructor_Has_Same_Type_Arguments_Then_Exception()
        {
               var WebContext = new WebContext( new object [] {test1.A1, test1.ALL});
        }

        [Test]
        public void When_WebContext_Empty_Then_Match() 
        {
            var WebContext1 = new WebContext();
            var WebContext2 = new WebContext();

            Assert.IsTrue(WebContext1.Match(WebContext2));
        }

        [Test]
        public void When_WebContext_Same_Then_Match()
        {
            var WebContext1 = new WebContext(new object [] { test1.A1 });
            var WebContext2 = new WebContext(new object []  { test1.A1 });

            Assert.IsTrue(WebContext1.Match(WebContext2));
        }

        [Test]
        public void When_WebContext_Same_Elements_Diff_Values_Same_Context_Then_Match()
        {
            var WebContext1 = new WebContext(new object[] { test1.A1 });
            var WebContext2 = new WebContext(new object[] { test1.ALL });

            Assert.IsTrue(WebContext1.Match(WebContext2));

        }

        [Test]
        public void When_WebContext_Same_Elements_Diff_Values_Diff_Context_Then_Match()
        {
            var WebContext1 = new WebContext(new object[] { test1.A1 });
            var WebContext2 = new WebContext(new object[] { test1.B1 });

            Assert.IsFalse(WebContext1.Match(WebContext2));
        }

        [Test]
        public void When_Other_Has_More_Elements_Then_Match() {
            var WebContext1 = new WebContext(new object[] { test1.A1 });
            var WebContext2 = new WebContext(new object[] { test1.ALL, test2.A2 });

            Assert.IsTrue(WebContext1.Match(WebContext2));
        }

        [Test]
        public void When_Other_Has_Less_Elements_Then_Match()
        {
            var WebContext1 = new WebContext(new object[] { test1.A1, test2.ALL });
            var WebContext2 = new WebContext(new object[] { test1.A1 });

            Assert.IsTrue(WebContext1.Match(WebContext2));
        }

        [Test]
        public void When_Other_IsEmpty_Then_Match()
        {
            var WebContext1 = new WebContext(new object[] { test1.A1, test2.ALL });
            var WebContext2 = new WebContext();

            Assert.IsTrue(WebContext1.Match(WebContext2));
        }

        [Test]
        public void When_This_IsEmpty_Then_Match()
        {
            var WebContext1 = new WebContext();
            var WebContext2 = new WebContext(new object[] { test1.A1, test2.ALL });

            Assert.IsTrue(WebContext1.Match(WebContext2));
        }

        [Test]
        public void When_WebContext_IsEmpty_Then_Any_ContextPrecision_Is_1()
        {
            var WebContext = new WebContext();

            Assert.That(WebContext.ContextPrecision(typeof(test1).FullName), Is.EqualTo(1));
        }


        [Test]
        public void When_WebContext_Is_Full_Then_Precision_Is_1()
        {
            var WebContext = new WebContext(new object[] { test3.ALL });

            Assert.That(WebContext.ContextPrecision(typeof(test3).FullName), Is.EqualTo(1));
        }

        [Test]
        public void When_WebContext_Is_Two_Of_Three_Then_Precision_Is_two_thirds()
        {
            var WebContext = new WebContext(new object[] { test3.A3 | test3.B3 });

            Assert.That(WebContext.ContextPrecision(typeof(test3).FullName), Is.AtMost(0.7).And.AtLeast(0.6));
        }

        [Test]
        public void When_WebContext_Is_One_Of_Three_Then_Precision_Is_One_third()
        {
            var WebContext = new WebContext(new object[] { test3.A3 });

            Assert.That(WebContext.ContextPrecision(typeof(test3).FullName), Is.AtMost(0.34).And.AtLeast(0.32));
        }

        [Test]
        public void When_MaxPrecision_IsEmpty_Then_MaxPrecisionMatch_Is_Number_Of_Elements()
        {
            var WebContext = new WebContext(new object[] { test3.A3, test2.ALL });
            var maxPrecision = new Dictionary<string, double>();

            Assert.That(WebContext.MaxPrecisionMatch(maxPrecision), Is.EqualTo(2));
        }

        [Test]
        public void When_All_IsEmpty_Then_MaxPrecisionMatch_Is_0()
        {
            var WebContext = new WebContext();
            var maxPrecision = new Dictionary<string, double>();

            Assert.That(WebContext.MaxPrecisionMatch(maxPrecision), Is.EqualTo(0));
        }

        [Test]
        public void When_MaxPrecision_has_OneElement_same_as_WebContext_Then_MaxPrecisionMatch_Is_One()
        {
            var WebContext = new WebContext(new object[] { test3.A3 });
            var maxPrecision = new Dictionary<string, double>();
            maxPrecision.Add(typeof(test3).FullName, 1 / (double)3);

            Assert.That(WebContext.MaxPrecisionMatch(maxPrecision), Is.EqualTo(1));
        }

        [Test]
        public void When_MaxPrecision_has_OneElement_less_then_WebContext_Then_MaxPrecisionMatch_Is_Zero()
        {
            var WebContext = new WebContext(new object[] { test3.A3 | test3.B3});
            var maxPrecision = new Dictionary<string, double>();
            maxPrecision.Add(typeof(test3).FullName, 1 / (double)3);

            Assert.That(WebContext.MaxPrecisionMatch(maxPrecision), Is.EqualTo(0));
        }


        [Test]
        public void When_MaxPrecision_has_OneElement_more_then_WebContext_Then_MaxPrecisionMatch_Is_One()
        {
            var WebContext = new WebContext(new object[] { test3.A3 });
            var maxPrecision = new Dictionary<string, double>();
            maxPrecision.Add(typeof(test3).FullName, 2 / (double)3);

            Assert.That(WebContext.MaxPrecisionMatch(maxPrecision), Is.EqualTo(1));
        }


        [Test]
        public void When_MaxPrecision_has_less_elements_then_MaxPrecisionMatch_Counts_Them()
        {
            var WebContext = new WebContext(new object[] { test3.A3, test1.ALL });
            var maxPrecision = new Dictionary<string, double>();
            maxPrecision.Add(typeof(test3).FullName, 2 / (double)3);

            Assert.That(WebContext.MaxPrecisionMatch(maxPrecision), Is.EqualTo(2));
        }

        [Test]
        public void When_MaxPrecision_has_new_element_with_value_1_Then_MaxPrecisionMatch_Count_It()
        {
            var WebContext = new WebContext(new object[] { test3.A3 });
            var maxPrecision = new Dictionary<string, double>();
            maxPrecision.Add(typeof(test1).FullName, 1);

            Assert.That(WebContext.MaxPrecisionMatch(maxPrecision), Is.EqualTo(2));
        }

        [Test]
        public void When_MaxPrecision_has_new_element_with_value_Less_Then_1_Then_MaxPrecisionMatch_Does_Not_Count_It()
        {
            var WebContext = new WebContext(new object[] { test3.A3 });
            var maxPrecision = new Dictionary<string, double>();
            maxPrecision.Add(typeof(test1).FullName, 0.5);

            Assert.That(WebContext.MaxPrecisionMatch(maxPrecision), Is.EqualTo(1));
        }


    }
}

