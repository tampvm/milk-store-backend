using AutoMapper;
using MilkStore.Domain.Entities;
using MilkStore.Domain.Enums;
using MilkStore.Repository.Common;
using MilkStore.Service.Models.ViewModels.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
            CreateMap<RegisterDTO, Account>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => UserStatusEnums.Active.ToString()))
                .ForMember(dest => dest.TotalPoints, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)).ReverseMap();
        }
    }
}
