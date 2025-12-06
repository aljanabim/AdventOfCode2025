using System.Linq;
using System.Numerics;
using System.Text;

namespace Puzzles.Day6;


internal enum Operation
{
    Add,
    Multiply,
}

public class Solution(string inputFileName) : SolutionBase<ulong>(inputFileName)
{
    readonly List<List<ulong>> NumberRows = [];
    readonly List<Operation> Operations = [];

    public override ulong SolvePart1()
    {
        ulong[] results = new ulong[Operations.Count];

        for (int row = 0; row < NumberRows.Count; row++)
        {
            for (int col = 0; col < Operations.Count; col++)
            {
                if (Operations[col] == Operation.Add)
                {
                    if (row == 0)
                        results[col] = NumberRows[row][col];
                    else
                        results[col] += NumberRows[row][col];
                }
                else
                {
                    if (row == 0)
                        results[col] = NumberRows[row][col];
                    else
                        results[col] *= NumberRows[row][col];
                }
            }
        }
        ulong sum = 0;
        foreach (var v in results)
        {

            sum += v;
        }

        return sum;
    }

    public override ulong SolvePart2()
    {
        ulong total = 0;
        var opLine = Input.Last();
        Operation op = Operation.Add;


        ulong subtotal = 0;
        for (int i = 0; i <= opLine.Length; i++)
        {
            // Empty column
            if (i < opLine.Length - 1 && opLine[i] == ' ' && opLine[i + 1] != ' ' || i == opLine.Length)
            {
                total += subtotal;
                subtotal = 0;
                continue;
            }

            // Update operator
            if (opLine[i] != ' ')
            {
                op = opLine[i] == '*' ? Operation.Multiply : Operation.Add;
                if (op == Operation.Multiply)
                    subtotal = 1;
            }

            // Compute subtotal of column
            var numStr = new StringBuilder();
            foreach (var row in Input[..^1])
            {
                numStr.Append(row[i]);
            }
            if (op == Operation.Add)
                subtotal += ulong.Parse(numStr.ToString());
            else
                subtotal *= ulong.Parse(numStr.ToString());
        }
        return total;
    }

    private char[,] convertInputToArray()
    {
        var output = new char[Input.Length, Input.Last().Length];
        for (int row = 0; row < Input.Length - 1; row++)
        {
            for (int col = 0; col < Input.Last().Length; col++)
            {
                output[row, col] = Input[row][col];
            }
        }
        return output;
    }

    internal override void ParseInput()
    {
        // Parse operations
        foreach (var value in Input.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries))
            Operations.Add(value == "*" ? Operation.Multiply : Operation.Add);

        for (int i = 0; i < Input.Length - 1; i++)
        {
            var values = Input[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            NumberRows.Add([.. values.Select(ulong.Parse)]);
        }
    }
}

