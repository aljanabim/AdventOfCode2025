namespace Puzzles.Test;

using Day6;

public class Day6Tests
{
    [Fact]
    public void TestPart1()
    {
        // Arrange
        var solver = new Solution("Data/day6.txt");
        // Act 
        var actual = solver.SolvePart1();
        // Assert
        Assert.Equal(4277556u, actual);
    }

    [Fact]
    public void TestPart2()
    {
        // Arrange
        var solver = new Solution("Data/day6.txt");
        // Act 
        var actual = solver.SolvePart2();
        // Assert
        Assert.Equal(3263827u, actual);
    }
}