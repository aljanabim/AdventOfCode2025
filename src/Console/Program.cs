
using Utils;

var day1Solver = new Puzzles.Day1.Solution();
var day11Solution = Timing.Measure(() => day1Solver.SolvePart1(new StreamReader("Data/day1_input.txt")));
var day12Solution = Timing.Measure(() => day1Solver.SolvePart2(new StreamReader("Data/day1_input.txt")));
var day12SolutionNaive = Timing.Measure(() => day1Solver.SolvePart2Naive(new StreamReader("Data/day1_input.txt")));
Console.WriteLine($"Day 1 Part 1:      \t {day11Solution.result} ({day11Solution.elapsed.TotalMilliseconds}ms)");
Console.WriteLine($"Day 1 Part 2 Naive:\t {day12SolutionNaive.result} ({day12SolutionNaive.elapsed.TotalMilliseconds}ms)");
Console.WriteLine($"Day 1 Part 2:      \t {day12Solution.result} ({day12Solution.elapsed.TotalMilliseconds}ms)");

var day2Solver = new Puzzles.Day2.Solution("Data/day2_input.txt");
var day21SolutionBatched = Timing.Measure(day2Solver.SolvePart1Batched);
var day21SolutionNormal = Timing.Measure(day2Solver.SolvePart1Normal);
var day21SolutionCached = Timing.Measure(day2Solver.SolvePart1Cached);
Console.WriteLine($"Day 2 Part 1 (batched):\t{day21SolutionBatched.result} ({day21SolutionBatched.elapsed.TotalMilliseconds}ms)");
Console.WriteLine($"Day 2 Part 1 (normal):\t{day21SolutionNormal.result} ({day21SolutionNormal.elapsed.TotalMilliseconds}ms)");
Console.WriteLine($"Day 2 Part 1 (cached):\t{day21SolutionCached.result} ({day21SolutionCached.elapsed.TotalMilliseconds}ms)");
