namespace Day17;

public static class Solution
{
    public static int Dijkstra<T>(IVertexDescriptor<T> vertexDescriptor, T start, Action<string>? log) where T : notnull
    {
        var surface =
            new Dictionary<T, int>();
        HashSet<T> visited = new HashSet<T>();
        surface.Add(start, vertexDescriptor.GetCost(start));
        while (surface.Count > 0)
        {
            var currentKv = surface.MinBy(x => x.Value);
            
            // log?.Invoke(string.Join(",", surface.Values));
            
            var currentNode = currentKv.Key;
            var currentCost = currentKv.Value;

            log?.Invoke(currentNode.ToString());
            surface.Remove(currentNode);
            visited.Add(currentNode);
            if (vertexDescriptor.IsDestination(currentNode))
            {
                return currentCost;
            }

            var neighbours = vertexDescriptor.GetNeighbours(currentNode).ToArray();
            foreach (var neighbour in neighbours)
            {
                if(visited.Contains(neighbour)) continue;

                var cost = vertexDescriptor.GetCost(neighbour);
                
                if (surface.TryGetValue(neighbour, out var existingCost))
                {
                    if (currentCost + cost < existingCost)
                    {
                        surface[neighbour] = currentCost + cost;
                    }
                }
                else
                {
                    surface[neighbour] = currentCost + cost;
                }
            }
        }

        throw new ArgumentException("Cannot find result");
    }

    public static int Solve(string inputStr, Action<string>? log = null)
    {
        var input = inputStr.Trim().Split("\n").Select(x => x.Trim().Select(x => int.Parse(x.ToString())).ToArray()).ToArray();
        return Dijkstra(new CrucibleDescriptor(input), new CrucibleStep(0, 0, Direction.Right, -1), log) - input[0][0];
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

public class CrucibleDescriptor : IVertexDescriptor<CrucibleStep>
{
    private readonly int[][] _field;

    private readonly Direction[] directions = new Direction[]
        { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
    public CrucibleDescriptor(int[][] field)
    {
        _field = field;
    }
    public IEnumerable<CrucibleStep> GetNeighbours(CrucibleStep input)
    {
        var oppositeOfCurrent = Opposite(input.Direction);
        foreach (var direction in directions)
        {
            if(direction == oppositeOfCurrent) continue;

            var penalty = direction == input.Direction ? input.Penalty + 1 : 0;
            if (penalty > 2)
            {
                continue;
            }
            var step = GetStep(direction);
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
        return _field[input.Row][input.Col];
    }

    public bool IsDestination(CrucibleStep input)
    {
        return input.Row == MaxRow && input.Col == MaxCol;
    }

    private int MaxRow => _field.Length - 1;
    private int MaxCol => _field[0].Length - 1;

    private (int, int) GetStep(Direction direction)
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

    private Direction Opposite(Direction direction)
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


//public class PriorityQueue<T>
//{
//    private readonly SortedDictionary<int, Queue<T>> _dictionary = new();

//    public int Count { get; private set; }

//    public void Enqueue(T item, int priority)
//    {
//        if (!_dictionary.ContainsKey(priority))
//        {
//            _dictionary[priority] = new Queue<T>();
//        }

//        _dictionary[priority].Enqueue(item);
//        Count++;
//    }

//    public T Dequeue()
//    {
//        if (Count == 0)
//        {
//            throw new InvalidOperationException("Priority queue is empty.");
//        }

//        foreach (var queue in _dictionary)
//        {
//            if (queue.Value.Count > 0)
//            {
//                Count--;
//                return queue.Value.Dequeue();
//            }
//        }

//        throw new InvalidOperationException("Priority queue is empty.");
//    }
//}
