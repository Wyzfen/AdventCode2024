using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day17
    {
        private readonly Program values = new(53437164, 0, 0, [2, 4, 1, 7, 7, 5, 4, 1, 1, 4, 5, 5, 0, 3, 3, 0]);
        private readonly Program test = new(729, 0, 0, [0, 1, 5, 4, 3, 0]);
        private readonly Program test2 = new(2024, 0, 0, [0, 3, 5, 4, 3, 0]);

        private record struct Program(int A, int B, int C, int[] instructions)
        {
            private enum OpCodes : int
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
                int a = A, b = B, c = C;

                int Combo(int operand) =>
                    operand switch
                    {
                        <4 => operand,
                        4 => a,
                        5 => b,
                        6 => c,
                        _ => -1
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

        [TestMethod]
        public void Problem2b()
        {
            var program = values;
            int a = Convert.ToInt32("377410300", 8);
            var result = string.Join(',', (program with {A = a}).Execute());
            Assert.AreEqual(result, string.Join(',', program.instructions));
        }

        [TestMethod]
        public void Problem2()
        {
            var program = test2;
            int length = program.instructions.Length;
            int a = 0;
            
            while(a < int.MaxValue)
            {
                var inter = (program with {A = a}).Execute().GetEnumerator();
                using var inter1 = inter as IDisposable;

                int index = 0;
                
                while (inter.MoveNext())
                {
                    var output = inter.Current;
                    
                    // if (index == 0) Console.Write($"{a} -> {output}");
                    // else Console.Write($", {output}");
                    if (index >= length || output != program.instructions[index])
                    {
                        index = -1;
                        break;
                    }
                    index++;
                }
                
                //Console.WriteLine();

                if (index == length) break;
                a++;
            }

            Assert.AreEqual(a, 117440); //  14892282347106 too low,
                                        // 119138258776848 too high
        }
    }
}
