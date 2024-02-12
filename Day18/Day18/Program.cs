// See https://aka.ms/new-console-template for more information
using Day18;

var result = Solution.Solve(File.ReadAllText("./input.txt"));
var result2 = Solution.Solve(File.ReadAllText("./input.txt"), Solution.ParseInputPt2);

Console.WriteLine($"Result is {result}");
Console.WriteLine($"Result pt.2 is {result2}");