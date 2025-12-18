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


    Dictionary<string, int> finderCache = [];
    internal int PathFinder(Node node, bool fftVisited, bool dacVisited)
    {
        if (node.Type == NodeType.Out)
            return fftVisited && dacVisited ? 1 : 0;
        else if (node.Outputs.Length == 0)
            return 0;

        if (finderCache.TryGetValue(node.Id, out int counter))
            return counter;
        counter = 0;
        bool visitingFFT = node.Id == "fft" || fftVisited;
        bool visitingDAC = node.Id == "dac" || dacVisited;

        foreach (var output in node.Outputs)
        {
            var pathCount = PathFinder(output, visitingFFT, visitingDAC);
            Console.WriteLine($"Node {node.Id} Output {output.Id} pathCount {pathCount} fft={visitingFFT} dac={visitingDAC}");


            counter += pathCount;
        }
        finderCache[node.Id] = counter;
        return counter;
    }

    public override ulong SolvePart2()
    {
        // foreach (var node in Nodes)
        //     Console.WriteLine($"{node.Key} \n\tInputs:{string.Join(", ", node.Value.Inputs.Select(n => n.Id))} \n\tOutput:{string.Join(", ", node.Value.Outputs.Select(n => n.Id))}");
        var queue = new Queue<Node>();
        var outNode = Nodes["out"];
        outNode.visits = 1;
        queue.Enqueue(outNode);
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            if (node.isChecked)
                continue;
            node.isChecked = true;
            // Console.WriteLine($"{node.Id} {node.visits} fft={node.passedFft} dac={node.passedDac}");
            if (node.Id == "fft")
                node.passedFft = true;
            if (node.Id == "dac")
                node.passedDac = true;

            foreach (var input in node.Inputs)
            {
                /* node and input have matching passed */
                if (node.passedFft == input.passedFft && node.passedDac == input.passedDac)
                {
                    input.visits += node.visits;
                }
                /* node has more passed than input */
                else if ((!input.passedFft && !input.passedDac && (node.passedDac || node.passedFft)) ||
                         ((input.passedFft || input.passedDac) && input.passedDac != input.passedFft && node.passedDac && node.passedFft))
                {
                    input.visits = node.visits;
                    input.passedFft = node.passedFft;
                    input.passedDac = node.passedDac;
                }
                /* both node and parent have a different passed */
                else if ((input.passedFft || input.passedDac) && (node.passedFft || node.passedDac) && node.passedDac != input.passedDac && node.passedFft != input.passedFft)
                {
                    input.visits += node.visits;
                    input.passedFft = true;
                    input.passedDac = true;
                }
                queue.Enqueue(input);
            }
        }
        return Nodes["svr"].visits;
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
