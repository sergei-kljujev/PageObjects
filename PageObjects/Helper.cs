using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjects
{
    public static class Helper
    {
        public static int NumberOfOnes(UInt32 i)
        {
            var current = i;
            var count = 0;
            do
            {
                if (current % 2 == 1)
                    count++;
                current = current >> 1;
            } while (current != 0);

            return count;
        }

    }
}
