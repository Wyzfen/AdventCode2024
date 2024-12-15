using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day15
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        private readonly string[] test = {
            "########",
            "#..O.O.#",
            "##@.O..#",
            "#...O..#",
            "#.#.O..#",
            "#...O..#",
            "#......#",
            "########",
            "",
            "<^^>>>vv<v>>v<<"
        };

        private readonly string[] test2 = {
            "##########",
            "#..O..O.O#",
            "#......O.#",
            "#.OO..O.O#",
            "#..O@..O.#",
            "#O#..O...#",
            "#O..O..O.#",
            "#.OO.O.OO#",
            "#....O...#",
            "##########",
            "",
            "<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^",
            "vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v",
            "><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<",
            "<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^",
            "^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><",
            "^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^",
            ">^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^",
            "<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>",
            "^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>",
            "v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^",
        };
        
        private record struct Level(Vector2 Start, Vector2[] Walls, List<Vector2> Blocks)
        {
            public Level(string[] level) : this(
                Start: level.IterateStringArray().First(p => p.character == '@').location,
                Walls: level.IterateStringArray().Where(p => p.character == '#').Select(p => p.location).ToArray(),
                Blocks: level.IterateStringArray().Where(p => p.character == 'O').Select(p => p.location).ToList()
            ) { }

            public void Expand()
            {
                Walls = Walls.SelectMany<Vector2, Vector2>(v => [v with { X = v.X * 2 }, v with { X = v.X * 2 + 1}]).ToArray();
                Start = Start with { X = Start.X * 2 };
                Blocks = Blocks.Select(v => v with { X = v.X * 2 }).ToList();
            }
        }
        
        private static (Level level, string path) Parse(string [] input) =>
        (
            level: new Level(input.Where(x => x.Contains('#')).ToArray()), 
            path: string.Join("", input.Where(x => !string.IsNullOrWhiteSpace(x) && !x.Contains('#')).Select(x => x.Trim()))
        );
        
        private static Vector2 Direction(char step) =>
            step switch
            {
                '^' => Vector2.Up,
                '>' => Vector2.Right,
                'v' => Vector2.Down,
                '<' => Vector2.Left,
                _ => Vector2.Zero,
            };

        [TestMethod]
        public void Problem1()
        {           
            var ((current, walls, blocks), path) = Parse(values.ToArray());

            foreach (var direction in path.Select(Direction))
            {
                var next = current;

                do
                {
                    next += direction;
                } while (blocks.Contains(next));

                if (walls.Contains(next)) continue;

                current += direction;

                if (!blocks.Contains(current)) continue;
                
                blocks.Remove(current);
                blocks.Add(next);
            }
            
            var result = blocks.Sum(v => v.Y * 100 + v.X);

            Assert.AreEqual(result, 1509074);            
        }

        [TestMethod]
        public void Problem2()
        {
            var (level, path) = Parse(values.ToArray());
            level.Expand();
            var (current, walls, blocks) = level;

            bool CanMoveBlock(Vector2 block, Vector2 direction)
            {
                // something in this function is off
                if (walls.Contains(block)) return false;
                
                var next = block + direction;
                if(walls.Contains(next)) return false;
                
                var neighbour = next + (direction.X == 0 ? Vector2.Right : direction);  
                if(walls.Contains(neighbour)) return false;
               
                var canMove = true;
                if(blocks.Contains(next)) canMove &= CanMoveBlock(next, direction);
                if(blocks.Contains(neighbour)) canMove &= CanMoveBlock(neighbour, direction);

                return canMove;
            }

            void MoveBlock(Vector2 block, Vector2 direction)
            {
                var next = block + direction;
                
                var neighbour = next + (direction.X == 0 ? Vector2.Right : direction);  
                if (blocks.Contains(neighbour)) MoveBlock(neighbour, direction); // move right side first, incase moving right
                
                if(blocks.Contains(next)) MoveBlock(next, direction);

                blocks.Remove(block);
                blocks.Add(next);
            }
            
            foreach (var direction in path.Select(Direction))
            {
                var next = current + direction;
                if (walls.Contains(next)) continue;

                var hasNext = blocks.Contains(next);
                if(hasNext && !CanMoveBlock(next, direction)) continue;

                var neighbour = next + Vector2.Left;
                var hasNeighbour = blocks.Contains(neighbour);
                if (hasNeighbour && !CanMoveBlock(neighbour, direction)) continue;

                if (hasNext && hasNeighbour)
                {
                    System.Diagnostics.Debugger.Break();    // shouldn't be possible
                }
                
                if(hasNext) MoveBlock(next, direction);
                if(hasNeighbour) MoveBlock(neighbour, direction);
               
                current = next;
            }
            
            var result = blocks.Sum(v => v.Y * 100 + v.X);

            Assert.AreEqual(result, 1515117);     // 1515117 too low
        }
    }
}
