using AutoMapper;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ViewModels;

namespace Snittlistan.Web.Areas.V2.AutoMapper
{
    public class AbsenceProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Absence, CreateAbsenceViewModel>();
        }
    }
}