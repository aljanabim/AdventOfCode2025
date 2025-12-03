namespace Puzzles.Test;

using Day3;

public class Day3Tests
{
    [Fact]
    public void TestPart1()
    {
        // Arrange
        var solver = new Solution("Data/day3.txt");
        // Act 
        var actual = solver.SolvePart1();
        // Assert
        Assert.Equal(357, actual);
    }
}