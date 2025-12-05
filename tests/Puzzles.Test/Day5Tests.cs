namespace Puzzles.Test;

using Day5;

public class Day5Tests
{
    [Fact]
    public void TestPart1()
    {
        // Arrange
        var solver = new Solution("Data/day5.txt");
        // Act 
        var actual = solver.SolvePart1();
        // Assert
        Assert.Equal(3, actual);
    }

    [Theory]
    [InlineData("Data/day5.txt", 14)]
    [InlineData("Data/day5_2.txt", 8)]
    [InlineData("Data/day5_3.txt", 13)]
    public void TestPart2(string file, long expected)
    {
        // Arrange
        var solver = new Solution(file);
        // Act 
        var actual = solver.SolvePart2();
        // Assert
        Assert.Equal(expected, actual);
    }
}