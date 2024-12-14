using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventCode2024
{
    public static class Utils
    {
        public static IEnumerable<T> FromFile<T>(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => ValueFromString<T>(s));

        public static IEnumerable<(T, U)> FromFile<T, U>(string filename, string split = " ") =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => FromString<T, U>(s, split));

        public static IEnumerable<T> FromString<T>(string str, params string[] split) =>
           SplitClean(str, split).Select(s => ValueFromString<T>(s));

        public static (T, U) FromString<T, U>(string mystring, params string[] split)
        {
            var p = SplitClean(mystring, split);
            return (ValueFromString<T>(p[0]), ValueFromString<U>(p[1]));
        }

        private static string[] SplitClean(string mystring, params string [] split) => mystring.Trim().Split(split, StringSplitOptions.RemoveEmptyEntries).ToArray();

        public static (T, U, V) FromString<T, U, V>(string mystring, params string[] split)
        {
            var p = SplitClean(mystring, split);
            return (ValueFromString<T>(p[0]), ValueFromString<U>(p[1]), ValueFromString<V>(p[2]));
        }

        public static (T, U, V, W) FromString<T, U, V, W>(string mystring, params string [] split)
        {
            var p = SplitClean(mystring, split);
            return (ValueFromString<T>(p[0]), ValueFromString<U>(p[1]), ValueFromString<V>(p[2]), ValueFromString<W>(p[3]));
        }

        public static (T, U, V, W, X) FromString<T, U, V, W, X>(string mystring, params string[] split)
        {
            var p = SplitClean(mystring, split);
            return (ValueFromString<T>(p[0]), ValueFromString<U>(p[1]), ValueFromString<V>(p[2]), ValueFromString<W>(p[3]), ValueFromString<X>(p[4]));
        }

        public static (T, U, V, W, X, Y) FromString<T, U, V, W, X, Y>(string mystring, params string[] split)
        {
            var p = SplitClean(mystring, split);
            return (ValueFromString<T>(p[0]), ValueFromString<U>(p[1]), ValueFromString<V>(p[2]), ValueFromString<W>(p[3]), ValueFromString<X>(p[4]), ValueFromString<Y>(p[5]));
        }

        public static T ValueFromString<T>(string mystring)
        {
            var foo = TypeDescriptor.GetConverter(typeof(T));
            return (T)(foo.ConvertFromInvariantString(mystring));
        }

        public static IEnumerable<int> IntsFromFile(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => int.Parse(s));

        public static IEnumerable<long> LongsFromFile(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => long.Parse(s));

        public static IEnumerable<int> IntsFromString(string input, params string [] split) =>
            input.Split(split, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

        public static string [] StringsFromFile(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8);

        public static IEnumerable<String> StringsFromString(string input) =>
            input.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

        // Returns an array of arrays from a CSV file
        public static T[][] FromCSVFile<T>(string filename, string split=",") =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => FromString<T>(s, split).ToArray()).ToArray();


        // Returns an array of arrays from a CSV file
        public static String[][] StringsFromCSVFile(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => s.Split(',')).ToArray();

        // Returns an array of arrays from a CSV file
        public static int[][] IntsFromCSVFile(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => s.Split(',').Select(i => int.Parse(i)).ToArray()).ToArray();

        public static long[][] LongsFromCSVFile(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => s.Split(',').Select(i => long.Parse(i)).ToArray()).ToArray();

        // Returns an array of arrays from a CSV file
        public static String[][] StringsFromCSVString(string input) =>
            input.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Select(s => s.Split(',')).ToArray();

        public static int ToInt(this IEnumerable<int> source) =>
             source.Select(d => Math.Abs(d) % 10).Aggregate(0, (t, n) => t * 10 + n);

        public static Dictionary<string, string> SplitFromFile(string filename, char split = ')') =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(i => i.Split(split)).ToDictionary(s => s[1], s => s[0]);

        public static Dictionary<string, string> SplitFromString(string input, char split = ')') =>
            input.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Select(i => i.Split(split)).ToDictionary(s => s[1], s => s[0]);

        // Group consecutive lines into array, splitting into new group on blank line
        public static List<List<string>> MergeLines(IEnumerable<string> lines)
        {
            List<List<string>> output = new List<List<string>>();

            List<string> current = new List<string>();
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    output.Add(current);
                    current = new List<string>();
                }
                else
                {
                    current.Add(line.TrimEnd());
                }
            }

            if (current.Count() > 0) output.Add(current);

            return output;
        }

        public static ulong Factorial(int n)
        {
            ulong value = 1;
            for (int i = 2; i <= n; i++)
            {
                value *= (ulong)i;
            }

            return value;
        }

        public static ulong GreatestCommonFactor(ulong a, ulong b)
        {
            while (b != 0)
            {
                ulong temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static ulong GCD(ulong a, ulong b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                {
                    a %= b;
                }
                else
                {
                    b %= a;
                }
            }

            return a == 0 ? b : a;
        }

        public static ulong LeastCommonMultiple(ulong a, ulong b) => (a / GreatestCommonFactor(a, b)) * b;

        public static ulong LeastCommonMultiple(params ulong[] f) => f.Aggregate((a, b) => LeastCommonMultiple(a, b));


        // Split string to tuples
        public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default;
            rest = list.Skip(1).ToList();
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default;
            second = list.Count > 1 ? list[1] : default;
            rest = list.Skip(2).ToList();
        }

        public static (T, T, T) ToTuple3<T>(this IList<T> list) => (list[0], list[1], list[2]);
        public static (T, T) ToTuple2<T>(this IList<T> list) => (list[0], list[1]);

        public static string FromAlphabet6(string[] input, char set = '#', char unset = '.')
        {
            if (input == null || input.Length != 6) return "";
            
            int length = input[0].Length;
            if (!input.Select(l => l.Length).All(l => l == input[0].Length)) return "";

            Dictionary<string, string> alphabet6 = new()
                        {
                            {".##.\n#..#\n#..#\n####\n#..#\n#..#", "A"},
                            {"###.\n#..#\n###.\n#..#\n#..#\n###.", "B"},
                            {".##.\n#..#\n#...\n#...\n#..#\n.##.", "C"},
                            {"####\n#...\n###.\n#...\n#...\n####", "E"},
                            {"####\n#...\n###.\n#...\n#...\n#...", "F"},
                            {".##.\n#..#\n#...\n#.##\n#..#\n.###", "G"},
                            {"#..#\n#..#\n####\n#..#\n#..#\n#..#", "H"},
                            {".###\n..#.\n..#.\n..#.\n..#.\n.###", "I"},
                            {"..##\n...#\n...#\n...#\n#..#\n.##.", "J"},
                            {"#..#\n#.#.\n##..\n#.#.\n#.#.\n#..#", "K"},
                            {"#...\n#...\n#...\n#...\n#...\n####", "L"},
                            {".##.\n#..#\n#..#\n#..#\n#..#\n.##.", "O"},
                            {"###.\n#..#\n#..#\n###.\n#...\n#...", "P"},
                            {"###.\n#..#\n#..#\n###.\n#.#.\n#..#", "R"},
                            {".###\n#...\n#...\n.##.\n...#\n###.", "S"},
                            {"#..#\n#..#\n#..#\n#..#\n#..#\n.##.", "U"},
                            {"#...\n#...\n.#.#\n..#.\n..#.\n..#.", "Y"},
                            {"####\n...#\n..#.\n.#..\n#...\n####", "Z"}
                        };

            string result = "";

            for(int x = 0; x < length; x += 5)
            {
                var character = string.Join('\n', input.Select(l => l.Substring(x, 4)));
                
                result += alphabet6.TryGetValue(character, out string output) ? output : "?";
            }

            return result;
        }
        
        public static T[][] TransposeArray<T>(this T[][] array) => Enumerable.Range(0, array[0].Length).Select(index => array.Select(v => v[index]).ToArray()).ToArray();

        public static string[] Transpose(this string[] array) => Enumerable.Range(0, array[0].Length).Select(index => new string(array.Select(v => v[index]).ToArray())).ToArray();
        public static string[] Rotate(this string[] array) => Enumerable.Range(0, array[0].Length).Select(index => new string(array.Select(v => v[v.Length - 1 - index]).ToArray())).ToArray();

        public static IEnumerable<(Vector2 location, T item)> Iterate2DArray<T>(this T[][] array)
        {
            for (int y = 0; y < array.Length; y++)
            {
                for (int x = 0; x < array[y].Length; x++)
                {
                    yield return (new Vector2(x, y), array[y][x]);
                }
            }
        }
        
        public static IEnumerable<(Vector2 location, char character)> IterateStringArray(this string[] array)
        {
            for (int y = 0; y < array.Length; y++)
            {
                for (int x = 0; x < array[y].Length; x++)
                {
                    yield return (new Vector2(x, y), array[y][x]);
                }
            }
        }
        
        public static IEnumerable<U> ExecuteWith<T, U>(Func<T, U> func, params T [] elements) => elements.Select(func);
        
        public static T IndexBy<T>(this T [][] array, Vector2 v) => array[v.Y][v.X]; 
        public static T IndexBy<T>(this T [][][] array, Vector3 v) => array[v.Z][v.Y][v.X]; 
        
        public static char IndexBy(this string [] array, Vector2 v) => array[v.Y][v.X]; 
        public static char IndexBy(this string [][] array, Vector3 v) => array[v.Z][v.Y][v.X]; 
        
        public static bool InBounds<T>(this T [][] array, int x, int y) => array.Length > 0 && x >= 0 && x < array[0].Length && y>= 0 && y < array.Length;

        public static bool InBounds<T>(this T [][] array, Vector2 v) => v.InBounds(array);

        
        public static int Length(this ulong n)
        {
            if (n <= 0) return 0;
            if (n < 10L) return 1;
            if (n < 100L) return 2;
            if (n < 1000L) return 3;
            if (n < 10000L) return 4;
            if (n < 100000L) return 5;
            if (n < 1000000L) return 6;
            if (n < 10000000L) return 7;
            if (n < 100000000L) return 8;
            if (n < 1000000000L) return 9;
            if (n < 10000000000L) return 10;
            if (n < 100000000000L) return 11;
            if (n < 1000000000000L) return 12;
            if (n < 10000000000000L) return 13;
            if (n < 100000000000000L) return 14;
            if (n < 1000000000000000L) return 15;
            if (n < 10000000000000000L) return 16;
            if (n < 100000000000000000L) return 17;
            if (n < 1000000000000000000L) return 18;
            if (n < 10000000000000000000UL) return 19;
            return 20;
        }
        
        public static int Length(this uint n) // only for positive numbers
        {
            if (n <= 0) return 0;
            if (n < 10) return 1;
            if (n < 100) return 2;
            if (n < 1000) return 3;
            if (n < 10000) return 4;
            if (n < 100000) return 5;
            if (n < 1000000) return 6;
            if (n < 10000000) return 7;
            if (n < 100000000) return 8;
            if (n < 1000000000) return 9;
            return 10;
        }
    }

    public class MultiMap<TKey, TValue> : Dictionary<TKey, List<TValue>> where TKey : notnull
    {
        public MultiMap(IEnumerable<(TKey key, TValue value)> pairs)
        {
            foreach(var pair in pairs) Add(pair.key, pair.value);    
        }
        
        public void Add(TKey key, TValue value)
        {
            // Add a key.
            if (TryGetValue(key, out List<TValue> list))
            {
                list.Add(value);
            }
            else
            {
                Add(key, new List<TValue> { value });
            }
        }
    }

    [TypeConverter(typeof(Vector2Converter))]
    public record struct Vector2(int X, int Y)
    {
        internal static Regex Regex = new Regex(@"^[^-\d]*(?<x>-?\d+)[^-\d]+(?<y>-?\d+)$");

        public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);

        public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);

        public static Vector2 operator -(Vector2 a) => new(-a.X, -a.Y);

        public static Vector2 operator %(Vector2 a, Vector2 b) => new((a.X % b.X + b.X) % b.X, (a.Y % b.Y + b.Y) % b.Y);
        public static Vector2 operator *(Vector2 a, int b) => new(a.X * b, a.Y * b);
        
        public static Vector2 Sign(Vector2 vector) => new(Math.Sign(vector.X), Math.Sign(vector.Y));
        
        public static Vector2 One { get; } = new Vector2(1, 1);
        public static Vector2 Zero { get; } = new Vector2(0, 0);
        public static Vector2 Up { get; } = new Vector2(0, -1);
        public static Vector2 Down { get; } = new Vector2(0, 1);
        public static Vector2 Left { get; } = new Vector2(-1, 0);
        public static Vector2 Right { get; } = new Vector2(1, 0);

        public static IEnumerable<Vector2> Interpolate(Vector2 from, Vector2 to)
        {
            var delta = Sign(to - from);
            var current = from;
            while (current != to)
            {
                yield return current;
                current += delta;
            }
        }
        
        public bool InBounds<T>(T [][] array) => array.Length > 0 && X >= 0 && X < array[0].Length && Y >= 0 && Y < array.Length;
        public bool InBounds(string [] array) => array.Length > 0 && X >= 0 && X < array[0].Length && Y >= 0 && Y < array.Length;

        public static Vector2 Parse(string str)
        {
            var match = Regex.Match(str);
            if (match.Groups["x"].Success && match.Groups["y"].Success)
            {
                return new Vector2(int.Parse(match.Groups["x"].Value), int.Parse(match.Groups["y"].Value));
            }
            
            return Zero;
        }
    }
    
    public record struct Vector2Long(long X, long Y)
    {
        public static Vector2Long operator +(Vector2Long a, Vector2Long b) => new(a.X + b.X, a.Y + b.Y);

        public static Vector2Long operator -(Vector2Long a, Vector2Long b) => new(a.X - b.X, a.Y - b.Y);

        public static Vector2Long operator -(Vector2Long a) => new(-a.X, -a.Y);

        public static Vector2Long operator %(Vector2Long a, Vector2Long b) => new((a.X % b.X + b.X) % b.X, (a.Y % b.Y + b.Y) % b.Y);

        public static Vector2Long operator *(Vector2Long a, long b) => new(a.X * b, a.Y * b);
        
        public static Vector2Long Sign(Vector2Long vector) => new(Math.Sign(vector.X), Math.Sign(vector.Y));
        
        public static Vector2Long One { get; } = new Vector2Long(1, 1);
        public static Vector2Long Zero { get; } = new Vector2Long(0, 0);
        public static Vector2Long Up { get; } = new Vector2Long(0, -1);
        public static Vector2Long Down { get; } = new Vector2Long(0, 1);
        public static Vector2Long Left { get; } = new Vector2Long(-1, 0);
        public static Vector2Long Right { get; } = new Vector2Long(1, 0);

        public static IEnumerable<Vector2Long> Interpolate(Vector2Long from, Vector2Long to)
        {
            var delta = Sign(to - from);
            var current = from;
            while (current != to)
            {
                yield return current;
                current += delta;
            }
        }
        
        public bool InBounds<T>(T [][] array) => array.Length > 0 && X >= 0 && X < array[0].Length && Y >= 0 && Y < array.Length;
        public bool InBounds(string [] array) => array.Length > 0 && X >= 0 && X < array[0].Length && Y >= 0 && Y < array.Length;

        public static Vector2Long Parse(string str)
        {
            var match = Vector2.Regex.Match(str);
            if (match.Groups["x"].Success && match.Groups["y"].Success)
            {
                return new Vector2Long(long.Parse(match.Groups["x"].Value), long.Parse(match.Groups["y"].Value));
            }
            
            return Zero;
        }
    }

    public class Vector2Converter : TypeConverter
    { 
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(String);
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string str)
            {
                // if (str.Contains('=')) // x = 1, y = 2
                // {
                //     (_, int x, _, int y) = Utils.FromString<string, int, string, int>(str, "=", ",");
                //     return new Vector2(x, y);
                // }
                // else // 1, 2
                // {
                //     (int x, int y) = Utils.FromString<int, int>(str, ",");
                //     return new Vector2(x, y);
                // }
                //else

                // use regex 
                {
                    var match = Vector2.Regex.Match(str);
                    if (match.Groups["x"].Success && match.Groups["y"].Success)
                    {
                        return new Vector2(int.Parse(match.Groups["x"].Value), int.Parse(match.Groups["y"].Value));
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    [TypeConverter(typeof(Vector3Converter))]
    public record struct Vector3(int X, int Y, int Z)
    {
        public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        
    }

    public class Vector3Converter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(String);
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string str)
            {
                if (str.Contains('=')) // x = .., y = .., z = ..
                {
                    (_, int x, _, int y, _, int z) = Utils.FromString<string, int, string, int, string, int>(str, "=", ",");
                    return new Vector3(x, y, z);
                }
                else // 1, 2
                {
                    (int x, int y, int z) = Utils.FromString<int, int, int>(str, ",");
                    return new Vector3(x, y, z);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    public record struct Rectangle(int X, int Y, int Width, int Height)
    {
        public static Rectangle Bounds(IEnumerable<Vector2> points)
        {
            int minX = points.Min(e => e.X);
            int maxX = points.Max(e => e.X);
            int minY = points.Min(e => e.Y);
            int maxY = points.Max(e => e.Y);

            return new Rectangle(minX, minY, (maxX - minX + 1), (maxY - minY + 1));
        }

        public Vector2 Size => new (Width, Height);
        
        public bool InBounds(Vector2 point) => point.X >= 0 && point.X < Width && point.Y >= 0 && point.Y < Height;
    }

    public static class VectorExtensions
    {
        public static Rectangle Bounds(this IEnumerable<Vector2> points) => Rectangle.Bounds(points);
    }

    public static class LinqExtensions
    {        
        public static IEnumerable<IEnumerable<T>> Sliding<T>(this IEnumerable<T> input, int length) => Enumerable.Range(length, input.Count() - 1).Select(i => input.Skip(i - length).Take(length));

        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> sequence, int size) {
            List<T> partition = new List<T>(size);
            foreach(var item in sequence) {
                partition.Add(item);
                if (partition.Count == size) {
                    yield return partition;
                    partition = new List<T>(size);
                }
            }
            if (partition.Count > 0)
                yield return partition;
        }
        
        public static int FirstIndex<T>(this IEnumerable<T> input, Func<T, bool> predicate) => input.Select((value, index) => (value, index)).Where(p => predicate(p.value)).Select(p => p.index).DefaultIfEmpty(-1).First();

        public static IEnumerable<TAcc> Scan<T, TAcc>(this IEnumerable<T> seq, Func<TAcc, T, TAcc> f, TAcc initial)
        {
            TAcc current = initial;
            yield return current;
            foreach(T item in seq)
            {
                current = f(current, item);
                yield return current;
            }
        }

        public static MultiMap<TKey, TValue> ToMultiMap<TKey, TValue>(this IEnumerable<(TKey, TValue)> pairs) where TKey: notnull => new (pairs);

        /// <summary>
        /// Uses factorial notation to give all permutations of input set.
        /// if input set is in lexigraphical order, the results will be too.
        /// ie, pass in 012 (the lowest combination of 0,1 and 2) and the next one will be 021
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public static IEnumerable<List<int>> Permutations(this IEnumerable<int> set)
        {
            int count = set.Count();
            ulong number = Utils.Factorial(count);
            int[] factors = new int[count];

            for (ulong n = 0; n < number; n++)
            {
                List<int> workingSet = new List<int>(set);
                List<int> result = new List<int>();

                for (int i = count - 1; i >= 0; i--)
                {
                    int j = factors[i];
                    result.Add(workingSet[j]);
                    workingSet.RemoveAt(j);
                }

                yield return result;

                for (int index = 1; index < count; index++)
                {
                    factors[index]++;
                    if (factors[index] <= index) break;

                    factors[index] = 0;
                }
            }
        }

        /// <summary>
        /// Select (choose) from (set) where order doesnt matter
        /// </summary>
        /// <param name="set"></param>
        /// <param name="choose"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> Combinations<T>(this IEnumerable<T> input, int choose)
        {
            List<T> set = new List<T>(input);
            for (int i = 0; i < set.Count(); i++)
            {
                // only want 1 character, just return this one
                if (choose == 1)
                    yield return new List<T>(new T[] { set[i] });

                // want more than one character, return this one plus all combinations one shorter
                // only use characters after the current one for the rest of the combinations
                else
                    foreach (List<T> next in Combinations(set.GetRange(i + 1, set.Count - (i + 1)), choose - 1))
                    {
                        next.Add(set[i]);
                        yield return next;
                    }
            }
        }

        public static IEnumerable<T> Generate<T>(T value, Func<T, T> func, Func<T, bool>? predicate = null)
        {
            while (predicate?.Invoke(value) ?? true)
            {
                yield return value;
                value = func(value);
            }
        }
    }

    public static class DebugExtensions
    {
        public static string ToGridString<T>(this T [][] source, Func<T, string> callback = null, int pad = 10)
        {
            var result = "";
            for (int y = 0; y < source.Length; y++)
            {
                for (int x = 0; x < source[y].Length; x++)
                {
                    T value = source[y][x];
                    result += (callback?.Invoke(value) ?? value.ToString()).PadLeft(pad);
                }
                result += "\n";
            }
            return result;
        }
        //public static string ToGridString(this Array source, int pad = 10)
        //{
        //    var result = "";
        //    for (int i = source.GetLowerBound(0); i <= source.GetUpperBound(0); i++)
        //    {
        //        for (int j = source.GetLowerBound(1); j <= source.GetUpperBound(1); j++)
        //            result += source.GetValue(i, j).ToString().PadLeft(pad);
        //        result += "\n";
        //    }
        //    return result;
        //}
    }
}
