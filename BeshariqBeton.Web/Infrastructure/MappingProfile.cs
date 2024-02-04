using AutoMapper;
using BeshariqBeton.Common.Entities;
using BeshariqBeton.Common.Models.Parameters;
using BeshariqBeton.Web.ViewModels;

namespace BeshariqBeton.Web.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(u => u.StandardPermissions, o => o.Ignore())
                .ReverseMap()
                .ForMember(u => u.StandardPermissions, o => o.Ignore());

            CreateMap<PriceSettingsViewModel, ConcreteConsistancesParameters>().ReverseMap();
            CreateMap<PriceSettingsViewModel, ConcreteConsistancesPricesParameters>().ReverseMap();
            CreateMap<PriceSettingsViewModel, DistancePriceParameters>().ReverseMap();
            CreateMap<PriceSettingsViewModel, ConcreteTypesPricesParameters>().ReverseMap();
            CreateMap<PriceSettingsViewModel, SumpPiecesPricesParameters>().ReverseMap();
            CreateMap<StorageViewModel, StorageParameters>().ReverseMap();
            CreateMap<ClientViewModel, Client>().ReverseMap();
            CreateMap<SaleViewModel, Sale>().ReverseMap();
            CreateMap<StorageViewModel, Storage>().ReverseMap();
        }
    }
}
