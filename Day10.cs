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
        
        private static Vector2 [] Recurse(string [] values, Vector2 location, int target)
        {
            if (!location.InBounds(values)) return [];
            if (values.IndexBy(location) - '0' != target) return [];
            if (target == 9) return [location];
                
            target++;

            return Utils.ExecuteWith<Vector2, Vector2 []>(
                    v => Recurse(values, v, target),
                        location + Vector2.Down, 
                        location + Vector2.Up,
                        location + Vector2.Right,
                        location + Vector2.Left)
                .SelectMany(v => v).ToArray();
        }

        private static IEnumerable<Vector2[]> FindTrailheads(string[] values) 
            => values.IterateStringArray()
                .Where(v => v.character == '0')
                .Select(v => Recurse(values, v.location, 0));

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
