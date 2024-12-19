using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day18
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        readonly IEnumerable<Vector2> values = Utils.FromFile<Vector2>($"{day}.txt");
        private readonly Vector2[] test =
        [
            new(5, 4), new(4, 2), new(4, 5), new(3, 0), new(2, 1), new(6, 3), new(2, 4), new(1, 5), new(0, 6),
            new(3, 3), new(2, 6), new(5, 1), new(1, 2), new(5, 5), new(2, 5), new(6, 5), new(1, 4), new(0, 4),
            new(6, 4), new(1, 1), new(6, 1), new(1, 0), new(0, 5), new(1, 6), new(2, 0),
        ];

        
        [TestMethod]
        public void Problem1()
        {
            var input = values.ToList();

            var output = Utils.BreadthFirstSearch(Vector2.Zero, new Vector2(71, 71), (v, _) => input.IndexOf(v) is < 0 or > 1024);
            int result = output[70][70];

            Assert.AreEqual(result, 304);            
        }

        [TestMethod]
        public void Problem2()
        {
            var input = values.ToList();
            var result = Vector2.Zero;

            var output = Utils.BreadthFirstSearch(Vector2.Zero, new Vector2(71, 71), (v, _) => input.IndexOf(v) is < 0 or > 1024);
            var path = Utils.PathFromBFS(Vector2.Zero, new Vector2(70, 70), output).ToList();
            
            for(var i = 1025; i < input.Count; i++)
            {
                var next = input[i];
                if (!path.Contains(next)) continue; // only recheck the path if something blocks the current path
                
                output = Utils.BreadthFirstSearch(Vector2.Zero, new Vector2(71, 71),
                    (v, _) => input.IndexOf(v) is var index && (index < 0 || index > i));

                if (output[70][70] == int.MaxValue)
                {
                    result = input[i];
                    break;
                }
                
                path = Utils.PathFromBFS(Vector2.Zero, new Vector2(70, 70), output).ToList();
            }
            
            Assert.AreEqual(result, new Vector2(50, 28));   
        }
    }
}
