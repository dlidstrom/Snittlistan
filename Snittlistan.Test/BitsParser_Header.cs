using System;
using System.Collections.Generic;
using NUnit.Framework;
using Snittlistan.Test.Properties;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Test
{
    [TestFixture]
    public class BitsParser_Header
    {
        [Test]
        public void ParsesTeamName1()
        {
            // Arrange
            var content = Resources.Id3048477;
            var possibleTeams = new HashSet<string> { "Fredrikshof F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.That(header.Team, Is.EqualTo("Fredrikshof F"));
        }

        [Test]
        public void ParsesTeamName2()
        {
            // Arrange
            var content = Resources.Id3048477;
            var possibleTeams = new HashSet<string> { "Fredrikshof IF F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.That(header.Team, Is.EqualTo("Fredrikshof IF F"));
        }

        [Test]
        public void ParsesTeamName3()
        {
            // Arrange
            var content = Resources.Id3048477;
            var possibleTeams = new HashSet<string> { "Fredrikshof IF F", "Fredrikshof F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.That(header.Team, Is.EqualTo("Fredrikshof IF F"));
        }

        [Test]
        public void ParsesTeamName4()
        {
            // Arrange
            var content = Resources.Id3048477;
            var possibleTeams = new HashSet<string> { "Fredrikshof F", "Fredrikshof IF F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.That(header.Team, Is.EqualTo("Fredrikshof IF F"));
        }

        [Test]
        public void ParsesTeamName5()
        {
            // Arrange
            var content = Resources.Id3048747;
            var possibleTeams = new HashSet<string> { "Fredrikshof F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.That(header.Team, Is.EqualTo("Fredrikshof F"));
        }

        [Test]
        public void ParsesTeamName6()
        {
            // Arrange
            var content = Resources.Id3048747;
            var possibleTeams = new HashSet<string> { "Fredrikshof IF F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.That(header.Team, Is.EqualTo("Fredrikshof IF F"));
        }

        [Test]
        public void ParsesTeamName7()
        {
            // Arrange
            var content = Resources.Id3048747;
            var possibleTeams = new HashSet<string> { "Fredrikshof IF F", "Fredrikshof F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.That(header.Team, Is.EqualTo("Fredrikshof F"));
        }

        [Test]
        public void ParsesTeamName8()
        {
            // Arrange
            var content = Resources.Id3048747;
            var possibleTeams = new HashSet<string> { "Fredrikshof F", "Fredrikshof IF F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.That(header.Team, Is.EqualTo("Fredrikshof IF F"));
        }

        [Test]
        public void ParsesDate()
        {
            // Arrange
            var content = Resources.Id3048477;
            var possibleTeams = new HashSet<string> { "Fredrikshof IF F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.That(header.Date, Is.EqualTo(new DateTime(2013, 4, 20)));
        }

        [Test]
        public void ParsesOilPattern()
        {
            // Arrange
            var content = Resources.Id3152235;
            var possibleTeams = new HashSet<string> { "Fredrikshof IF BK F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.That(header.OilPattern, Is.EqualTo("ABT#2"));
        }
    }
}