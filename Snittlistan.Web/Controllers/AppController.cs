namespace Snittlistan.Web.Controllers
{
    using System.Web.Mvc;

    using Raven.Client;

    using Snittlistan.Web.ViewModels;

    public class AppController : AbstractController
    {
        public AppController(IDocumentSession session)
            : base(session)
        {
        }

        public ActionResult Index()
        {
            var turns = new TurnsViewModel[]
                {
                    new TurnsViewModel
                        {
                            Season = "2012-2013",
                            Turn = 2,
                            StartDate = "22 sep",
                            EndDate = "23 sep",
                            Rosters = new RosterViewModel[]
                                {
                                    new RosterViewModel
                                        {
                                            Team = "Fredrikshof A",
                                            TeamLevel = "a",
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
                                            TeamLevel = "f",
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
                                            TeamLevel = "b",
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

            var vm = new InitialDataViewModel
                {
                    Session = new SessionViewModel
                        {
                            IsAuthenticated = this.Request.IsAuthenticated,
                            Email = this.Request.IsAuthenticated ? this.User.Identity.Name : string.Empty
                        },
                    Turns = turns,
                    Players = players
                };
            return this.View(vm);
        }

        public ActionResult Results()
        {
            return RedirectToAction("Index");
        }

        public ActionResult Players()
        {
            return RedirectToAction("Index");
        }
    }
}
