#nullable enable

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
            PlayerStatusViewModel left = new(new Player("A", "e@d.com", Player.Status.Active, 0, null, new string[0]), new PlayerFormViewModel("A"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            left.AddAbsence(new AbsenceIndex.Result());
            PlayerStatusViewModel right = new(new Player("B", "e@d.com", Player.Status.Active, 0, null, new string[0]), new PlayerFormViewModel("B"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            right.AddAbsence(new AbsenceIndex.Result());
            PlayerStatusViewModel.Comparer comparer = new(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesLeftAbsence()
        {
            // Arrange
            PlayerStatusViewModel left = new(
                new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]),
                null,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1));
            left.AddAbsence(new AbsenceIndex.Result
            {
                From = new DateTime(2018, 1, 1),
                To = new DateTime(2018, 1, 1)
            });
            PlayerStatusViewModel right = new(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), null, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel.Comparer comparer = new(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void ComparesRightAbsence()
        {
            // Arrange
            PlayerStatusViewModel left = new(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel right = new(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            right.AddAbsence(new AbsenceIndex.Result());
            PlayerStatusViewModel.Comparer comparer = new(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesSeasonAverageLess()
        {
            // Arrange
            PlayerStatusViewModel left = new(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A")
            {
                SeasonAverage = 190
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel right = new(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B")
            {
                SeasonAverage = 195
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel.Comparer comparer = new(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesSeasonAverageGreater()
        {
            // Arrange
            PlayerStatusViewModel left = new(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A")
            {
                SeasonAverage = 197
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel right = new(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B")
            {
                SeasonAverage = 195
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel.Comparer comparer = new(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesPlayerFormLess()
        {
            // Arrange
            PlayerStatusViewModel left = new(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A")
            {
                SeasonAverage = 190,
                Last5Average = 190
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel right = new(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B")
            {
                SeasonAverage = 190,
                Last5Average = 195
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel.Comparer comparer = new(CompareMode.PlayerForm);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesPlayerFormGreater()
        {
            // Arrange
            PlayerStatusViewModel left = new(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A")
            {
                SeasonAverage = 190,
                Last5Average = 197
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel right = new(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B")
            {
                SeasonAverage = 190,
                Last5Average = 195
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel.Comparer comparer = new(CompareMode.PlayerForm);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesSeasonAverageLeft()
        {
            // Arrange
            PlayerStatusViewModel left = new(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A")
            {
                SeasonAverage = 190,
                HasResult = true
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel right = new(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel.Comparer comparer = new(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesSeasonAverageRight()
        {
            // Arrange
            PlayerStatusViewModel left = new(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel right = new(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B")
            {
                SeasonAverage = 195,
                HasResult = true
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel.Comparer comparer = new(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void ComparesPlayerFormLeft()
        {
            // Arrange
            PlayerStatusViewModel left = new(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A")
            {
                Last5Average = 190,
                HasResult = true
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel right = new(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel.Comparer comparer = new(CompareMode.PlayerForm);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesPlayerFormRight()
        {
            // Arrange
            PlayerStatusViewModel left = new(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel right = new(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B")
            {
                Last5Average = 195,
                HasResult = true
            }, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel.Comparer comparer = new(CompareMode.PlayerForm);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void ComparesAbsenceToForm()
        {
            // Arrange
            PlayerStatusViewModel left = new(
                new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]),
                null,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1));
            left.AddAbsence(new AbsenceIndex.Result
            {
                From = new DateTime(2018, 1, 1),
                To = new DateTime(2018, 1, 1)
            });
            PlayerStatusViewModel right = new(new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("B"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel.Comparer comparer = new(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void ComparesFormToAbsence()
        {
            // Arrange
            PlayerStatusViewModel left = new(new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]), new PlayerFormViewModel("A"), new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
            PlayerStatusViewModel right = new(
                new Player("B", "e@d.com", Player.Status.Active, -1, null, new string[0]),
                null,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1));
            right.AddAbsence(new AbsenceIndex.Result
            {
                From = new DateTime(2018, 1, 1),
                To = new DateTime(2018, 1, 1)
            });
            PlayerStatusViewModel.Comparer comparer = new(CompareMode.SeasonAverage);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesPartialAbsenceToForm()
        {
            // Arrange
            PlayerStatusViewModel left = new(
                new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]),
                new PlayerFormViewModel("A")
                {
                    Last5Average = 190
                },
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 2));
            PlayerStatusViewModel right = new(
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
            PlayerStatusViewModel.Comparer comparer = new(CompareMode.PlayerForm);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ComparesFormToPartialAbsence()
        {
            // Arrange
            PlayerStatusViewModel left = new(
                new Player("A", "e@d.com", Player.Status.Active, -1, null, new string[0]),
                new PlayerFormViewModel("A")
                {
                    Last5Average = 180
                },
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 2));
            PlayerStatusViewModel right = new(
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
            PlayerStatusViewModel.Comparer comparer = new(CompareMode.PlayerForm);

            // Act
            int result = comparer.Compare(left, right);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }
    }
}
