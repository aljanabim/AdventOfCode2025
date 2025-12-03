using System;
using Puzzles.Day2;

namespace Puzzles.Test;

public class Day2Tests
{
    [Fact]
    public void TestSolutionPart1()
    {
        // Arrange 
        var solver = new Solution("Data/day2.txt");

        // Act
        var actual = solver.SolvePart1();

        // Assert
        Assert.Equal(1227775554, actual);
    }

    [Fact]
    public void TestSolutionPart2()
    {
        // Arrange 
        var solver = new Solution("Data/day2.txt");

        // Act
        var actual = solver.SolvePart2();

        // Assert
        Assert.Equal(4174379265, actual);
    }
}
