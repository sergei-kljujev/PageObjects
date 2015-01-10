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

    [WebControlExport(typeof(IMyPage))]
    public class MyPage : WebControl, IMyPage 
    {
        public void AssertIsVisible()
        {
            
        }

        public IMyPage DoSomething()
        {
            return this;
        }

        public virtual string GetInfo() {
            return this.GetType().FullName;
        }
    }

    [WebControlExport(typeof(IMyPage), PageTechnology.SL)]
    public class MyNonMatchedPage : MyPage
    { }

    [WebControlExport(typeof(IMyPage), PageTechnology.Asp)]
    public class MyRestrictedPage1 : MyPage
    {
        public override string GetInfo()
        {
            return "Restricted overriden " + this.GetType().FullName;
        }
    }


    [WebControlExport(typeof(IMyPage), PageTechnology.Asp, MyContext.test1)]
    public class MyRestrictedPage2 : MyRestrictedPage1, IMyPage
    {

        public string GetInfo()
        {
            return "Restricted overriden 2 " + this.GetType().FullName;
        }
    }
}
