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

        public interface IControl_NoImpl : IWebControl
        {
             
        }

        public interface IControl_NoMatchedImpl : IWebControl
        {

        }

        [WebPage(typeof(IControl_NoMatchedImpl), test2.B2)]
        public class Control_NoMatchedImpl : IControl_NoMatchedImpl
        {
        }

        public interface IControl_1_Impl : IWebControl
        {
        }
        
        [WebPage(typeof(IControl_1_Impl), test1.A1)]
        public class Control_1_Impl : IControl_1_Impl
        {
        }

        public interface IControl_2_Impl : IWebControl
        {
        }

        [WebPage(typeof(IControl_2_Impl))]
        public class Control_2_Impl_1 : IControl_2_Impl
        {
        }

        [WebPage(typeof(IControl_2_Impl), test1.A1)]
        public class Control_2_Impl_2 : IControl_2_Impl
        {
        }

        private Mock<ICurrentContext> CurrentContext;

        private ControlFactory TestFactory;

        [SetUp]
        public void Compose()
        {
            CurrentContext = new Mock<ICurrentContext>();


            var currentContext = new List<ContextElement>();
            currentContext.Add(new ContextElement(typeof(test1), (uint)test1.A1, (uint)test1.ALL));
            currentContext.Add(new ContextElement(typeof(test2), (uint)test2.A2, (uint)test2.ALL));
            currentContext.Add(new ContextElement(typeof(test3), (uint)test3.A3, (uint)test3.ALL));

            CurrentContext.Setup(x => x.ContextElements).Returns(currentContext);

            TestFactory = new ControlFactory();

            var c = new CompositionContainer();
            c.ComposeExportedValue("CurrentContext",CurrentContext.Object);
            c.ComposeParts(TestFactory);

        }

        public interface ITestPage : IPageObject
        {
            ITestPage DoSomething();
        };

        [Test]
        public void When_No_Implementations_Then_GetAllMatched_Returns_Empty()
        {
            var actual = TestFactory.GetAllMatchedImplementations<IControl_NoImpl>();

            Assert.IsNull(actual);
        }


        [Test]
        public void When_No_Matched_Implementations_Then_GetAllMatched_Returns_Empty()
        {
            
            CurrentContext.Setup(x => x.Match(It.IsAny<IWebContext>())).Returns(false);

            var actual = TestFactory.GetAllMatchedImplementations<IControl_NoMatchedImpl>();

            CollectionAssert.IsEmpty(actual);
        }

        [Test]
        public void When_One_Matched_Implementation_Then_GetAllMatched_Returns_It()
        {
            CurrentContext.Setup(x => x.Match(It.IsAny<IWebContext>())).Returns(true);

            var actual = TestFactory.GetAllMatchedImplementations<IControl_1_Impl>();

            CollectionAssert.AreEquivalent(new [] { typeof(Control_1_Impl)}, actual );
        }


        [Test]
        public void When_Two_Matched_Implementation_Then_GetAllMatched_Returns_Both()
        {
            CurrentContext.Setup(x => x.Match(It.IsAny<IWebContext>())).Returns(true);

            var actual = TestFactory.GetAllMatchedImplementations<IControl_2_Impl>();

            CollectionAssert.AreEquivalent(new[] { typeof(Control_2_Impl_2), typeof(Control_2_Impl_1) }, actual);
        }


        [Test]
        public void When_Two_Matched_Then_Most_Restricted_Implementation_Returned()
        {
            CurrentContext.Setup(x => x.Match(It.IsAny<IWebContext>())).Returns(true);

            var actual = TestFactory.Generate<IControl_2_Impl>();

            Assert.That(actual, Is.TypeOf(typeof(Control_2_Impl_2)));
        }

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
        public void When_Empty_Dictionary_Then_MissingControlException()
        {
            // Arrange
            var _t = new Dictionary<Type, IWebContext>();

            // act
            TestFactory.GetImplementation<ITestPage>(_t);
        }


        [Test]
        [ExpectedException(typeof(MissingControlException))]
        public void When_Null_Then_MissingControlException()
        {
            // act
            TestFactory.GetImplementation<ITestPage>(null);
        }

        [Test]
        [ExpectedException(typeof(AmbiguousControlsException))]
        public void When_Two_Pages_With_Same_Context_Then_AmbiguousControlException()
        {
            // Arrange
            var _t = new Dictionary<Type, IWebContext>();
            
            var p1_Mock = new Mock<IWebContext>();
            _t.Add(typeof (string), p1_Mock.Object);

            var p2_Mock = new Mock<IWebContext>();
            _t.Add(typeof(int), p2_Mock.Object);

            p1_Mock.Setup(x => x.Match(It.IsAny<IWebContext>())).Returns(true);
            p1_Mock.Setup(x => x.ContextPrecision(It.IsAny<Type>())).Returns(1);
            p2_Mock.Setup(x => x.ContextPrecision(It.IsAny<Type>())).Returns(1);
            p2_Mock.Setup(x => x.Match(It.IsAny<IWebContext>())).Returns(true);
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

            var p1_Mock = new Mock<IWebContext>();
            _t.Add(typeof(string), p1_Mock.Object);

            var p2_Mock = new Mock<IWebContext>();
            _t.Add(typeof(int), p2_Mock.Object);
            var p1_ContextPrecisionQueue = new Queue<double>();
            p1_ContextPrecisionQueue.Enqueue(1);
            p1_ContextPrecisionQueue.Enqueue(0.5);
            p1_ContextPrecisionQueue.Enqueue(1);
            var p2_ContextPrecisionQueue = new Queue<double>();
            p2_ContextPrecisionQueue.Enqueue(1);
            p2_ContextPrecisionQueue.Enqueue(1);
            p2_ContextPrecisionQueue.Enqueue(0.5);

            p1_Mock.Setup(x => x.Match(It.IsAny<IWebContext>())).Returns(true);
            p1_Mock.Setup(x => x.ContextPrecision(It.IsAny<Type>())).Returns(p1_ContextPrecisionQueue.Dequeue);
            p2_Mock.Setup(x => x.ContextPrecision(It.IsAny<Type>())).Returns(p2_ContextPrecisionQueue.Dequeue);
            p2_Mock.Setup(x => x.Match(It.IsAny<IWebContext>())).Returns(true);
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

        [Test]
        public void Assert_Control_Factory_Instance_Is_Available() 
        {
            var actual = ControlFactory.Instance;

            Assert.IsNotNull(actual);
        }



    }
}
