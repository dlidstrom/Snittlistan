using System.Linq;
using Raven.Client;
using Snittlistan.Infrastructure.Indexes;
using Snittlistan.Models;

namespace Snittlistan.Helpers
{
	public static class DocumentSessionExtensions
	{
		public static User FindUserByEmail(this IDocumentSession sess, string email)
		{
            return sess.Query<User>().FirstOrDefault(u => u.Email == email);
		}

		public static IQueryable<User> FindUserByActivationKey(this IDocumentSession sess, string key)
		{
			return sess.Query<User>().Where(u => u.ActivationKey == key);
		}

		public static IQueryable<Match> FindByBitsId(this IDocumentSession sess, int id)
		{
			return sess.Query<Match, Match_ByBitsMatchId>()
				.Customize(c => c.WaitForNonStaleResultsAsOfNow())
				.Where(m => m.BitsMatchId == id);
		}
	}
}