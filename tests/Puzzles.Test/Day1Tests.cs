using Puzzles.Day1;

namespace Puzzles.Test;

public class Day1Tests
{
    [Theory]
    [InlineData(11, "R8", 19)]
    [InlineData(19, "L19", 0)]
    [InlineData(0, "L1", 99)]
    [InlineData(99, "R1", 0)]
    [InlineData(5, "L10", 95)]
    [InlineData(95, "R5", 0)]
    public void TestDial(int startValue, string instruction, int expected)
    {
        // Arrange
        var dial = new Dial(startValue);

        // Act
        dial.Turn(instruction);

        // Assert
        Assert.Equal(expected, dial.Value);
    }

    [Fact]
    public void TestSolution()
    {
        // Arrange 
        var stream = new StreamReader("Data/day1.txt");

        // Act
        var solver = new Solution();
        var actual = solver.SolvePart1(stream);

        Assert.Equal(3, actual);
    }
}
