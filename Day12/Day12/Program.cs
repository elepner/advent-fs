// See https://aka.ms/new-console-template for more information
using Day12;

Console.WriteLine("Hello, World!");
var count = 0;
var result = System.IO.File.ReadAllText("./input.txt")
    .Split(Environment.NewLine)
    .Select(x => x.Trim())
    .Where(x => !string.IsNullOrWhiteSpace(x))
    .AsParallel()
    .Select((s) => Solution.SolveCount(Solution.Parse(s).Unfold()))
    .Aggregate(0l, (acc, current) =>
    {
        Interlocked.Increment(ref count);
        Console.WriteLine($"Done {count}");
        return acc + current;
    });
Console.WriteLine($"Result: {result}");