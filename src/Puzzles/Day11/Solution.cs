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
    internal NodeType Type = NodeType.Other;
};

public class Solution(string inputFileName) : SolutionBase<int>(inputFileName)
{
    readonly Dictionary<string, Node> Nodes = [];

    public override int SolvePart1()
    {

        int outCounter = 0;
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
        // foreach (var node in Nodes.Values)
        // {
        //     Console.WriteLine($"{node.Id}: {string.Join("-", node.Outputs.Select(n => n.Id))} {node.Type}");
        // }
        return outCounter;
    }

    public override int SolvePart2()
    {
        throw new NotImplementedException();
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
        }
    }
}
