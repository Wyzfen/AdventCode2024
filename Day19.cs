using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day19
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        private readonly string[] test =
        [
            "r, wr, b, g, bwu, rb, gb, br",
            "",
            "brwrr",
            "bggr",
            "gbbr",
            "rrbgbr",
            "ubwu",
            "bwurrg",
            "brgr",
            "bbrgwb",
        ];
        
        (string[] towels, string[] patterns) ParseInput(IEnumerable<string> input) =>
            (   towels: input.First().Split(", "),
                patterns:input.Skip(2).ToArray());
        
        [TestMethod]
        public void Problem1()
        {           
            var (towels, patterns) = ParseInput(values);
            
            bool Recurse(string input) => towels.Any(t => input.StartsWith(t) && (input.Length == t.Length || Recurse(input[t.Length..])));

            int result = patterns.Count(Recurse);

            Assert.AreEqual(result, 333);            
        }

        [TestMethod]
        public void Problem2()
        {
            var (towels, patterns) = ParseInput(values);
            
            long Recurse(string input, Dictionary<string, long> memo)
            {
                if (memo.TryGetValue(input, out var count)) return count;

                var total = 0L;
                foreach (var towel in towels.Where(input.StartsWith))
                {
                    count = towel.Length == input.Length ? 1 : Recurse(input[towel.Length..], memo);
                    memo.Increase(input, count);
                    total += count;
                }

                return total;
            }

            var result = patterns.Sum(p => Recurse(p, []));

            Assert.AreEqual(result, 678536865274732);
        }
    }
}
