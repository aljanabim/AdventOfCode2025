using Puzzles.Day10;

namespace Puzzles.Test;


public class Day10Tests
{
    [Theory]
    [InlineData(0b0110, 0, 0b0111)]
    [InlineData(0b0110, 2, 0b0010)]
    public void TestToggleLight(int initState, int toggleIndex, int expectedState)
    {
        var l = new LightIndicator(initState);
        l.ToggleLight(toggleIndex);
        Assert.Equal(expectedState, l.State);
    }

    [Theory]
    [InlineData(0b0110, 0b1001, 0b1111)]
    [InlineData(0b01101, 0b0101, 0b01000)]
    [InlineData(0b01111, 0b100101, 0b101010)]
    public void TestToggleLights(int initState, int toggleMask, int expectedState)
    {
        var l = new LightIndicator(initState);
        l.ToggleLights(toggleMask);
        Assert.Equal(expectedState, l.State);
    }

    [Theory]
    [InlineData(new bool[] { true, true, true, true }, 0b1111)]
    [InlineData(new bool[] { false, true, false, false, false }, 0b01000)]
    [InlineData(new bool[] { true, false, true, false, true, false }, 0b101010)]
    public void TestInitializeLightIndicatorByArray(bool[] initVal, int expectedState)
    {
        var l = new LightIndicator(initVal);
        Assert.Equal(expectedState, l.State);
    }

    [Fact]
    public void TestPart1()
    {
        // Given
        var solver = new Solution("Data/day10.txt");

        // When
        var actual = solver.SolvePart1();

        // Then
        Assert.Equal(7, actual);
    }
    [Fact]
    public void TestPart2()
    {
        // Given
        var solver = new Solution("Data/day10.txt");
        // When
        var actual = solver.SolvePart2();
        // Then
        Assert.Equal(33, actual);
    }

}

