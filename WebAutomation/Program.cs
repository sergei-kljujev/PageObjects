using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PageObjects.Factory;

namespace WebAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            var f = ControlFactory.Instance;

            var p = f.Generate<IMyPage>();

            Console.WriteLine(p.GetInfo());

            Console.WriteLine("");
        }
    }
}
