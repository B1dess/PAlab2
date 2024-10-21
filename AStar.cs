namespace lr2_search;

public class AStar
{
    private int iterations;
    private int generated;
    public PuzzleState Solve(PuzzleState initialState)
    {
        iterations = 0;
        var openSet = new SortedSet<(int, PuzzleState)>(Comparer<(int, PuzzleState)>.Create((a, b) =>
        {
            int cmp = a.Item1.CompareTo(b.Item1);
            return cmp == 0 ? a.Item2.GetHashCode().CompareTo(b.Item2.GetHashCode()) : cmp;
        }));

        openSet.Add((ManhattanDistance(initialState), initialState));

        var closedSet = new HashSet<PuzzleState>();

        while (openSet.Count > 0)
        {
            iterations++;
            var current = openSet.Min.Item2;
            openSet.Remove(openSet.Min);

            if (current.IsGoal())
            {
                Console.WriteLine("A* solved!");
                Console.WriteLine($"{iterations} iterations");
                Console.WriteLine($"{generated} states generated");
                Console.WriteLine($"{openSet.Count+closedSet.Count} states stored in memory");
                current.PrintSteps();
                return current;
            }

            closedSet.Add(current);

            foreach (var neighbor in current.GetSuccessors())
            {
                if (closedSet.Contains(neighbor))
                    continue;

                generated++;
                int cost = neighbor.Depth + ManhattanDistance(neighbor);
                openSet.Add((cost, neighbor));
            }
        }

        return null;
    }

    private int ManhattanDistance(PuzzleState state)
    {
        int distance = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (state.Board[i, j] != 0)
                {
                    int value = state.Board[i, j];
                    int targetRow = (value - 1) / 3;
                    int targetCol = (value - 1) % 3;
                    distance += Math.Abs(i - targetRow) + Math.Abs(j - targetCol);
                }
            }
        }
        return distance;
    }
}