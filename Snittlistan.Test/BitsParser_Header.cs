using System;
using System.Collections.Generic;
using Snittlistan.Test.Properties;
using Snittlistan.Web.Areas.V2.Domain;
using Xunit;

namespace Snittlistan.Test
{
    public class BitsParser_Header
    {
        [Fact]
        public void ParsesTeamName1()
        {
            // Arrange
            var content = Resources.Id3048477;
            var possibleTeams = new HashSet<string> { "Fredrikshof F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.Equal("Fredrikshof F", header.Team);
        }

        [Fact]
        public void ParsesTeamName2()
        {
            // Arrange
            var content = Resources.Id3048477;
            var possibleTeams = new HashSet<string> { "Fredrikshof IF F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.Equal("Fredrikshof IF F", header.Team);
        }

        [Fact]
        public void ParsesTeamName3()
        {
            // Arrange
            var content = Resources.Id3048477;
            var possibleTeams = new HashSet<string> { "Fredrikshof IF F", "Fredrikshof F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.Equal("Fredrikshof IF F", header.Team);
        }

        [Fact]
        public void ParsesTeamName4()
        {
            // Arrange
            var content = Resources.Id3048477;
            var possibleTeams = new HashSet<string> { "Fredrikshof F", "Fredrikshof IF F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.Equal("Fredrikshof IF F", header.Team);
        }

        [Fact]
        public void ParsesTeamName5()
        {
            // Arrange
            var content = Resources.Id3048747;
            var possibleTeams = new HashSet<string> { "Fredrikshof F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.Equal("Fredrikshof F", header.Team);
        }

        [Fact]
        public void ParsesTeamName6()
        {
            // Arrange
            var content = Resources.Id3048747;
            var possibleTeams = new HashSet<string> { "Fredrikshof IF F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.Equal("Fredrikshof IF F", header.Team);
        }

        [Fact]
        public void ParsesTeamName7()
        {
            // Arrange
            var content = Resources.Id3048747;
            var possibleTeams = new HashSet<string> { "Fredrikshof IF F", "Fredrikshof F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.Equal("Fredrikshof F", header.Team);
        }

        [Fact]
        public void ParsesTeamName8()
        {
            // Arrange
            var content = Resources.Id3048747;
            var possibleTeams = new HashSet<string> { "Fredrikshof F", "Fredrikshof IF F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.Equal("Fredrikshof IF F", header.Team);
        }

        [Fact]
        public void ParsesDate()
        {
            // Arrange
            var content = Resources.Id3048477;
            var possibleTeams = new HashSet<string> { "Fredrikshof IF F" };

            // Act
            var header = BitsParser.ParseHeader(content, possibleTeams);

            // Assert
            Assert.Equal(header.Date, new DateTime(2013, 4, 20));
        }
    }
}