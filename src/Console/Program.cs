
using Utils;

var day1Solver = new Puzzles.Day1.Solution();
var day11Solution = Timing.Measure(() => day1Solver.SolvePart1(new StreamReader("Data/day1_input.txt")));
var day12Solution = Timing.Measure(() => day1Solver.SolvePart2(new StreamReader("Data/day1_input.txt")));
var day12SolutionNaive = Timing.Measure(() => day1Solver.SolvePart2Naive(new StreamReader("Data/day1_input.txt")));
Console.WriteLine($"Day 1 Part 1:      \t {day11Solution.result} ({day11Solution.elapsed.TotalMilliseconds}ms)");
Console.WriteLine($"Day 1 Part 2 Naive:\t {day12SolutionNaive.result} ({day12SolutionNaive.elapsed.TotalMilliseconds}ms)");
Console.WriteLine($"Day 1 Part 2:      \t {day12Solution.result} ({day12Solution.elapsed.TotalMilliseconds}ms)");