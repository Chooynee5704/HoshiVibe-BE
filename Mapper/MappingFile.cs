using AutoMapper;
using HoshiVibe.Entities.DTO;
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
            // ===== UserProfile <-> UserProfileDTO =====
            CreateMap<UserProfile, UserProfileDTO>()
                .ForMember(d => d.ZodiacName, o => o.MapFrom(s => s.Zodiac != null ? s.Zodiac.Name : null))
                .ForMember(d => d.DestinyName, o => o.MapFrom(s => s.Destiny != null ? s.Destiny.Name : null))
                .ReverseMap()
                .ForMember(d => d.Zodiac, o => o.Ignore())
                .ForMember(d => d.Destiny, o => o.Ignore());

            // ===== User <-> UserDTO (kèm Profile) =====
            CreateMap<User, UserDTO>()
                .ForMember(d => d.ProfileDTO, o => o.MapFrom(s => s.Profile));
            CreateMap<UserDTO, User>()
                .ForMember(d => d.Profile, o => o.MapFrom(s => s.ProfileDTO))
                // tránh ghi đè Password/Role/IsDisabled từ DTO
                .ForMember(d => d.Password, o => o.Ignore())
                .ForMember(d => d.Role, o => o.Ignore())
                .ForMember(d => d.IsDisabled, o => o.Ignore());

            // ===== RegisterDTO <-> User =====
            CreateMap<RegisterDTO, User>()
                .ForMember(d => d.User_Id, o => o.Ignore())
                // Password sẽ hash ở service, không map trực tiếp
                .ForMember(d => d.Password, o => o.Ignore());
            CreateMap<User, RegisterDTO>(); // chỉ để view; không dùng để update entity

            // ===== ProfileUpdateDTO -> UserProfile (IGNORE NULLS) =====
            // Dùng cho PUT/PATCH profile: chỉ cập nhật field có giá trị
            CreateMap<ProfileUpdateDTO, UserProfile>()
                .ForMember(d => d.UserProfile_Id, o => o.Ignore())
                .ForMember(d => d.Zodiac, o => o.Ignore())
                .ForMember(d => d.Destiny, o => o.Ignore())
                // QUAN TRỌNG: không overwrite bằng null
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            // Chiều ngược chỉ để hiển thị form
            CreateMap<UserProfile, ProfileUpdateDTO>();

            // ===== Zodiac <-> DTO =====
            CreateMap<Zodiac, ZodiacUpdateDTO>().ReverseMap();

            // ===== Destiny <-> DTO =====
            CreateMap<Destiny, DestinyUpdateDTO>().ReverseMap();

            // ===== Product =====
            CreateMap<Product, ProductRequestDTO>().ReverseMap()
                .ForMember(d => d.Product_Id, o => o.Ignore());
            CreateMap<Product, ProductDTO>()
                .ForMember(d => d.Product_Id, o => o.MapFrom(s => s.Product_Id))
                .ReverseMap();

            // ===== Order =====
            CreateMap<OrderRequestDTO, Order>()
                .ForMember(d => d.Order_Id, o => o.Ignore())
                .ForMember(d => d.Status, o => o.Ignore())
                .ReverseMap();

            CreateMap<Order, OrderDTO>()
                .ForMember(d => d.Order_Id, o => o.MapFrom(s => s.Order_Id))
                .ForMember(d => d.OrderDetails, o => o.MapFrom(s => s.OrderDetails))
                .ReverseMap();

            // ===== OrderDetail =====
            CreateMap<OrderDetail, OrderDetailDTO>()
                .ForMember(d => d.OrderDetailId, o => o.MapFrom(s => s.OrderDetail_Id))
                .ForMember(d => d.Product, o => o.MapFrom(s => s.Product))
                .ReverseMap();

            CreateMap<OrderDetailRequestDTO, OrderDetail>()
                .ForMember(d => d.OrderDetail_Id, o => o.Ignore())
                .ReverseMap();

            // ===== Cart =====
            CreateMap<CartRequestDTO, Cart>()
                .ForMember(d => d.Cart_Id, o => o.Ignore())
                .ReverseMap();
            CreateMap<Cart, CartDTO>()
                .ForMember(d => d.Cart_Id, o => o.MapFrom(s => s.Cart_Id))
                .ReverseMap();

            // ===== CartItem =====
            CreateMap<CartItemsRequestDTO, CartItem>()
                .ForMember(d => d.CartItem_Id, o => o.Ignore())
                .ForMember(d => d.Cart_Id, o => o.MapFrom(s => s.Cart_Id))
                .ReverseMap();
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(d => d.CartItem_Id, o => o.MapFrom(s => s.CartItem_Id))
                .ReverseMap();

            // ===== CustomProduct =====
            CreateMap<CustomProduct, HoshiVibe.Entities.DTO.ModelRequests.Product.CustomPdRqDTO>()
                .ReverseMap()
                .ForMember(d => d.CProduct_Id, o => o.Ignore());

            CreateMap<CustomProduct, HoshiVibe.Entities.DTO.ModelRequests.Product.CustomProductDTO>()
                .ReverseMap();

            // ===== Voucher =====
            CreateMap<Voucher, VoucherDTO>().ReverseMap();
        }
    }
}
