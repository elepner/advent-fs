using Xunit.Abstractions;

namespace Day17.Test;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    private string _inputStr = """
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
                               """;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    [Fact]
    public void Test1()
    {
        var result = Solution.Solve(_inputStr, (str) => _testOutputHelper.WriteLine(str));
        Assert.Equal(102, result);
    }

    [Fact]
    public void Part2Test()
    {
        var result = Solution.Solve2(_inputStr);
        Assert.Equal(94, result);
    }

    [Fact]
    public void Part2TestSimpleCase()
    {
        var input = """
                    111111111111
                    999999999991
                    999999999991
                    999999999991
                    999999999991
                    """;
        var result = Solution.Solve2(input);
        Assert.Equal(71, result);
    }

    [Fact]
    public void Part1BigData()
    {
        var result = Solution.Solve(File.ReadAllText("./input.txt"));
        Assert.Equal(886, result);
    }

    [Fact]
    public void Part2BigData()
    {
        var result = Solution.Solve2(File.ReadAllText("./input.txt"));
        Assert.True(result < 1077);
    }
}

public record TestRecord(string Value);