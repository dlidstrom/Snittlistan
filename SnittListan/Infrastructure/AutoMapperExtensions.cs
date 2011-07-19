using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;

namespace SnittListan.Infrastructure
{
	public static class AutoMapperExtensions
	{
		public static List<TResult> MapTo<TResult>(this IEnumerable self)
		{
			if (self == null)
				throw new ArgumentNullException("self");

			return (List<TResult>)Mapper.Map(self, self.GetType(), typeof(List<TResult>));
		}
	}
}