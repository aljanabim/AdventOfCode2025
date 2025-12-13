namespace Puzzles.Day10;

public class Machine
{
    public int nLights;

    public List<int> ButtonMasks = [];
    public int[] Joltages { get; }
    public LightIndicator Indicator { get; private init; }

    public Machine(string input)
    {
        // input format example
        // [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
        int diagramEnd = input.IndexOf(']');
        int joltsStart = input.IndexOf('{');

        // Parse lights
        var lightBools = input[1..diagramEnd].Select(s => s == '#').ToArray();
        Indicator = new LightIndicator(lightBools);
        // Prase joltages
        Joltages = [.. input[(joltsStart + 1)..^1].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)];

        // Parse buttons
        foreach (var btnGroup in input[(diagramEnd + 1)..joltsStart].Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            var btnNums = btnGroup.Trim()[1..^1].Split(',').Select(int.Parse);
            int btnMask = 0;
            foreach (var btnNum in btnNums)
            {
                btnMask |= 1 << btnNum;
            }
            ButtonMasks.Add(btnMask);
        }

        nLights = lightBools.Length;
        Console.WriteLine($"Button masks");
        foreach (var item in ButtonMasks)
        {
            Console.WriteLine($"{item.ToBinString(nLights)}");
        }
    }
}

