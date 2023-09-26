using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyScrapersKata
{
    public class Skyscrapers
    {
        public static int[][] SolvePuzzle(int[] clues)
        {
            var rows = RowUtilities.Rows.Select(x => x.ToArray()).ToList();

            var grid = BuildGrid(new List<int[]>(), rows, clues);

            return grid.Grid.ToArray();
        }

        public static GridMetadata BuildGrid(List<int[]> grid, List<int[]> rows, int[] clues)
        {
            if (grid.Count == 4)
            {
                var side = 0;
                for (int i = 0; i < clues.Length; i++)
                {
                    var position = i % 4;
                    if (position == 0) side++;
                    var check = IsGridValidForClue(position, side, clues[i], grid);
                    if (!check)
                    {
                        return new GridMetadata { IsValid = false, Grid = grid };
                    }
                }

                return new GridMetadata { IsValid = true, Grid = grid };
            }
            foreach (var row in rows)
            {
                var permutation = new List<int[]>();
                permutation.AddRange(grid);
                permutation.Add(row);
                var leftOvers = rows.Where(x =>
                    x[0] != row[0] &&
                    x[1] != row[1] &&
                    x[2] != row[2] &&
                    x[3] != row[3]
                    ).ToList();
                var metadata = BuildGrid(permutation, leftOvers, clues);
                if (metadata.IsValid)
                {
                    return metadata;
                }
            }
            return new GridMetadata { IsValid = false };
        }

        private static bool IsGridValidForClue(int position, int side, int clue, List<int[]> grid)
        {
            if (clue == 0) return true;
            if (side == 1)
            {
                var column = grid.Select(x => x[position]).ToList();
                return clue == RowUtilities.CountVisibleScrapers(column);
            }
            if (side == 2)
            {
                return clue == RowUtilities.CountVisibleScrapers(grid[position].Reverse().ToList());
            }
            if (side == 3)
            {
                var column = grid.Select(x => x[3 - position]).Reverse().ToList();
                return clue == RowUtilities.CountVisibleScrapers(column);
            }

            return clue == RowUtilities.CountVisibleScrapers(grid[3 - position].ToList());
        }
    }

    public class GridMetadata
    {
        public bool IsValid { get; set; }
        public List<int[]> Grid { get; set; }
    }

    public static class RowUtilities
    {
        static RowUtilities()
        {
            var elements = new List<int> { 1, 2, 3, 4 };
            Rows = GenerateRows(elements);
        }

        public static List<List<int>> GetPossibleRows(int visibleScrapers)
        {

            return Rows.Where(x => CountVisibleScrapers(x) == visibleScrapers).ToList();
        }

        public static int CountVisibleScrapers(List<int> row)
        {
            var count = 0;
            var lookedAt = new List<int>();
            foreach (var scraper in row)
            {
                if (!lookedAt.Any(x => x > scraper))
                {
                    count++;
                }
                lookedAt.Add(scraper);
            }
            return count;

        }

        public static List<List<int>> Rows;

        public static List<List<int>> GenerateRows(List<int> elements)
        {
            if (elements.Count() == 1)
            {
                return new List<List<int>> { elements };
            }
            var rows = new List<List<int>>();
            foreach (var element in elements)
            {
                var leftOvers = elements.Where(x => x != element).ToList();
                var current = new List<int> { element };
                var permutations = GenerateRows(leftOvers);
                foreach (var row in permutations)
                {
                    var toAdd = new List<int> { element };
                    toAdd.AddRange(row);
                    rows.Add(toAdd);
                }
            }
            return rows;
        }
    }

}
