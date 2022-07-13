using AutoMapper;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;
using YogurtCleaning.Models;

namespace YogurtCleaning.API;

public class MapperConfigStorage : Profile
{
    private readonly IClientsRepository _clientsRepository;

    public MapperConfigStorage(IClientsRepository clientsRepository)
	{
		_clientsRepository = clientsRepository;

		CreateMap<ClientRegisterRequest, Client>();
		CreateMap<ClientUpdateRequest, Client>();
		CreateMap<Client, ClientResponse>();

		CreateMap<CleaningObjectRequest, CleaningObject>()
			.ForMember(dest => dest.Client, opt => opt.MapFrom(src => _clientsRepository.GetClient(src.ClientId)));
		CreateMap<CleaningObjectUpdateRequest, CleaningObject>();
		CreateMap<CleaningObject, CleaningObjectResponse>();
	}
}
