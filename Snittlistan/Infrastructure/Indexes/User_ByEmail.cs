namespace Snittlistan.Infrastructure.Indexes
{
    using System.Linq;
    using Models;
    using Raven.Client.Indexes;

    public class User_ByEmail : AbstractIndexCreationTask<User>
    {
        public User_ByEmail()
        {
            Map = users => from user in users
                           select new { user.Email, user.ActivationKey };
        }
    }
}