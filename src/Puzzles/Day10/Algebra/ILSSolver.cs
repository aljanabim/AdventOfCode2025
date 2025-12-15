using System.Diagnostics;
using System.Xml.Linq;
using Google.OrTools.LinearSolver;
using Puzzles.Day8;

namespace Puzzles.Day10.Algebra;


// Integer Least Squares Solves
public static class ILSSolver
{
    public static int iters = 0;
    private static Random rng = new Random();
    private static int MaxIters = 100;
    private static Solver solver = Solver.CreateSolver("SCIP");

    public static void SetMaxIters(int n)
    {
        MaxIters = n;
    }

    public static double ComputeCost(Matrix A, Vector Input, Vector Target)
    {
        var diff = Target - A.Multiply(Input);
        // Console.WriteLine($"Compute diff {Target}-{A.Multiply(Input)}={diff} {diff.Norm()}");
        return diff.Norm();
    }

    private static IEnumerable<int[]> GenerateDigitGroup(int size)
    {
        var digitGroupQuque = new Queue<int[]>();
        for (int d = 0; d < size; d++)
        {
            digitGroupQuque.Enqueue([d]);
        }
        while (digitGroupQuque.Count > 0)
        {
            var group = digitGroupQuque.Dequeue();

            yield return group;

            for (int d = group.Last() + 1; d < size; d++)
            {
                digitGroupQuque.Enqueue([.. group, d]);
            }
            // Console.WriteLine($"Queue {string.Join(',', group)}");
        }
    }


    private static void IterateSolution(List<Vector> solutions, Vector sol, ref Vector Target, ref Matrix A, double maxDigitValue, int digit, ref double minSum, bool findAll = true)
    {
        iters++;
        var currentCost = ComputeCost(A, sol, Target);
        // Console.WriteLine($"ITER digit={digit} sol={sol} minSum={minSum} currentCost={currentCost}");

        var newCost = ComputeCost(A, sol, Target);
        if (newCost == 0)
        {
            var newSum = sol.Sum();
            if (newSum < minSum)
            {
                minSum = newSum;
                solutions.Add(new Vector([.. sol.Data]));
                Console.WriteLine($"Added optimal and updated cost {sol} {sol.Sum()}");
            }
            return;
        }

        // stopping condition
        if (digit >= sol.Size)
            return;

        for (int newVal = 0; newVal <= maxDigitValue; newVal++)
        {
            if (sol.Data[digit] == newVal)
                continue;
            // var oldCost = ComputeCost(A, sol, Target);
            // var oldValue = sol.Data[digit];
            sol.Data[digit] = newVal;
            // var updtCost = ComputeCost(A, sol, Target);
            // Console.WriteLine($"NewVal {newVal} digit {digit} oldcost {oldCost} {newCost} oldval {oldValue} new {newVal}");
            // if (updtCost > oldCost)
            // {
            // sol.Data[digit] = oldValue;
            // return;
            // }
            IterateSolution(solutions, sol, ref Target, ref A, maxDigitValue, digit + 1, ref minSum, findAll);
            // IterateSolution(solutions, new Vector([.. sol.Data]), ref Target, ref A, maxDigitValue, digit + 1, ref minSum, findAll);

        }

        if (false)
        {
            for (int i = 0; i <= maxDigitValue; i++)
            {
                currentCost = ComputeCost(A, sol, Target);

                // Stop searching for an optimal solution with more button presses than an existing solution
                if (currentCost == 0 && sol.Sum() > minSum)
                    break;

                // Return a single optimal solution
                // if (currentCost == 0 && minSum == sol.Sum() && findAll == false)
                // break;


                var oldValue = sol.Data[digit];
                sol.Data[digit] = i;
                // var newCost = ComputeCost(A, sol, Target);
                // Update minimum sum
                var newSum = sol.Sum();
                if (newCost == 0 && newSum < minSum)
                {
                    minSum = newSum;

                    // if (newCost == 0 && newSum == minSum)
                    // {
                    Console.WriteLine($"Digit {digit} Iter {i} Sol {sol} {sol.Data.Sum()} (vs {minSum}) cost {currentCost}->{newCost}");
                    // return new Vector(sol.Data);
                    // solutions.Add(new Vector(sol.Data));
                    // foreach (var s in solutions)
                    // {
                    // Console.WriteLine($"new sols [s]{s}");
                    // }
                }


                if (findAll)
                {
                    // solution = IterateSolution(solutions, sol, ref Target, ref A, maxDigitValue, digit + 1, ref minSum, findAll);

                }
                else
                {
                    if (newCost < currentCost)
                        IterateSolution(solutions, sol, ref Target, ref A, maxDigitValue, digit + 1, ref minSum, findAll);
                    else
                        sol.Data[digit] = oldValue;
                }

            }
        }
        // return solution;
    }

    public static double Solve(Matrix A, Vector Y)
    {
        // var sol = new Vector(A.Cols);

        // var currentCost = ComputeCost(A, sol, Y);
        // var newCost = currentCost;
        // var maxInc = Y.Data.Max();

        // var target = new Vector([3, 0, 7]);
        // var mat = new Matrix { Data = new double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } } };
        // var minSum = maxInc * sol.Size;
        // List<Vector> solutions = [sol];
        // var tempVec = new Vector([0, 0, 0]);
        // IterateSolution(solutions, tempVec, ref target, ref mat, 7, 0, ref minSum);
        // IterateSolution(solutions, sol, ref Y, ref A, maxInc, 0, ref minSum);

        /*
            Given y \in \mathbb{R}^M
                x \in \mathbb{R}^N
                A \in \mathbb{R}^{M\times N}
            minimize sum_{n=0}^N x_n
            subject to  
            y_m = sum_{n=0}^N A_{m,n} x_n
            where m=0,1,...M
        */
        // var timer = Stopwatch.StartNew();
        List<Variable> xVars = [.. Enumerable.Range(0, A.Cols).Select(i => solver.MakeIntVar(0, double.PositiveInfinity, $"x{i}"))];
        // Add constraints
        for (int row = 0; row < A.Rows; row++)
        {
            LinearExpr? constraints = null;
            for (int col = 0; col < A.Cols; col++)
            {
                if (A.Data[row, col] == 1)
                {
                    if (constraints == null)
                        constraints = xVars[col];
                    else
                        constraints += xVars[col];
                }
            }
            if (constraints != null)
                solver.Add(constraints == Y.Data[row]);
        }
        LinearExpr? cost = null;
        foreach (var x in xVars)
        {
            if (cost == null)
                cost = x;
            else
                cost += x;
        }
        if (cost != null)
            solver.Minimize(cost);

        var resultStatus = solver.Solve();
        if (resultStatus != Solver.ResultStatus.OPTIMAL)
            throw new Exception($"The problem does not have an optimal solution \n{A}\n{Y}");

        // Console.WriteLine($"Min number {solver.Objective().Value()}");
        // Console.WriteLine($"Sol={new Vector([.. xVars.Select(x => x.SolutionValue())])}");
        // timer.Stop();
        // Console.WriteLine($"Problem solving took {timer.Elapsed.TotalMilliseconds}ms");

        // alglib.lsfitlinear(Y.Data, A.Data, out var info, out var sold, out rep);
        // Console.WriteLine($"{new Vector(sold)}, {info}, {rep}");

        return solver.Objective().Value();
        // return solutions.First();//.OrderBy(s => s.Sum()).First();

        // GenerateDigitGroup(sol.Size).ToList();

        // for (int iter = 0; iter < 3; iter++)
        // {
        //     Console.WriteLine($"Iteration {iter} cost {currentCost} {sol} #{sol.Data.Sum()}");
        //     foreach (var digitGroup in GenerateDigitGroup(sol.Size))
        //     {
        //         Console.WriteLine($"Working with digit group {string.Join(',', digitGroup)}");
        //         // Temp save old values
        //         var oldValues = new double[sol.Size];
        //         foreach (var digit in digitGroup)
        //             oldValues[digit] = sol.Data[digit];


        //         for (int inc = 1; inc <= maxInc; inc++)
        //         {
        //             // Try decrease
        //             foreach (var digit in digitGroup)
        //                 sol.Data[digit] = oldValues[digit] - Math.Min(sol.Data[digit], inc);
        //             newCost = ComputeCost(A, sol, Y);
        //             if (newCost < currentCost)
        //                 break;
        //             // Try increase
        //             foreach (var digit in digitGroup)
        //                 sol.Data[digit] = oldValues[digit] + inc;
        //             newCost = ComputeCost(A, sol, Y);
        //             if (newCost < currentCost)
        //                 break;
        //         }
        //         Console.WriteLine($"  New digit sol={sol} currCost {currentCost} newCost {newCost}");
        //         if (newCost < currentCost)
        //         {
        //             currentCost = newCost;
        //             Console.WriteLine($"  Updating sol {sol}");

        //         }
        //         else
        //             foreach (var digit in digitGroup)
        //                 sol.Data[digit] = oldValues[digit];


        //         if (newCost == 0)
        //             return sol;
        //     }
        // }
        // return sol;

        // for (int i = 0; i < MaxIters; i++)
        // {
        // Console.WriteLine($"Iteration {i} cost {currentCost} {sol} #{sol.Data.Sum()}");
        // for (int d = 0; d < size; d++)
        // {
        //     var oldValue = sol.Data[d];
        //     Console.WriteLine($"  Digit sol[{d}]={oldValue}");
        //     for (int inc = 1; inc <= maxInc; inc++)
        //     {
        //         Console.WriteLine($"     Attempting inc={inc}");
        //         sol.Data[d] = oldValue - Math.Min(oldValue, inc);
        //         newCost = ComputeCost(A, sol, Y);
        //         if (newCost < currentCost)
        //             break;
        //         sol.Data[d] = oldValue + inc;
        //         newCost = ComputeCost(A, sol, Y);
        //         if (newCost < currentCost)
        //             break;
        //     }
        //     if (newCost < currentCost)
        //     {
        //         currentCost = newCost;
        //     }
        //     else
        //     {
        //         sol.Data[d] = oldValue;
        //     }
        //     Console.WriteLine($"  New digit sol[{d}]={sol.Data[d]} currCost {currentCost} newCost {newCost}");
        // }

        // // Converged
        // if (newCost == 0)
        // {
        //     Console.WriteLine($"Converges to solution {sol} at iteration {i}");
        //     break;
        // }
        // if (currentCost == ComputeCost(A, sol, Y))
        // {
        //     Console.WriteLine($"randomzing {sol}");
        //     sol.Data[rng.Next(sol.Size - 1)] = 0;
        //     Console.WriteLine($"randomzing new {sol}");
        // }
        // }
        // return sol;
    }
}

