using Dapper;
using Dapper.Contrib.Extensions;
using Translator.Backend.Data.Entities;

namespace Translator.Backend.Data.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DapperContext _context;

        public SessionRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> Add(string connectionId, string language)
        {
            using var connection = _context.CreateConnection();

            return await connection.InsertAsync(new Session 
            { 
                ConnectionId = connectionId, 
                Language = language 
            });
        }

        public async Task<Session?> Get(string connectionId)
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Session>
                ("select * from Sessions where ConnectionId = @connectionId", new
            {
                connectionId
            });
        }
    }
}
