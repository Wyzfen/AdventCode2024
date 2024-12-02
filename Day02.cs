using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day02
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name
            .ToLower();

        readonly int[][] values = Utils.FromCSVFile<int>($"{day}.txt", " ");

        readonly int[][] test =
        [
            [7, 6, 4, 2, 1],
            [1, 2, 7, 8, 9],
            [9, 7, 6, 2, 1],
            [1, 3, 2, 4, 5],
            [8, 6, 4, 4, 1],
            [1, 3, 6, 7, 9]
        ];

        private static bool IsSafe(bool rising, int previous, int current)
        {
            return (rising ? current - previous : previous - current) is >= 1 and <= 3;
        }


        [TestMethod]
        public void Problem1()
        {
            int result = 0;

            foreach (var value in values)
            {
                int previous = value[0];
                bool rising = value[1] > previous;
                bool safe = true;

                for (int i = 1; i < value.Length; i++)
                {
                    int current = value[i];
                    safe &= IsSafe(rising, previous, current);
                    if (!safe)
                    {
                        break;
                    }

                    previous = current;
                }

                if (safe) result++;
            }

            Assert.AreEqual(result, 680);
        }
        
        [TestMethod]
        public void Problem1b()
        {
            int result = values.Count(
                v => v.Sliding(2).Select(Enumerable.ToArray)   // use a sliding window to get consecutive pairs
                      .All(pair => IsSafe(v[1] > v[0], pair[0], pair[1]))  // check that all the pairs pass
                ); 
            Assert.AreEqual(result, 680);
        }

        [TestMethod]
        public void Problem2()
        {
            int result = 0;

            foreach (var value in values)
            {
                for (int skip = -1; skip < value.Length; skip++)
                {
                    int first = skip == 0 ? 1 : 0;
                    int second = skip is 0 or 1 ? 2 : 1;
                    int previous = value[first];
                    bool rising = value[second] > previous;
                    bool safe = true;

                    for (int i = second; i < value.Length; i++)
                    {
                        if (i == skip) continue;

                        int current = value[i];
                        safe &= IsSafe(rising, previous, current);
                        if (!safe)
                        {
                            break;
                        }

                        previous = current;
                    }

                    if (safe)
                    {
                        result++;
                        break;
                    }
                }
            }

            Assert.AreEqual(result, 710);
        }
        
        [TestMethod]
        public void Problem2b()
        {
            int[] Skip(IEnumerable<int> input, int skip) => input.Where((_, i) => i != skip).ToArray();
            
            int result = values.Count(v => 
                Enumerable.Range(0, v.Length).Select((_, i) => Skip(v, i)) // Repeat the test (length) times, removing one item each time
                    .Any(
                        vs => vs.Sliding(2).Select(Enumerable.ToArray) // use a sliding window to get consecutive pairs
                                .All(pair => IsSafe(vs[1] > vs[0], pair[0], pair[1])) // check that all the pairs pass
                    ));
            Assert.AreEqual(result, 710);
        }
    }
}
