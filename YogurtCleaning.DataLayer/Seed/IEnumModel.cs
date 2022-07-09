namespace YogurtCleaning.DataLayer.Seed
{
    public interface IEnumModel<TModel, TModelIdType>
    {
        TModelIdType Id { get; set; }
        string Name { get; set; }
    }
}
