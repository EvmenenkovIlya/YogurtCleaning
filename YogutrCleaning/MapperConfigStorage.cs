using AutoMapper;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.Models;

namespace YogurtCleaning;

public class MapperConfigStorage : Profile
{
	public MapperConfigStorage()
	{
		CreateMap<ServiceRequest, Service>();
		CreateMap<Service, ServiceResponse>();
	}

}
