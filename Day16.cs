using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day16
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        readonly string [] values = Utils.StringsFromFile($"{day}.txt");

        private readonly string[] test =
        [
            "###############",
            "#.......#....E#",
            "#.#.###.#.###.#",
            "#.....#.#...#.#",
            "#.###.#####.#.#",
            "#.#.#.......#.#",
            "#.#.#####.###.#",
            "#...........#.#",
            "###.#.#####.#.#",
            "#...#.....#.#.#",
            "#.#.#.###.#.#.#",
            "#.....#...#.#.#",
            "#.###.#.#.#.#.#",
            "#S..#.....#...#",
            "###############",
        ];

        private readonly string[] test2 =
        [
            "#################",
            "#...#...#...#..E#",
            "#.#.#.#.#.#.#.#.#",
            "#.#.#.#...#...#.#",
            "#.#.#.#.###.#.#.#",
            "#...#.#.#.....#.#",
            "#.#.#.#.#.#####.#",
            "#.#...#.#.#.....#",
            "#.#.#####.#.###.#",
            "#.#.#.......#...#",
            "#.#.###.#####.###",
            "#.#.#...#.....#.#",
            "#.#.#.#####.###.#",
            "#.#.#.........#.#",
            "#.#.#.#########.#",
            "#S#.............#",
            "#################",
        ];
        
        static bool IsValid(Ray destination, Ray current, string [] input) =>
            destination.Direction != -current.Direction &
            destination.Location.Index(input) != '#';
            
        static int MoveCost(Ray destination, Ray current)
        {
            var cost = current.Location.ManhattanTo(destination.Location);
            if (current.Direction == destination.Direction) return cost;
            return cost + 1000;
        }
        
        [TestMethod]
        public void Problem1()
        {
            var input = values;
            
            var start = input.FindChar('S');
            var end = input.FindChar('E');

            var costs = Utils.BreadthFirstSearchDirection(new Ray(start, Vector2.Right), input.Size(), (a, b) => IsValid(a, b, input), MoveCost);
            var result = end.Index(costs);
            
            Assert.AreEqual(result, 99488);    
        }

        [TestMethod]
        public void Problem2()
        {
            var input = test2;
            var start = input.FindChar('S');
            var end = input.FindChar('E');

            var costs = Utils.BreadthFirstSearchDirection(new Ray(start, Vector2.Right), input.Size(), (a, b) => IsValid(a, b, input), MoveCost);

            bool IsLess(Vector2 destination, Vector2 current) => 
                (1 + current.Index(costs) % 1000 == destination.Index(costs) % 1000) 
            &&  (current.Index(costs) / 1000 >= destination.Index(costs) / 1000);

            var visits = Utils.BreadthFirstSearch(end, input.Size(), IsLess);
            
            
            var result = visits.Sum(s => s.Count(v => v != int.MaxValue));
            
            Console.WriteLine(Debug(visits, input));
            
            
            
            Assert.AreEqual(result, 587);    // 587 too high
        }

        string Debug(int[][] source, string [] input)
        {
            var result = "";
            for (int y = 0; y < source.Length; y++)
            {
                for (int x = 0; x < source[y].Length; x++)
                {
                    result += (source[y][x], input[y][x]) switch
                    {
                        (_, '#') => '#',
                        ('#', _) => '.',
                        (_, _) => '0',
                    };
                }
                result += "\n";
            }
            return result;
        }
    }
}
