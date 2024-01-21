using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Day12;

public class Solution
{
    public static SpringInput Parse(string input)
    {
        var parts = input.Split(' ');
        var data = parts[0];
        var criteria = parts[1].Split(',').Select(int.Parse).ToArray();
        return new SpringInput(data, criteria);
    }

    public static bool Solve(SpringInput input)
    {
        var state = new State(0, null, input.Criteria);
        foreach (var c in input.Data)
        {
            if (c == '#')
            {
                state = state.ProcessBroken();
            }

            if (c == '.')
            {
                state = state.ProcessNormal();
            }
            
            if (state == null)
            {
                return false;
            }
        }
        return state.IsCompleted();
    }

    public static long SolveCount(SpringInput input, Action<string>? log = null)
    {
        return SolveCount(new Params(input.Data, 0, new State(0, null, input.Criteria), log ?? (s => {})), new Dictionary<Params, long>());
        
        //List<string> solutions = new List<string>();
        //parent.Traverse((node) =>
        //{

        //    if (node.IsLeaf())
        //    {
        //        string s = "";
        //        var current = node;
        //        while (current != null)
        //        {
        //            s = current.Value + s;
        //            current = current.Parent;
        //        }
        //        solutions.Add(s);
        //    }
        //});
    }
    
    private static long SolveCount(Params param, Dictionary<Params, long> cache)
    {
        State? state = param.State;

        for (var i = param.StartFrom; i < param.Input.Length; i++)
        {
            if (state == null)
            {
                return 0;
            }

            var c = param.Input[i];
            switch (c)
            {
                case '#':
                    state = state.ProcessBroken();
                    break;
                case '.':
                    state = state.ProcessNormal();
                    break;
                case '?':
                {
                    var state1 = state;
                    var r1 = Process(param, i, () => state1.ProcessNormal(), cache);
                    var r2 = Process(param, i, () => state1.ProcessBroken(), cache);
                    return r1 + r2;

                }
            }
        }

        if (state != null && state.IsCompleted())
        {
            return 1;
        }

        return 0;
    }

    private static long Process(Params current, int i, Func<State?> process, Dictionary<Params, long> cache)
    {
        var newParams = current with
        {
            StartFrom = i + 1,
            State = process(),
        };
        if (!cache.TryGetValue(newParams, out var res))
        {
            res = SolveCount(current with
            {
                StartFrom = i + 1,
                State = process(),
            }, cache);
            cache.Add(newParams, res);
            newParams.Log("miss");

        }
        else
        {
            newParams.Log("hit");
        }
        
        return res;
    }
}


public record SpringInput(string Data, int[] Criteria)
{
    public SpringInput Unfold()
    {
        return new SpringInput(string.Join("?", Enumerable.Range(0, 5).Select(x => Data)), Enumerable.Range(0, 5).SelectMany(x => Criteria).ToArray());
    }
}

public record State(int CriteriaIndex, int? CriteriaPosition, int[] Criteria)
{
    public State? Next(SpringType springType)
    {
        if (springType == SpringType.Broken)
        {
            return ProcessBroken();
        }

        if (springType == SpringType.Normal)
        {
            return ProcessNormal();
        }

        throw new ArgumentException();
    }

   
    

    public State? ProcessBroken()
    {
        return WithBoundsCheck(() =>
        {
            if (IsCompleted())
            {
                return null;
            }

            if (CriteriaPosition == null)
            {
                return this with
                {
                    CriteriaPosition = 0
                };
            }

            if (IsOnLastBrokenSpring()) return null;
            return this with
            {
                CriteriaPosition = CriteriaPosition + 1
            };
        });
    }

    public State? ProcessNormal()
    {
        return WithBoundsCheck(() =>
        {
            if (CriteriaPosition == null)
            {
                return this;
            }

            if (IsOnLastBrokenSpring())
            {
                return this with
                {
                    CriteriaIndex = CriteriaIndex + 1,
                    CriteriaPosition = null
                };
            }

            return null;

        });
    }

    private State? WithBoundsCheck(Func<State?> process)
    {
        var result = process();
        if (result == null) return null;
        

        if (result.CriteriaPosition != null)
        {
            if (result.CriteriaPosition == result.Criteria[result.CriteriaIndex])
            {
                return null;
            }
        }
        return result;
    }

    public bool IsOnLastBrokenSpring()
    {
        return CriteriaPosition != null && CriteriaPosition == Criteria[CriteriaIndex] - 1;
    }

    public bool IsCompleted()
    {
        var isOnLastBrokenSpring = IsOnLastBrokenSpring();
        return (CriteriaIndex == Criteria.Length && CriteriaPosition == null) || (isOnLastBrokenSpring && CriteriaIndex >= Criteria.Length - 1) ;
    }
}

public enum SpringType
{
    Broken,
    Normal,
}

public record Params(string Input, int StartFrom, State? State, Action<string> Log);

public class Node<T>
{
    public T Value { get; set; }
    public List<Node<T>> Children = new();
    public Node<T>? Parent { get; set; }

    public void AddChild(Node<T> node)
    {
        Children.Add(node);
        node.Parent = this;
    }

    public void Traverse(Action<Node<T>> action)
    {
        action(this);
        foreach (var child in Children)
        {
            child.Traverse(action);
        }
    }

    public bool IsLeaf()
    {
        return Children.Count == 0;
    }
}