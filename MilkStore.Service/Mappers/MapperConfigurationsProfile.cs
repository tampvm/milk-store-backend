using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using MilkStore.Domain.Entities;
using MilkStore.Domain.Enums;
using MilkStore.Repository.Common;
using MilkStore.Service.Models.ViewModels.AccountViewModels;
using MilkStore.Service.Models.ViewModels.AuthViewModels;
using MilkStore.Service.Models.ViewModels.BrandViewModels;
using MilkStore.Service.Models.ViewModels.RoleViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MilkStore.Service.Models.ViewModels.AccountViewModels.UserRolesDTO;

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
				.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => GenderEnums.Unknown.ToString()))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => UserStatusEnums.Active.ToString()))
				.ForMember(dest => dest.TotalPoints, opt => opt.MapFrom(src => 0))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)).ReverseMap();

			CreateMap<SocialLoginDTO, Account>()
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
				.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => GenderEnums.Unknown.ToString()))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => UserStatusEnums.Active.ToString()))
				.ForMember(dest => dest.TotalPoints, opt => opt.MapFrom(src => 0))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)).ReverseMap();

			CreateMap<Account, ViewListUserDTO>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
				.ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
				.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => src.LastLogin))
				.ForMember(dest => dest.TotalPoints, opt => opt.MapFrom(src => src.TotalPoints))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt)).ReverseMap();

			CreateMap<Account, ViewUserRolesDTO>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
			//.ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name).ToList()));

			CreateMap<Role, ViewListRoleDTO>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)).ReverseMap();

			CreateMap<UpdateRoleDTO, Role>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoleName))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description)).ReverseMap();


			CreateMap<Brand, ViewListBrandDTO>()
				.ForMember(dest => dest.Id, otp => otp.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, otp => otp.MapFrom(src => src.Name))
				.ForMember(dest => dest.BrandOrigin, otp => otp.MapFrom(src => src.BrandOrigin))
				.ForMember(dest => dest.Description, otp => otp.MapFrom(src => src.Description))
				.ForMember(dest => dest.Active, otp => otp.MapFrom(src => src.Active))
				.ForMember(dest => dest.ImageId, otp => otp.MapFrom(src => src.ImageId))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)).ReverseMap();

			CreateMap<CreateBrandDTO, Brand>()
				.ForMember(dest => dest.Name, otp => otp.MapFrom(src => src.Name))
				.ForMember(dest => dest.BrandOrigin, otp => otp.MapFrom(src => src.BrandOrigin))
				.ForMember(dest => dest.Description, otp => otp.MapFrom(src => src.Description))
				.ForMember(dest => dest.Active, otp => otp.MapFrom(src => true))
				.ForMember(dest => dest.ImageId, otp => otp.MapFrom(src => src.ImageId)).ReverseMap();
		}
	}
}
