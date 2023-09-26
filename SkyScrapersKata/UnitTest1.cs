namespace SkyScrapersKata
{
    [TestFixture]
    public class SkyscrapersTests
    {
        [Test]
        public void SolveSkyscrapers1()
        {
            var clues = new[]{  2, 2, 1, 3,
                                2, 2, 3, 1,
                                1, 2, 2, 3,
                                3, 2, 1, 3};

            var expected = new[]{   new []{1, 3, 4, 2},
                                    new []{4, 2, 1, 3},
                                    new []{3, 4, 2, 1},
                                    new []{2, 1, 3, 4}};

            var actual = Skyscrapers.SolvePuzzle(clues);
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void SolveSkyscrapers2()
        {
            var clues = new[]{  0, 0, 1, 2,
                                0, 2, 0, 0,
                                0, 3, 0, 0,
                                0, 1, 0, 0};

            var expected = new[]{ new []{   2, 1, 4, 3},
                               new []{      3, 4, 1, 2},
                               new []{      4, 2, 3, 1},
                               new []{      1, 3, 2, 4}};

            var actual = Skyscrapers.SolvePuzzle(clues);
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void GenerateRowsTest()
        {
            Assert.That(RowUtilities.Rows.Count(), Is.EqualTo(24));
        }

        [TestCaseSource(nameof(ScraperCases))]
        public void CountVisibleScrapersTest((List<int> row, int expected) td)
        {
            var result = RowUtilities.CountVisibleScrapers(td.row);

            Assert.That(result, Is.EqualTo(td.expected));
        }

        public static IEnumerable<(List<int>, int)> ScraperCases()
        {
            yield return (new List<int> { 4, 2, 3, 1 }, 1);
            yield return (new List<int> { 4, 2, 1, 3 }, 1);
            yield return (new List<int> { 4, 1, 2, 3 }, 1);
            yield return (new List<int> { 4, 1, 3, 2 }, 1);
            yield return (new List<int> { 4, 3, 1, 2 }, 1);
            yield return (new List<int> { 4, 3, 2, 1 }, 1);
            yield return (new List<int> { 3, 4, 2, 1 }, 2);
            yield return (new List<int> { 2, 4, 3, 1 }, 2);
            yield return (new List<int> { 1, 2, 4, 3 }, 3);
            yield return (new List<int> { 3, 2, 1, 4 }, 2);
            yield return (new List<int> { 1, 2, 3, 4 }, 4);
        }

        [Test]
        public void GetPossibleRowsTest()
        {
            var rows1 = RowUtilities.GetPossibleRows(1);
            var rows2 = RowUtilities.GetPossibleRows(2);
            var rows3 = RowUtilities.GetPossibleRows(3);
            var rows4 = RowUtilities.GetPossibleRows(4);

            Assert.That(rows1.Count(), Is.EqualTo(6));
            Assert.That(rows2.Count(), Is.EqualTo(11));
            Assert.That(rows3.Count(), Is.EqualTo(6));
            Assert.That(rows4.Count(), Is.EqualTo(1));
        }

        [Test]
        public void BuildGrid_PassInTheAnswer()
        {
            var clues = new[]{  2, 2, 1, 3,
                                2, 2, 3, 1,
                                1, 2, 2, 3,
                                3, 2, 1, 3};

            var rows = new List<int[]>{   new []{1, 3, 4, 2},
                                    new []{4, 2, 1, 3},
                                    new []{3, 4, 2, 1},
                                    new []{2, 1, 3, 4}};

            var grid = Skyscrapers.BuildGrid(new List<int[]>(), rows, clues);

            Assert.That(grid.IsValid, Is.True);
            CollectionAssert.AreEqual(grid.Grid, rows);
        }

        [Test]
        public void BuildGrid_WrongRowFromLeft()
        {
            var clues = new[]{  2, 2, 1, 3,
                                2, 2, 3, 1,
                                1, 2, 2, 3,
                                3, 2, 1, 3};

            var rows = new List<int[]>{ new []{1, 2, 3, 4},
                                        new []{2, 3, 4, 1},
                                        new []{1, 4, 2, 3},
                                        new []{3, 4, 1, 2},
                                        new []{4, 1, 2, 3},
                                        new []{2, 4, 3, 1},
                                        new []{1, 3, 4, 2},
                                        new []{4, 2, 1, 3},
                                        new []{3, 4, 2, 1},
                                        new []{2, 1, 3, 4}};

            var expected = new List<int[]>{ 
                                    new []{1, 3, 4, 2},
                                    new []{4, 2, 1, 3},
                                    new []{3, 4, 2, 1},
                                    new []{2, 1, 3, 4}};

            var grid = Skyscrapers.BuildGrid(new List<int[]>(), rows, clues);

            Assert.That(grid.IsValid, Is.True);
            CollectionAssert.AreEqual(grid.Grid, expected);
        }

        [Test]
        public void BuildGrid_Full()
        {
            var clues = new[]{  2, 2, 1, 3,
                                2, 2, 3, 1,
                                1, 2, 2, 3,
                                3, 2, 1, 3};

            var rows = RowUtilities.Rows.Select(x => x.ToArray()).ToList();

            var expected = new List<int[]>{
                                    new []{1, 3, 4, 2},
                                    new []{4, 2, 1, 3},
                                    new []{3, 4, 2, 1},
                                    new []{2, 1, 3, 4}};

            var grid = Skyscrapers.BuildGrid(new List<int[]>(), rows, clues);

            Assert.That(grid.IsValid, Is.True);
            CollectionAssert.AreEqual(grid.Grid, expected);
        }
    }
}