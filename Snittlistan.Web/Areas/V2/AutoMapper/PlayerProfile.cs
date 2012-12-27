namespace Snittlistan.Web.Infrastructure.AutoMapper.Profiles
{
    using global::AutoMapper;

    using Snittlistan.Web.Areas.V2.Models;
    using Snittlistan.Web.Areas.V2.ViewModels;

    public class PlayerProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Player, PlayerViewModel>();

            Mapper.CreateMap<Player, CreatePlayerViewModel>();
        }
    }
}