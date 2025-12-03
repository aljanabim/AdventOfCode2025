namespace Puzzles.Day3;

public class Solution(string inputFileName) : SolutionBase<int>(inputFileName)
{
    internal override void ParseInput()
    {
    }

    public override int SolvePart1()
    {
        int result = 0;
        foreach (var line in Input)
        {
            var (leftDigit, rightDigit) = FindMax(line.ToCharArray());
            result += 10 * leftDigit + rightDigit;
        }
        return result;
    }

    private (int leftDigit, int rightDigit) FindMax(char[] numbers)
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
        return (leftMaxNum - '0', rightMaxNum - '0');
    }

    public override int SolvePart2()
    {
        return 0;
    }

}

