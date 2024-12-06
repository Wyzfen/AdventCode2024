using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day06
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        private string [] values = File.ReadAllLines($"{day}.txt");

        private string[] test = new []
        {
            "....#.....",
            ".........#",
            "..........",
            "..#.......",
            ".......#..",
            "..........",
            ".#..^.....",
            "........#.",
            "#.........",
            "......#...",
        };
        
        private static List<Vector2> GetObstructions(string []  input) =>
            input.SelectMany((line, y) => 
                line.Select((c, x) => c == '#' ? new Vector2(x, y) : Vector2.Up))
                    .Where(v => v != Vector2.Up)
                 .ToList();

        private static Vector2 FindGuard(string [] input) =>
            input.SelectMany((line, y) => 
                line.Select((c, x) => c != '#' && c != '.' ? new Vector2(x, y) : Vector2.Up)
                    .Where(v => v != Vector2.Up))
                 .First();

        private static Vector2 Direction(string[] input, Vector2 guard) =>
            input[guard.Y][guard.X] switch
            {
                '^' => Vector2.Up,
                '>' => Vector2.Right,
                '<' => Vector2.Left,
                'v' => Vector2.Down,
                _ => Vector2.Zero
            };

        private static Vector2 GetObstruction(List<Vector2> obstructions, Vector2 location, Vector2 direction, Rectangle bounds)
        {
            return direction switch
            {
                (_, -1) => new Vector2(location.X,
                    obstructions
                        .Where(v => v.X == location.X && v.Y < location.Y)
                        .Append(new Vector2(location.X, -1))
                        .Max(v => v.Y)),
                (_, 1) =>  new Vector2(location.X,
                    obstructions
                        .Where(v => v.X == location.X && v.Y > location.Y)
                        .Append(new Vector2(location.X, bounds.Height))
                        .Min(v => v.Y)),
                (1, _) =>  new Vector2(obstructions
                        .Where(v => v.Y == location.Y && v.X > location.X)
                        .Append(new Vector2(bounds.Width, location.Y))
                        .Min(v => v.X), 
                    location.Y),
                (-1, _) =>  new Vector2(obstructions
                        .Where(v => v.Y == location.Y && v.X < location.X)
                        .Append(new Vector2(-1, location.Y))
                        .Max(v => v.X), 
                    location.Y),

                _ => Vector2.Down
            };
        }

        private static Vector2 GetObstruction(string [] values, Vector2 location, Vector2 direction,
            Rectangle bounds, Vector2? extra = null)
        {
            int x = location.X;
            int y = location.Y;
            
            switch (direction)
            {
                case (_, -1):
                    while (y >= 0 && values[y][x] != '#' && (extra?.X != x  || extra?.Y != y) ) y--;
                    break;
                case (_, 1):
                    while (y < bounds.Height && values[y][x] != '#'&& (extra?.X != x  || extra?.Y != y)) y++;
                    break;
                case (-1, _):
                    while (x >= 0 && values[y][x] != '#'&& (extra?.X != x  || extra?.Y != y)) x--;
                    break;
                case (1, _):
                    while (x < bounds.Width && values[y][x] != '#'&& (extra?.X != x  || extra?.Y != y)) x++;
                    break;
            }
            
            return new Vector2(x, y);
        }
        
        

        private static Vector2 RotateRight(Vector2 input) => new (-input.Y, input.X);
        
        [TestMethod]
        public void Problem1()
        {
            var values = this.values;
            var bounds = new Rectangle(0, 0, values[0].Length, values.Length);
            var obstructions = GetObstructions(values);
            
            var current = FindGuard(values);
            var direction = Direction(values, current);

            var path = new HashSet<Vector2> { current };

            while(true)
            {
                var obstruction = GetObstruction(values, current, direction, bounds);

                path.UnionWith(Vector2.Interpolate(current, obstruction));
                if (!bounds.InBounds(obstruction)) break;
                
                current = obstruction - direction;
                direction = RotateRight(direction);
            }

            var result = path.Count();
            Assert.AreEqual(result, 5453);            
        }

        [TestMethod]
        public void Problem2()
        {
            var values = this.values;
            var bounds = new Rectangle(0, 0, values[0].Length, values.Length);
            
            var startLocation = FindGuard(values);
            var startDirection = Direction(values, startLocation);

            var current = startLocation;
            var direction = startDirection;
            
            var path = new HashSet<Vector2>( );

            while(true)
            {
                var obstruction = GetObstruction(values, current, direction, bounds);

                path.UnionWith(Vector2.Interpolate(current, obstruction));
                if (!bounds.InBounds(obstruction)) break;
                
                current = obstruction - direction;
                direction = RotateRight(direction);
            }
            
            var result = 0;
            
            foreach(var step in path)
            {
                //obstructions.Add(step);
                
                current = startLocation;
                direction = startDirection;

                var waypoints = new List<(Vector2 location, Vector2 direction)>();
                
                while(true)
                {
                    var newWaypoint = (current, direction);
                    if (waypoints.Contains(newWaypoint))
                    {
                        result++;
                        break;
                    }
                    waypoints.Add(newWaypoint);
                    
                    var obstruction = GetObstruction(values, current, direction, bounds, step);
                    if (!bounds.InBounds(obstruction)) break;
                    
                    current = obstruction - direction;
                    direction = RotateRight(direction);
                }
                
                //obstructions.Remove(step);
            }

            Assert.AreEqual(result, 2188);            
        }
    }
}
