using System.Numerics;
using Utils;

namespace Puzzles.Day8;

internal record Vec(double X, double Y, double Z)
{
    public double X { get; init; } = X;
    public double Y { get; init; } = Y;
    public double Z { get; init; } = Z;
    public bool Connected { get; set; } = false;
}

internal class Edge(Vec V1, Vec V2)
{
    public Vec V1 { get; } = V1;
    public Vec V2 { get; } = V2;
    public double Distance { get; } = Math.Sqrt(Math.Pow(V1.X - V2.X, 2) + Math.Pow(V1.Y - V2.Y, 2) + Math.Pow(V1.Z - V2.Z, 2));
}

public class Solution(string inputFileName) : SolutionBase<long>(inputFileName)
{

    readonly List<Edge> edges = [];


    public override long SolvePart1()
    {
        foreach (var edge in edges)
        {
            edge.V1.Connected = true;
            Console.WriteLine($"{edge.Distance} \t {edge.V1}");
        }
        return 0;
    }

    public override long SolvePart2()
    {
        return 0;
    }

    internal override void ParseInput()
    {
        var result = Timing.Measure(() =>
        {
            Vec[] vectors = new Vec[Input.Length];

            for (int i = 0; i < Input.Length; i++)
                vectors[i] = ParseVecLine(Input[i]);

            for (int i = 0; i < vectors.Length - 1; i++)
            {
                for (int j = i + 1; j < Input.Length; j++)
                {
                    edges.Add(new Edge(vectors[i], vectors[j]));
                }
            }
            edges.Sort((a, b) => a.Distance.CompareTo(b.Distance));
            return true;
        });
        Console.WriteLine($"Parsing took {result.elapsed.TotalMilliseconds}ms");
    }

    static Vec ParseVecLine(string line)
    {
        var values = line.Split(',').Select(double.Parse).ToList();
        return new Vec(values[0], values[1], values[2]);
    }

}

