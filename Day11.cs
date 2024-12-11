using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day11
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        readonly ulong [] values = [4189, 413, 82070, 61, 655813, 7478611, 0, 8];

        private readonly ulong[] test = [125, 17];

        private static long SplitStonesRecursive(int count, ulong item, Dictionary<(ulong, int), long> memo)
        {
            if (count == 0) return 1;

            if (memo.TryGetValue((item, count), out var result)) return result;
            
            if (item == 0)
            {
                result = SplitStonesRecursive(count - 1, 1, memo);
            }
            else if ( item.Length() is var length && (length & 1) == 0)
            {
                var divisor = (ulong)Math.Pow(10, length >> 1);
                result = SplitStonesRecursive(count - 1, item / divisor, memo) +
                         SplitStonesRecursive(count - 1, item % divisor, memo);
            }
            else
            {
                checked
                {
                    result = SplitStonesRecursive(count - 1, item * 2024, memo);
                }
            }
            
            memo[(item, count)] = result;
            return result;
        }

        
        [TestMethod]
        public void Problem1()
        {
            var dict = new Dictionary<(ulong, int), long>();
            var result = values.Sum(v => SplitStonesRecursive(25, v, dict));
            Assert.AreEqual(result, 186203);            
        }

        [TestMethod]
        public void Problem2()
        {
            var dict = new Dictionary<(ulong, int), long>();
            long result = values.Select(v => SplitStonesRecursive(75, v, dict)).Sum();
            Assert.AreEqual(result, 221291560078593);
        }
    }
}
