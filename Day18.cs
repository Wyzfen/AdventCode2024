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
            var input = test;

            var output = Utils.BreadthFirstSearch(Vector2.Zero, new Vector2(7, 7), (v, _) => !input.Contains(v));
            int result = output[6][6];

            Assert.AreEqual(result, 4267809);            
        }

        [TestMethod]
        public void Problem2()
        {
            int result = 0;

            Assert.AreEqual(result, 4267809);
        }
    }
}
