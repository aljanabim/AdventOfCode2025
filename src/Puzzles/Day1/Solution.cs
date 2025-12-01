namespace Puzzles.Day1;

public class Solution
{
    public int SolvePart1(StreamReader stream)
    {
        int result = 0;
        var dial = new Dial(50);
        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine();
            if (line != null)
            {
                var (direction, steps) = parseLine(line);
                dial.Turn(direction, steps);
                if (dial.Value == 0)
                    result++;
            }
        }
        return result;
    }

    public int SolvePart2Naive(StreamReader stream)
    {
        int result = 0;
        var dial = new Dial(50);
        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine();
            if (line != null)
            {
                var (direction, steps) = parseLine(line);
                for (int i = 0; i < steps; i++)
                {
                    dial.Turn(direction, 1);
                    if (dial.Value == 0)
                    {
                        result++;
                    }
                }
            }
        }
        return result;
    }

    public int SolvePart2(StreamReader stream)
    {
        int result = 0;
        var dial = new Dial(50);
        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine();
            if (line != null)
            {
                var (direction, steps) = parseLine(line);
                int loops = steps / 100;
                result += loops;
                int stepsSimple = steps % 100;
                if (direction == "R" && 100 - dial.Value < stepsSimple)
                    result++;
                else if (direction == "L" && dial.Value < stepsSimple && dial.Value != 0)
                    result++;

                dial.Turn(direction, stepsSimple);
                if (dial.Value == 0)
                    result++;
            }
        }
        return result;
    }


    private static (string direction, int steps) parseLine(string line)
    {
        string direction = line[0..1];
        if (int.TryParse(line[1..], out int steps))
        {
            return (direction, steps);
        }
        return (direction, 0);
    }
}
