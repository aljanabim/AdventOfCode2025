namespace Puzzles.Day10;

public static class IntExtensions
{
    public static string ToBinString(this int n, int padding = 0)
    {
        return $"{Convert.ToString(n, 2).PadLeft(padding, '0')}";
    }
}

