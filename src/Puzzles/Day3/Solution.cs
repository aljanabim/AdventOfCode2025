namespace Puzzles.Day3;

public class Solution(string inputFileName) : SolutionBase<long>(inputFileName)
{
    internal override void ParseInput()
    {
    }

    public override long SolvePart1()
    {
        int result = 0;
        foreach (var line in Input)
            result += FindLargest2Digits(line.ToCharArray());
        return result;
    }

    public override long SolvePart2()
    {
        long result = 0;
        foreach (var line in Input)
            result += FindLargestNDigits(line.ToCharArray(), 12);
        return result;
    }

    private int FindLargest2Digits(char[] numbers)
    {
        char leftMaxNum = '0';
        char rightMaxNum = '0';

        int leftMaxIdx = 0;

        for (int i = 0; i < numbers.Length - 1; i++)
        {
            if (numbers[i] > leftMaxNum)
            {
                leftMaxIdx = i;
                leftMaxNum = numbers[i];
            }
        }
        for (int i = leftMaxIdx + 1; i < numbers.Length; i++)
        {
            if (numbers[i] > rightMaxNum)
                rightMaxNum = numbers[i];
        }
        return (leftMaxNum - '0') * 10 + rightMaxNum - '0';
    }

    private long FindLargestNDigits(char[] numbers, int numDigits)
    {
        long num = 0;
        int lastMaxDigitIdx = -1;
        for (int d = numDigits; d > 0; d--)
        {
            char lastMaxDigitNum = '0';
            for (int i = lastMaxDigitIdx + 1; i < numbers.Length - d + 1; i++)
            {
                if (numbers[i] > lastMaxDigitNum)
                {
                    lastMaxDigitIdx = i;
                    lastMaxDigitNum = numbers[i];
                }
            }
            num += (lastMaxDigitNum - '0') * (long)Math.Pow(10, d - 1);
        }
        return num;
    }


}

