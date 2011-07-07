using System.Linq;
using Raven.Client;
using SnittListan.Models;

namespace SnittListan.Helpers
{
	public static class DocumentSessionExtensions
	{
		public static IQueryable<User> FindUserByEmail(this IDocumentSession sess, string email)
		{
			return sess.Query<User>().Where(u => u.Email == email);
		}
	}
}