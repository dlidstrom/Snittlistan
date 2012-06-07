namespace Snittlistan.Test
{
    using System;
    using System.Collections.Generic;
    using Models;

    public class DbSeed
    {
        public static Match8x4 Create8x4Match()
        {
            var series = new List<Serie8x4>
            {
                new Serie8x4(new List<Table8x4>
                {
                    new Table8x4
                    {
                        Score = 1,
                        Game1 = new Game8x4("Mikael Axelsson", 202)
                        {
                            Strikes = 5,
                            Misses = 2,
                            OnePinMisses = 1,
                            Splits = 2,
                            CoveredAll = true
                        },
                        Game2 = new Game8x4("Christer Liedholm", 218)
                    },
                    new Table8x4
                    {
                        Score = 0,
                        Game1 = new Game8x4("Kjell Persson", 172),
                        Game2 = new Game8x4("Peter Sjöberg", 220),
                    },
                    new Table8x4
                    {
                        Score = 0,
                        Game1 = new Game8x4("Kjell Jansson", 166),
                        Game2 = new Game8x4("Hans Norbeck", 194)
                    },
                    new Table8x4
                    {
                        Score = 1,
                        Game1 = new Game8x4("Lars Öberg", 222),
                        Game2 = new Game8x4("Torbjörn Jensen", 204)
                    }
                }),
                new Serie8x4(new List<Table8x4>
                {
                    new Table8x4
                    {
                        Score = 1,
                        Game1 = new Game8x4("Lars Öberg", 182),
                        Game2 = new Game8x4("Torbjörn Jensen", 211)
                    },
                    new Table8x4
                    {
                        Score = 1,
                        Game1 = new Game8x4("Kjell Jansson", 208),
                        Game2 = new Game8x4("Hans Norbeck", 227)
                    },
                    new Table8x4
                    {
                        Score = 0,
                        Game1 = new Game8x4("Kjell Persson", 194),
                        Game2 = new Game8x4("Peter Sjöberg", 195)
                    },
                    new Table8x4
                    {
                        Score = 0,
                        Game1 = new Game8x4("Mikael Axelsson", 206),
                        Game2 = new Game8x4("Christer Liedholm", 150)
                    }
                }),
                new Serie8x4(new List<Table8x4>
                {
                    new Table8x4
                    {
                        Score = 0,
                        Game1 = new Game8x4("Kjell Persson", 174),
                        Game2 = new Game8x4("Peter Sjöberg", 182)
                    },
                    new Table8x4
                    {
                        Score = 1,
                        Game1 = new Game8x4("Mikael Axelsson", 214),
                        Game2 = new Game8x4("Christer Liedholm", 176)
                    },
                    new Table8x4
                    {
                        Score = 0,
                        Game1 = new Game8x4("Lars Öberg", 168),
                        Game2 = new Game8x4("Torbjörn Jensen", 199)
                    },
                    new Table8x4
                    {
                        Score = 0,
                        Game1 = new Game8x4("Kjell Jansson", 180),
                        Game2 = new Game8x4("Hans Norbeck", 212)
                    }
                }),
                new Serie8x4(new List<Table8x4>
                {
                    new Table8x4
                    {
                        Score = 0,
                        Game1 = new Game8x4("Kjell Jansson", 189),
                        Game2 = new Game8x4("Hans Norbeck", 181)
                    },
                    new Table8x4
                    {
                        Score = 0,
                        Game1 = new Game8x4("Lars Öberg", 227),
                        Game2 = new Game8x4("Torbjörn Jensen", 180)
                    },
                    new Table8x4
                    {
                        Score = 1,
                        Game1 = new Game8x4("Mikael Axelsson", 223),
                        Game2 = new Game8x4("Christer Liedholm", 191)
                    },
                    new Table8x4
                    {
                        Score = 0,
                        Game1 = new Game8x4("Thomas Gurell", 159),
                        Game2 = new Game8x4("Peter Sjöberg", 190)
                    }
                }),
            };
            var match = new Match8x4(
                location: "Sollentuna Bowlinghall",
                date: new DateTime(2011, 03, 26),
                bitsMatchId: 3003231,
                homeTeam: new Team8x4("Sollentuna Bwk", 13),
                awayTeam: new Team8x4("Fredrikshof IF", 6, series));

            return match;
        }

        public static Match4x4 Create4x4Match()
        {
            var series = new List<Serie4x4>
            {
                new Serie4x4(new List<Game4x4>
                {
                    new Game4x4("Tomas Gustavsson", 160, 0),
                    new Game4x4("Markus Norbeck", 154, 0),
                    new Game4x4("Lars Norbeck", 169, 1),
                    new Game4x4("Matz Classon", 140, 0),
                }),
                new Serie4x4(new List<Game4x4>
                {
                    new Game4x4("Tomas Gustavsson", 141, 0),
                    new Game4x4("Markus Norbeck", 114, 0),
                    new Game4x4("Lars Norbeck", 163, 1),
                    new Game4x4("Matz Classon", 127, 0),
                }),
                new Serie4x4(new List<Game4x4>
                {
                    new Game4x4("Tomas Gustavsson", 128, 1),
                    new Game4x4("Markus Norbeck", 165, 0),
                    new Game4x4("Lars Norbeck", 231, 1),
                    new Game4x4("Matz Classon", 165, 0),
                }),
                new Serie4x4(new List<Game4x4>
                {
                    new Game4x4("Tomas Gustavsson", 132, 0),
                    new Game4x4("Markus Norbeck", 165, 0),
                    new Game4x4("Lars Norbeck", 154, 1),
                    new Game4x4("Matz Classon", 162, 1),
                })
            };

            var match = new Match4x4(
                    location: "Bowl-O-Rama",
                    date: new DateTime(2012, 01, 28),
                    homeTeam: new Team4x4("Fredrikshof C", 6, series),
                    awayTeam: new Team4x4("Librex", 14));
            return match;
        }
    }
}