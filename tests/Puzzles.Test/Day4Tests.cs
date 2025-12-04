namespace Puzzles.Test;

using Day4;

public class Day4Tests
{
    [Fact]
    public void TestPart1()
    {
        // Arrange
        var solver = new Solution("Data/day4.txt");
        // Act 
        var actual = solver.SolvePart1();
        // Assert
        Assert.Equal(13, actual);
    }

    [Fact]
    public void TestPart2()
    {
        // Arrange
        var solver = new Solution("Data/day4.txt");
        // Act 
        var actual = solver.SolvePart2();
        // Assert
        Assert.Equal(43, actual);
    }
}