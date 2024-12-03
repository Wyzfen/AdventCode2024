using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{

    [TestClass]
    public class Day03
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        private readonly string values = File.ReadAllText($"{day}.txt");

        private string test = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";
            
        [TestMethod]
        public void Problem1()
        {
            Regex regex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");

            int result = regex.Matches(values)
                              .Sum(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value));

            Assert.AreEqual(result, 167090022);
        }

        [TestMethod]
        public void Problem2()
        {
            Regex regex = new Regex(@"(?<op>mul)\((\d{1,3}),(\d{1,3})\)|(?<op>do)\(\)|(?<op>don\'t)\(\)");

            bool active = true;
            int result = regex.Matches(values)
                .Sum(m =>
                {
                    switch (m.Groups["op"].Value)
                    {
                        case "mul":
                            return active ?  int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value) : 0;
                        case "do": 
                            active = true;
                            break;  
                        case "don't":
                            active = false; 
                            break;
                    }

                    return 0;
                });

            Assert.AreEqual(result, 89823704);
        }
    }
}
