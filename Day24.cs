using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day24
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        readonly string [] values = Utils.StringsFromFile($"{day}.txt");

        record struct Gate(string Id, string A, string B, Gate.Op op)
        {
            public enum Op
            {
                AND,
                OR,
                XOR
            };
            
            private static Regex regex = new (@"^(?<a>\w+) (?<op>\w+) (?<b>\w+) -> (?<id>\w+)$");

            public static Gate FromString(string input)
            {
                var match = regex.Match(input);    
                
                return new Gate(match.Groups["id"].Value, match.Groups["a"].Value, match.Groups["b"].Value, (Op) Enum.Parse(typeof(Op), match.Groups["op"].Value));
            }

            public uint Process(uint a, uint b, Op op) =>
                op switch
                {
                    Op.AND => a & b,
                    Op.OR => a | b,
                    Op.XOR => a ^ b,
                };
        }

        private static (Dictionary<string, uint> startValues, Dictionary<string, Gate> gates) ParseInput(string [] input) =>
            (
                startValues: input.Where(s => s.Contains(':'))
                    .Select(s => s.Split(":"))
                    .ToDictionary(s => s[0], s => uint.Parse(s[1])), 
                gates: input.Where(s => s.Contains('>')).Select(Gate.FromString).ToDictionary(n => n.Id, n => n)
            );

        private static uint GetValue(Dictionary<string, uint> values, Dictionary<string, Gate> gates, string gateId)
        {
            if (values.TryGetValue(gateId, out uint value)) return value;
            var gate = gates[gateId];
            value = gate.Process(GetValue(values, gates, gate.A), GetValue(values, gates, gate.B), gate.op);
            values[gateId] = value;
            return value;
        }

        private static ulong AccumulateBits(Dictionary<string, uint> values, Dictionary<string, Gate> gates, char gateId)
        {
            ulong result = 0;
            foreach (var gate in gates.Where(n => n.Key[0] == gateId))
            {
                var index = int.Parse(gate.Key[1..3]);
                var value = GetValue(values, gates, gate.Key);
                result |= (ulong)value << index;
            }

            return result;
        }

        [TestMethod]
        public void Problem1()
        {           
            var (values, gates) = ParseInput(this.values);
            
            var result = AccumulateBits(values, gates, 'z');
            Assert.AreEqual(result, 63168299811048u);
        }

        [TestMethod]
        public void Problem2()
        {
            int result = 0;

            Assert.AreEqual(result, 4267809);
        }
    }
}
