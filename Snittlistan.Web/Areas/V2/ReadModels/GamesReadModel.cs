using System.Collections.Generic;

namespace Snittlistan.Web.Areas.V2.ReadModels
{
    public class ResultReadModel
    {
        public ResultReadModel()
        {
            Series = new List<Serie> { new Serie(), new Serie(), new Serie(), new Serie() };
        }

        public List<Serie> Series { get; set; }

        public static string IdFromBitsMatchId(int id)
        {
            return "Series-" + id;
        }

        public class Serie
        {
            public Serie()
            {
                Tables = new List<Table> { new Table(), new Table(), new Table(), new Table() };
            }

            public List<Table> Tables { get; set; }
        }

        public class Table
        {
            public Table()
            {
                Game1 = new Game();
                Game2 = new Game();
            }

            public int Score { get; set; }

            public Game Game1 { get; set; }

            public Game Game2 { get; set; }
        }

        public class Game
        {
            public Game()
            {
                Player = string.Empty;
            }

            public string Player { get; set; }

            public int Pins { get; set; }

            public int Strikes { get; set; }

            public int Spares { get; set; }
        }
    }
}