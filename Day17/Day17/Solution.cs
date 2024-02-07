using System.Runtime.InteropServices.ComTypes;

namespace Day17;

public static class Solution
{
    public static int Dijkstra<T>(IVertexDescriptor<T> vertexDescriptor, T start, Action<string>? log) where T : notnull
    {
        var surface =
            new PrioritySortedQueue<T>();
        HashSet<T> visited = new HashSet<T>();
        surface.Enqueue(start, vertexDescriptor.GetCost(start));
        while (surface.Count() > 0)
        {
            var (currentNode, currentCost)= surface.Dequeue();
            visited.Add(currentNode);
            if (vertexDescriptor.IsDestination(currentNode))
            {
                return currentCost;
            }

            var neighbours = vertexDescriptor.GetNeighbours(currentNode).ToArray();
            foreach (var neighbour in neighbours)
            {
                if (visited.Contains(neighbour)) continue;

                var cost = vertexDescriptor.GetCost(neighbour);

                if (surface.TryGetValue(neighbour, out var existingCost))
                {
                    if (currentCost + cost < existingCost)
                    {
                        surface.Enqueue(neighbour, currentCost + cost);
                        //surface[neighbour] = currentCost + cost;
                    }
                }
                else
                {
                    surface.Enqueue(neighbour, currentCost + cost);
                }
            }
        }

        throw new ArgumentException("Cannot find result");
    }

    public static int Solve(string inputStr, Action<string>? log = null)
    {
        var input = ParseInput(inputStr);
        return Dijkstra(new CrucibleDescriptor(input), new CrucibleStep(0, 0, Direction.Right, -1), log) - input[0][0];
    }

    public static int Solve2(string inputStr)
    {
        var input = ParseInput(inputStr);
        return Dijkstra(new Pt2CrucibleDescriptor(input), new (0, 0, Direction.Up, -1), null) - input[0][0];
    }

    private static int[][] ParseInput(string inputStr)
    {
        var input = inputStr.Trim().Split("\n").Select(x => x.Trim().Select(x => int.Parse(x.ToString())).ToArray()).ToArray();
        return input;
    }
}


public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public record CrucibleStep(int Row, int Col, Direction Direction, int Penalty);


public static class DirectionHelpers
{
    public static readonly Direction[] Directions = [Direction.Up, Direction.Down, Direction.Left, Direction.Right];
    public static (int, int) GetStep(Direction direction)
    {
        return direction switch
        {
            Direction.Up => (-1, 0),
            Direction.Down => (1, 0),
            Direction.Left => (0, -1),
            Direction.Right => (0, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static Direction Opposite(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}

public class CrucibleDescriptor(int[][] field) : IVertexDescriptor<CrucibleStep>
{
    

    public IEnumerable<CrucibleStep> GetNeighbours(CrucibleStep input)
    {
        var oppositeOfCurrent = DirectionHelpers.Opposite(input.Direction);
        foreach (var direction in DirectionHelpers.Directions)
        {
            if (direction == oppositeOfCurrent) continue;

            var penalty = direction == input.Direction ? input.Penalty + 1 : 0;
            if (penalty > 2)
            {
                continue;
            }
            var step = DirectionHelpers.GetStep(direction);
            var newRow = input.Row + step.Item1;
            var newCol = input.Col + step.Item2;
            if (0 <= newRow && newRow <= MaxRow && 0 <= newCol && newCol <= MaxCol)
            {
                yield return new CrucibleStep(Direction: direction, Penalty: penalty, Row: newRow, Col: newCol);
            }
        }

    }

    public int GetCost(CrucibleStep input)
    {
        return field[input.Row][input.Col];
    }

    public bool IsDestination(CrucibleStep input)
    {
        return input.Row == MaxRow && input.Col == MaxCol;
    }

    private int MaxRow => field.Length - 1;
    private int MaxCol => field[0].Length - 1;

    
}

public class CostComparer<T> : IComparer<T>
{
    private readonly Func<T, T, int> _compare;

    public CostComparer(Func<T, T, int> compare)
    {
        _compare = compare;
    }
    public int Compare(T? x, T? y)
    {
        return _compare(x, y);
    }
}

public interface IVertexDescriptor<T>
{
    public IEnumerable<T> GetNeighbours(T input);
    public int GetCost(T input);
    public bool IsDestination(T input);
}



public class PrioritySortedQueue<T> where T : notnull
{
    record ItemHolder(T Element, int Priority);
    

    private readonly Dictionary<T, int> _values = new();

    private readonly SortedSet<ItemHolder> _soredItems = new(new CostComparer<ItemHolder>((a, b) =>
    {
        var result = Comparer<int>.Default.Compare(a.Priority, b.Priority);
        if (result == 0)
        {
            return Comparer<int>.Default.Compare(a.GetHashCode(), b.GetHashCode());
        }
        return result;
    }));

    public void Enqueue(T el, int priority)
    {

        if (_values.TryGetValue(el, out var existingValue))
        {
            var result = _soredItems.Remove(new ItemHolder(el, existingValue));
            if (!result)
            {
                throw new ArgumentException("Impossible");
            }
        }
        
        _values[el] = priority;

        bool isInserted = _soredItems.Add(new ItemHolder(el, priority));
        if (!isInserted)
        {
            throw new Exception("Impossible");
        }
    }

    public (T, int) Dequeue()
    {
        var result = _soredItems.First();
        _soredItems.Remove(result);
        _values.Remove(result.Element);
        return (result.Element, result.Priority);
    }

    public int Count()
    {
        return _values.Count;
    }

    public bool TryGetValue(T key, out int value)
    {
        return _values.TryGetValue(key, out value);
    }
}