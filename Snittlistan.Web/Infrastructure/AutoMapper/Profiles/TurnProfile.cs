namespace Snittlistan.Web.Infrastructure.AutoMapper.Profiles
{
    using Snittlistan.Web.Models;
    using Snittlistan.Web.ViewModels;

    public class TurnProfile : global::AutoMapper.Profile
    {
        protected override void Configure()
        {
            global::AutoMapper.Mapper.CreateMap<TurnModel, TurnViewModel>();
        }
    }
}