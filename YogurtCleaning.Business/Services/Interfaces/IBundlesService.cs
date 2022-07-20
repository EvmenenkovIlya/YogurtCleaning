using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services;

public interface IBundlesService
{
    List<Service> GetAdditionalServices(int id);
}
