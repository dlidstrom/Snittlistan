using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SnittListan.Models
{
	public class User
	{
		private const string ConstantSalt = "CheFe2ra8en9SW";
		private Guid passwordSalt;

		public string Id { get; set; }
		public string UserName { get; set; }

		// Other user properties
		public string FullName { get; set; }

		private string HashedPassword { get; set; }

		private Guid PasswordSalt
		{
			get
			{
				if (passwordSalt == default(Guid))
					passwordSalt = Guid.NewGuid();
				return passwordSalt;
			}

			set { passwordSalt = value; }
		}

		public void SetPassword(string password)
		{
			HashedPassword = ComputeHashedPassword(password);
		}

		public bool ValidatePassword(string somePassword)
		{
			return HashedPassword == ComputeHashedPassword(somePassword);
		}

		private string ComputeHashedPassword(string password)
		{
			string hashedPassword;
			using (var sha = SHA256.Create())
			{
				var saltPerUser = Id;
				var computedHash = sha.ComputeHash(
					PasswordSalt.ToByteArray().Concat(
						Encoding.Unicode.GetBytes(saltPerUser + password + ConstantSalt))
						.ToArray());

				hashedPassword = Convert.ToBase64String(computedHash);
			}

			return hashedPassword;
		}
	}
}