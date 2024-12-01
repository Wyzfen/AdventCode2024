using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day01
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        readonly int[][] values = Utils.FromCSVFile<int>($"{day}.txt", " ").TransposeArray();

        [TestMethod]
        public void Problem1()
        {
            var (left, right, _) = values;

            var result = Sort(left)
                .Zip(Sort(right))
                .Sum(p => Math.Abs(p.First - p.Second));

            Assert.AreEqual(result, 1223326);
            return;

            int[] Sort(int[] input) => input
                .Select((v, i) => (value: v, index: i))
                .OrderBy(p => p.value)
                .ThenBy(p => p.index).Select(p => p.value).ToArray();
        }
        
        [TestMethod]
        public void Problem2()
        {
            var (left, right, _) = values;
            var leftGroup =left.GroupBy(x => x);
            var rightGroup = right.GroupBy(x => x);
            
            long result = leftGroup.Sum(g => g.Key * g.Count() * rightGroup.FirstOrDefault(g2 => g2.Key == g.Key)?.Count() ?? 0);

            Assert.AreEqual(result, 21070419);
        }
    }
}
