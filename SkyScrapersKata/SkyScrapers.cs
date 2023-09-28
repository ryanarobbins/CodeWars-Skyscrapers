using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SkyScrapersKata
{
    public class Skyscrapers
    {
        public static int GridSize { get; set; }

        public static int[][] SolvePuzzle(int[] clues)
        {
            GridSize = clues.Length / 4;

            var rows = RowUtilities.GenerateRows(GridSize).Select(x => x.ToArray()).ToList();

            var grid = BuildGrid(new List<int[]>(), rows, clues);

            return grid.Grid.ToArray();
        }

        public static GridMetadata BuildGrid(List<int[]> grid, List<int[]> rows, int[] clues)
        {
            if (grid.Count == GridSize)
            {
                var side = 0;
                for (int i = 0; i < clues.Length; i++)
                {
                    var position = i % GridSize;
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
                var leftOvers = rows.Where(x => IsRowStillAllowed(x, row)
                    ).ToList();
                var metadata = BuildGrid(permutation, leftOvers, clues);
                if (metadata.IsValid)
                {
                    return metadata;
                }
            }
            return new GridMetadata { IsValid = false };
        }

        private static bool IsRowStillAllowed(int[] x, int[] row)
        {
            for (int i = 0; i < row.Length; i++)
            {
                if (x[i] == row[i]) return false;
            }
            return true;
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
            var offset = GridSize - 1;
            if (side == 3)
            {
                var column = grid.Select(x => x[offset - position]).Reverse().ToList();
                return clue == RowUtilities.CountVisibleScrapers(column);
            }
            return clue == RowUtilities.CountVisibleScrapers(grid[offset - position].ToList());
        }
    }

    public class GridMetadata
    {
        public bool IsValid { get; set; }
        public List<int[]> Grid { get; set; }
    }

    public static class RowUtilities
    {
        public static List<List<int>> GetPossibleRows(int visibleScrapers, int gridSize = 4)
        {

            return GenerateRows(gridSize).Where(x => CountVisibleScrapers(x) == visibleScrapers).ToList();
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

        public static List<List<int>> GenerateRows(int gridSize)
        {
            var elements = Enumerable.Range(1, gridSize).ToList();
            return GenerateRows(elements);
        }

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
