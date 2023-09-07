using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Data
{
    public class MaterialService : IMaterialService
    {
        private readonly IDbContextFactory<PurchaseContext> _contextFactory;

        public MaterialService(IDbContextFactory<PurchaseContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Material>> GetAllMaterials()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Materials.ToListAsync();
        }

        public async Task<Material?> Get(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var material = await context.Materials.FindAsync(id);
            if (material != null && material.State == MaterialState.DELETED)
                material = null;
            return material;
        }

        public async Task<List<Material>?> GetMaterials(int requestId)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Materials
                .Where(m => m.RequestId == requestId && m.State != MaterialState.DELETED)
                .ToListAsync();
        }

        public async Task Create(Material material)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Add(material);
            await context.SaveChangesAsync();
        }

        public async Task Edit(Material material)
        {
            using var context = _contextFactory.CreateDbContext();
            try
            {
                context.Update(material);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialExists(material.Id)) return; 
                else throw;
            }
        }

        public async Task Delete(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var material = await context.Materials.FindAsync(id);
            if (material != null)
            {
                material.State = MaterialState.DELETED;
                await Edit(material);
            }
        }

        private bool MaterialExists(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return (context.Materials
                ?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
