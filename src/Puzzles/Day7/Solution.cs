namespace Puzzles.Day7;

public class Solution(string inputFileName) : SolutionBase<long>(inputFileName)
{
    public override long SolvePart1()
    {
        var numSplittings = 0;
        bool[] beamsAbove = new bool[Input.Skip(1).First().Length];
        for (int row = 0; row < Input.Length; row += 2)
        {
            for (int col = 0; col < Input[row].Length; col++)
            {

                if (Input[row][col] == '^')
                {
                    if (beamsAbove[col])
                    {
                        numSplittings++;
                        // Update beam above
                        beamsAbove[col] = false;
                        if (col - 1 >= 0)
                            beamsAbove[col - 1] = true;
                        if (col + 1 < beamsAbove.Length)
                            beamsAbove[col + 1] = true;
                    }
                    continue;
                }
                if (Input[row][col] == 'S')
                {
                    beamsAbove[col] = true;
                    break;
                }
            }
        }
        return numSplittings;
    }

    private readonly Dictionary<(int depth, int position), long> _cache = [];

    private long Traverse(int depth, int position)
    {
        if (_cache.TryGetValue((depth, position), out long val))
            return val;

        if (depth == Input.Length - 1)
            return 1;

        if (Input[depth][position] == '^')
        {
            long valLeft = 0, valRight = 0;
            if (position > 0)
            {
                valLeft = Traverse(depth + 1, position - 1);
                _cache[(depth + 1, position - 1)] = valLeft;
            }
            if (position < Input[depth + 1].Length - 1)
            {
                valRight = Traverse(depth + 1, position + 1);
                _cache[(depth + 1, position + 1)] = valRight;
            }
            return valLeft + valRight;
        }

        val = Traverse(depth + 1, position);
        _cache[(depth + 1, position)] = val;
        return val;
    }

    public override long SolvePart2()
    {
        var startPos = Input.First().IndexOf('S');
        var totalPaths = Traverse(1, startPos);

        return totalPaths;
    }

    internal override void ParseInput()
    {
    }
}

