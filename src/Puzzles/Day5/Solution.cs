using System.Diagnostics;

namespace Puzzles.Day5;

public class Solution(string inputFileName) : SolutionBase<long>(inputFileName)
{
    private record struct Range(long From, long To);
    readonly List<Range> Ranges = [];
    readonly List<long> Ingredients = [];

    public override long SolvePart1()
    {
        var nFresh = 0;
        foreach (var ing in Ingredients)
        {
            foreach (var range in Ranges)
            {
                if (range.From <= ing && ing <= range.To)
                {
                    nFresh++;
                    break;
                }
            }
        }
        return nFresh;
    }

    public override long SolvePart2()
    {
        long rangeSum = 0;
        var sortedRanges = Ranges.OrderBy(range => range.From).ThenBy(range => range.To - range.From).ToList();
        for (int i = 0; i < sortedRanges.Count; i++)
        {
            if (i < sortedRanges.Count - 1 && sortedRanges[i].To >= sortedRanges[i + 1].From)
            {
                sortedRanges[i + 1] = new Range(sortedRanges[i].To + 1, Math.Max(sortedRanges[i + 1].To, sortedRanges[i].To));
            }
            rangeSum += sortedRanges[i].To - sortedRanges[i].From + 1;
        }
        return rangeSum;
    }

    internal override void ParseInput()
    {
        bool parseRanges = true;
        foreach (var line in Input)
        {
            if (line.Length == 0)
            {
                parseRanges = false;
                continue;
            }

            if (parseRanges)
            {
                var rangeStr = line.Split('-');
                Ranges.Add(new Range(long.Parse(rangeStr.First()), long.Parse(rangeStr.Last())));
            }
            else
            {
                Ingredients.Add(long.Parse(line));
            }
        }

    }
}

