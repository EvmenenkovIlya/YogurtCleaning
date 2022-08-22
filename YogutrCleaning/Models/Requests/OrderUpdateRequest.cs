﻿using YogurtCleaning.DataLayer.Enums;
namespace YogurtCleaning.Models;
public class OrderUpdateRequest
{
    public DateTime StartTime { get; set; }
    public List<int> BundlesIds { get; set; }
    public List<int> ServicesIds { get; set; }
    public List<int>? CleanersBandIds { get; set; }
}
