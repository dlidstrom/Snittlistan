namespace Snittlistan.Test
{
    using System;
    using NUnit.Framework;
    using Web.Areas.V2.Domain;
    using Web.Areas.V2.Indexes;
    using Web.Areas.V2.ViewModels;

    [TestFixture]
    public class PlayerStatusViewModelTest
    {
        [Test]
        public void ComparesAbsences()
        {
            // Arrange
            var left = new PlayerStatusViewModel(new Player("A", "e@d.com", Player.Status.Active, 0, null, new string[0]), new PlayerFormViewModel("A"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            left.AddAbsence(new AbsenceIndex.Result());
            var right = new PlayerStatusViewModel(new Player("B", "e@d.com", Player.Status.Active, 0, null, new string[0]), new PlayerFormViewModel("B"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            right.AddAbsence(new AbsenceIndex.Result());
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesLeftAbsence()
        {
            // Arrange
            var left = new PlayerStatusViewModel(
                new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]),
                null,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1));
            left.AddAbsence(new AbsenceIndex.Result
            {
                From = new DateTime(2018, 1, 1),
                To = new DateTime(2018, 1, 1)
            });
            var right = new PlayerStatusViewModel(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), null, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void ComparesRightAbsence()
        {
            // Arrange
            var left = new PlayerStatusViewModel(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var right = new PlayerStatusViewModel(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            right.AddAbsence(new AbsenceIndex.Result());
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesSeasonAverageLess()
        {
            // Arrange
            var left = new PlayerStatusViewModel(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A")
            {
                SeasonAverage = 190
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var right = new PlayerStatusViewModel(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B")
            {
                SeasonAverage = 195
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesSeasonAverageGreater()
        {
            // Arrange
            var left = new PlayerStatusViewModel(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A")
            {
                SeasonAverage = 197
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var right = new PlayerStatusViewModel(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B")
            {
                SeasonAverage = 195
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesPlayerFormLess()
        {
            // Arrange
            var left = new PlayerStatusViewModel(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A")
            {
                SeasonAverage = 190,
                Last5Average = 190
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var right = new PlayerStatusViewModel(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B")
            {
                SeasonAverage = 190,
                Last5Average = 195
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.PlayerForm);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesPlayerFormGreater()
        {
            // Arrange
            var left = new PlayerStatusViewModel(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A")
            {
                SeasonAverage = 190,
                Last5Average = 197
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var right = new PlayerStatusViewModel(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B")
            {
                SeasonAverage = 190,
                Last5Average = 195
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.PlayerForm);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesSeasonAverageLeft()
        {
            // Arrange
            var left = new PlayerStatusViewModel(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A")
            {
                SeasonAverage = 190,
                HasResult = true
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var right = new PlayerStatusViewModel(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesSeasonAverageRight()
        {
            // Arrange
            var left = new PlayerStatusViewModel(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var right = new PlayerStatusViewModel(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B")
            {
                SeasonAverage = 195,
                HasResult = true
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void ComparesPlayerFormLeft()
        {
            // Arrange
            var left = new PlayerStatusViewModel(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A")
            {
                Last5Average = 190,
                HasResult = true
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var right = new PlayerStatusViewModel(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.PlayerForm);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesPlayerFormRight()
        {
            // Arrange
            var left = new PlayerStatusViewModel(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var right = new PlayerStatusViewModel(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B")
            {
                Last5Average = 195,
                HasResult = true
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.PlayerForm);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void ComparesAbsenceToForm()
        {
            // Arrange
            var left = new PlayerStatusViewModel(
                new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]),
                null,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1));
            left.AddAbsence(new AbsenceIndex.Result
            {
                From = new DateTime(2018, 1, 1),
                To = new DateTime(2018, 1, 1)
            });
            var right = new PlayerStatusViewModel(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void ComparesFormToAbsence()
        {
            // Arrange
            var left = new PlayerStatusViewModel(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            var right = new PlayerStatusViewModel(
                new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]),
                null,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1));
            right.AddAbsence(new AbsenceIndex.Result
            {
                From = new DateTime(2018, 1, 1),
                To = new DateTime(2018, 1, 1)
            });
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesPartialAbsenceToForm()
        {
            // Arrange
            var left = new PlayerStatusViewModel(
                new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]),
                new PlayerFormViewModel("A")
                {
                    Last5Average = 190
                },
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 2));
            var right = new PlayerStatusViewModel(
                new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]),
                new PlayerFormViewModel("B")
                {
                    Last5Average = 180
                }, 
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 2));
            left.AddAbsence(new AbsenceIndex.Result
            {
                From = new DateTime(2018, 1, 1),
                To = new DateTime(2018, 1, 1)
            });
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.PlayerForm);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesFormToPartialAbsence()
        {
            // Arrange
            var left = new PlayerStatusViewModel(
                new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]),
                new PlayerFormViewModel("A")
                {
                    Last5Average = 180
                },
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 2));
            var right = new PlayerStatusViewModel(
                new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]),
                new PlayerFormViewModel("B")
                {
                    Last5Average = 190
                }, 
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 2));
            right.AddAbsence(new AbsenceIndex.Result
            {
                From = new DateTime(2018, 1, 1),
                To = new DateTime(2018, 1, 1)
            });
            var comparer = new PlayerStatusViewModel.Comparer(CompareMode.PlayerForm);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }
    }
}