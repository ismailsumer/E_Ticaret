using AutoMapper;
using ETicaretAPI.DTOs;
using ETicaretAPI.Models.Entities;

namespace ETicaretAPI.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            // Product Eşleşmeleri
            CreateMap<Product, ProductViewDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "-"))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : "-"))
                .ReverseMap();

            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<Product, ProductCreateDto>();
            CreateMap<Product, ProductUpdateDto>().ReverseMap();

            // Category Eşleşmeleri
            CreateMap<Category, CategoryViewDto>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();

            // Brand Eşleşmeleri
            CreateMap<Brand, BrandViewDto>().ReverseMap();
            CreateMap<Brand, BrandCreateDto>().ReverseMap();

            // Address Eşleşmeleri
            CreateMap<Address, AddressDto>().ReverseMap();

            // Comment Eşleşmeleri
            CreateMap<Comment, CommentCreateDto>().ReverseMap();
            CreateMap<Comment, CommentViewDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User != null ? src.User.Username : "Anonim"))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : "-"));

            // ShippingCompany Eşleşmeleri
            CreateMap<ShippingCompany, ShippingViewDto>().ReverseMap();
            CreateMap<ShippingCreateDto, ShippingCompany>();

            // Slider Eşleşmeleri
            CreateMap<Slider, SliderViewDto>().ReverseMap();
            CreateMap<SliderCreateDto, Slider>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            // SiteSetting Eşleşmeleri
            CreateMap<SiteSetting, SiteSettingDto>().ReverseMap();
            CreateMap<SiteSettingUpdateDto, SiteSetting>();
        }
    }
}
