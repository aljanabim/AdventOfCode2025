namespace Puzzles.Day9;

internal record Position(int X, int Y)
{
    public ulong Area(Position p)
    {
        return (ulong)(Math.Abs(X - p.X) + 1) * (ulong)(Math.Abs(Y - p.Y) + 1);
    }
}

public class Solution(string inputFileName) : SolutionBase<ulong>(inputFileName)
{
    Position[] Tiles = [];

    public override ulong SolvePart1()
    {
        var tl = Tiles.First();
        var tr = Tiles.First();
        var bl = Tiles.First();
        var br = Tiles.First();

        // Update corners
        foreach (var tile in Tiles)
        {
            if (tile.X + tile.Y < tl.X + tl.Y)
                tl = tile;
            if (-tile.X + tile.Y < -tr.X + tr.Y)
                tr = tile;
            if (tile.X - tile.Y < bl.X - bl.Y)
                bl = tile;
            if (tile.X + tile.Y > br.X + br.Y)
                br = tile;
        }
        ulong a1 = tl.Area(br);
        ulong a2 = tr.Area(bl);
        return a1 > a2 ? a1 : a2;
    }

    public ulong SolvePart1DoubleLoop()
    {
        ulong maxArea = 0;
        for (int i = 0; i < Tiles.Length - 1; i++)
        {
            for (int j = i + 1; j < Tiles.Length; j++)
            {
                var area = Tiles[i].Area(Tiles[j]);
                if (area > maxArea)
                    maxArea = area;

            }
        }
        return maxArea;
    }

    public override ulong SolvePart2()
    {
        return 0;
    }

    internal override void ParseInput()
    {
        Tiles = new Position[Input.Length];
        for (int i = 0; i < Input.Length; i++)
        {
            var values = Input[i].Split(",").Select(int.Parse);
            Tiles[i] = new Position(values.First(), values.Last());
        }
    }
}