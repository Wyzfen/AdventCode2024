using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day05
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        readonly string [] values = File.ReadAllLines($"{day}.txt");
        
        static (MultiMap<int, int> rules, int [][] pages) ParseInput(string[] input) =>
        (
            rules: input.Where(x => x.Contains('|')).Select(x => Utils.FromString<int, int>(x, "|")).ToMultiMap(),
            pages: input.Where(x => x.Contains(',')).Select(x => Utils.FromString<int>(x, ",").ToArray()).ToArray()
        );

        private static int CompareWithRules(MultiMap<int, int> rules, int x, int y)
        {
            if (rules.TryGetValue(x, out var ruleX))
            {
                if (ruleX.Contains(y)) return -1;
            }
            if (rules.TryGetValue(y, out var ruleY))
            {
                if (ruleY.Contains(x)) return 1;
            }
            return 0;
        }

        [TestMethod]
        public void Problem1()
        {           
            int result = 0;
            
            var (rules, pages) = ParseInput(values);
            
            foreach (var page in pages)
            {
                var sorted = new List<int>(page);
                sorted.Sort((a, b) => CompareWithRules(rules, a, b));

                if (page.SequenceEqual(sorted)) result += page[page.Length/ 2];
            }

            Assert.AreEqual(result, 5948);            
        }

        [TestMethod]
        public void Problem2()
        {
            int result = 0;
            
            var (rules, pages) = ParseInput(values);
            
            foreach (var page in pages)
            {
                var sorted = new List<int>(page);
                sorted.Sort((a, b) => CompareWithRules(rules, a, b));

                if (!page.SequenceEqual(sorted)) result += sorted[page.Length/ 2];
            }

            Assert.AreEqual(result, 3062);      
        }
    }
}
