namespace lr2_search;

public class Program
{
    public static void Main(string[] args)
    {
        MemoryController.LimitMemoryUsage(1024);
        
        int[,] initialBoard =
        {
            { 2, 4, 3 },
            { 1, 8, 7 },
            { 0, 6, 5 }
        };
        
        // { 1, 2, 3 },
        // { 4, 5, 6 },
        // { 7, 8, 0 }

        var initialState = new PuzzleState(initialBoard);
        initialState.Shuffle();
        
        Console.WriteLine("Initial:");
        initialState.PrintBoard();

        // IDS
        var idsSolver = new IDS();
        var idsSolution = idsSolver.Solve(initialState);

        // A*
        var aStarSolver = new AStar();
        var aStarSolution = aStarSolver.Solve(initialState);
    }
}