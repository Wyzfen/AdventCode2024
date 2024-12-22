using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day22
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        readonly IEnumerable<long> values = Utils.FromFile<long>($"{day}.txt");

        private static readonly IEnumerable<long> test = [ 1, 10, 100, 2024 ];
        private static readonly IEnumerable<long> test2 = [ 1, 2, 3, 2024 ];

        private static long Hash(long input)
        {
            input = ((input <<  6) ^ input) % (1 << 24);
            input = ((input >>  5) ^ input) % (1 << 24);
            input = ((input << 11) ^ input) % (1 << 24);
            return input;
        }
        
        [TestMethod]
        public void Problem1()
        {
            long result = values.Sum(v => Enumerable.Range(0, 2000).Aggregate(v, (s, _) => Hash(s)));

            Assert.AreEqual(result, 17577894908);            
        }

        [TestMethod]
        public void Problem2()
        {

            IDictionary<int, int> counts = new Dictionary<int, int>();

            foreach (var item in values)
            {
                var hashed = Enumerable.Range(0, 1999).Scan(item, (s, _) => Hash(s)).Select(v => (int)(v % 10)).ToArray();
                var delta = hashed.Zip(hashed.Skip(1), (a, b) => b - a).ToArray();

                var subCounts = new Dictionary<int, int>();
                
                for(var i = 4; i < delta.Length; i++)
                {
                    var key = ((((delta[i - 4] + 19) % 19) * 19 + 
                                 (delta[i - 3] + 19) % 19) * 19 + 
                                 (delta[i - 2] + 19) % 19) * 19 +
                                 (delta[i - 1] + 19) % 19;

                    if (!subCounts.ContainsKey(key))
                    {
                        subCounts[key] = hashed[i];
                        counts.Increase(key, hashed[i]);
                    }
                }
            }

            var result = counts.Values.Max();

            Assert.AreEqual(result, 1931);
        }
    }
}
