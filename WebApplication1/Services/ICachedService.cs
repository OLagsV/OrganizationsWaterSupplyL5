namespace OrganizationsWaterSupplyL4.Services
{
    public interface ICachedService<T> where T : class
    {
        public void AddData(string cacheKey);
        public IEnumerable<T> GetData(string cacheKey);
        public void UpdateData(string cacheKey);
    }
}
