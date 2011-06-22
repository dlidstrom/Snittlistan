using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SnittListan.Events;

namespace SnittListan.Models
{
	public class User
	{
		private const string ConstantSalt = "CheFe2ra8en9SW";
		private Guid passwordSalt;

		public User(string firstName, string lastName, string userName, string email, string password)
		{
			FirstName = firstName;
			LastName = lastName;
			UserName = userName;
			Email = email;
			HashedPassword = ComputeHashedPassword(password);
		}

		public string Id { get; set; }
		public string UserName { get; private set; }
		public bool IsActive { get; set; }

		// Other user properties
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }

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