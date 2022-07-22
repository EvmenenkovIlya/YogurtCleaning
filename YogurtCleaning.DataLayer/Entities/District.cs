using System.ComponentModel.DataAnnotations;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Seed;

namespace YogurtCleaning.DataLayer.Entities;

public class District : IEnumModel<District, DistrictEnum>
{
    [Key]
    public DistrictEnum Id { get; set; }
    public List<Cleaner> Cleaners { get; set; }
    public string Name { get; set; }    
}
