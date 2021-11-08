#nullable enable

namespace Snittlistan.Web.Areas.V2.Domain
{
    using System;
    using System.Diagnostics;
    using Infrastructure.Bits.Contracts;

    [DebuggerDisplay("{Id} {Name} {Email}")]
    public class Player
    {
        public Player(
            string name,
            string email,
            Status playerStatus,
            int personalNumber,
            string? nickname,
            string[] roles,
            PlayerItem? playerItem = null,
            string? uniqueId = null,
            bool? hidden = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Email = email; // allow null
            PlayerStatus = playerStatus;
            PersonalNumber = (personalNumber != 0 ? personalNumber : playerItem?.GetPersonalNumber()) ?? 0;
            Nickname = string.IsNullOrEmpty(nickname) ? name : nickname;
            Roles = roles ?? new string[0];

            // nullable fields
            PlayerItem = playerItem;
            Hidden = hidden ?? false;
            UniqueId = uniqueId ?? Guid.NewGuid().ToString();
        }

        public enum Status
        {
            Active,
            Supporter,
            Inactive
        }

        public string Id { get; set; } = null!;

        public string Name { get; private set; }

        public string Email { get; private set; }

        public Status PlayerStatus { get; private set; }

        public int PersonalNumber { get; private set; }

        public string? Nickname { get; private set; }

        public string[] Roles { get; private set; }

        public PlayerItem? PlayerItem { get; set; }

        public bool Hidden { get; }

        public string UniqueId { get; }

        public void SetName(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public void SetEmail(string email)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }

        public void SetStatus(Status status)
        {
            PlayerStatus = status;
        }

        public void SetPersonalNumber(int personalNumber)
        {
            PersonalNumber = personalNumber;
        }

        public void SetNickname(string? nickname)
        {
            Nickname = nickname ?? Name;
        }

        public void SetRoles(string[] roles)
        {
            Roles = roles ?? throw new ArgumentNullException(nameof(roles));
        }
    }
}
