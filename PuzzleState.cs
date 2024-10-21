namespace lr2_search;
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleState
{
    public int[,] Board { get; private set; }
    public int EmptyRow { get; private set; }
    public int EmptyCol { get; private set; }
    public PuzzleState Parent { get; private set; }
    public int Depth { get; private set; }

    private static readonly int[,] goal = new int[,]
    {
        { 1, 2, 3 },
        { 4, 5, 6 },
        { 7, 8, 0 }
    };

    public PuzzleState(int[,] board, PuzzleState parent = null, int depth = 0)
    {
        Board = (int[,])board.Clone();
        Parent = parent;
        Depth = depth;
        FindEmpty();
    }

    private void FindEmpty()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (Board[i, j] == 0)
                {
                    EmptyRow = i;
                    EmptyCol = j;
                    return;
                }
            }
        }
    }
    
    public void Shuffle()
    {
        Random rand = new Random();
        int[] arr = Board.Cast<int>().OrderBy(x => rand.Next()).ToArray();
        while (!IsSolvable(arr))
        {
            arr = arr.OrderBy(x => rand.Next()).ToArray();
        } 
        
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Board[i, j] = arr[i * 3 + j];
                if (Board[i, j] == 0)
                {
                    EmptyRow = i;
                    EmptyCol = j;
                }
            }
        }
    }
    
    public static bool IsSolvable(int[] board)
    {
        var arr = board.Where(x => x != 0).ToArray();
        int inversions = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = i + 1; j < 8; j++)
            {
                if (arr[i] > arr[j]) inversions++;
            }
        }
        return inversions % 2 == 0;
    }

    public bool IsGoal()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (Board[i, j] != goal[i, j])
                    return false;
            }
        }
        return true;
    }

    public IEnumerable<PuzzleState> GetSuccessors()
    {
        var directions = new []
        {
            (-1, 0), // Up
            (1, 0),  // Down
            (0, -1), // Left
            (0, 1)   // Right
        };

        foreach (var (dx, dy) in directions)
        {
            int newRow = EmptyRow + dx;
            int newCol = EmptyCol + dy;

            if (newRow >= 0 && newRow < 3 && newCol >= 0 && newCol < 3)
            {
                int[,] newBoard = (int[,])Board.Clone();
                newBoard[EmptyRow, EmptyCol] = newBoard[newRow, newCol];
                newBoard[newRow, newCol] = 0;
                yield return new PuzzleState(newBoard, this, Depth + 1);
            }
        }
    }
    
    public void PrintSteps()
    {
        var steps = new Stack<PuzzleState>();
        var current = this;

        while (current != null)
        {
            steps.Push(current);
            current = current.Parent;
        }

        Console.WriteLine("Solving steps:");
        int stepNumber = 0;
        while (steps.Count > 0)
        {
            var step = steps.Pop();
            Console.WriteLine($"Step {stepNumber++}:");
            step.PrintBoard();
            Console.WriteLine();
        }
    }

    public void PrintBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.Write(Board[i, j] == 0 ? " " : Board[i, j].ToString());
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
}