using NUnit.Framework;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Areas.V2.ViewModels;

namespace Snittlistan.Test
{
    [TestFixture]
    public class PlayerStatusViewModelTest
    {
        [Test]
        public void ComparesAbsences()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", null);
            left.Absences.Add(new AbsenceIndex.Result());
            var right = new PlayerStatusViewModel("B", null);
            right.Absences.Add(new AbsenceIndex.Result());
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void ComparesLeftAbsence()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", null);
            left.Absences.Add(new AbsenceIndex.Result());
            var right = new PlayerStatusViewModel("B", null);
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesRightAbsence()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", null);
            var right = new PlayerStatusViewModel("B", null);
            right.Absences.Add(new AbsenceIndex.Result());
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void ComparesSeasonAverageLess()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", new PlayerFormViewModel("A")
            {
                SeasonAverage = 190
            });
            var right = new PlayerStatusViewModel("B", new PlayerFormViewModel("B")
            {
                SeasonAverage = 195
            });
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void ComparesSeasonAverageGreater()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", new PlayerFormViewModel("A")
            {
                SeasonAverage = 197
            });
            var right = new PlayerStatusViewModel("B", new PlayerFormViewModel("B")
            {
                SeasonAverage = 195
            });
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesPlayerFormLess()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", new PlayerFormViewModel("A")
            {
                SeasonAverage = 190,
                Last5Average = 190
            });
            var right = new PlayerStatusViewModel("B", new PlayerFormViewModel("B")
            {
                SeasonAverage = 190,
                Last5Average = 195
            });
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.PlayerForm);

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void ComparesPlayerFormGreater()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", new PlayerFormViewModel("A")
            {
                SeasonAverage = 190,
                Last5Average = 197
            });
            var right = new PlayerStatusViewModel("B", new PlayerFormViewModel("B")
            {
                SeasonAverage = 190,
                Last5Average = 195
            });
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.PlayerForm);

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesSeasonAverageLeft()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", new PlayerFormViewModel("A")
            {
                SeasonAverage = 190
            });
            var right = new PlayerStatusViewModel("B", null);
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesSeasonAverageRight()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", null);
            var right = new PlayerStatusViewModel("B", new PlayerFormViewModel("B")
            {
                SeasonAverage = 195
            });
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void ComparesPlayerFormLeft()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", new PlayerFormViewModel("A")
            {
                Last5Average = 190
            });
            var right = new PlayerStatusViewModel("B", null);
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.PlayerForm);

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesPlayerFormRight()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", null);
            var right = new PlayerStatusViewModel("B", new PlayerFormViewModel("B")
            {
                Last5Average = 195
            });
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.PlayerForm);

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void ComparesAbsenceToForm()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", null);
            left.Absences.Add(new AbsenceIndex.Result());
            var right = new PlayerStatusViewModel("B", new PlayerFormViewModel("B"));
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesFormToAbsence()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", new PlayerFormViewModel("A"));
            var right = new PlayerStatusViewModel("B", null);
            right.Absences.Add(new AbsenceIndex.Result());
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }
    }
}