namespace AdventCode2024
{
    [TestClass]
    public class Day01
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        readonly int[][] values = Utils.FromCSVFile<int>($"{day}.txt", " ").TransposeArray();

        [TestMethod]
        public void Problem1()
        {
            var (left, right, _) = values;

            var result = left.Order()
                             .Zip(right.Order())
                             .Sum(p => Math.Abs(p.First - p.Second));

            Assert.AreEqual(result, 1223326);
        }
        
        [TestMethod]
        public void Problem2()
        {
            var (left, right, _) = values;
            long result = left.Sum(l => l * right.Count(r => r == l));
            
            Assert.AreEqual(result, 21070419);
        }
    }
}
