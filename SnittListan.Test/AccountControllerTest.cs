namespace SnittListan.Test
{
	using System;
	using Moq;
	using NUnit.Framework;
	using SnittListan.Controllers;
	using SnittListan.Models;
	using SnittListan.Services;

	[TestFixture]
	public class AccountControllerTest
	{
		[Test]
		public void ReturnUrlCanBeNull()
		{
			// Arrange
			var ac = new AccountController();
			ac.MembershipService = Mock.Of<IMembershipService>();

			// Act
			ac.LogOn(new LogOnModel { UserName = "User", Password = "Pwd" }, null);
		}
	}
}
