using Puzzles.Day11;

namespace Puzzles.Test;


public class Day11Tests
{
    [Fact]
    public void TestPart1()
    {
        var solver = new Solution("Data/day11.txt");

        var actual = solver.SolvePart1();

        Assert.Equal(11, actual);
    }

    [Fact]
    public void TestPart2()
    {
        var solver = new Solution("Data/day11.txt");

        var actual = solver.SolvePart2();

        Assert.Equal(0, actual);
    }
}

