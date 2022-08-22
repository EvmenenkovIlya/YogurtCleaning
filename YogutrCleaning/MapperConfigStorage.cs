using AutoMapper;
using YogurtCleaning.Business.Models;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Models;

namespace YogurtCleaning.API;

public class MapperConfigStorage : Profile
{
	public MapperConfigStorage()
	{
		CreateMap<ClientRegisterRequest, Client>();
		CreateMap<ClientUpdateRequest, Client>();
		CreateMap<Client, ClientResponse>();

		CreateMap<CleaningObjectRequest, CleaningObject>()
			.ForMember(o => o.Client, opt => opt.MapFrom(src => new Client() { Id = src.ClientId }))
			.ForMember(o => o.District, opt => opt.MapFrom(src => new District() { Id = src.District}));
		CreateMap<CleaningObjectUpdateRequest, CleaningObject>()
			.ForMember(o => o.District, opt => opt.MapFrom(src => new District() { Id = src.District }));
		CreateMap<CleaningObject, CleaningObjectResponse>()
			.ForMember(o => o.ClientId, opt => opt.MapFrom(src => src.Client.Id ))
			.ForMember(o => o.District, opt => opt.MapFrom(src => src.District.Id ));

		CreateMap<Order, OrderResponse>();
		CreateMap<OrderUpdateRequest, Order>()
			.ForMember(o => o.CleanersBand, opt => opt.MapFrom(src => src.CleanersBandIds!.Select(t => new Cleaner { Id = t }).ToList()))
			.ForMember(o => o.Bundles, opt => opt.MapFrom(src => src.BundlesIds.Select(t => new Bundle { Id = t }).ToList()))
			.ForMember(o => o.Services, opt => opt.MapFrom(src => src.ServicesIds.Select(t => new Service { Id = t }).ToList()));
		CreateMap<OrderUpdateRequest, OrderBusinessModel>()
			.ForMember(o => o.CleanersBand, opt => opt.MapFrom(src => src.CleanersBandIds!.Select(t => new Cleaner { Id = t }).ToList()))
			.ForMember(o => o.Bundles, opt => opt.MapFrom(src => src.BundlesIds.Select(t => new BundleBusinessModel { Id = t }).ToList()))
			.ForMember(o => o.Services, opt => opt.MapFrom(src => src.ServicesIds.Select(t => new Service { Id = t }).ToList()));
		CreateMap<OrderRequest, OrderBusinessModel>()
			.ForMember(o => o.CleaningObject, opt => opt.MapFrom(src => new CleaningObject() { Id = src.CleaningObjectId }))
			.ForMember(o => o.Bundles, opt => opt.MapFrom(src => src.BundlesIds.Select(t => new BundleBusinessModel { Id = t }).ToList()))
			.ForMember(o => o.Services, opt => opt.MapFrom(src => src.ServicesIds.Select(t => new Service { Id = t }).ToList()))
			.ForMember(o => o.Client, opt => opt.MapFrom(src => new Client() { Id = src.ClientId }))
			.AfterMap((src, dest) => { dest.SetTotalDuration(); dest.SetCleanersCount(); dest.SetEndTime(); });

		CreateMap<CleanerRegisterRequest, Cleaner>()
			.ForMember(o => o.Services, opt => opt.MapFrom(src => src.ServicesIds.Select(t => new Service { Id = t }).ToList()))
			.ForMember(o => o.Districts, opt => opt.MapFrom(src => src.Districts.Select(t => new District { Id = t}).ToList()));
		CreateMap<CleanerUpdateRequest, Cleaner>()
			.ForMember(o => o.Services, opt => opt.MapFrom(src => src.ServicesIds.Select(t => new Service { Id = t }).ToList()))
			.ForMember(o => o.Districts, opt => opt.MapFrom(src => src.Districts.Select(t => new District { Id = t }).ToList())); ;
		CreateMap<Cleaner, CleanerResponse>();

		CreateMap<ServiceRequest, Service>();
		CreateMap<Service, ServiceResponse>();
		CreateMap<BundleRequest, Bundle>()
			.ForMember(o => o.Services, opt => opt.MapFrom(src => src.ServicesIds.Select(t => new Service { Id = t }).ToList()));
		CreateMap<Bundle, BundleResponse>();
        CreateMap<Bundle, BundleBusinessModel>().ReverseMap();

        CreateMap<CommentRequest, Comment>()
			.ForMember(c => c.Order, opt => opt.MapFrom(src => new Order() { Id = src.OrderId}));
		CreateMap<Comment, CommentResponse>()
			.ForMember(c => c.OrderId, opt => opt.MapFrom(src => src.Order.Id))
			.ForMember(c => c.CleanerId, opt => opt.MapFrom(src => src.Cleaner!.Id))
			.ForMember(c => c.ClientId, opt => opt.MapFrom(src => src.Client!.Id));
		CreateMap<Comment, CommentAboutResponse>();
	}
}