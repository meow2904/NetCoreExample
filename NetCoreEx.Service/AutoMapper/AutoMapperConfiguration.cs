using AutoMapper;
using NetCoreEx.Model.Entities;
using NetCoreEx.Utilities.ViewModels;

namespace NetCoreEx.Service.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration Configure()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AppUser, AppUserViewModel>();
                cfg.CreateMap<AppUserViewModel, AppUser>();
                cfg.CreateMap<FormDemo, FormDemoViewModel>();
            });
        }
    }
}