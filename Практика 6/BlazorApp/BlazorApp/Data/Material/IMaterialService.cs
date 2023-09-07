namespace BlazorApp.Data
{
    public interface IMaterialService
    {
        public Task<List<Material>> GetAllMaterials();

        public Task<Material?> Get(int id);

        public Task<List<Material>?> GetMaterials(int requestId);

        public Task Create(Material material);

        public Task Edit(Material material);

        public Task Delete(int id);
    }
}
