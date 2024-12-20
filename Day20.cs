using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day20
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        readonly string [] values = Utils.StringsFromFile($"{day}.txt");

        private readonly string[] test =
        [
            "###############",
            "#...#...#.....#",
            "#.#.#.#.#.###.#",
            "#S#...#.#.#...#",
            "#######.#.#.###",
            "#######.#.#...#",
            "#######.#.###.#",
            "###..E#...#...#",
            "###.#######.###",
            "#...###...#...#",
            "#.#####.#.###.#",
            "#.#...#.#.#...#",
            "#.#.#.#.#.#.###",
            "#...#...#...###",
            "###############",
        ];
        
        [TestMethod]
        public void Problem1()
        {           
            var input = values;
            
            var start = input.FindChar('S');
            var end = input.FindChar('E');

            var costs = Utils.BreadthFirstSearch(start, input.Size(), (dest, _) => input.IndexBy(dest) != '#');
            var path = Utils.PathFromBFS(start, end, costs).ToArray();
            
            var result = path.Sum(step => Vector2.Directions.Count(d => input.IndexBy(step + d) == '#' &&
                                                                        step + d + d is var next && 
                                                                        costs.InBounds(next) &&
                                                                        input.IndexBy(next) != '#' && 
                                                                        costs.IndexBy(next) - costs.IndexBy(step) >= 102));

            Assert.AreEqual(result, 1375);
        }

        [TestMethod]
        public void Problem2()
        {
            var input = values;
            
            var start = input.FindChar('S');
            var end = input.FindChar('E');

            var costs = Utils.BreadthFirstSearch(start, input.Size(), (dest, _) => input.IndexBy(dest) != '#');
            var path = Utils.PathFromBFS(start, end, costs).ToArray();

            var result = 0;
            for(int s = 0; s < path.Length; s++)
            {
                var step = path[s];
                var cost = costs.IndexBy(step);
                for (int d = s + 4; d < path.Length; d++)
                {
                    var dest = path[d];
                    var distance = dest.ManhattanTo(step);
                    
                    if (distance > 20) continue;

                    if (cost - costs.IndexBy(dest) >= 100 + distance) result++;
                }
            }

            Assert.AreEqual(result, 983054); 
        }
    }
}
