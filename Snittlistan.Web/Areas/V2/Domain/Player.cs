using System;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class Player
    {
        public Player(string name, string email, Status status)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (email == null) throw new ArgumentNullException("email");
            Name = name;
            Email = email;
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

        public void SetName(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            Name = name;
        }

        public void SetEmail(string email)
        {
            if (email == null) throw new ArgumentNullException("email");
            Email = email;
        }

        public void SetStatus(Status status)
        {
            PlayerStatus = status;
        }
    }
}