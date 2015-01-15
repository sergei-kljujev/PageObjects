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



    public interface IMyPage2 : IPageObject
    {
        IMyPage2 DoSomething();

        string GetInfo();
    }


    [WebPage(typeof (IMyPage2))]
    public class MyPage2 : WebControl, IMyPage2
    {
        public void AssertIsVisible()
        {
            throw new NotImplementedException();
        }

        public IMyPage2 DoSomething()
        {
            throw new NotImplementedException();
        }

        public string GetInfo()
        {
            return "kaka";
        }
    }

    [WebPage(typeof(IMyPage))]
    public class MyPage : WebControl, IMyPage 
    {
        protected string MyInfo { get; set; }

        public void AssertIsVisible()
        {
            
        }

        public IMyPage DoSomething()
        {
            return this;
        }

        public virtual string GetInfo() {
            return this.GetType().FullName + MyInfo;
        }
    }












    [WebPage(typeof(IMyPage), PageTechnology.SL)]
    public class MyNonMatchedPage : MyPage
    { }




    [WebPage(typeof(IMyPage), PageTechnology.Asp, MyContext.ANY)]
    public class MyRestrictedPage1 : MyPage
    {
        public new string GetInfo()
        {
            return "Restricted overriden " + this.GetType().FullName;
        }
    }


    [WebPage(typeof(IMyPage), PageTechnology.Asp, MyContext.test4)]
    public class MyRestrictedPage2 : MyRestrictedPage1
    {

        public new string GetInfo()
        {
            return "Restricted overriden 2 " + this.GetType().FullName;
        }
    }

    [WebPage(typeof(IMyPage), PageTechnology.Asp, MyContext.NEW)]
    public class MyAmbiguousPage : MyRestrictedPage2
    {
        public MyAmbiguousPage()
        {
            MyInfo = " Ambiguous";
        }

        public new string GetInfo()
        {
            return "Ambiguous overriden " + this.GetType().FullName;
        }
    }
}
