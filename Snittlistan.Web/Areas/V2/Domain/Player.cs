using System;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class Player
    {
        public Player(string name, string email, Status playerStatus, int personalNumber, string nickname)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            PlayerStatus = playerStatus;
            PersonalNumber = personalNumber;
            Nickname = nickname ?? name;
        }

        public enum Status
        {
            Active,
            Supporter,
            Inactive
        }

        public string Id { get; set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public Status PlayerStatus { get; private set; }

        public int PersonalNumber { get; private set; }

        public string Nickname { get; private set; }

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

        public void SetNickname(string nickname)
        {
            Nickname = nickname ?? Name;
        }
    }
}