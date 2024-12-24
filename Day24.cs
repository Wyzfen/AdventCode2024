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

        private static void ProcessOutputGates(Dictionary<string, uint> values, Dictionary<string, Gate> gates, char gateId)
        {
            foreach (var gate in gates.Where(n => n.Key[0] == gateId))
            {
                _ = GetValue(values, gates, gate.Key);
            }
        }

        private static ulong AccumulateBits(Dictionary<string, uint> values, char wireId)
        {
            ulong result = 0;
            foreach (var wire in values.Where(n => n.Key[0] == wireId))
            {
                var index = int.Parse(wire.Key[1..3]);
                var value = wire.Value;
                result |= (ulong)value << index;
            }

            return result;
        }
        
        
        private static int FindIndex(Dictionary<string, Gate> gates, Gate.Op op, string a, string b)
            => gates.Values.FirstIndex(g => g.op == op && ((g.A == a && g.B == b) || (g.A == b && g.B == a)));

        private static string CorrectGates(Dictionary<string, Gate> gates, Gate.Op op, ref string a, ref string b, HashSet<string> swaps)
        {
            string s = a, t = b;
            var alt = gates.Values.First(g => g.op == op && (g.A == s || g.B == s || g.A == t || g.B == t));
            var altDelta = alt.A == a || alt.A == b ? alt.B : alt.A;
            var origDelta = alt.A == a || alt.B == a ? b : a;

            if (origDelta == a)
            {
                a = altDelta;
            }
            else
            {
                b = altDelta;
            }
            
            var swapA = gates[altDelta] with {Id = origDelta};
            var swapB = gates[origDelta] with { Id = altDelta};

            Console.WriteLine($"Swapped {altDelta} and {origDelta}");
            
            gates.Remove(altDelta);
            gates.Remove(origDelta);
            
            gates.Add(swapA.Id, swapA);
            gates.Add(swapB.Id, swapB);
            
            swaps.Add(swapA.Id);
            swaps.Add(swapB.Id);
            
            return alt.Id;
        }

        [TestMethod]
        public void Problem1()
        {           
            var (values, gates) = ParseInput(this.values);
            
            ProcessOutputGates(values, gates, 'z');
            var result = AccumulateBits(values, 'z');
            Assert.AreEqual(result, 63168299811048u);
        }

        [TestMethod]
        public void Problem2()
        {
            var (values, gates) = ParseInput(this.values);
            
            var x = AccumulateBits(values, 'x');
            var y = AccumulateBits(values, 'y');

            var carrys = new string[45];

            HashSet<string> swaps = [];
            
            for (var i = 0; i < 45; i++)
            {
                var a = "x" + i.ToString("00");
                var b = "y" + i.ToString("00");

                // A XOR B
                var xor = gates.Values.First(g => g.op == Gate.Op.XOR && ((g.A == a && g.B == b) || (g.A == b && g.B == a))).Id;
                
                // A AND B
                var and = gates.Values.First(g => g.op == Gate.Op.AND && ((g.A == a && g.B == b) || (g.A == b && g.B == a))).Id;

                if (i == 0)
                {
                    carrys[0] = and;
                }

                if (i <= 0) continue;
                
                // (A XOR B) XOR C -> S
                var sIndex = FindIndex(gates, Gate.Op.XOR, xor, carrys[i - 1]);
                var s = sIndex == -1 
                    ? CorrectGates(gates, Gate.Op.XOR, ref xor, ref carrys[i - 1], swaps) 
                    : gates.Values.ElementAtOrDefault(sIndex).Id;
                    
                // (A XOR B) AND C -> ANDC 
                var andcIndex = FindIndex(gates, Gate.Op.AND, xor, carrys[i - 1]);
                var andc = andcIndex == -1
                    ? CorrectGates(gates, Gate.Op.AND, ref xor, ref carrys[i - 1], swaps)
                    : gates.Values.ElementAtOrDefault(andcIndex).Id;
                        
                // ((A XOR B) AND C) OR (A AND B) -> Carry
                var carryIndex = FindIndex(gates, Gate.Op.OR, andc, and);
                var carry = carryIndex == -1
                    ? CorrectGates(gates, Gate.Op.OR, ref andc, ref and, swaps)
                    : gates.Values.ElementAtOrDefault(carryIndex).Id;
                carrys[i] = carry;
            }

            var result = string.Join(",", swaps.Order().ToArray());
            Assert.AreEqual(result, "dwp,ffj,gjh,jdr,kfm,z08,z22,z31");
        }

    }
}
