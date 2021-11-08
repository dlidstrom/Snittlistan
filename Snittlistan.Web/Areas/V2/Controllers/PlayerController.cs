#nullable enable

namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.Indexes;
    using Snittlistan.Web.Areas.V2.ViewModels;
    using Snittlistan.Web.Controllers;

    public class PlayerController : AbstractController
    {
        public ActionResult Index()
        {
            List<Player> players = DocumentSession.Query<Player, PlayerSearch>()
                .OrderBy(p => p.PlayerStatus)
                .ThenBy(p => p.Name)
                .ToList();
            List<PlayerViewModel> vm = players.Where(x => x.Hidden == false)
                .Select(x => new PlayerViewModel(x, WebsiteRoles.UserGroup().ToDict()))
                .ToList();
            return View(vm);
        }

        [Authorize(Roles = WebsiteRoles.Player.Admin)]
        public ActionResult Create()
        {
            return View(new CreatePlayerViewModel());
        }

        [HttpPost]
        [Authorize(Roles = WebsiteRoles.Player.Admin)]
        public ActionResult Create(CreatePlayerViewModel vm)
        {
            // prevent duplicates
            bool isDuplicate = DocumentSession.Query<Player, PlayerSearch>().Any(x => x.Name == vm.Name);
            if (isDuplicate)
            {
                ModelState.AddModelError("namn", "Namnet är redan registrerat");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            Player player = new(
                vm.Name,
                vm.Email,
                vm.Status,
                vm.PersonalNumber.GetValueOrDefault(),
                vm.Nickname,
                vm.Roles);
            DocumentSession.Store(player);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = WebsiteRoles.Player.Admin)]
        public ActionResult Edit(int id)
        {
            Player player = DocumentSession.Load<Player>(id);
            if (player == null)
            {
                throw new HttpException(404, "Player not found");
            }

            return View(new CreatePlayerViewModel(player));
        }

        [Authorize(Roles = WebsiteRoles.Player.Admin)]
        [HttpPost]
        public ActionResult Edit(int id, CreatePlayerViewModel vm)
        {
            Player player = DocumentSession.Load<Player>(id);
            if (player == null)
            {
                throw new HttpException(404, "Player not found");
            }

            // prevent duplicates
            Player[] duplicates = DocumentSession.Query<Player, PlayerSearch>().Where(x => x.Name == vm.Name).ToArray();
            if (duplicates.Any(x => x.Id != player.Id))
            {
                ModelState.AddModelError("namn", "Namnet är redan registrerat");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            player.SetName(vm.Name);
            player.SetEmail(vm.Email);
            player.SetStatus(vm.Status);
            player.SetPersonalNumber(vm.PersonalNumber.GetValueOrDefault());
            player.SetNickname(vm.Nickname);
            HashSet<string> allowedRoles = new(
                WebsiteRoles.UserGroup().Except(WebsiteRoles.PlayerGroup()).Select(x => x.Name));
            player.SetRoles(vm.Roles.Where(x => allowedRoles.Contains(x)).ToArray());

            return RedirectToAction("Index");
        }

        [Authorize(Roles = WebsiteRoles.Player.Admin)]
        public ActionResult Delete(int id)
        {
            Player player = DocumentSession.Load<Player>(id);
            if (player == null)
            {
                throw new HttpException(404, "Player not found");
            }

            return View(new PlayerViewModel(player, WebsiteRoles.UserGroup().ToDictionary(x => x.Name)));
        }

        [HttpPost]
        [Authorize(Roles = WebsiteRoles.Player.Admin)]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Player player = DocumentSession.Load<Player>(id);
            if (player == null)
            {
                throw new HttpException(404, "Player not found");
            }

            DocumentSession.Delete(player);
            return RedirectToAction("Index");
        }

        public class CreatePlayerViewModel
        {
            public CreatePlayerViewModel()
            {
                Name = string.Empty;
                Nickname = string.Empty;
                Email = string.Empty;
                Status = Player.Status.Active;
                Roles = new string[0];
            }

            public CreatePlayerViewModel(Player player)
            {
                Name = player.Name;
                Nickname = player.Nickname;
                Email = player.Email;
                Status = player.PlayerStatus;
                PersonalNumber = player.PersonalNumber;
                Roles = player.Roles;
            }

            [Required]
            public string Name { get; set; }

            public string? Nickname { get; set; }

            [Required]
            public string Email { get; set; }

            [Required]
            public Player.Status Status { get; set; }

            public int? PersonalNumber { get; set; }

            public MultiSelectList RolesList
            {
                get
                {
                    WebsiteRoles.WebsiteRole[] roles = WebsiteRoles.UserGroup()
                                            .Except(WebsiteRoles.PlayerGroup())
                                            .OrderBy(x => x.Description)
                                            .ToArray();
                    MultiSelectList rolesList =
                        new(roles, "Name", "Description");
                    return rolesList;
                }
            }

            [Required]
            [Display(Name = "Funktioner:")]
            public string[] Roles { get; set; }
        }
    }
}
