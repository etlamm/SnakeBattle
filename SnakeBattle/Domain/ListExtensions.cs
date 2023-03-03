using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    static class ListExtensions
    {
        public static List<T> Shuffle<T>(this List<T> list, Random random)
        {
            var result = list.ToList();
            int n = result.Count;
            while (n > 1)
            {
                n--;
                var k = random.Next(n + 1);
                (result[k], result[n]) = (result[n], result[k]);
            }
            return result;
        }
    }
}
