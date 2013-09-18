using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Areas.V2.ViewModels;
using Xunit;

namespace Snittlistan.Test
{
    public class PlayerStatusViewModelTest
    {
        [Fact]
        public void ComparesAbsences()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", null);
            left.Absences.Add(new AbsenceIndex.Result());
            var right = new PlayerStatusViewModel("B", null);
            right.Absences.Add(new AbsenceIndex.Result());
            var comparer = new PlayerStatusViewModel.Comparer();

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void ComparesLeftAbsence()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", null);
            left.Absences.Add(new AbsenceIndex.Result());
            var right = new PlayerStatusViewModel("B", null);
            var comparer = new PlayerStatusViewModel.Comparer();

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void ComparesRightAbsence()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", null);
            var right = new PlayerStatusViewModel("B", null);
            right.Absences.Add(new AbsenceIndex.Result());
            var comparer = new PlayerStatusViewModel.Comparer();

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void ComparesPlayerFormLess()
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
            var comparer = new PlayerStatusViewModel.Comparer();

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void ComparesPlayerFormGreater()
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
            var comparer = new PlayerStatusViewModel.Comparer();

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void ComparesPlayerFormLeft()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", new PlayerFormViewModel("A")
            {
                SeasonAverage = 190
            });
            var right = new PlayerStatusViewModel("B", null);
            var comparer = new PlayerStatusViewModel.Comparer();

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void ComparesPlayerFormRight()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", null);
            var right = new PlayerStatusViewModel("B", new PlayerFormViewModel("B")
            {
                SeasonAverage = 195
            });
            var comparer = new PlayerStatusViewModel.Comparer();

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void ComparesAbsenceToForm()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", null);
            left.Absences.Add(new AbsenceIndex.Result());
            var right = new PlayerStatusViewModel("B", new PlayerFormViewModel("B"));
            var comparer = new PlayerStatusViewModel.Comparer();

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void ComparesFormToAbsence()
        {
            // Arrange
            var left = new PlayerStatusViewModel("A", new PlayerFormViewModel("A"));
            var right = new PlayerStatusViewModel("B", null);
            right.Absences.Add(new AbsenceIndex.Result());
            var comparer = new PlayerStatusViewModel.Comparer();

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.Equal(-1, result);
        }
    }
}