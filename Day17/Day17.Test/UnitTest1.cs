using Xunit.Abstractions;

namespace Day17.Test
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public UnitTest1(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        [Fact]
        public void Test1()
        {
            var result = Solution.Solve("""
                         2413432311323
                         3215453535623
                         3255245654254
                         3446585845452
                         4546657867536
                         1438598798454
                         4457876987766
                         3637877979653
                         4654967986887
                         4564679986453
                         1224686865563
                         2546548887735
                         4322674655533
                         """, (str) => _testOutputHelper.WriteLine(str));
            Assert.Equal(102, result);
        }
    }
}