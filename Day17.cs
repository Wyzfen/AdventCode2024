using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day17
    {
        private readonly Program values = new(53437164, 0, 0, [2, 4, 1, 7, 7, 5, 4, 1, 1, 4, 5, 5, 0, 3, 3, 0]);
        private readonly Program test = new(729, 0, 0, [0, 1, 5, 4, 3, 0]);
        private readonly Program test2 = new(2024, 0, 0, [0, 3, 5, 4, 3, 0]);

        private record struct Program(ulong A, ulong B, ulong C, int[] instructions)
        {
            private enum OpCodes
            {
                adv,
                bxl,
                bst,
                jnz,
                bxc,
                @out,
                bdv,
                cdv
            };

            public IEnumerable<int> Execute() 
            {
                ulong a = A, b = B, c = C;

                int Combo(int operand) =>
                    operand switch
                    {
                        <4 => operand,
                        4 => (int)a,
                        5 => (int)b,
                        6 => (int)c
                    };
                
                for(var sp = 0; sp < instructions.Length; sp += 2)
                {
                    var operand = instructions[sp + 1];
                    switch ((OpCodes)instructions[sp])
                    {
                        case OpCodes.adv:
                            a >>= Combo(operand);
                            break;
                        case OpCodes.bxl:
                            b ^= (ulong)operand;
                            break;
                        case OpCodes.bst:
                            b = (ulong) Combo(operand) % 8;
                            break;
                        case OpCodes.jnz:
                            if (a != 0) sp = operand - 2;
                            break;
                        case OpCodes.bxc:
                            b ^= c;
                            break;
                        case OpCodes.@out:
                            yield return Combo(operand) % 8;
                            break;
                        case OpCodes.bdv: 
                            b = a >> Combo(operand);
                            break;
                        case OpCodes.cdv: 
                            c = a >> Combo(operand);
                            break;
                    }
                }
            }
        }
        
        [TestMethod]
        public void Problem1()
        {           
            var program = values;
            var result = string.Join(',', program.Execute());

            Assert.AreEqual(result, "2,1,0,4,6,2,4,2,0" );
        }

        //             ulong a = Convert.ToInt64("3774103210000000", 8);

        [TestMethod]
        public void Problem2b()
        {
            var program = values;
            
            void Recurse(int x, int[] input)
            {
                if (x == input.Length)
                {
                    Console.WriteLine(string.Join(",", input));
                    Assert.Fail();
                }
                
                var programInstructions = program.instructions[^(x+1)..];
                
                for (var v = 0; v < 8; v++)
                {
                    input[x] = v;
                    
                    var str = string.Join("", input);
                    var a = Convert.ToUInt64(str, 8);
                    
                    var result = (program with {A = a}).Execute().ToArray();
                    if (result.Length <= x) continue;
                    var items = result[^(x + 1)..];
                    
                    if (items.SequenceEqual(programInstructions))
                    {
                        Console.WriteLine($"Digit {x}, using {v} works, {str} -> {string.Join("", items)}");
                        Recurse(x + 1, input);
                    }
                }
            }

            Recurse(0, new int[program.instructions.Length]);
        }

        [TestMethod]
        public void Problem2()
        {
            var program = test2;
            long length = program.instructions.Length;
            long a = 0;

            Assert.AreEqual(a, 117440); //  14892282347106 too low,
                                        // 119138258776848 too high
        }
    }
}
