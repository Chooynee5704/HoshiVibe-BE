using AutoMapper;
using HoshiVibe.Entities.DTO.ModelRequests.OderProcess;
using HoshiVibe.Entities.DTO.ModelRequests.Product;
using HoshiVibe.Entities.DTO.ModelRequests.User;
using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Entity.DTO;
using HoshiVibe.Entity.DTO.ModelDTO;
using HoshiVibe.Entity.Model;

namespace HoshiVibe.Mapper
{
    public class MappingFile : Profile
    {
        public MappingFile()
        {
            // UserProfile <-> UserProfileDTO
            CreateMap<UserProfile, UserProfileDTO>().ReverseMap();

            // User <-> UserDTO (map kèm Profile)
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.ProfileDTO, opt => opt.MapFrom(src => src.Profile))
                .ReverseMap()
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.ProfileDTO));

            // RegisterDTO <-> User
            CreateMap<RegisterDTO, User>()
                .ForMember(dest => dest.User_Id, opt => opt.Ignore());
            CreateMap<User, RegisterDTO>();

            // ProfileUpdateDTO <-> UserProfile
            CreateMap<ProfileUpdateDTO, UserProfile>()
                .ForMember(dest => dest.UserProfile_Id, opt => opt.Ignore());
            CreateMap<UserProfile, ProfileUpdateDTO>();

            // Product
            CreateMap<Product, ProductRequestDTO>().ReverseMap()
                .ForMember(dest => dest.Product_Id, opt => opt.Ignore());
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.Product_Id, opt => opt.MapFrom(src => src.Product_Id))
                .ReverseMap();

            // Order
            CreateMap<OrderRequestDTO, Order>()
                .ForMember(dest => dest.Order_Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.Order_Id, opt => opt.MapFrom(src => src.Order_Id))
                .ReverseMap();

            // OrderDetail
            CreateMap<OrderDetail, OrderDetailDTO>()
                 .ForMember(dest => dest.OrderDetailId, opt => opt.MapFrom(src => src.OrderDetail_Id))
                 .ReverseMap();

            CreateMap<OrderDetailRequestDTO, OrderDetail>()
                .ForMember(dest => dest.OrderDetail_Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }

}

