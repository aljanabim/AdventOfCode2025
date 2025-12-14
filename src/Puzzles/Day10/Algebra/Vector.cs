namespace Puzzles.Day10.Algebra;

public class Vector
{
    public double[] Data;
    public int Size => Data.GetLength(0);

    public Vector() => Data = [];
    public Vector(double[] data) => Data = data;
    public Vector(int size) => Data = new double[size];

    public double Sum()
    {
        return Data.Sum();
    }

    public double Norm()
    {
        double norm = 0;
        foreach (var d in Data)
        {
            norm += d * d;
        }
        return norm;
    }

    public static Vector operator -(Vector v1, Vector v2)
    {
        if (v1.Size != v2.Size)
            throw new ArgumentException("Mismatch vector sizes v1({v1.Size})!=v2({v2.Size})");

        var output = new Vector(v1.Size);
        for (int i = 0; i < v1.Size; i++)
        {
            output.Data[i] = v1.Data[i] - v2.Data[i];
        }
        return output;
    }

    public override string ToString()
    {
        return $"({string.Join(',', Data)})";
    }
}

