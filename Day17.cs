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

        private record struct Program(ulong A, ulong B, ulong C, uint[] instructions)
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

            public IEnumerable<uint> Execute() 
            {
                ulong a = A, b = B, c = C;

                ulong Combo(uint operand) =>
                    operand switch
                    {
                        <4 => operand,
                        4 => a,
                        5 => b,
                        6 => c
                    };
                
                for(uint sp = 0; sp < instructions.Length; sp += 2)
                {
                    var operand = instructions[sp + 1];
                    switch ((OpCodes)instructions[sp])
                    {
                        case OpCodes.adv:
                            a >>= (int)Combo(operand);
                            break;
                        case OpCodes.bxl:
                            b ^= operand;
                            break;
                        case OpCodes.bst:
                            b = Combo(operand) % 8;
                            break;
                        case OpCodes.jnz:
                            if (a != 0) sp = operand - 2;
                            break;
                        case OpCodes.bxc:
                            b ^= c;
                            break;
                        case OpCodes.@out:
                            yield return (uint) (Combo(operand) % 8);
                            break;
                        case OpCodes.bdv: 
                            b = a >> (int)Combo(operand);
                            break;
                        case OpCodes.cdv: 
                            c = a >> (int)Combo(operand);
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

        [TestMethod]
        public void Problem2()
        {
            var program = values;
            List<string> results = new();
            
            void Recurse(int x, int[] input)
            {
                if (x == input.Length)
                {
                    results.Add(string.Join("", input));
                    return;
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
                        Recurse(x + 1, input);
                    }
                }
            }

            Recurse(0, new int[program.instructions.Length]);

            var result = Convert.ToUInt64(results.Order().First(), 8);
            Assert.AreEqual(result, 109685330781408u); 
        }
    }
}
