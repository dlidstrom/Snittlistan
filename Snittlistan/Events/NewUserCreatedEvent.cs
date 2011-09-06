using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snittlistan.Models;

namespace Snittlistan.Events
{
	public class NewUserCreatedEvent : IDomainEvent
	{
		public User User { get; set; }
	}
}
