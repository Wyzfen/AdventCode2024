using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day21
    {
        private readonly string[] values = ["341A", "803A", "149A", "683A", "208A"]; 
        private readonly string[] test = ["029A", "980A", "179A", "456A", "379A"];

        private static Dictionary<char, Vector2> digitPad = new()
        {
            { 'A', new Vector2(2, 3) },
            { '0', new Vector2(1, 3) },
            { '1', new Vector2(0, 2) },
            { '2', new Vector2(1, 2) },
            { '3', new Vector2(2, 2) },
            { '4', new Vector2(0, 1) },
            { '5', new Vector2(1, 1) },
            { '6', new Vector2(2, 1) },
            { '7', new Vector2(0, 0) },
            { '8', new Vector2(1, 0) },
            { '9', new Vector2(2, 0) },
            { ' ', new Vector2(0, 3) }
        };

        private static Dictionary<char, Vector2> arrowPad = new()
        {
            { 'A', new Vector2(2, 0) },
            { '^', new Vector2(1, 0) },
            { '<', new Vector2(0, 1) },
            { 'v', new Vector2(1, 1) },
            { '>', new Vector2(2, 1) },
            { ' ', new Vector2(0, 0) }
        };

        private IEnumerable<Vector2> GetMovements(char start, IEnumerable<char> targets, Dictionary<char, Vector2> pads)
        {
            var current = pads[start];
            var space = pads[' '];
            
            foreach (var target in targets)
            {
                var next = pads[target];
                var delta = next - current;
                
                // should be returning an enumeration of steps for each item. e.g. IEnumerable<IEnumerable<Vector2>>
                // also, the below is all wrong - doesn't handle the end points well.
                
                if ((current.X == space.X || next.X == space.X) && (current.Y == space.Y || next.Y == space.Y))
                {
                    // Crosses space - not allowed
                    if (current.Y == space.Y) // do Y first
                    {
                        for (var y = current.Y; y != next.Y; y += delta.Y)
                        {
                            yield return delta.Y > 0 ? Vector2.Down : Vector2.Up;
                        }

                        for (var x = current.X; x != next.X + delta.X; x += delta.X)
                        {
                            yield return delta.X > 0 ? Vector2.Right : Vector2.Left;
                        }

                        continue;
                    }
                }

                for (var x = current.X; x != next.X; x += delta.X)
                {
                    yield return delta.X > 0 ? Vector2.Right : Vector2.Left;
                }

                for (var y = current.Y; y != next.Y + delta.Y; y += delta.Y)
                {
                    yield return delta.Y > 0 ? Vector2.Down : Vector2.Up;
                }
            
                current = next;
            }
        }

        
        [TestMethod]
        public void Problem1()
        {
            var input = test;

            var targets = GetMovements('A', input[0], digitPad).ToArray();
            
            int result = 0;

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
