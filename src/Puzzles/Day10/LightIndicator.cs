namespace Puzzles.Day10;

public class LightIndicator
{
    private readonly int InitialState;
    public int State { get; private set; }
    private readonly int _numberOfLights = 0;

    public LightIndicator(bool[] lightArray)
    {
        _numberOfLights = lightArray.Length;

        for (int i = 0; i < lightArray.Length; i++)
        {
            if (lightArray[i])
                State |= 1 << i;
        }
        InitialState = State;
    }

    public LightIndicator(int initialState)
    {
        State = initialState;
        InitialState = State;
    }
    public override string ToString() => State.ToBinString(_numberOfLights);

    public void ToggleLight(int lightIndex)
    {
        State ^= 1 << lightIndex;
    }

    public void ToggleLights(int mask)
    {
        State ^= mask;
    }

    public void Reset()
    {
        State = InitialState;
    }
}

