using System.Linq;
using Raven.Client;
using Snittlistan.Models;

namespace Snittlistan.Helpers
{
	public static class DocumentSessionExtensions
	{
		public static IQueryable<User> FindUserByEmail(this IDocumentSession sess, string email)
		{
			return sess.Query<User>().Where(u => u.Email == email);
		}

		public static IQueryable<User> FindUserByActivationKey(this IDocumentSession sess, string key)
		{
			return sess.Query<User>().Where(u => u.ActivationKey == key);
		}
	}
}