using System.Text;

namespace Puzzles.Day10.Algebra;

public class Matrix
{
    public double[,] Data = new double[,] { };
    public int Rows => Data.GetLength(0);
    public int Cols => Data.GetLength(1);

    public Vector Multiply(Vector v)
    {
        if (v.Size != Cols)
            throw new ArgumentException($"Vector size {v.Size} mismatches Matrix second dimension {Cols}");

        var result = new Vector(Rows);

        for (int row = 0; row < Rows; row++)
        {
            double tmpRes = 0;
            for (int col = 0; col < Cols; col++)
            {
                tmpRes += Data[row, col] * v.Data[col];
            }
            result.Data[row] = tmpRes;
        }
        return result;
    }

    public override string ToString()
    {
        var str = new StringBuilder();
        str.Append('[');
        for (int row = 0; row < Rows; row++)
        {
            if (row > 0)
                str.Append(' ');
            for (int col = 0; col < Cols; col++)
            {
                str.Append(Data[row, col]);
                if (col < Cols - 1)
                    str.Append(',');
            }
            if (row < Rows - 1)
                str.Append('\n');
            else
                str.Append(']');
        }
        return str.ToString();
    }
}

