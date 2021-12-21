namespace ProjectBank.Server.Entities
{
    internal interface IViewRepository
    {
        public Task<Response> AddViewAsync(int projectId, int userId);

        public Task<int> GetViewsOfProjectAsync(int projectId);

        public Task<Response> DeleteViewAsync(int projectId);

    }
}
