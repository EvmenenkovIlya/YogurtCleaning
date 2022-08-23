﻿using AutoMapper;
using YogurtCleaning.Business.Models;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business;

public class BusinessMapperConfigStorage : Profile
{
	public BusinessMapperConfigStorage()
	{
		CreateMap<OrderBusinessModel, Order>()
            .ForMember(c => c.EndTime, opt => opt.MapFrom(src => src.StartTime.AddHours(Decimal.ToDouble(src.TotalDuration))));
        CreateMap<Order, OrderBusinessModel>()
            .AfterMap((src, dest) => { dest.SetTotalDuration(); dest.SetCleanersCount(); dest.SetEndTime(); });
        CreateMap<Bundle, BundleBusinessModel>().ReverseMap();
	}
}
