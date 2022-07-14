using AutoMapper;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;
using YogurtCleaning.Models;

namespace YogurtCleaning.API;

public class MapperConfigStorage : Profile
{
    private readonly IClientsRepository _clientsRepository;
	private readonly ICleaningObjectsRepository _cleaningObjectsRepository;
	private readonly ICleanersRepository _cleanersRepository;

    public MapperConfigStorage(IClientsRepository clientsRepository, ICleaningObjectsRepository cleaningObjectsRepository, ICleanersRepository cleanersRepository)
	{
		_clientsRepository = clientsRepository;
		_cleaningObjectsRepository = cleaningObjectsRepository;
		_cleanersRepository = cleanersRepository;

		CreateMap<ClientRegisterRequest, Client>();
		CreateMap<ClientUpdateRequest, Client>();
		CreateMap<Client, ClientResponse>();

		CreateMap<CleaningObjectRequest, CleaningObject>()
			.ForMember(o => o.Client, opt => opt.MapFrom(src => _clientsRepository.GetClient(src.ClientId)));
		CreateMap<CleaningObjectUpdateRequest, CleaningObject>();
		CreateMap<CleaningObject, CleaningObjectResponse>();

		// for orders neded to add methods from services and bundles repos
		CreateMap<Order, OrderResponse>();
		CreateMap<OrderUpdateRequest, Order>()
			.ForMember(o => o.CleanersBand, opt => opt.MapFrom(src => _cleanersRepository.GetAllCleanersByListIds(src.CleanersBandIds)));
		//.ForMember(o => o.Bundles, opt => opt.MapFrom()
		//.ForMember(o => o.Services, opt => opt.MapFrom();
		CreateMap<OrderRequest, Order>()
			.ForMember(o => o.CleaningObject, opt => opt.MapFrom(src => _cleaningObjectsRepository.GetCleaningObject(src.CleaningObjectId)));
		//.ForMember(o => o.Bundles, opt => opt.MapFrom()
		//.ForMember(o => o.Services, opt => opt.MapFrom();

		// for Cleaners need to add methods from services repos
		CreateMap<CleanerRegisterRequest, Cleaner>();
		//.ForMember(o => o.Services, opt => opt.MapFrom();
		CreateMap<CleanerUpdateRequest, Cleaner>();
		//.ForMember(o => o.Services, opt => opt.MapFrom();
		CreateMap<Cleaner, CleanerResponse>();

		CreateMap<ServiceRequest, Service>();
		CreateMap<Service, ServiceResponse>();
	}
}