namespace Snittlistan.Web.Areas.V2.AutoMapper
{
    using Snittlistan.Web.Areas.V2.Models;
    using Snittlistan.Web.Areas.V2.ViewModels;

    using global::AutoMapper;

    public class AbsenceProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Absence, AbsenceViewModel>();
            Mapper.CreateMap<Absence, CreateAbsenceViewModel>();
        }
    }
}