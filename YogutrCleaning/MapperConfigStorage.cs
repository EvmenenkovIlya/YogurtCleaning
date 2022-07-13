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
	}
}
