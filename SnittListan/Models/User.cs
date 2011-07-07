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

		public User(string firstName, string lastName, string email, string password)
		{
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			HashedPassword = ComputeHashedPassword(password);
		}

		public string Id { get; private set; }
		public string Email { get; private set; }
		public bool IsActive { get; private set; }
		public string ActivationKey { get; set; }

		// Other user properties
		public string FirstName { get; private set; }
		public string LastName { get; private set; }

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

		public void Initialize()
		{
			ActivationKey = Guid.NewGuid().ToString();
			DomainEvent.Raise(new NewUserCreatedEvent { User = this });
		}

		public void Activate()
		{
			IsActive = true;
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