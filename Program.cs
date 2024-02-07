﻿using System.Diagnostics;
using System.Globalization;

class WordFinder
{
    private static readonly int[] dx = [-1, -1, -1, 0, 0, 1, 1, 1];
    private static readonly int[] dy = [-1, 0, 1, -1, 1, -1, 0, 1];

    public static List<string> FindWords(char[,] grid)
    {
        List<string> words = [];
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                FindWordsUtil(grid, rows, cols, i, j, new bool[rows, cols], grid[i, j].ToString(), words);
            }
        }

        return words;
    }

    private static void FindWordsUtil(char[,] grid, int rows, int cols, int x, int y, bool[,] visited, string word, List<string> words)
    {
        visited[x, y] = true;

        if (word.Length >= 3)
        {
            // Minimum word length constraint
            words.Add(word);
        }

        for (int i = 0; i < 8; i++)
        {
            int newX = x + dx[i];
            int newY = y + dy[i];

            if (IsValid(newX, newY, rows, cols) && !visited[newX, newY])
            {
                FindWordsUtil(grid, rows, cols, newX, newY, visited, word + grid[newX, newY], words);
            }
        }

        visited[x, y] = false;
    }

    private static bool IsValid(int x, int y, int rows, int cols)
    {
        return x >= 0 && x < rows && y >= 0 && y < cols;
    }
}

class Program
{
    static void Main()
    {
        char[,] grid = {
            {'N', 'D', 'L', 'L'},
            {'G', 'I', 'S', 'K'},
            {'R', 'T', 'N', 'R'},
            {'E', 'Ä', 'N', 'D'}
        };

        //var stopwatch = new Stopwatch();
        //stopwatch.Start();
        var words = WordFinder.FindWords(grid).Distinct().Where(s => !string.IsNullOrWhiteSpace(s)).Select(x => x.ToLower()).ToList();
        //stopwatch.Stop();
        //Console.WriteLine($"Algorithm took: {stopwatch.ElapsedMilliseconds} ms");
        // WriteToFileOrdered("all_combinations.txt", words); // Takes about 12s
        var swedishWords = File.ReadAllLines("dictionary\\swedish.txt");
        //stopwatch.Restart();
        var valid = words.Intersect(swedishWords);
        Console.WriteLine($"Found {valid.Count()} swedish words");
        //stopwatch.Stop();
        //Console.WriteLine($"Intersect took: {stopwatch.ElapsedMilliseconds} ms");
        WriteToFileOrdered("all_valid.txt", valid);
    }

    static void WriteToFileOrdered(string fileName, IEnumerable<string> list)
    {
        //var stopwatch = new Stopwatch();
        //stopwatch.Start();
        var swedishCulture = new CultureInfo("sv-SE");
        var ordered = list.OrderBy(word => word.Length).ThenBy(w => w, StringComparer.Create(swedishCulture, true));
        File.WriteAllLines($"result\\{fileName}", ordered);
        //stopwatch.Stop();
        //Console.WriteLine($"Ordering and writing to {fileName} took: {stopwatch.ElapsedMilliseconds} ms");
    }
}