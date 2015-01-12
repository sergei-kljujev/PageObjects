using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageObjects.Attributes;
using PageObjects.Context.ContextElements;
using PageObjects.Controls;

namespace WebAutomation
{

    public interface IMyPage : IPageObject
    {
        IMyPage DoSomething();

        string GetInfo();
    }

    [WebPage(typeof(IMyPage))]
    public class MyPage : WebControl, IMyPage 
    {
        public void AssertIsVisible()
        {
            
        }

        public IMyPage DoSomething()
        {
            return this;
        }

        public string GetInfo() {
            return this.GetType().FullName;
        }
    }

    [WebPage(typeof(IMyPage), PageTechnology.SL)]
    public class MyNonMatchedPage : MyPage
    { }

    [WebPage(typeof(IMyPage), PageTechnology.Asp, MyContext.ANY)]
    public class MyRestrictedPage1 : MyPage, IMyPage
    {
        public string GetInfo()
        {
            return "Restricted overriden " + this.GetType().FullName;
        }
    }


    [WebPage(typeof(IMyPage), PageTechnology.Asp, MyContext.test2)]
    public class MyRestrictedPage2 : MyRestrictedPage1, IMyPage
    {

        public string GetInfo()
        {
            return "Restricted overriden 2 " + this.GetType().FullName;
        }
    }

    [WebPage(typeof(IMyPage), PageTechnology.Asp, MyContext.NEW)]
    public class MyAmbiguousPage : MyRestrictedPage2, IMyPage
    {
        public new string GetInfo()
        {
            return "Ambiguous overriden " + this.GetType().FullName;
        }
    }
}
