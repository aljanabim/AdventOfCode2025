using Puzzles.Day7;

namespace Puzzles.Test;


public class Day7Tests
{
    [Fact]
    public void TestPart1()
    {
        // Given
        var solver = new Solution("Data/day7.txt");

        // When
        var actual = solver.SolvePart1();

        // Then
        Assert.Equal(21, actual);
    }
    [Fact]
    public void TestPart2()
    {
        // Given
        var solver = new Solution("Data/day7.txt");
        // When
        var actual = solver.SolvePart2();
        // Then
        Assert.Equal(40, actual);
    }

}

