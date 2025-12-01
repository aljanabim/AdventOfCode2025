namespace Puzzles.Day1;

public class Dial(int startValue)
{
    public int Value { get; private set; } = startValue;

    public void Turn(string instruction)
    {
        string direction = instruction[0..1];
        if (int.TryParse(instruction[1..], out int steps))
        {
            Turn(direction, steps);
        }
    }

    public void Turn(string direction, int steps)
    {
        switch (direction)
        {
            case "R":
                Value = (Value + steps) % 100;
                break;
            default:
                Value = (Value + 100 - steps) % 100;
                break;
        }
        ;
    }

}