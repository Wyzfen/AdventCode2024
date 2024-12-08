using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day08
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        MultiMap<char, Vector2> ParseInput(IEnumerable<string> input) =>
            input.SelectMany((v,y) => v
                .Select((c, x) => (type:c, Location:new Vector2(x, y)))
                .Where(p => p.type != '.')
            ).ToMultiMap();
        
        [TestMethod]
        public void Problem1()
        {       
            var inputs = ParseInput(values);
            var outputs = new HashSet<Vector2>();
            var bounds = new Rectangle(0, 0, values.First().Length, values.Count());

            foreach (var (_, locations) in inputs)
            {
                foreach (var (a, b, _) in locations.Combinations(2))
                {
                    var delta = b - a;
                    outputs.Add(b + delta);
                    outputs.Add(a - delta);
                }
            }

            var result = outputs.Count(v => bounds.InBounds(v)); 
            Assert.AreEqual(result, 308);            
        }

        [TestMethod]
        public void Problem2()
        {
            var inputs = ParseInput(values);
            var outputs = new HashSet<Vector2>();
            var bounds = new Rectangle(0, 0, values.First().Length, values.Count());

            foreach (var (_, locations) in inputs)
            {
                foreach (var (a, b, _) in locations.Combinations(2))
                {
                    void AddLine(Vector2 start, Vector2 step)
                    {
                        do
                        {
                            outputs.Add(start);
                            start += step;
                        } while(bounds.InBounds(start));
                    }

                    var delta = b - a;
                    
                    AddLine(b, delta);
                    AddLine(a, -delta);
                }
            }

            var result = outputs.Count(); 
            Assert.AreEqual(result, 1147);      
        }
    }
}
