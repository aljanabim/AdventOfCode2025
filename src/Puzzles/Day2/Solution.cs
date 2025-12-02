using System.Threading.Tasks.Dataflow;

namespace Puzzles.Day2;

public record Range(long Start, long End);

public class Solution
{
    public Range[] Ranges { get; set; }
    private Dictionary<long, bool> _cache = [];

    public Solution(string infile)
    {
        var line = new StreamReader(infile).ReadToEnd();
        Ranges = ParseLine(line);

    }

    private Range[] ParseLine(string line)
    {
        var rangesRaw = line.Split(',');
        var output = new Range[rangesRaw.Length];
        for (int i = 0; i < rangesRaw.Length; i++)
        {
            var range = rangesRaw[i].Trim().Split('-');
            output[i] = new Range(long.Parse(range[0]), long.Parse(range[1]));
        }
        return output;
    }

    public long SolvePart1Batched()
    {
        long result = 0;
        var linkOption = new DataflowLinkOptions { PropagateCompletion = true };
        var blockOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

        var batch = new BatchBlock<Range>(5);
        var computeBatchResult = new ActionBlock<Range[]>((ranges) =>
        {
            foreach (var range in ranges)
            {
                for (long i = range.Start; i < range.End + 1; i++)
                {
                    var idStr = i.ToString().ToCharArray();
                    if (idStr.Length % 2 != 0) continue;
                    bool firstHalfRepeats = true;
                    for (long k = 0; k < idStr.Length / 2; k++)
                    {
                        if (firstHalfRepeats && idStr[k] != idStr[idStr.Length / 2 + k])
                        {
                            firstHalfRepeats = false;
                            break;
                        }
                    }
                    if (firstHalfRepeats)
                    {
                        result += i;
                    }
                }
            }
        }, blockOptions);

        batch.LinkTo(computeBatchResult, linkOption);
        foreach (var range in Ranges)
        {
            batch.Post(range);
        }
        batch.Complete();
        computeBatchResult.Completion.Wait();
        return result;
    }

    public long SolvePart1Normal()
    {
        long result = 0;

        foreach (var range in Ranges)
        {
            for (long i = range.Start; i < range.End + 1; i++)
            {
                var idStr = i.ToString().ToCharArray();
                if (idStr.Length % 2 != 0) continue;
                bool firstHalfRepeats = true;
                for (long k = 0; k < idStr.Length / 2; k++)
                {
                    if (firstHalfRepeats && idStr[k] != idStr[idStr.Length / 2 + k])
                    {
                        firstHalfRepeats = false;
                        break;
                    }
                }
                if (firstHalfRepeats)
                {
                    result += i;
                }
            }
        }
        return result;
    }

    public long SolvePart1Cached()
    {
        long result = 0;

        foreach (var range in Ranges)
        {
            for (long i = range.Start; i < range.End + 1; i++)
            {

                var idStr = i.ToString().ToCharArray();
                if (idStr.Length % 2 != 0) continue;
                if (_cache.TryGetValue(i, out _))
                {
                    result += i;
                    continue;
                }
                bool firstHalfRepeats = true;
                for (long k = 0; k < idStr.Length / 2; k++)
                {
                    if (firstHalfRepeats && idStr[k] != idStr[idStr.Length / 2 + k])
                    {
                        firstHalfRepeats = false;
                        break;
                    }
                }
                if (firstHalfRepeats)
                {
                    _cache[i] = true;
                    _cache[long.Parse(string.Join("", idStr.Reverse()))] = true;
                    result += i;
                }
            }
        }
        return result;
    }
}

