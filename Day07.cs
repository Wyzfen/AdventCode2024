using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day07
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name
            .ToLower();

        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        private readonly string[] test = new[]
        {
            "190: 10 19",
            "3267: 81 40 27",
            "83: 17 5",
            "156: 15 6",
            "7290: 6 8 6 15",
            "161011: 16 10 13",
            "192: 17 8 14",
            "21037: 9 7 18 13",
            "292: 11 6 16 20"
        };

        IEnumerable<(ulong target, int[] values)> ParseValues(IEnumerable<string> input) =>
            input
                .Select<string, (ulong target, string values)>(value => Utils.FromString<ulong, string>(value, ":"))
                .Select(x => (x.target, Utils.IntsFromString(x.values, " ")
                    .ToArray()));

        [TestMethod]
        public void Problem1()
        {
            checked
            {
                ulong result = 0;
                var inputs = ParseValues(values);

                foreach (var (target, values) in inputs)
                {
                    var count = 1 << (values.Length - 1);
                    for (int p = 0; p < count; p++)
                    {
                        ulong output = (ulong)values[0];

                        for (int i = 1; i < values.Length; i++)
                        {
                            int q = 1 << (i - 1);
                            output = (p & q) == 0 ? output * (ulong)values[i] : output + (ulong)values[i];
                            if (output > target) break;
                        }

                        if (output == target)
                        {
                            result += output;
                            break;
                        }
                    }
                }

                Assert.AreEqual(result, 945512582195u);
            }
        }

        [TestMethod]
        public void Problem2()
        {
            checked
            {
                ulong result = 0;
                var inputs = ParseValues(values);

                foreach (var (target, values) in inputs)
                {
                    var count = (int) Math.Pow(3, values.Length - 1);
                    for (var p = 0; p < count; p++)
                    {
                        ulong output = (ulong)values[0];

                        int j = p;
                        for (int i = 1; i < values.Length; i++)
                        {
                            output = (j % 3) switch
                            {
                                0 => output * (ulong)values[i],
                                1 => output + (ulong)values[i],
                                2 => ulong.Parse(output.ToString() + values[i]),
                            };
                            j /= 3;   

                            if (output > target || output == 0) break;
                        }

                        if (output == target)
                        {
                            result += output;
                            break;
                        }
                    }
                }

                Assert.AreEqual(result, 271691107779347u);
            }
        }
    }
}