using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2.AutoMapper
{
    using global::AutoMapper;
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