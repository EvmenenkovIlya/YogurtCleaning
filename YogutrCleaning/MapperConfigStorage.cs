using AutoMapper;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;
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
			.ForMember(o => o.CleanersBand, opt => opt.MapFrom(src => GetCleanersByListIds(src.CleanersBandIds)))
		.ForMember(o => o.Bundles, opt => opt.MapFrom(src => GetBundlesByListIds(src.BundlesIds)))
		.ForMember(o => o.Bundles, opt => opt.MapFrom(src => GetServicesByListIds(src.ServicesIds)));
		CreateMap<OrderRequest, Order>()
			.ForMember(o => o.CleaningObject, opt => opt.MapFrom(src => new CleaningObject() { Id = src.CleaningObjectId }))
		.ForMember(o => o.Bundles, opt => opt.MapFrom(src => GetBundlesByListIds(src.BundlesIds)))
		.ForMember(o => o.Services, opt => opt.MapFrom(src => GetServicesByListIds(src.ServicesIds)));

		CreateMap<CleanerRegisterRequest, Cleaner>()
		.ForMember(o => o.Services, opt => opt.MapFrom(src => GetServicesByListIds(src.ServicesIds)));
		CreateMap<CleanerUpdateRequest, Cleaner>()
		.ForMember(o => o.Services, opt => opt.MapFrom(src => GetServicesByListIds(src.ServicesIds)));
		CreateMap<Cleaner, CleanerResponse>();

		CreateMap<CommentRequest, Comment>();
		CreateMap<Comment, CommentResponse>();

		CreateMap<ServiceRequest, Service>();
		CreateMap<Service, ServiceResponse>();
	}
	
	private List<Cleaner> GetCleanersByListIds(List<int> servicesIds)
    {
		List<Cleaner> services = new List<Cleaner>();
		foreach (int serviceId in servicesIds)
        {
			services.Add(new Cleaner() { Id = serviceId });
        }
		return services;
    }

	private List<Bundle> GetBundlesByListIds(List<int> bundlesIds)
	{
		List<Bundle> bundles = new List<Bundle>();
		foreach (int bundleId in bundlesIds)
		{
			bundles.Add(new Bundle() { Id = bundleId });
		}
		return bundles;
	}

	private List<Service> GetServicesByListIds(List<int> servicesIds)
	{
		List<Service> services = new List<Service>();
		foreach (int serviceId in servicesIds)
		{
			services.Add(new Service() { Id = serviceId });
		}
		return services;
	}
}