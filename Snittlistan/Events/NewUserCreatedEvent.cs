using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnittListan.Models;

namespace SnittListan.Events
{
	public class NewUserCreatedEvent : IDomainEvent
	{
		public User User { get; set; }
	}
}
