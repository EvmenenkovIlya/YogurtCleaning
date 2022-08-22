using AutoMapper;
using YogurtCleaning.Business.Models;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business;

public class BusinessMapperConfigStorage : Profile
{
	public BusinessMapperConfigStorage()
	{
		CreateMap<OrderBusinessModel, Order>().ReverseMap();
		CreateMap<Bundle, BundleBusinessModel>().ReverseMap();
	}
}
