using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day10
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        readonly string [] values = File.ReadAllLines($"{day}.txt");

        private readonly string[] test = new[]
        {
            "89010123",
            "78121874",
            "87430965",
            "96549874",
            "45678903",
            "32019012",
            "01329801",
            "10456732",
        };
        
        private static Vector2 [] Recurse(string [] values, int x, int y, int target)
        {
            if (x < 0 || x >= values[0].Length || y < 0 || y >= values.Length) return [];
                
            int value = values[y][x] - '0';
                
            if (value != target) return [];
            if (target == 9) return [new Vector2(x, y)];
                
            target++;

            return new []{
                    Recurse(values, x, y + 1, target),
                    Recurse(values, x, y - 1, target), 
                    Recurse(values, x + 1, y, target),
                    Recurse(values, x - 1, y, target)
            }.SelectMany(v => v).ToArray();
        }

        private static Vector2[][] FindTrailheads(string[] values)
        {
            List<Vector2 []> result = [];
            for (int y = 0; y < values.Length; y++)
            {
                for (int x = 0; x < values[0].Length; x++)
                {
                    if (values[y][x] == '0')
                    {
                        result.Add(Recurse(values, x, y, 0));
                    }
                }
            }
            
            return result.ToArray();
        }
        
        [TestMethod]
        public void Problem1()
        {           
            int result = FindTrailheads(values).Select(v => v.Distinct().Count()).Sum();
            Assert.AreEqual(result, 644);            
        }

        [TestMethod]
        public void Problem2()
        {           
            int result = FindTrailheads(values).Select(v => v.Length).Sum();
            Assert.AreEqual(result, 1366);            
        }
    }
}
