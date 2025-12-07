namespace Puzzles;

using Utils;

public abstract class SolutionBase<TSolution>
{
    internal string[] Input { get; set; } = [];

    public SolutionBase(string inputFileName)
    {
        ReadInput(inputFileName);
        ParseInput();
    }

    void ReadInput(string fileName)
    {
        Input = File.ReadAllLines(fileName);
    }

    internal abstract void ParseInput();

    public abstract TSolution SolvePart1();

    public abstract TSolution SolvePart2();

    public virtual void Solve(int day, int part, Func<TSolution> solver, string? solverName = null)
    {
        var result = Timing.Measure(solver);
        var fullSolverName = solverName == null ? "" : $" ({solverName})";

        var elapsed = result.elapsed.TotalMilliseconds > 1 ? $"{result.elapsed.TotalMilliseconds}ms" : $"{result.elapsed.TotalMicroseconds}µs";

        Console.WriteLine($"Day {day} Part {part}{fullSolverName}:\t{result.result} ({elapsed})");
    }
    public virtual void Solve(int day, int part, string? solverName = null)
    {
        switch (part)
        {
            case 1:
                Solve(day, part, SolvePart1, solverName);
                break;
            case 2:
                Solve(day, part, SolvePart2, solverName);
                break;
            default:
                throw new ArgumentException($"Invalid part number {part}");
        }

    }
}