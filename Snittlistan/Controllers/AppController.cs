namespace Snittlistan.Controllers
{
    using System.Web.Mvc;
    using ViewModels;

    public class AppController : Controller
    {
        public ActionResult Index()
        {
            var turns = new TurnsViewModel[]
                {
                    new TurnsViewModel
                        {
                            Turn = 2,
                            StartDate = "22 sep",
                            EndDate = "23 sep",
                            Rosters = new RosterViewModel[]
                                {
                                    new RosterViewModel
                                        {
                                            Team = "Fredrikshof A",
                                            Location = "Birka",
                                            Opponent = "Stockholm IFK",
                                            Date = "lördag den 22 september 2012, 10:00",
                                            DeclinedClass = "warning",
                                            DeclinedCount = 2,
                                            DeclinedNames = "Kjell Jansson, Christer Liedholm"
                                        },
                                    new RosterViewModel
                                        {
                                            Team = "Fredrikshof F",
                                            Location = "Bowl-O-Rama",
                                            Opponent = "AIK F",
                                            Date = "lördag den 22 september 2012, 11:40",
                                            DeclinedClass = "warning",
                                            DeclinedCount = 2,
                                            DeclinedNames = "Daniel Lidström, Daniel Solvander"
                                        },
                                    new RosterViewModel
                                        {
                                            Team = "Fredrikshof B",
                                            Location = "Bowl-O-Rama",
                                            Opponent = "Hellas B",
                                            Date = "söndag den 23 september 2012, 10:00",
                                            DeclinedClass = "success",
                                            DeclinedCount = 0,
                                            DeclinedNames = string.Empty
                                        }
                                }
                        }
                };

            var players = new PlayerViewModel[]
                {
                    new PlayerViewModel
                        {
                            Index = 1,
                            Name = "Daniel Lidström"
                        },
                    new PlayerViewModel
                        {
                            Index = 2,
                            Name = "Daniel Solvander"
                        }
                };
            return View(new InitialDataViewModel
            {
                Turns = turns,
                Players = players
            });
        }
    }
}
