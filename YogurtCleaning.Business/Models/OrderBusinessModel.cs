using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.Business.Models;

public class OrderBusinessModel
{
    public int Id { get; set; }
    public Client Client { get; set; }
    public CleaningObject CleaningObject { get; set; }
    public Status Status { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime UpdateTime { get; set; }
    public decimal Price { get; set; }
    public List<BundleBusinessModel> Bundles { get; set; }
    public List<Service> Services { get; set; }
    public decimal TotalDuration { get; set; }
    public int CleanersCount { get; set; }
    public List<Cleaner>? CleanersBand { get; set; }
    public List<Comment> Comments { get; set; }
    public bool IsDeleted { get; set; }

    public decimal GetTotalDuration()
    {
        var bundlesDuration = this.Bundles.Select(b => b.GetDuration(this.CleaningObject)).Sum();
        var servicesDuration = this.Services.Select(s => s.Duration).Sum();
        this.TotalDuration = bundlesDuration + servicesDuration;
        return this.TotalDuration;
    }

    public int GetCleanersCount()
    {
        var maxHour = 21;
        var maxOrderDuration = maxHour - this.StartTime.Hour;

        if (this.TotalDuration > maxOrderDuration)
        {
            this.CleanersCount = Convert.ToInt32(Math.Ceiling(this.TotalDuration / maxOrderDuration));
            return this.CleanersCount;
        }
        else return this.CleanersCount = 1;
    }

    public DateTime GetEndTime()
    {
        this.EndTime = this.StartTime.AddHours((double)this.TotalDuration / this.CleanersCount);
        return this.EndTime;
    }

    //public decimal GetPrice()
    //{
    //    var bundlesPrice = this.Bundles.Select(b => b.GetPrice(this.CleaningObject)).Sum();
    //    var servicesPrice = this.Services.Select(s => s.Price).Sum();
    //    this.Price = bundlesPrice + servicesPrice;
    //    if (this.Bundles[0].Type == CleaningType.Regular)
    //    {
    //        var discount = (decimal)0.2;
    //        var lastOrder = _clientsRepository.GetLastOrderForCleaningObject(order.Client.Id, order.CleaningObject.Id);
    //        if (lastOrder != null && lastOrder.Bundles[0].Type == CleaningType.General || lastOrder.Bundles[0].Type == CleaningType.AfterRenovation)
    //        {
    //            orderPrice -= orderPrice * discount;
    //        }
    //    }
    //    return orderPrice;
    //}
}