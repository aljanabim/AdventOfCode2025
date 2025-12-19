namespace Puzzles.Day11;

public enum NodeType
{
    You,
    Out,
    Other
}

internal class Node
{
    internal required string Id;
    internal Node[] Outputs = [];
    internal List<Node> Inputs = [];
    internal NodeType Type = NodeType.Other;
    internal bool passedFft = false;
    internal bool passedDac = false;
    internal ulong visits = 0;
    internal bool isChecked = false;
};

public class Solution(string inputFileName) : SolutionBase<ulong>(inputFileName)
{
    readonly Dictionary<string, Node> Nodes = [];

    public override ulong SolvePart1()
    {
        ulong outCounter = 0;
        var queue = new Queue<Node>();
        queue.Enqueue(Nodes["you"]);
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            if (node.Type == NodeType.Out)
                outCounter++;
            foreach (var output in node.Outputs)
                queue.Enqueue(output);
        }
        return outCounter;
    }


    internal record NodeCheckPoint(ulong Visits, ulong DacVisits, ulong FftVisits);
    readonly Dictionary<string, NodeCheckPoint> finderCache = [];
    internal NodeCheckPoint PathFinder(Node node)
    {
        if (node.Type == NodeType.Out)
        {
            return new NodeCheckPoint(1, 0, 0);
        }
        if (node.Outputs.Length == 0)
            return new NodeCheckPoint(0, 0, 0);

        if (finderCache.TryGetValue(node.Id, out var checkPoint))
        {
            return checkPoint;
        }
        ulong visits = 0;
        ulong dacVisits = 0;
        ulong fftVisits = 0;
        foreach (var output in node.Outputs)
        {
            var point = PathFinder(output);
            visits += point.Visits;
            dacVisits += point.DacVisits;
            fftVisits += point.FftVisits;
        }
        // Console.WriteLine($"Node {node.Id}");
        finderCache[node.Id] = new NodeCheckPoint(
            visits,
            node.Id == "dac" ? (fftVisits > 0 ? fftVisits : visits) : dacVisits,
            node.Id == "fft" ? (dacVisits > 0 ? dacVisits : visits) : fftVisits);
        return finderCache[node.Id];
    }

    public override ulong SolvePart2()
    {
        var counter = PathFinder(Nodes["svr"]);
        // foreach (var node in finderCache)
        // Console.WriteLine($"{node.Key}: {node.Value.Visits} fft={node.Value.FftVisits} dac={node.Value.DacVisits}");
        return Math.Min(counter.DacVisits, counter.FftVisits);
    }

    internal override void ParseInput()
    {
        // Populate nodes
        for (int i = 0; i < Input.Length; i++)
        {
            var nodeId = Input[i].Split(":").First();
            Nodes[nodeId] = new Node
            {
                Id = nodeId,
                Outputs = [],
                Type = nodeId switch
                {
                    "you" => NodeType.You,
                    "out" => NodeType.Out,
                    _ => NodeType.Other
                }
            };
        }
        Nodes["out"] = new Node { Id = "out", Outputs = [], Type = NodeType.Out };
        // Populate output connections foreach node
        for (int i = 0; i < Input.Length; i++)
        {
            var nodeId = Input[i].Split(':', StringSplitOptions.RemoveEmptyEntries).First();
            Nodes[nodeId].Outputs = [.. Input[i].Split(':').Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => Nodes[n])];
            foreach (var output in Nodes[nodeId].Outputs)
                output.Inputs.Add(Nodes[nodeId]);
        }
    }
}
