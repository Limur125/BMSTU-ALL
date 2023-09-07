using Azure.Core;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Data
{
    public class RequestService : IRequestService
    {
        private readonly IDbContextFactory<PurchaseContext> _contextFactory;

        public RequestService(IDbContextFactory<PurchaseContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public List<Request> GetAllRequests()
        {
            using var context = _contextFactory.CreateDbContext();
            return context.Requests.Where(r => r.State != RequestState.DELETED).ToList();
        }

        public async Task<Request?> Get(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var request = await context.Requests
                .Include(r => r.Materials
                    .Where(t => t.State != MaterialState.DELETED))
                .Where(t => t.Id == id && t.State != RequestState.DELETED)
                .FirstOrDefaultAsync();
            return request;
        }

        public async Task Create(Request request)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Add(request);
            await context.SaveChangesAsync();
        }

        public async Task Edit(Request request)
        {
            using var context = _contextFactory.CreateDbContext();
            try
            {
                context.Update(request);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(request.Id)) return;
                else throw;
            }
        }

        public async Task Delete(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var request = await context.Requests.FindAsync(id);
            if (request != null)
            {
                request.State = RequestState.DELETED;
                await Edit(request);
            }
        }

        private bool RequestExists(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return (context.Requests
                ?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
