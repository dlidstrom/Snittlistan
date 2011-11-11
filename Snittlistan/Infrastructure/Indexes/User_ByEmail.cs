namespace Snittlistan.Infrastructure.Indexes
{
    using System.Linq;
    using Raven.Client.Indexes;
    using Snittlistan.Models;

    public class User_ByEmail : AbstractIndexCreationTask<User>
    {
        public User_ByEmail()
        {
            Map = users => from user in users
                           select new { user.Email };
        }
    }
}