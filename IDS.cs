namespace lr2_search;

public class IDS
{
    private int iterations;
    private int generated;
    
    public PuzzleState Solve(PuzzleState initialState)
    {
        iterations = 0;
        int depthLimit = 0;
        while (true)
        {
            var result = DFSLimited(initialState, depthLimit);
            if (result != null)
            {
                Console.WriteLine("\nIDS solved!");
                Console.WriteLine($"{iterations} iterations");
                Console.WriteLine($"{generated} states generated");
                Console.WriteLine($"{depthLimit} states stored in memory");
                result.PrintSteps();
                return result;
            }
            depthLimit++;
        }
    }

    private PuzzleState DFSLimited(PuzzleState state, int limit)
    {
        iterations++;
        
        if (state.IsGoal())
            return state;

        if (state.Depth == limit)
            return null;

        foreach (var successor in state.GetSuccessors())
        {
            generated++;
            var result = DFSLimited(successor, limit);
            if (result != null)
                return result;
        }

        return null;
    }
}