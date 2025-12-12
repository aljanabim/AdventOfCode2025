using System.Numerics;
using ScottPlot;

namespace Puzzles.Day9;

enum Satisfaction
{
    None,
    Horizontal,
    Vertical,
    Both
}

internal record Position(int X, int Y)
{
    public ulong Area(Position p)
    {
        return (ulong)(Math.Abs(X - p.X) + 1) * (ulong)(Math.Abs(Y - p.Y) + 1);
    }
    public ulong Distance(Position p)
    {
        return (ulong)Math.Sqrt(Math.Pow(X - p.X, 2) + Math.Pow(Y - p.Y, 2));
    }

    public Vector2 ToVector()
    {
        return Vector2.Create(X, Y);
    }
}

internal class Constraint(Position position, bool horizontalGeq, bool verticalGeq)
{
    public Position Position { get; } = position;
    public int VerticalDir { get; } = verticalGeq ? 1 : -1;
    public int HorizontalDir { get; } = horizontalGeq ? 1 : -1;
    public List<Constraint> Adjacent = [];


    public Satisfaction Check(Position point)
    {
        var horzSatisfied = false;
        var vertSatisfied = false;
        if (horizontalGeq && Position.X <= point.X || !horizontalGeq && Position.X >= point.X)
        {
            horzSatisfied = true;
        }
        if (verticalGeq && Position.Y <= point.Y || !verticalGeq && Position.Y >= point.Y)
        {
            vertSatisfied = true;
        }

        if (horzSatisfied && vertSatisfied)
            return Satisfaction.Both;
        if (horzSatisfied)
            return Satisfaction.Horizontal;
        if (vertSatisfied)
            return Satisfaction.Vertical;
        return Satisfaction.None;
    }

    public bool PointsOk(Position p1, Position p2)
    {
        var cp1 = Check(p1);
        var cp2 = Check(p2);
        if (cp1 == Satisfaction.Both || cp2 == Satisfaction.Both || cp1 == cp2)
            return true;

        if (cp1 == Satisfaction.None)
        {
            foreach (var adj in Adjacent)
            {
                var cp1Adj = adj.Check(p1);
                var cp2Adj = adj.Check(p2);
                if (cp1Adj != Satisfaction.None && (cp2Adj == Satisfaction.Both || cp1Adj == cp2Adj))
                    return true;
            }
        }
        if (cp2 == Satisfaction.None)
        {
            foreach (var adj in Adjacent)
            {
                var cp1Adj = adj.Check(p1);
                var cp2Adj = adj.Check(p2);
                if (cp2Adj != Satisfaction.None && (cp1Adj == Satisfaction.Both || cp1Adj == cp2Adj))
                    return true;
            }
        }
        return false;
    }
}


public class Solution(string inputFileName, bool debug = false, bool plot = false) : SolutionBase<ulong>(inputFileName)
{
    public bool Debug { get; set; } = debug;
    List<Position> tiles = [];

    public override ulong SolvePart1()
    {
        var tl = tiles.First();
        var tr = tiles.First();
        var bl = tiles.First();
        var br = tiles.First();

        // Update corners
        foreach (var tile in tiles)
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
        for (int i = 0; i < tiles.Count - 1; i++)
        {
            for (int j = i + 1; j < tiles.Count; j++)
            {
                var area = tiles[i].Area(tiles[j]);
                if (area > maxArea)
                    maxArea = area;

            }
        }
        return maxArea;
    }



    public override ulong SolvePart2()
    {
        // Solution approach
        // 1. Find and define all point constraints
        // 2. Foreach point check all satisfied constraints and satisfaction type
        // 3. Check all other points and compute area only with the points that satisfy the same constraints with an
        //    identical or better satisfaction type (if point in 2 has full satisfaction then either 
        //    satisfaction suffices for this point).
        var prevLine = Vector2.Create(tiles.First().X - tiles.Last().X, tiles.First().Y - tiles.Last().Y);
        List<Constraint> constraints = [];
        tiles.Add(tiles.First());
        int lastConstraintNum = -1;
        for (int i = 0; i < tiles.Count - 1; i++)
        {
            var nextLine = Vector2.Create(tiles[i + 1].X - tiles[i].X, tiles[i + 1].Y - tiles[i].Y);
            if (prevLine.Cross(nextLine) < 0)
            {
                var horzSign = float.Sign(prevLine.X) - float.Sign(nextLine.X);
                var vertSign = float.Sign(prevLine.Y) - float.Sign(nextLine.Y);
                // if (Debug)
                // {
                //     Console.WriteLine($"{tiles[i]} {prevLine} {nextLine} danger {prevLine.Cross(nextLine)}");
                //     Console.WriteLine($"Rule {tiles[i].X} {(horzSign > 0 ? "<" : ">")}=x");
                //     Console.WriteLine($"Rule {tiles[i].Y} {(vertSign > 0 ? "<" : ">")}=Y");
                //     Console.WriteLine($"");
                // }
                var newConst = new Constraint(tiles[i], horzSign > 0, vertSign > 0);
                // When two constraints follow directly assign them as adjacent
                if (lastConstraintNum + 1 == i && i > 0)
                {
                    constraints.Last().Adjacent.Add(newConst);
                    newConst.Adjacent.Add(constraints.Last());
                }
                constraints.Add(newConst);
                lastConstraintNum = i;
            }
            prevLine = nextLine;
        }
        ulong maxArea = 0;

        var scatterPlot = new Plot(); ;
        var point1 = new Position(0, 0);
        var point2 = new Position(0, 0);

        if (plot)
        {
            scatterPlot = new Plot();
            var sp1 = scatterPlot.Add.Scatter(tiles.Select(t => new Coordinates(t.X, -t.Y)).ToList());
            sp1.Color = Colors.Blue;
            sp1.LineWidth = 2;
            var sp2 = scatterPlot.Add.Scatter(tiles.Select(t => new Coordinates(t.X, -t.Y)).First());
            sp2.Color = Colors.Purple;
            sp2.MarkerSize = 10;

            foreach (var c in constraints)
            {
                var con = scatterPlot.Add.Scatter(new Coordinates(c.Position.X, -c.Position.Y));
                con.Color = Colors.Red;
                con.MarkerSize = 5;
            }
        }

        for (int i = 0; i < tiles.Count - 1; i++)
        {
            if (Debug)
                Console.WriteLine($"P1 {tiles[i]}");
            for (int j = i + 1; j < tiles.Count; j++)
            {
                var allConstraintsMatch = true;
                if (Debug)
                    Console.WriteLine($"  P2 {tiles[j]}");
                foreach (var c in constraints)
                {
                    if (!c.PointsOk(tiles[i], tiles[j]))
                    {
                        if (Debug)
                        {
                            Console.WriteLine($"    C {c.Position} P1C {c.Check(tiles[i])} P2C {c.Check(tiles[j])} Ok={c.PointsOk(tiles[i], tiles[j])} vertDir {c.VerticalDir} horzDir {c.HorizontalDir}");
                            if (c.Adjacent.Count > 0)
                                Console.WriteLine($"    Adj:{c.Adjacent.First().Position} P1C {c.Adjacent.First().Check(tiles[i])} P2C {c.Adjacent.First().Check(tiles[j])} Ok={c.Adjacent.First().PointsOk(tiles[i], tiles[j])} vertDir {c.Adjacent.First().VerticalDir} horzDir {c.Adjacent.First().HorizontalDir}");
                        }
                        allConstraintsMatch = false;
                        break;
                    }
                }
                if (allConstraintsMatch)
                {
                    var area = tiles[i].Area(tiles[j]);
                    if (Debug)
                        Console.WriteLine($"Matching constraints {tiles[i]} {tiles[j]} A={area}");
                    if (area >= maxArea)
                    {
                        maxArea = area;
                        if (plot)
                        {
                            point1 = tiles[i];
                            point2 = tiles[j];
                            var opt = scatterPlot.Add.Scatter(new Coordinates[] { new(point1.X, -point1.Y), new(point2.X, -point2.Y) });
                            opt.Color = Colors.RandomHue(1).First();
                            opt.MarkerSize = 10;
                        }
                    }
                }

            }
        }
        if (plot)
            scatterPlot.SavePng($"Data/part2_plot.png", 1500, 1500);

        return maxArea;
    }


    public void Plot(string plotFileName)
    {
        var plot = new Plot();
        var sp1 = plot.Add.Scatter(tiles.Select(t => new Coordinates(t.X, -t.Y)).ToList());
        sp1.Color = Colors.Blue;
        sp1.LineWidth = 2;
        var sp2 = plot.Add.Scatter(tiles.Select(t => new Coordinates(t.X, -t.Y)).First());
        sp2.Color = Colors.Red;
        sp2.MarkerSize = 10;


        var opt = plot.Add.Scatter(new Coordinates[] { new(90857, -74283), new(10032, -76100) });
        opt.Color = Colors.Green;
        opt.MarkerSize = 10;

        plot.SavePng($"{plotFileName}.png", 1500, 1500);

    }

    internal override void ParseInput()
    {
        tiles = [.. new Position[Input.Length]];
        for (int i = 0; i < Input.Length; i++)
        {
            var values = Input[i].Split(",").Select(int.Parse);
            tiles[i] = new Position(values.First(), values.Last());
        }
    }

}


public static class Vector2Extension
{
    public static float Cross(this Vector2 vec1, Vector2 vec2)
    {
        return (vec1.X * vec2.Y) - (vec1.Y * vec2.X);
    }
}