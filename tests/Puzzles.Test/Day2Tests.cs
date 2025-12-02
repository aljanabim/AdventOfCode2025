using System;
using Puzzles.Day2;

namespace Puzzles.Test;

public class Day2Tests
{
    [Fact]
    public void TestSolution()
    {
        // Arrange 
        var solver = new Solution("Data/day2.txt");

        // Act
        var actual = solver.SolvePart1Batched();

        // Assert
        Assert.Equal(1227775554, actual);
    }
}
