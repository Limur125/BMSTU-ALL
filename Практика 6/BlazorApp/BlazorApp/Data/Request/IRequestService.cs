namespace BlazorApp.Data
{
    public interface IRequestService
    {
        public List<Request> GetAllRequests();

        public Task<Request?> Get(int id);

        public Task Create(Request material);

        public Task Edit(Request material);

        public Task Delete(int id);
    }
}
