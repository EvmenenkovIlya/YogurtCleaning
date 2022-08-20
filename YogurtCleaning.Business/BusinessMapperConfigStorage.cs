using AutoMapper;
using YogurtCleaning.Business.Models;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business;

public class BusinessMapperConfigStorage : Profile
{
	public BusinessMapperConfigStorage()
	{
		CreateMap<OrderBusinessModel, Order>();
        CreateMap<Order, OrderBusinessModel>();
        CreateMap<Bundle, BundleBusinessModel>().ReverseMap();
	}
}
