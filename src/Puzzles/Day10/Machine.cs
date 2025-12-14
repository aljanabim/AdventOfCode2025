using Puzzles.Day10.Algebra;

namespace Puzzles.Day10;

public class Machine
{
    public List<int> ButtonMasks = [];
    public LightIndicator Indicator { get; private init; }

    public Vector Joltages { get; private init; }
    public Matrix ButtonMatrix { get; private init; }

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
        Joltages = new Vector([.. input[(joltsStart + 1)..^1].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)]);


        // Parse buttons
        var buttons = input[(diagramEnd + 1)..joltsStart].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        ButtonMatrix = new Matrix
        {
            Data = new double[Joltages.Size, buttons.Length]
        };
        int btnIdx = 0;
        foreach (var btnGroup in buttons)
        {
            var btnNums = btnGroup.Trim()[1..^1].Split(',').Select(int.Parse);
            int btnMask = 0;
            foreach (var btnNum in btnNums)
            {
                ButtonMatrix.Data[btnNum, btnIdx] = 1;
                btnMask |= 1 << btnNum;
            }
            ButtonMasks.Add(btnMask);
            btnIdx++;
        }


        // var nLights = lightBools.Length;
        // Console.WriteLine($"Button masks");
        // foreach (var item in ButtonMasks)
        // {
        //     Console.WriteLine($"{item.ToBinString(nLights)}");
        // }
    }
}

