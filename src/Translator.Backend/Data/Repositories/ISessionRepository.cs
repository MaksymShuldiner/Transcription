using Translator.Backend.Data.Entities;

namespace Translator.Backend.Data.Repositories
{
    public interface ISessionRepository
    {
        public Task<int> Add(string connectionId, string language);
        public Task<Session?> Get(string connectionId);
    }
}
