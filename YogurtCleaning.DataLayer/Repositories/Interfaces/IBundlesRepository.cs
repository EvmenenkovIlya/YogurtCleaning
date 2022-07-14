﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories.Interfaces;

public interface IBundlesRepository
{
    Bundle GetBundle(int id);
    List<Bundle> GetAllBundles();
    void UpdateBundle(Bundle bundle, int id);
    int AddBundle(Bundle bundle);
    void DeleteBundle(int id);
}
