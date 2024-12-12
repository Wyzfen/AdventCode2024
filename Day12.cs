using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day12
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        readonly string [] values = Utils.StringsFromFile($"{day}.txt");

        private readonly string [] test = {
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
        
        private static IEnumerable<(int perimeter, int area)> GetFences(string[] input)
        {
            char [][] data = input.Select(line => line.ToCharArray()).ToArray();
            
            foreach(var (location, item) in data.Iterate2DArray().Where(d => d.item <= 'Z'))
            {
                yield return FloodFill(location, item, ref data);
            }
        }

        private static (int perimeter, int area) FloodFill(Vector2 location, char item, ref char[][] data)
        {
            bool IsValid(int x, int y, char[][] data) => data.InBounds(x, y) && data[y][x] == item;
            
            if (!IsValid(location.X, location.Y, data)) return (0, 0);

            var altItem = (char)(item + 32);
            var perimeter = 0;
            var area = 0;
            
            var unvisited = new Queue<Vector2>();
            unvisited.Enqueue(location);

            while (unvisited.Count > 0)
            {
                var (x, y) = unvisited.Dequeue();
                if(!IsValid(x, y, data)) continue; // could have visited something in this span
                
                var lx = x - 1;
                perimeter += 2;
                
                while (IsValid(lx, y, data))
                {
                    data[y][lx] = altItem;
                    area++;
                    lx--;
                }

                while(IsValid(x, y, data))
                {
                    data[y][x] = altItem;
                    area++;
                    x++;
                }

                Scan(lx + 1, x - 1, y + 1, data);
                Scan(lx + 1, x - 1, y - 1, data);
            }
            
            //Console.WriteLine($"{item} @ {location} ->  area: {area}, perimeter: {perimeter} = {perimeter * area}");
            return (perimeter, area);

            void Scan(int sx, int ex, int y, char [][] data)
            {
                var added = false;
                
                for (int x = sx; x <= ex; x++)
                {
                    if (!IsValid(x, y, data))
                    {
                        added = false;
                        if(!data.InBounds(x, y) || data[y][x] != altItem) perimeter++;
                    }
                    else if (!added)
                    {
                        unvisited.Enqueue(new Vector2(x, y));
                        added = true;
                    }
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
            int result = 0;

            Assert.AreEqual(result, 4267809);
        }
    }
}
