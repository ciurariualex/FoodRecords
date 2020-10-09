using AutoMapper;
using Core.Common.Api.Authentication;
using Core.Common.Api.Food;
using Core.Common.Api.Registration;
using Core.Common.Api.User;
using Core.Data.Entities;
using Core.Data.Entities.Identity;
using Core.Data.NotConfigured;
using Newtonsoft.Json;

namespace Core.Utils.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserAdd, ApplicationUser>();
            CreateMap<UserEdit, ApplicationUser>()
                .ForMember(dest => dest.ContactJson, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Contact)))
                .ForMember(dest => dest.SettingsJson, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Settings)))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ApplicationUser, UserView>()
                .ForMember(
                    dest => dest.Contact,
                    opt => opt.MapFrom(
                        src =>
                        string.IsNullOrEmpty(src.ContactJson)
                        ? null
                        : JsonConvert.DeserializeObject<Contact>(src.ContactJson)))
                .ForMember(
                    dest => dest.Settings,
                    opt => opt.MapFrom(
                        src =>
                        string.IsNullOrEmpty(src.SettingsJson)
                        ? null
                        : JsonConvert.DeserializeObject<Settings>(src.SettingsJson)))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
            CreateMap<FoodManage, Food>();
            CreateMap<FoodEdit, Food>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Food, FoodView>();
            CreateMap<ApplicationUser, AuthenticationView>();
            CreateMap<RegistrationDto, ApplicationUser>();
            CreateMap<ApplicationUser, RegistrationView>();
            CreateMap<UserRoleEdit, ApplicationUser>();
        }
    }
}
