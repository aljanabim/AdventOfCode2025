using System.Numerics;
using Utils;

namespace Puzzles.Day8;

internal record Vec(double X, double Y, double Z)
{
    public double X { get; init; } = X;
    public double Y { get; init; } = Y;
    public double Z { get; init; } = Z;
    public int? CircuitId { get; set; } = null;
}

internal class Edge(Vec V1, Vec V2)
{
    public Vec V1 { get; } = V1;
    public Vec V2 { get; } = V2;
    public double Distance { get; } = Math.Sqrt(Math.Pow(V1.X - V2.X, 2) + Math.Pow(V1.Y - V2.Y, 2) + Math.Pow(V1.Z - V2.Z, 2));
}

public class Solution(string inputFileName) : SolutionBase<long>(inputFileName)
{

    Vec[] vectors = [];
    readonly List<Edge> edges = [];


    public override long SolvePart1()
    {
        List<int> circuits = [];
        foreach (var edge in edges.OrderBy(e => e.Distance).Take(1000))
        {
            // var edge = edges[i];
            if (edge.V1.CircuitId == edge.V2.CircuitId && edge.V1.CircuitId != null)
                continue;
            else if (edge.V1.CircuitId != null && edge.V2.CircuitId == null)
            {
                circuits[(int)edge.V1.CircuitId]++;
                edge.V2.CircuitId = edge.V1.CircuitId;
            }
            else if (edge.V1.CircuitId == null && edge.V2.CircuitId != null)
            {
                circuits[(int)edge.V2.CircuitId]++;
                edge.V1.CircuitId = edge.V2.CircuitId;
            }
            else if (edge.V1.CircuitId != null && edge.V2.CircuitId != null)
            {
                var minId = edge.V1.CircuitId < edge.V2.CircuitId ? edge.V1.CircuitId : edge.V2.CircuitId;
                var maxId = edge.V1.CircuitId > edge.V2.CircuitId ? edge.V1.CircuitId : edge.V2.CircuitId;
                circuits[(int)minId] += circuits[(int)maxId];
                circuits[(int)maxId] = 0;
                foreach (var vec in vectors)
                    if (vec.CircuitId == maxId)
                        vec.CircuitId = minId;
            }
            else
            {
                circuits.Add(2);
                edge.V1.CircuitId = circuits.Count - 1;
                edge.V2.CircuitId = circuits.Count - 1;
            }
        }
        return circuits.OrderDescending().Take(3).Aggregate((a, b) => a * b);
    }

    public override long SolvePart2()
    {
        return 0;
    }

    internal override void ParseInput()
    {
        // var result = Timing.Measure(() =>
        // {
        vectors = new Vec[Input.Length];

        for (int i = 0; i < Input.Length; i++)
            vectors[i] = ParseVecLine(Input[i]);

        for (int i = 0; i < vectors.Length - 1; i++)
        {
            for (int j = i + 1; j < Input.Length; j++)
            {
                edges.Add(new Edge(vectors[i], vectors[j]));
            }
        }
        // return true;
        // });
        // Console.WriteLine($"Parsing took {result.elapsed.TotalMilliseconds}ms");
    }

    static Vec ParseVecLine(string line)
    {
        var values = line.Split(',').Select(double.Parse).ToList();
        return new Vec(values[0], values[1], values[2]);
    }

}

