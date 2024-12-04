using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day04
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        private readonly string [] values = Utils.StringsFromFile($"{day}.txt");

        private readonly string[] test = new[]
        {
            "MMMSXXMASM",
            "MSAMXMSMSA",
            "AMXSXMAAMM",
            "MSAMASMSMX",
            "XMASAMXAMM",
            "XXAMMXXAMA",
            "SMSMSASXSS",
            "SAXAMASAAA",
            "MAMMMXMMMM",
            "MXMXAXMASX"
        };
        

        
        [TestMethod]
        public void Problem1()
        {           
            string xmas = "XMAS";

            Vector2[] directions = {
                new Vector2(0, -1),
                new Vector2(1, -1),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1),
                new Vector2(-1, 1),
                new Vector2(-1, 0),
                new Vector2(-1, -1),
            };
        
            int result = 0;
            
            int width = values[0].Length;
            int height = values.Length;

            Rectangle bounds = new Rectangle(0, 0, width, height);
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector2 p = new Vector2(x, y);
                    foreach (Vector2 direction in directions)
                    {
                        if (!bounds.InBounds(direction * 3 + p)) continue;

                        bool found = true;
                        for (int i = 0; i < 4; i++)
                        {
                            if (xmas[i] == values[y + i * direction.Y][x + i * direction.X]) continue;
                            found = false;
                            break;
                        }

                        if (found) result++;

                    }
                }
            }

            Assert.AreEqual(result, 2551);            
        }

        [TestMethod]
        public void Problem2()
        {
            int result = 0;
            
            int width = values[0].Length;
            int height = values.Length;

            Rectangle bounds = new Rectangle(0, 0, width, height);
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (values[y][x] != 'A') continue;
                    
                    if (x - 1 < 0 || x + 1 >= width || y - 1 < 0 || y + 1 >= width) continue;

                    if(((values[y - 1][x - 1] == 'M' && values[y + 1][x + 1] == 'S')  ||
                       (values[y - 1][x - 1] == 'S' && values[y + 1][x + 1] == 'M')) && 
                       ((values[y - 1][x + 1] == 'M' && values[y + 1][x - 1] == 'S')  ||
                        (values[y - 1][x + 1] == 'S' && values[y + 1][x - 1] == 'M'))) result++;
                }
            }
            
            Assert.AreEqual(result, 1985);
        }
    }
}
