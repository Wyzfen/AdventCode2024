using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day13
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name
            .ToLower();

        readonly string[] values = Utils.StringsFromFile($"{day}.txt");

        readonly string[] test =
        [
            "Button A: X+94, Y+34",
            "Button B: X+22, Y+67",
            "Prize: X=8400, Y=5400",
            "",
            "Button A: X+26, Y+66",
            "Button B: X+67, Y+21",
            "Prize: X=12748, Y=12176",
            "",
            "Button A: X+17, Y+86",
            "Button B: X+84, Y+37",
            "Prize: X=7870, Y=6450",
            "",
            "Button A: X+69, Y+23",
            "Button B: X+27, Y+71",
            "Prize: X=18641, Y=10279",
        ];

        private static IEnumerable<(Vector2Long a, Vector2Long b, Vector2Long p)> ParseInput(string[] test)
        {
            return test.Partition(4).Select(set => set.Select(Vector2Long.Parse).ToList().ToTuple3());
        }

        private static (long a, long b) CalculateIntersection(Vector2Long a, Vector2Long b, Vector2Long p)
        {
            checked
            {
                var an = p.X * b.Y - b.X * p.Y;
                var bn = p.X * a.Y - a.X * p.Y;
                var d = a.X * b.Y - a.Y * b.X;

                if (an % d == 0 && bn % d == 0) return (an / d, -bn / d);

                return (-1, -1);
            }
        }

        [TestMethod]
        public void Problem1()
        {        
            var input = ParseInput(values);
            long result = input.Select(i => CalculateIntersection(i.a, i.b, i.p))
                    .Where(r => r.a >= 0 & r.b >= 0 & r.a <= 100 & r.b <= 100)
                    .Sum(r => r.a * 3 + r.b);

            Assert.AreEqual(result, 35729);            
        }

        [TestMethod]
        public void Problem2()
        {
            var error = Vector2Long.One * 10000000000000;
            var input = ParseInput(values);
            input = input.Select(i => (i.a, i.b, i.p + error)).ToList();
            long result = input.Select(i => CalculateIntersection(i.a, i.b, i.p))
                .Where(r => r.a >= 0 & r.b >= 0)
                .Sum(r => r.a * 3 + r.b);

            Assert.AreEqual(result, 88584689879723);  
        }
    }
}
