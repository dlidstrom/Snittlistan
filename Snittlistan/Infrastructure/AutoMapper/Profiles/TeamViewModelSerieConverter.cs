using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Snittlistan.Models;
using Snittlistan.ViewModels;

namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
	public class TeamViewModelSerieConverter : ITypeConverter<TeamViewModel.Serie, Serie>
	{
		public Serie Convert(ResolutionContext context)
		{
			var vm = (TeamViewModel.Serie)context.SourceValue;
			return new Serie(new List<Table>
			{
					vm.Table1.MapTo<Table>(),
					vm.Table2.MapTo<Table>(),
					vm.Table3.MapTo<Table>(),
					vm.Table4.MapTo<Table>()
			});
		}
	}
}