using AutoMapper;
using YogurtCleaning.DataLayer.Entities;
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
			.ForMember(o => o.Client, opt => opt.MapFrom(src => new Client() { Id = src.ClientId }));
		CreateMap<CleaningObjectUpdateRequest, CleaningObject>();
		CreateMap<CleaningObject, CleaningObjectResponse>();

		CreateMap<Order, OrderResponse>();
		CreateMap<OrderUpdateRequest, Order>()
			.ForMember(o => o.CleanersBand, opt => opt.MapFrom(src => src.CleanersBandIds.Select(t => new Cleaner { Id = t }).ToList()))
			.ForMember(o => o.Bundles, opt => opt.MapFrom(src => src.BundlesIds.Select(t => new Bundle { Id = t }).ToList()))
			.ForMember(o => o.Services, opt => opt.MapFrom(src => src.ServicesIds.Select(t => new Service { Id = t }).ToList()));
		CreateMap<OrderRequest, Order>()
			.ForMember(o => o.CleaningObject, opt => opt.MapFrom(src => new CleaningObject() { Id = src.CleaningObjectId }))
			.ForMember(o => o.Bundles, opt => opt.MapFrom(src => src.BundlesIds.Select(t => new Bundle { Id = t }).ToList()))
			.ForMember(o => o.Services, opt => opt.MapFrom(src => src.ServicesIds.Select(t => new Service { Id = t }).ToList()));

		CreateMap<CleanerRegisterRequest, Cleaner>()
			.ForMember(o => o.Services, opt => opt.MapFrom(src => src.ServicesIds.Select(t => new Service { Id = t }).ToList()));
		CreateMap<CleanerUpdateRequest, Cleaner>()
			.ForMember(o => o.Services, opt => opt.MapFrom(src => src.ServicesIds.Select(t => new Service { Id = t }).ToList()));
		CreateMap<Cleaner, CleanerResponse>();

		CreateMap<ServiceRequest, Service>();
		CreateMap<Service, ServiceResponse>();
		CreateMap<UserLoginRequest, LoginData>();
	}
}