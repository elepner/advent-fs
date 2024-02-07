using System.Security.Cryptography.X509Certificates;

namespace Day17;
// 1077 too high
public class Pt2CrucibleDescriptor(int[][] field) : IVertexDescriptor<CrucibleStep>
{
    private IEnumerable<CrucibleStep> GetNeighboursInternal(CrucibleStep input)
    {
        var oppositeOfCurrent = DirectionHelpers.Opposite(input.Direction);
        foreach (var direction in DirectionHelpers.Directions)
        {
            if (direction == oppositeOfCurrent && input.Penalty != -1) continue;

            var step = DirectionHelpers.GetStep(direction);
            if (direction != input.Direction)
            {
                var newRow = input.Row + step.Item1 * 4;
                var newCol = input.Col + step.Item2 * 4;
                yield return new CrucibleStep(Direction: direction, Penalty: 0, Row: newRow, Col: newCol);
                continue;
            }

            var newPenalty = input.Penalty + 1;
            if (newPenalty > 6)
            {
                continue;
            }

            yield return new CrucibleStep(input.Row + step.Item1, input.Col + step.Item2, direction, newPenalty);
        }
        
    }

    public bool FitField(int row, int col)
    {
        return 0 <= row && row <= MaxRow && 0 <= col && col <= MaxCol;
    }

    public IEnumerable<CrucibleStep> GetNeighbours(CrucibleStep input)
    {
        return GetNeighboursInternal(input).Where(x => FitField(x.Row, x.Col));
    }

    public int GetCost(CrucibleStep input)
    {
        if (input.Penalty == 0)
        {
            return EnumerateInOppositeDirection(input).Sum();
        }
        return field[input.Row][input.Col];
    }

    private IEnumerable<int> EnumerateInOppositeDirection(CrucibleStep step)
    {
        var (dRow, dCol) = DirectionHelpers.GetStep(DirectionHelpers.Opposite(step.Direction));
        return Enumerable.Range(0, 4)
            .Select(i =>
            {
                return field[step.Row + dRow * i][step.Col + dCol * i];
            });
    }

    public bool IsDestination(CrucibleStep input)
    {
        return input.Row == MaxRow && input.Col == MaxCol;
    }

    private int MaxRow => field.Length - 1;
    private int MaxCol => field[0].Length - 1;
}