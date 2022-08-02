using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.Business.Models;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business;

public class BusinessMapperConfigStorage : Profile
{
	public BusinessMapperConfigStorage()
	{
		CreateMap<OrderBusinessModel, Order>();
		CreateMap<Bundle, BundleBusinessModel>();
	}
}
