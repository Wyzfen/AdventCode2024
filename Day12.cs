using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day12
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name
            .ToLower();

        readonly string[] values = Utils.StringsFromFile($"{day}.txt");

        private readonly string[] test =
        {
            "RRRRIICCFF",
            "RRRRIICCCF",
            "VVRRRCCFFF",
            "VVRCCCJFFF",
            "VVVVCJJCFE",
            "VVIVCCJJEE",
            "VVIIICJJEE",
            "MIIIIIJJEE",
            "MIIISIJEEE",
            "MMMISSJEEE"
        };

        private readonly string[] test2 =
        {
            "OOOOO",
            "OXOXO",
            "OOOOO",
            "OXOXO",
            "OOOOO",
        };
        
        private readonly string[] test3 =
        {
            "EEEEE",
            "EXXXX",
            "EEEEE",
            "EXXXX",
            "EEEEE",
        };
        
        private static IEnumerable<(int perimeter, int area, int sections)> GetFences(string[] input)
        {
            char [][] data = input.Select(line => line.ToCharArray()).ToArray();

            foreach(var (location, item) in data.Iterate2DArray().Where(d => d.item <= 'Z'))
            {
                yield return FloodFill(location, item, data);
            }
        }

        private static (int perimeter, int area, int sections) FloodFill(Vector2 location, char item, char[][] data)
        {
            char GetValue(int x, int y) => data.InBounds(x, y) ? data[y][x] : '.';
            bool IsTarget(int x, int y) => GetValue(x, y) == item;
            bool IsTargetPlus(int x, int y) => (GetValue(x, y) & ~32) == item;

            
            if (!IsTarget(location.X, location.Y)) return (0, 0, 0);

            var altItem = (char)(item + 32);
            var perimeter = 0;
            var area = 0;
            var sections = 0;
            
            var unvisited = new Queue<Vector2>();
            unvisited.Enqueue(location);

            while (unvisited.Count > 0)
            {
                var (x, y) = unvisited.Dequeue();
                if(!IsTarget(x, y)) continue; // could have visited something in this span
                
                var lx = x - 1;
                perimeter += 2;
                
                while (IsTarget(lx, y))
                {
                    data[y][lx] = altItem;
                    area++;
                    lx--;
                }
                
                if( !IsTargetPlus(lx + 1, y - 1) || 
                     IsTargetPlus(lx, y - 1)) sections++;

                while(IsTarget(x, y))
                {
                    data[y][x] = altItem;
                    area++;
                    x++;
                }
                
                if( !IsTargetPlus(x - 1, y - 1) ||
                     IsTargetPlus(x, y - 1)) sections++;


                Scan(lx + 1, x - 1, y + 1);
                Scan(lx + 1, x - 1, y - 1);
            }
            
            Console.WriteLine($"{item} @ {location} ->  area: {area}, perimeter: {perimeter}, sections: {sections} = {perimeter * area} or {sections * area}");
            return (perimeter, area, sections);

            void Scan(int sx, int ex, int y)
            {
                var added = false;
                bool inSection = false;
                
                for (int x = sx; x <= ex; x++)
                {
                    if (!IsTarget(x, y))
                    {
                        added = false;
                        inSection = true;
                        if (GetValue(x, y) != altItem)
                        {
                            perimeter++;
                        }
                    }
                    else if (!added)
                    {
                        if (inSection)
                        {
                            inSection = false;
                        //    sections++;
                        }
                        unvisited.Enqueue(new Vector2(x, y));
                        added = true;
                    }
                }
                
                if (inSection)
                {
                  //  sections++;
                }
            }
        }

        [TestMethod]
        public void Problem1()
        {           
            int result = GetFences(values).Sum(f => f.area * f.perimeter);

            Assert.AreEqual(result, 1304764); 
        }

        [TestMethod]
        public void Problem2()
        {
            int result = GetFences(values).Sum(f => f.area * f.sections) +
                         GetFences(values.Rotate()).Sum(f => f.area * f.sections);
            Assert.AreEqual(result, 811148);
        }
    }
}
