using Google.OrTools.LinearSolver;

namespace Puzzles.Day10.Algebra;


// Integer Least Squares Solves
public static class ILSSolver
{
    private static readonly Solver solver = Solver.CreateSolver("SCIP");
    /*
        Given y \in \mathbb{R}^M
            x \in \mathbb{R}^N
            A \in \mathbb{R}^{M\times N}
        minimize sum_{n=0}^N x_n
        subject to  
        y_m = sum_{n=0}^N A_{m,n} x_n
        where m=0,1,...M
    */
    public static double Solve(Matrix A, Vector Y)
    {
        // Setup variables
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

        return solver.Objective().Value();
    }
}

