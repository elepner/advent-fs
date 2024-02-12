using System.Collections.ObjectModel;

namespace Day18;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}


public static class DirectionHelpers
{
    public static readonly Direction[] Directions = { Direction.Up, Direction.Down, Direction.Left, Direction.Right };

    public static readonly IReadOnlyDictionary<string, Direction> DirectionsMap =
        new ReadOnlyDictionary<string, Direction>(new Dictionary<string, Direction>()
        {
            { "U", Direction.Up },
            { "D", Direction.Down },
            { "R", Direction.Right },
            { "L", Direction.Left },
        });
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