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

    // Ensure consistency for batched approach (does not give the same result every time)
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
                    // if (idStr.Length % 2 != 0) continue;
                    // bool firstHalfRepeats = CheckRepeats(i, idStr.Length / 2);
                    // int digitCount = CountDigits(i);
                    // if (digitCount % 2 != 0) continue;
                    // bool firstHalfRepeats = CheckRepeats(i, digitCount / 2);
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
                int digitCount = CountDigits(i);
                if (digitCount % 2 != 0) continue;
                bool firstHalfRepeats = CheckRepeats(i, digitCount / 2, digitCount);
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
                bool firstHalfRepeats = CheckRepeats(i, idStr.Length / 2, idStr.Length);
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


    public long SolvePart2Normal()
    {
        long result = 0;
        foreach (var range in Ranges)
        {
            for (long i = range.Start; i < range.End + 1; i++)
            {
                int digitCount = CountDigits(i);
                for (int j = 2; j <= digitCount; j++)
                {
                    if (digitCount % j != 0) continue;
                    bool doesRepeat = CheckRepeats(i, digitCount / j, digitCount);
                    if (doesRepeat)
                    {
                        result += i;
                        break;
                    }
                }
            }
        }
        return result;
    }

    private bool CheckRepeats(long number, int windowSize, int digitCount)
    {
        // 12341234  - 8 /2=4 /3 /4=2 /5/6/7 /8=1
        // 123123123 - 9 /2 /3=3 /4/5/6/7/8 /9=1
        // 11111 - 5 /2/3/4 /5=1

        var digits = number.ToString().ToCharArray();
        // var digits = GetDigits(number, digitCount);

        for (int i = 0; i < digits.Length - windowSize; i++)
        {
            if (digits[i] != digits[i + windowSize])
                return false;
        }
        return true;
    }

    private int CountDigits(long n)
    {
        return n == 0 ? 1 : (int)Math.Floor(Math.Log10(n)) + 1;
    }

    private int[] GetDigits(long n, int numberOfDigits)
    {
        var digits = new int[numberOfDigits];
        for (int i = 0; i < numberOfDigits; i++)
        {
            digits[numberOfDigits - i - 1] = (int)(n % 10);
            n /= 10;
        }
        return digits;
    }
}

