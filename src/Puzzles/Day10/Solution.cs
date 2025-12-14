using Puzzles.Day10.Algebra;

namespace Puzzles.Day10;

public class Solution(string inputFileName, bool debug = false) : SolutionBase<double>(inputFileName)
{
    record struct Node(int PrevState, int MaskIdx, int Depth);

    public int Search(Machine machine)
    {
        int initialState = 0;

        var queue = new Queue<Node>();
        for (int i = 0; i < machine.ButtonMasks.Count; i++)
            queue.Enqueue(new Node(initialState, i, 1));

        while (queue.Count > 0)
        {
            var op = queue.Dequeue();
            var newState = op.PrevState ^ machine.ButtonMasks[op.MaskIdx];
            if (debug)
                Console.WriteLine($"{op.Depth} > {op.PrevState.ToBinString(6)} ^ {machine.ButtonMasks[op.MaskIdx].ToBinString(6)} = {newState.ToBinString(6)}");
            if (newState == machine.Indicator.State)
            {
                if (debug)
                    Console.WriteLine($"Found solution at depth {op.Depth}");
                return op.Depth;
            }

            for (int i = op.MaskIdx + 1; i < machine.ButtonMasks.Count; i++)
                queue.Enqueue(new Node(newState, i, op.Depth + 1));
        }
        return -1;
    }

    public override double SolvePart1()
    {
        var result = 0;
        foreach (var line in Input)
        {
            var machine = new Machine(line);
            var minDeapth = Search(machine);
            if (minDeapth > 0)
                result += minDeapth;
        }
        return result;
    }


    public override double SolvePart2()
    {
        double result = 0;

        // ILSSolver.SetMaxIters(10);

        int i = 0;
        foreach (var line in Input)
        {
            i++;
            if (i != 9)
            {
                continue;
            }
            var machine = new Machine(line);
            // var v = new Vector([1, 3, 0, 3, 1, 2]);
            ILSSolver.iters = 0;
            Console.WriteLine($"{machine.ButtonMatrix}");
            // Console.WriteLine($"v1: {v} {machine.ButtonMatrix.Multiply(v)}");
            var sol = ILSSolver.Solve(machine.ButtonMatrix, machine.Joltages);
            result += sol.Sum();
            Console.WriteLine($"Total iterations {ILSSolver.iters} sol {sol} {sol.Data.Sum()} cost {ILSSolver.ComputeCost(machine.ButtonMatrix, sol, machine.Joltages)}");
            Console.WriteLine($"{machine.Joltages} {machine.ButtonMatrix.Multiply(sol)}");
            // break;
        }
        return result;
    }

    internal override void ParseInput()
    {
    }
}

