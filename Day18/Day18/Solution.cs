
namespace Day18;

public static class Solution
{

    public static IEnumerable<(Direction, long)> ParseInputPt2(string input)
    {
        return input.Trim().Split("\n").Select(x =>
        {
            var splitted = x.Trim().Split(" ");
            //var hash = splitted[2].Where(c => Char.IsNumber(c)).ToArray();
            var res = splitted[2][2..^1];
            
            return ((Direction)long.Parse(res.Last().ToString()), Convert.ToInt64(res[..^1], 16));
        });
    }

    public static IEnumerable<(Direction, long)> ParseInput(string input)
    {
        return input.Trim().Split("\n").Select(x =>
        {
            var splitted = x.Trim().Split(" ");
            return (DirectionHelpers.DirectionsMap[splitted[0]], long.Parse(splitted[1]));
        });
    }
    public static long Solve(string input, Func<string, IEnumerable<(Direction, long)>>? parser = null)
    {
        if (parser == null)
        {
            parser = ParseInput;
        }
        var parsed = parser(input).ToArray();
        List<(long, long)> Points = new List<(long, long)>(parsed.Length + 1)
        {
            (0, 0)
        };
        foreach (var (dir, count) in parsed)
        {
            var (lastX, lastY) = Points.Last();
            var (dX, dY) = DirectionHelpers.GetStep(dir);
            Points.Add((lastX + dX * count, lastY + dY * count));
        }

        Points.Add((0, 0));
        var zipped = Points.Zip(Points.Skip(1)).ToArray();
        var shoelace = zipped.Aggregate(0L, (acc, curr) =>
        {
            var (x1, y1) = curr.First;
            var (x2, y2) = curr.Second;

            return acc + x1 * y2 - y1 * x2;
        }) / 2;

        var result = Math.Abs(shoelace);

        var p = parsed.Sum(x => x.Item2) / 2;
        var finalResult = p + result + 1;
        return finalResult;
    }
}




