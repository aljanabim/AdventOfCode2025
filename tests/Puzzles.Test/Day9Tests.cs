using Puzzles.Day9;

namespace Puzzles.Test;


public class Day9Tests
{
    [Fact]
    public void TestPart1()
    {
        // Given
        var solver = new Solution("Data/day9.txt");

        // When
        var actual = solver.SolvePart1();

        // Then
        Assert.Equal((ulong)50, actual);
    }
    [Fact]
    public void TestPart2()
    {
        // Given
        var solver = new Solution("Data/day9.txt");
        // When
        var actual = solver.SolvePart2();
        // Then
        Assert.Equal((ulong)0, actual);
    }

}

