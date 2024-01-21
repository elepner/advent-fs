using Xunit.Abstractions;

namespace Day12.Test;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [InlineData("#.#.### 1,1,3", true)]
    [InlineData(".#.###.#.###### 1,3,1,6", true)]
    [InlineData(".#.###.#.######... 1,3,1,6", true)]
    [InlineData("#.#.###. 1,1,3", true)]
    [InlineData(".###......## 3,2,1", false)]
    [Theory]
    public void Simple(string input, bool expected)
    {
        var condition = Solution.Parse(input);
        var result = Solution.Solve(condition);
        Assert.Equal(expected, result);
    }


    [InlineData("???.### 1,1,3", 1)]
    [InlineData(".??..??...?##. 1,1,3", 4)]
    [InlineData("?#?#?#?#?#?#?#? 1,3,1,6", 1)]
    [InlineData("?###???????? 3,2,1", 10)]
    [Theory]
    public void Recursion(string input, int expected)
    {
        var result = Solution.SolveCount(Solution.Parse(input), (s) => _testOutputHelper.WriteLine(s));
        Assert.Equal(expected, result);
    }


    [InlineData("???.### 1,1,3", 1)]
    [InlineData(".??..??...?##. 1,1,3", 16384)]
    [InlineData("?#?#?#?#?#?#?#? 1,3,1,6", 1)]
    [InlineData("????.#...#... 4,1,1", 16)]
    [InlineData("????.######..#####. 1,6,5", 2500)]
    [InlineData("?###???????? 3,2,1", 506250)]
    [Theory]
    public void CountUnfolded(string input, int expected)
    {
        var data = Solution.Parse(input).Unfold();
        long cacheMiss = 0;
        long cacheHit = 0;
        var result = Solution.SolveCount(data, (s) =>
        {
            if (s == "hit")
            {
                cacheHit++;
            } else if (s == "miss")
            {
                cacheMiss++;
            }
        });
        Assert.Equal(expected, result);
        _testOutputHelper.WriteLine($"Hit: {cacheHit}, Miss: {cacheMiss}. Ratio: {(double)cacheHit/(cacheHit + cacheMiss)}");
    }

    [Fact]
    public void Recordstest()
    {
        var dic = new Dictionary<TestRecord, int>();
        var r1 = new TestRecord("foo", 42);
        dic[r1] = 69;
        Assert.True(dic.ContainsKey(new TestRecord("foo", 42)));
    }

    record TestRecord(string foo, int bar);
}