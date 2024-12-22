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

            IDictionary<(int, int, int, int), int> counts = new Dictionary<(int, int, int, int), int>();

            foreach (var item in values)
            {
                var hashed = Enumerable.Range(0, 1999).Scan(item, (s, _) => Hash(s)).Select(v => (int)(v % 10)).ToArray();
                var delta = hashed.SlidingWindow(2).Select(v => v.ToArray()).Select(v => v[1] - v[0]).ToArray();

                var subCounts = new Dictionary<(int, int, int, int), int>();
                var index = 4;
                foreach (var seq in delta.SlidingWindow(4))
                {
                    var arr = seq.ToArray();
                    var key = (arr[0], arr[1], arr[2], arr[3]);

                    if (!subCounts.ContainsKey(key))
                    {
                        subCounts[key] = hashed[index];
                    }

                    index++;
                }
                
                counts = counts.AddDictionaries(subCounts);
            }

            var result = counts.Values.Max();

            Assert.AreEqual(result, 1931);
        }
    }
}
