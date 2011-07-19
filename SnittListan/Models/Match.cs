using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnittListan.Models
{
	public class Match
	{
		public string Id { get; private set; }
		public string Place { get; set; }
		public DateTime Date { get; set; }

		public List<Serie> Series { get; set; }
	}
}