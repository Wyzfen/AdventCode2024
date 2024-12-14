using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day14
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name
            .ToLower();

        readonly IEnumerable<(Vector2 position, Vector2 velocity)> values =
            Utils.FromFile<Vector2, Vector2>($"{day}.txt");

        readonly IEnumerable<(Vector2 position, Vector2 velocity)> test = new [] {
                "p=0,4 v=3,-3",
                "p=6,3 v=-1,-3",
                "p=10,3 v=-1,2",
                "p=2,0 v=2,-1",
                "p=0,0 v=1,3",
                "p=3,0 v=-2,-2",
                "p=7,6 v=-1,-3",
                "p=3,0 v=-1,-2",
                "p=9,3 v=2,3",
                "p=7,3 v=-1,2",
                "p=2,4 v=2,-3",
                "p=9,5 v=-3,-3"
            }.Select(x => Utils.FromString<Vector2, Vector2>(x, " "));

    [TestMethod]
        public void Problem1()
        {
            var size = new Vector2(101, 103);
            var processed = values.Select(v => (v.position + v.velocity * 100) % size)
                                .GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count());
            var result = processed.Sum(v => v.Key.X < size.X / 2 && v.Key.Y < size.Y / 2 ? v.Value : 0) *
                         processed.Sum(v => v.Key.X < size.X / 2 && v.Key.Y > size.Y / 2 ? v.Value : 0) *
                         processed.Sum(v => v.Key.X > size.X / 2 && v.Key.Y < size.Y / 2 ? v.Value : 0) *
                         processed.Sum(v => v.Key.X > size.X / 2 && v.Key.Y > size.Y / 2 ? v.Value : 0);
                     
            Assert.AreEqual(result, 222901875);            
        }

        [TestMethod]
        public void Problem2()
        {
            var size = new Vector2(101, 103);
            var input = values;

            string[] DrawData(IEnumerable<(Vector2 position, Vector2 velocity)> valueTuples)
            {
                StringBuilder[] output = Enumerable.Range(0, size.Y)
                    .Select(_ => new StringBuilder(new string('.', size.X))).ToArray();
                foreach (var ((x, y), _) in valueTuples)
                {
                    output[y][x] = '*';
                }
                
                return output.Select(v => v.ToString()).ToArray();
            }

            int i = 1;
            for (; i < 100000; i++)
            {
                input = input.Select(v => ((v.position + v.velocity) % size, v.velocity)).ToArray();

                var output = DrawData(input);
                if(output.Any(o => o.Contains("**********")))
                {
                    Console.WriteLine(i);

                    foreach (var line in output)
                    {
                        Console.WriteLine(line);
                    }

                    Console.WriteLine();
                    break;
                }
            }
         
            Assert.AreEqual(i, 6243);
        }
    }
}
