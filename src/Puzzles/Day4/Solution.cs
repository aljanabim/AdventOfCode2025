using System.Text;

namespace Puzzles.Day4;


public class Solution(string inputFileName, bool debug = false) : SolutionBase<int>(inputFileName)
{
    private int rows;
    private int cols;

    public override int SolvePart1() => CountAccessibleRolls();

    public override int SolvePart2()
    {
        var count = 0;
        while (true)
        {
            if (debug)
                Console.Clear();

            int accessible = CountAccessibleRolls(withRemoval: true);
            count += accessible;
            if (accessible == 0)
                break;
            if (debug)
                Thread.Sleep(100);
        }
        return count;
    }

    internal override void ParseInput()
    {
        rows = Input.Length;
        cols = Input.First().Length;
    }

    private int CountAccessibleRolls(bool withRemoval = false)
    {
        int count = 0;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (debug)
                {
                    if (Input[row][col] == '@' && CountNeighboringRolls(row, col) < 4)
                        Console.Write($"x");
                    else
                        Console.Write($"{Input[row][col]}");
                }

                if (Input[row][col] == '@' && CountNeighboringRolls(row, col) < 4)
                {
                    count += 1;
                    if (withRemoval)
                    {
                        var stringBuilder = new StringBuilder(Input[row]);
                        stringBuilder[col] = 'x';
                        Input[row] = stringBuilder.ToString();
                    }
                }
            }
            if (debug)
                Console.WriteLine($"");
        }

        return count;
    }

    private int CountNeighboringRolls(int row, int col)
    {
        int count = 0;
        // top
        if (row - 1 >= 0)
        {
            if (Input[row - 1][col] == '@') count++;
            // top-left
            if (col - 1 >= 0 && Input[row - 1][col - 1] == '@') count++;
            // top-right
            if (col + 1 < cols && Input[row - 1][col + 1] == '@') count++;
        }

        // middle-left
        if (col - 1 >= 0 && Input[row][col - 1] == '@') count++;
        // middle-right
        if (col + 1 < cols && Input[row][col + 1] == '@') count++;

        // bottom
        if (row + 1 < rows)
        {
            if (Input[row + 1][col] == '@') count++;
            // bottom-left
            if (col - 1 >= 0 && Input[row + 1][col - 1] == '@') count++;
            // bottom-right
            if (col + 1 < cols && Input[row + 1][col + 1] == '@') count++;
        }

        return count;
    }
}