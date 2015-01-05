using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Moq;
using NUnit.Framework;
using PageObjects.Attributes;
using PageObjects.Context;
using PageObjects.Controls;
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

        [SetUp]
        public void Compose()
        {
            
        }

        interface ITestPage : IPageObject
        {
            ITestPage DoSomething();
        };

        //[WebControlExport(typeof(ITestPage), test1.ALL, test2.ALL, test3.ALL)]
        //class tp : ITestPage 
        //{
        //    ITestPage instance;
        //    public tp()
        //    {
        //        instance = Mock.Of<ITestPage>();
        //    }

        //    public ITestPage DoSomething()
        //    {
        //        return instance.DoSomething();
        //    }

        //    public void AssertIsVisible()
        //    {
        //        instance.AssertIsVisible();
        //    }
        //}

        [Test]
        public void When_One_Page_Any_Supported_Then_It_Is_Returned()
        {
            var _t = new List<Lazy<Type, IWebControlMetadata>>();
            var pageMock = new Mock<ITestPage>();

            _t.Add(new Lazy<Type, IWebControlMetadata>(
                () => pageMock.Object.GetType(),
                new WebControlExportAttribute(typeof(ITestPage), test1.ALL)));

            // Arrange
            var TestFactory = new ControlFactory();

            var FullContext = new Mock<IWebContext>();
            var CurrentContext = new Mock<IWebContext>();
            
            var fullContext = new List<ContextElement>();
            fullContext.Add(new ContextElement(typeof(test1), (uint)test1.ALL, (uint)test1.ALL));
            fullContext.Add(new ContextElement(typeof(test2), (uint)test2.ALL, (uint)test2.ALL));
            fullContext.Add(new ContextElement(typeof(test3), (uint)test3.ALL, (uint)test3.ALL));

            var currentContext = new List<ContextElement>();
            currentContext.Add(new ContextElement(typeof(test1), (uint)test1.A1, (uint)test1.ALL));
            currentContext.Add(new ContextElement(typeof(test2), (uint)test2.A2, (uint)test2.ALL));
            currentContext.Add(new ContextElement(typeof(test3), (uint)test3.A3, (uint)test3.ALL));
            CurrentContext.Setup(x => x.ContextElements).Returns(currentContext);
            FullContext.Setup(x => x.ContextElements).Returns(fullContext);

            CompositionContainer c = new CompositionContainer();
            c.ComposeExportedValue("CurrentContext", CurrentContext.Object);
            c.ComposeExportedValue("FullContext", FullContext.Object);
            
            c.ComposeParts(TestFactory);
            TestFactory.Controls = _t;

            // act
            var actual = TestFactory.Generate<ITestPage>();
            
            // assert
            Assert.IsNotNull(actual);
        }

    }
}
