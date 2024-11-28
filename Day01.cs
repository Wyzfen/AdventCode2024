using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day01
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        [TestMethod]
        public void Problem1()
        {           
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
