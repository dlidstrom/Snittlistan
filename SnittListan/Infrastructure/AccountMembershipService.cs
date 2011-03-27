namespace SnittListan.Infrastructure
{
	using System;
	using System.Web.Security;
	using SnittListan.Services;

	/// <summary>
	/// Implementation of the membership service that forwards to MembershipProvider.
	/// </summary>
	public class AccountMembershipService : IMembershipService
	{
		/// <summary>
		/// Membership provider.
		/// </summary>
		private readonly MembershipProvider provider;

		/// <summary>
		/// Initializes a new instance of the AccountMembershipService class.
		/// </summary>
		public AccountMembershipService()
			: this(null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the AccountMembershipService class.
		/// </summary>
		/// <param name="provider">Membership provider to use.</param>
		public AccountMembershipService(MembershipProvider provider)
		{
			this.provider = provider ?? Membership.Provider;
		}

		/// <summary>
		/// Gets the minimum password length.
		/// </summary>
		public int MinPasswordLength
		{
			get
			{
				return this.provider.MinRequiredPasswordLength;
			}
		}

		/// <summary>
		/// Validates user name and password.
		/// </summary>
		/// <param name="userName">User name.</param>
		/// <param name="password">User password.</param>
		/// <returns>A value indicating whether user was validated or not.</returns>
		public bool ValidateUser(string userName, string password)
		{
			if (string.IsNullOrEmpty(userName))
			{
				throw new ArgumentException("Value cannot be null or empty.", "userName");
			}

			if (string.IsNullOrEmpty(password))
			{
				throw new ArgumentException("Value cannot be null or empty.", "password");
			}

			return this.provider.ValidateUser(userName, password);
		}

		public MembershipCreateStatus CreateUser(string userName, string password, string email)
		{
			if (String.IsNullOrEmpty(userName))
				throw new ArgumentException("Value cannot be null or empty.", "userName");
			if (String.IsNullOrEmpty(password))
				throw new ArgumentException("Value cannot be null or empty.", "password");
			if (String.IsNullOrEmpty(email))
				throw new ArgumentException("Value cannot be null or empty.", "email");

			MembershipCreateStatus status;
			provider.CreateUser(userName, password, email, null, null, true, null, out status);
			return status;
		}

		public bool ChangePassword(string userName, string oldPassword, string newPassword)
		{
			if (String.IsNullOrEmpty(userName))
				throw new ArgumentException("Value cannot be null or empty.", "userName");
			if (String.IsNullOrEmpty(oldPassword))
				throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
			if (String.IsNullOrEmpty(newPassword))
				throw new ArgumentException("Value cannot be null or empty.", "newPassword");

			// The underlying ChangePassword() will throw an exception rather
			// than return false in certain failure scenarios.
			try
			{
				MembershipUser currentUser = provider.GetUser(userName, true /* userIsOnline */);
				return currentUser.ChangePassword(oldPassword, newPassword);
			}
			catch (ArgumentException)
			{
				return false;
			}
			catch (MembershipPasswordException)
			{
				return false;
			}
		}
	}
}