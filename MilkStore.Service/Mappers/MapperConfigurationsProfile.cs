using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using MilkStore.Domain.Entities;
using MilkStore.Domain.Enums;
using MilkStore.Repository.Common;
using MilkStore.Service.Models.ViewModels.AccountViewModels;
using MilkStore.Service.Models.ViewModels.AuthViewModels;
using MilkStore.Service.Models.ViewModels.BrandViewModels;
using MilkStore.Service.Models.ViewModels.FollowBrandViewModels;
using MilkStore.Service.Models.ViewModels.PointViewModels;
using MilkStore.Service.Models.ViewModels.BogViewModel;
using MilkStore.Service.Models.ViewModels.RoleViewModels;
using MilkStore.Service.Models.ViewModels.VoucherViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkStore.Service.Models.ViewModels.CategoryViewModel;
using MilkStore.Service.Models.ViewModels.BlogCategoryViewModels;
using MilkStore.Service.Models.ViewModels.ProductViewModels;
using MilkStore.Service.Models.ViewModels.AgeRangeViewModels;
using MilkStore.Service.Models.ViewModels.ProductTypeViewModels;
using MilkStore.Service.Models.ViewModels.ProductImageViewModels;
using MilkStore.Service.Models.ViewModels.InteractionModels;
using MilkStore.Service.Models.ViewModels.AddressViewModels;

namespace MilkStore.Service.Mappers
{
	public class MapperConfigurationsProfile : Profile
	{
		public MapperConfigurationsProfile()
		{
			CreateMap(typeof(Pagination<>), typeof(Pagination<>));

            #region Account
            CreateMap<RegisterDTO, Account>()
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
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
				.ForMember(dest => dest.GoogleEmail, opt => opt.MapFrom(src => src.GoogleEmail))
				.ForMember(dest => dest.FacebookEmail, opt => opt.MapFrom(src => src.FacebookEmail))
				.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => src.LastLogin))
				.ForMember(dest => dest.TotalPoints, opt => opt.MapFrom(src => src.TotalPoints))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy)).ReverseMap();

			CreateMap<Account, ViewUserProfileDTO>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
				.ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
				.ForMember(dest => dest.GoogleEmail, opt => opt.MapFrom(src => src.GoogleEmail))
				.ForMember(dest => dest.FacebookEmail, opt => opt.MapFrom(src => src.FacebookEmail))
				.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => src.LastLogin))
				.ForMember(dest => dest.Point, opt => opt.MapFrom(src => src.TotalPoints))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy)).ReverseMap();

			CreateMap<UpdateUserProfileDTO, Account>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
				.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender)).ReverseMap();

			CreateMap<Account, ViewUserRolesDTO>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
			//.ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name).ToList()));
			#endregion

			#region Address
			CreateMap<Address, ViewUserAddressDTO>()
                .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.AddressLine, opt => opt.MapFrom(src => src.AddressLine))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District))
                .ForMember(dest => dest.Ward, opt => opt.MapFrom(src => src.Ward))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault)).ReverseMap();

            CreateMap<CreateAddressDTO, Address>()
				.ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
				.ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
				.ForMember(dest => dest.AddressLine, opt => opt.MapFrom(src => src.AddressLine))
				.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
				.ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District))
				.ForMember(dest => dest.Ward, opt => opt.MapFrom(src => src.Ward))
				.ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault)).ReverseMap();

            CreateMap<UpdateAddressDTO, Address>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AddressId))
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.AddressLine, opt => opt.MapFrom(src => src.AddressLine))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District))
                .ForMember(dest => dest.Ward, opt => opt.MapFrom(src => src.Ward))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault)).ReverseMap();
            #endregion

            #region Role
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
            #endregion

            #region Brand
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
				.ForMember(dest => dest.Active, otp => otp.MapFrom(src => src.Active)).ReverseMap();

			CreateMap<UpdateBrandDTO, Brand>()
				.ForMember(dest => dest.Id, otp => otp.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, otp => otp.MapFrom(src => src.Name))
				.ForMember(dest => dest.BrandOrigin, otp => otp.MapFrom(src => src.BrandOrigin))
				.ForMember(dest => dest.Description, otp => otp.MapFrom(src => src.Description))
				.ForMember(dest => dest.Active, otp => otp.MapFrom(src => src.Active)).ReverseMap();

			CreateMap<Brand, ViewBrandDetailDTO>()
				.ForMember(dest => dest.Id, otp => otp.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, otp => otp.MapFrom(src => src.Name))
				.ForMember(dest => dest.BrandOrigin, otp => otp.MapFrom(src => src.BrandOrigin))
				.ForMember(dest => dest.Description, otp => otp.MapFrom(src => src.Description))
				.ForMember(dest => dest.Active, otp => otp.MapFrom(src => src.Active))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)).ReverseMap();
			#endregion

			#region FollowBrand
			CreateMap<FollowBrand, ViewListFollowBrandDTO>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.IsFollow, opt => opt.MapFrom(src => src.IsFollow))
				.ForMember(dest => dest.FollowedAt, opt => opt.MapFrom(src => src.FollowedAt))
				.ForMember(dest => dest.UnfollowedAt, opt => opt.MapFrom(src => src.UnfollowedAt))
				.ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
				.ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId)).ReverseMap();

			CreateMap<Pagination<FollowBrand>, Pagination<ViewFollowBrandByUserDTO>>()
				.ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

			CreateMap<FollowBrand, ViewFollowBrandByUserDTO>();

			CreateMap<UserFollowsBrandDTO, FollowBrand>()
				.ForMember(dest => dest.IsFollow, opt => opt.MapFrom(src => true))
				.ForMember(dest => dest.FollowedAt, opt => opt.MapFrom(src => src.FollowedAt))
				.ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
				.ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId)).ReverseMap();

			#endregion

			#region Voucher
			CreateMap<Voucher, ViewListVoucherDTO>()
				.ForMember(dest => dest.Id, otp => otp.MapFrom(src => src.Id))
				.ForMember(dest => dest.Code, otp => otp.MapFrom(src => src.Code))
				.ForMember(dest => dest.Description, otp => otp.MapFrom(src => src.Description))
				.ForMember(dest => dest.DiscountType, otp => otp.MapFrom(src => src.DiscountType))
				.ForMember(dest => dest.DiscountValue, otp => otp.MapFrom(src => src.DiscountValue))
				.ForMember(dest => dest.StartDate, otp => otp.MapFrom(src => src.StartDate))
				.ForMember(dest => dest.EndDate, otp => otp.MapFrom(src => src.EndDate))
				.ForMember(dest => dest.UsageLimit, otp => otp.MapFrom(src => src.UsageLimit))
				.ForMember(dest => dest.UsedCount, otp => otp.MapFrom(src => src.UsedCount))
				.ForMember(dest => dest.MiniumOrderValue, otp => otp.MapFrom(src => src.MiniumOrderValue))
				.ForMember(dest => dest.Status, otp => otp.MapFrom(src => src.Status))

				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)).ReverseMap();

			CreateMap<Voucher, ViewDetailVoucherDTO>()
				.ForMember(dest => dest.Id, otp => otp.MapFrom(src => src.Id))
				.ForMember(dest => dest.Code, otp => otp.MapFrom(src => src.Code))
				.ForMember(dest => dest.Description, otp => otp.MapFrom(src => src.Description))
				.ForMember(dest => dest.DiscountType, otp => otp.MapFrom(src => src.DiscountType))
				.ForMember(dest => dest.DiscountValue, otp => otp.MapFrom(src => src.DiscountValue))
				.ForMember(dest => dest.StartDate, otp => otp.MapFrom(src => src.StartDate))
				.ForMember(dest => dest.EndDate, otp => otp.MapFrom(src => src.EndDate))
				.ForMember(dest => dest.UsageLimit, otp => otp.MapFrom(src => src.UsageLimit))
				.ForMember(dest => dest.UsedCount, otp => otp.MapFrom(src => src.UsedCount))
				.ForMember(dest => dest.MiniumOrderValue, otp => otp.MapFrom(src => src.MiniumOrderValue))
				.ForMember(dest => dest.Status, otp => otp.MapFrom(src => src.Status)).ReverseMap();

			CreateMap<CreateVoucherDTO, Voucher>()
				.ForMember(dest => dest.Code, otp => otp.MapFrom(src => src.Code))
				.ForMember(dest => dest.Description, otp => otp.MapFrom(src => src.Description))
				.ForMember(dest => dest.DiscountType, otp => otp.MapFrom(src => src.DiscountType))
				.ForMember(dest => dest.DiscountValue, otp => otp.MapFrom(src => src.DiscountValue))
				.ForMember(dest => dest.StartDate, otp => otp.MapFrom(src => src.StartDate))
				.ForMember(dest => dest.EndDate, otp => otp.MapFrom(src => src.EndDate))
				.ForMember(dest => dest.UsageLimit, otp => otp.MapFrom(src => src.UsageLimit))
				.ForMember(dest => dest.MiniumOrderValue, otp => otp.MapFrom(src => src.MiniumOrderValue))
				.ForMember(dest => dest.Status, otp => otp.MapFrom(src => src.Status)).ReverseMap();

			CreateMap<UpdateVoucherDTO, Voucher>()
				.ForMember(dest => dest.Id, otp => otp.MapFrom(src => src.Id))
				.ForMember(dest => dest.Code, otp => otp.MapFrom(src => src.Code))
				.ForMember(dest => dest.Description, otp => otp.MapFrom(src => src.Description))
				.ForMember(dest => dest.DiscountType, otp => otp.MapFrom(src => src.DiscountType))
				.ForMember(dest => dest.DiscountValue, otp => otp.MapFrom(src => src.DiscountValue))
				.ForMember(dest => dest.StartDate, otp => otp.MapFrom(src => src.StartDate))
				.ForMember(dest => dest.EndDate, otp => otp.MapFrom(src => src.EndDate))
				.ForMember(dest => dest.UsageLimit, otp => otp.MapFrom(src => src.UsageLimit))
				//.ForMember(dest => dest.UsedCount, otp => otp.MapFrom(src => src.UsedCount))
				.ForMember(dest => dest.MiniumOrderValue, otp => otp.MapFrom(src => src.MiniumOrderValue))
				.ForMember(dest => dest.Status, otp => otp.MapFrom(src => src.Status)).ReverseMap();

			#endregion

			#region Point
			CreateMap<Point, ViewListPointDTO>()
				.ForMember(dest => dest.Id, otp => otp.MapFrom(src => src.Id))
				.ForMember(dest => dest.Points, otp => otp.MapFrom(src => src.Points))
				.ForMember(dest => dest.TransactionType, otp => otp.MapFrom(src => src.TransactionType))
				.ForMember(dest => dest.AccountId, otp => otp.MapFrom(src => src.AccountId))
				.ForMember(dest => dest.OrderId, otp => otp.MapFrom(src => src.OrderId)).ReverseMap();

			CreateMap<PointsTradingDTO, Point>()
				.ForMember(dest => dest.Points, opt => opt.MapFrom(src => src.Points))
				.ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType))
				.ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
				.ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId)).ReverseMap();

			#endregion

            #region Blog
            // Mapping from Post to ViewBlogModel
            CreateMap<Post, ViewBlogModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.BlogImg, opt => opt.MapFrom(src => src.PostImages.FirstOrDefault().ImageId))
                .ForMember(dest => dest.createAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.createBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.updateBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.updateAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.deleteAt, opt => opt.MapFrom(src => src.DeletedAt))
                .ForMember(dest => dest.deleteBy, opt => opt.MapFrom(src => src.DeletedBy))
                .ForMember(dest => dest.isDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ReverseMap();
            CreateMap<Post, ViewBlogModel>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
             .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
             .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
             .ForMember(dest => dest.createAt, opt => opt.MapFrom(src => src.CreatedAt))
             .ForMember(dest => dest.createBy, opt => opt.MapFrom(src => src.CreatedBy))
             .ForMember(dest => dest.updateBy, opt => opt.MapFrom(src => src.UpdatedBy))
             .ForMember(dest => dest.updateAt, opt => opt.MapFrom(src => src.UpdatedAt))
             .ForMember(dest => dest.deleteAt, opt => opt.MapFrom(src => src.DeletedAt))
             .ForMember(dest => dest.BlogImg, opt => opt.Ignore())
             .ForMember(dest => dest.deleteBy, opt => opt.MapFrom(src => src.DeletedBy))
             .ForMember(dest => dest.isDeleted, opt => opt.MapFrom(src => src.IsDeleted))
             .ReverseMap();
			#region Blog
			// Mapping from Post to ViewBlogModel
			CreateMap<Post, ViewBlogModel>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.createAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.createBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.updateBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.updateAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.deleteAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.deleteBy, opt => opt.MapFrom(src => src.DeletedBy))
				.ForMember(dest => dest.isDeleted, opt => opt.MapFrom(src => src.IsDeleted))
				.ReverseMap();

            // Mapping from Post to CreateBlogDTO
            CreateMap<Post, CreateBlogDTO>()
                 .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                 .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                 .ReverseMap();
            CreateMap<CreateBlogDTO, Post>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true));




            // Mapping from Post to CreateBlogDTO
            CreateMap<Post, CreateBlogDTO>()
                 .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                 .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
				 .ReverseMap();
			CreateMap<CreateBlogDTO, Post>()
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => true));

            // Mapping from UpdateBlogDTO to Post
            CreateMap<UpdateBlogDTO, Post>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()) // Assuming CreatedAt should not be updated
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Assuming CreatedAt should not be updated
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Assuming CreatedBy should not be updated
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore()) // Assuming DeletedAt should not be updated
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore()) // Assuming DeletedBy should not be updated
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore()) // Assuming IsDeleted should not be updated
                .ReverseMap();
			CreateMap<CreateBlogImgDTO, PostImage>()
				.ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
				.ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
				.ReverseMap();
			CreateMap<PostImage, CreateBlogImgDTO>()
				.ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
				.ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
				.ReverseMap();
            #endregion
			// Mapping from UpdateBlogDTO to Post
			CreateMap<UpdateBlogDTO, Post>()
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()) // Assuming CreatedAt should not be updated
				.ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Assuming CreatedAt should not be updated
				.ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Assuming CreatedBy should not be updated
				.ForMember(dest => dest.DeletedAt, opt => opt.Ignore()) // Assuming DeletedAt should not be updated
				.ForMember(dest => dest.DeletedBy, opt => opt.Ignore()) // Assuming DeletedBy should not be updated
				.ForMember(dest => dest.IsDeleted, opt => opt.Ignore()) // Assuming IsDeleted should not be updated
				.ReverseMap();
			#endregion

			#region Category
			CreateMap<Category, ViewListCategoryDTO>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
				.ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdateBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeleteAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeleteBy, opt => opt.MapFrom(src => src.DeletedBy))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
				.ReverseMap();
			CreateMap<ViewListCategoryDTO, Category>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreateAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreateBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdateAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdateBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeleteAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeleteBy))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
				.ReverseMap();
			//Map for create category
			CreateMap<CreateCategoryDTO, Category>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))

				.ReverseMap();
			//Map for update category
			CreateMap<UpdateCategoryDTO, Category>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))


				.ReverseMap();
			CreateMap<Category, UpdateCategoryDTO>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))


				.ReverseMap();
			#endregion

			#region BlogCategory
			// Mapping from BlogCategory to ViewBlogCategoryModel
			CreateMap<PostCategory, GetBlogCategoryDTO>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PostId))
				.ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
				.ReverseMap();
			// Mapping from CreateBlogCategoryDTO to BlogCategory
			CreateMap<CreateBlogCategoryDTO, PostCategory>()
				.ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
				.ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
				.ReverseMap();
			// Mapping from BlogCategory to CreateBlogCategoryDTO
			CreateMap<PostCategory, CreateBlogCategoryDTO>()
				.ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
				.ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
				.ReverseMap();
			// Mapping from UpdateBlogCategoryDTO to BlogCategory
			CreateMap<UpdateBlogCategoryDTO, PostCategory>()

				.ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
				.ReverseMap();
			// Mapping from BlogCategory to UpdateBlogCategoryDTO
			CreateMap<PostCategory, UpdateBlogCategoryDTO>()

				.ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
				.ReverseMap();
			#endregion

            #region Interaction
            CreateMap<LikePost, GetBlogLike>()
				.ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
				.ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
				.ForMember(dest => dest.IsLike, opt => opt.MapFrom(src => src.IsLike))
				.ForMember(dest => dest.LikeAt, opt => opt.MapFrom(src => src.LikeAt))
				.ForMember(dest => dest.UnLikeAt, opt => opt.MapFrom(src => src.UnLikeAt))
				.ReverseMap();

            CreateMap<LikePost, GetBlogLikeByUserId>()
				.ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
				.ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
				.ForMember(dest => dest.IsLike, opt => opt.MapFrom(src => src.IsLike))
				.ForMember(dest => dest.LikeAt, opt => opt.MapFrom(src => src.LikeAt))
				.ForMember(dest => dest.UnLikeAt, opt => opt.MapFrom(src => src.UnLikeAt))
				.ReverseMap();

			CreateMap<UpdateLike, LikePost>()
				.ForMember(dest => dest.IsLike, opt => opt.MapFrom(src => src.IsLike))
				.ForMember(dest => dest.LikeAt, opt => opt.MapFrom(src => src.LikeAt))
				.ForMember(dest => dest.UnLikeAt, opt => opt.MapFrom(src => src.UnLikeAt))
				.ReverseMap();

			//create like
			CreateMap<CreateLike, LikePost>()
				
				.ForMember(dest => dest.IsLike, opt => opt.MapFrom(src => src.IsLike))
				.ForMember(dest => dest.LikeAt, opt => opt.MapFrom(src => src.LikeAt))
				.ForMember(dest => dest.UnLikeAt, opt => opt.MapFrom(src => src.UnLikeAt))
				.ReverseMap();

		
			CreateMap<CommentPost, GetBlogComments>()
				.ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
				.ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
				.ForMember(dest => dest.CommentText, opt => opt.MapFrom(src => src.CommentText))
				.ReverseMap();

			//Create comment
			CreateMap<CreateCommentByBlogId, CommentPost>()
				.ForMember(dest => dest.CommentText, opt => opt.MapFrom(src => src.CommentText))
				.ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
				.ReverseMap();

			CreateMap<CommentPost, CreateCommentByBlogId>()
				.ForMember(dest => dest.CommentText, opt => opt.MapFrom(src => src.CommentText))
				.ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
				.ReverseMap();

            #endregion
        
			
		

			#region Product

			CreateMap<Product, CreateProductDTO>();
			CreateMap<CreateProductDTO, Product>();

			CreateMap<UpdateProductDTO, Product>();
			CreateMap<Product, UpdateProductDTO>();

			CreateMap<Product, DeleteProductDTO>();
            CreateMap<DeleteProductDTO, Product>();

            CreateMap<Product, RestoreProductDTO>();
            CreateMap<RestoreProductDTO, Product>();

            CreateMap<Product, ViewProductDTO>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku))
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
           .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
           .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
           .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
           .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
           .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.TypeId))
           .ForMember(dest => dest.AgeId, opt => opt.MapFrom(src => src.AgeId))
           .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
           .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active));

			CreateMap<Product, ViewListProductsDTO>()
				.ForMember(dest => dest.Brand, otp => otp.MapFrom(src => src.Brand))
				.ForMember(dest => dest.Type, otp => otp.MapFrom(src => src.Type))
				.ForMember(dest => dest.AgeRange, otp => otp.MapFrom(src => src.AgeRange));


            #endregion

            #region ProductImage
            CreateMap<ProductImage, ViewListProductImageDTO>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image)); 
            CreateMap<Image, ImageDTO>();
            CreateMap<ImageDTO, Image>();
			CreateMap<UpdateProductImageDTO, ProductImage>();
            #endregion

            #region AgeRange
            CreateMap<AgeRange, ViewListAgeRangeDTO>();
            #endregion

            #region ProductType
            CreateMap<ProductType, ViewListProductTypeDTO>();
            #endregion
        }

    }
}
