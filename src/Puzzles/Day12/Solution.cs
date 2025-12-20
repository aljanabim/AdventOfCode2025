using System.Text;

namespace Puzzles.Day12;


public class Shape
{
    public bool[,] Grid { get; set; } = new bool[3, 3];
    public int Size
    {
        get
        {
            int counter = 0;
            foreach (var row in Grid)
                if (row)
                    counter++;
            return counter;
        }
    }
    public override string ToString()
    {
        var str = new StringBuilder();
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                str.Append(Grid[row, col]);
            }
            str.AppendLine();
        }
        return str.ToString();
    }
}

public class Region
{
    required public int Width { get; set; }
    required public int Height { get; set; }
    required public int[] ShapeAmounts;
    public int Area { get { return Width * Height; } }
}

public class Solution(string inputFileName) : SolutionBase<int>(inputFileName)
{
    readonly List<Shape> Shapes = [];
    readonly List<Region> Regions = [];

    public override int SolvePart1()
    {
        var counter = 0;
        foreach (var region in Regions)
        {
            var requiredGiftArea = 0;
            for (int i = 0; i < region.ShapeAmounts.Length; i++)
                requiredGiftArea += Shapes[i].Size * region.ShapeAmounts[i];
            if (requiredGiftArea < region.Area)
                counter++;
        }
        return counter;
    }

    public override int SolvePart2()
    {
        return 0;
    }


    private Shape ParseShape(string[] lines)
    {
        var shape = new Shape();
        for (int row = 0; row < lines.Length; row++)
        {
            for (int col = 0; col < lines[row].Length; col++)
            {
                shape.Grid[row, col] = lines[row][col] == '#';
            }
        }
        return shape;
    }

    internal override void ParseInput()
    {
        // for (int i = 0; i < Input.Length; i++)
        int i = 0;
        foreach (var line in Input)
        {
            if (line.Length > 0 && line[1] == ':')
                Shapes.Add(ParseShape(Input[(i + 1)..(i + 4)]));
            if (line.Contains('x'))
            {
                var lineSplit = line.Split(':');
                var numbers = lineSplit.First().Split('x', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
                var presentCounts = lineSplit.Last().Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                Regions.Add(new Region { Width = numbers.First(), Height = numbers.Last(), ShapeAmounts = presentCounts });
            }
            i++;


        }
    }
}

