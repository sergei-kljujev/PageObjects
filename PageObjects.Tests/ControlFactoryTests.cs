using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Moq;
using NUnit.Framework;
using PageObjects.Attributes;
using PageObjects.Context;
using PageObjects.Controls;
using PageObjects.Exceptions;
using PageObjects.Factory;

namespace PageObjects.Tests
{
    [TestFixture]
    public class ControlFactoryTests
    {
        [Flags]
        [ContextElement((int)ALL)]
        public enum test1
        {
            A1 = 1,
            B1 = 2,
            ALL = 3
        }

        [Flags]
        [ContextElement((int)ALL)]
        public enum test2
        {
            A2 = 1,
            B2 = 2,
            ALL = 3
        }
        
        
        [Flags]
        [ContextElement((int)ALL)]
        public enum test3
        {
            A3 = 1,
            B3 = 2,
            C3 = 4,
            ALL = 7
        }

        private Mock<IWebContext> CurrentContext;

        private ControlFactory TestFactory;

        [SetUp]
        public void Compose()
        {
            CurrentContext = new Mock<IWebContext>();


            var currentContext = new List<ContextElement>();
            currentContext.Add(new ContextElement(typeof(test1), (uint)test1.A1, (uint)test1.ALL));
            currentContext.Add(new ContextElement(typeof(test2), (uint)test2.A2, (uint)test2.ALL));
            currentContext.Add(new ContextElement(typeof(test3), (uint)test3.A3, (uint)test3.ALL));

            CurrentContext.Setup(x => x.ContextElements).Returns(currentContext);

            TestFactory = new ControlFactory();

            var c = new CompositionContainer();
            c.ComposeExportedValue("CurrentContext", CurrentContext.Object);
            c.ComposeParts(TestFactory);

        }

        public interface ITestPage : IPageObject
        {
            ITestPage DoSomething();
        };


        [Test]
        public void When_One_Page_Any_Supported_Then_It_Is_Returned()
        {
            // Arrange
            var _t = new Dictionary<Type, IWebContext>();
            _t.Add(typeof (string),new WebContext());


            CurrentContext.Setup(x => x.Match(It.IsAny<IWebContext>())).Returns(true);

            // act
            var actual = TestFactory.GetImplementation<ITestPage>(_t);
            
            // assert
            Assert.That(actual, Is.EqualTo(typeof(string)));
        }

        [Test]
        [ExpectedException(typeof(MissingControlException))]
        public void When_Context_Not_Match_Then_MissingControlException()
        {
            // Arrange
            var _t = new Dictionary<Type, IWebContext>();
            
                _t.Add(
                    typeof(string),
                    new WebContext());
            


            CurrentContext.Setup(x => x.Match(It.IsAny<IWebContext>())).Returns(false);

            // act
            TestFactory.GetImplementation<ITestPage>(_t);
        }

        [Test]
        [ExpectedException(typeof(AmbiguousControlsException))]
        public void When_Two_Pages_With_Same_Context_Then_AmbiguousControlException()
        {
            // Arrange
            var _t = new Dictionary<Type, IWebContext>();
            
                _t.Add(
                    typeof (string),
                    new WebContext());
                _t.Add(
                    typeof(int),
                    new WebContext());

            


            CurrentContext.Setup(x => x.Match(It.IsAny<IWebContext>())).Returns(true);

            // act
            TestFactory.GetImplementation<ITestPage>(_t);
        }

        [Test]
        [ExpectedException(typeof(AmbiguousControlsException))]
        public void When_Two_Pages_With_Contradicted_Context_Then_AmbiguousControlException()
        {
            // Arrange
            var _t = new Dictionary<Type, IWebContext>();
            {
                _t.Add(
                    typeof (string),
                    new WebContext(new List<ContextElement>{ new ContextElement(typeof(test1), (uint)test1.A1, (uint)test1.ALL) }));
                _t.Add(
                    typeof (int),
                    new WebContext(new List<ContextElement> { new ContextElement(typeof(test2), (uint)test2.A2, (uint)test2.ALL) }));
            };


            

            CurrentContext.Setup(x => x.Match(It.IsAny<IWebContext>())).Returns(true);

            // act
            TestFactory.GetImplementation<ITestPage>(_t);
        }

        [Test]
        public void When_Two_Pages_Diff_Context_Then_Most_Restricted_Returned()
        {
            // Arrange
            var _t = new Dictionary<Type, IWebContext>();
            
                _t.Add(
                    typeof (string),
                    new WebContext(new List<ContextElement> { new ContextElement(typeof(test1), (uint)(test1.A1 | test1.B1), (uint)test1.ALL) }));
                _t.Add(
                    typeof (int),
                    new WebContext(new List<ContextElement> { new ContextElement(typeof(test1), (uint)test1.A1, (uint)test1.ALL) }));

            

            

            CurrentContext.Setup(x => x.Match(It.IsAny<IWebContext>())).Returns(true);

            // act
            var actual = TestFactory.GetImplementation<ITestPage>(_t);

            Assert.That(actual, Is.EqualTo(typeof(int)));
        }

    }
}
