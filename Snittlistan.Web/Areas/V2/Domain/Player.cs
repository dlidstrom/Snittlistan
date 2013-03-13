using System;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class Player
    {
        public Player(string name, string email, bool isSupporter)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (email == null) throw new ArgumentNullException("email");
            this.Name = name;
            this.Email = email;
            this.IsSupporter = isSupporter;
        }

        public string Id { get; set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public bool IsSupporter { get; private set; }

        public void SetName(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            this.Name = name;
        }

        public void SetEmail(string email)
        {
            if (email == null) throw new ArgumentNullException("email");
            this.Email = email;
        }

        public void SetIsSupporter(bool isSupporter)
        {
            this.IsSupporter = isSupporter;
        }
    }
}