using Dapper;
using Dapper.Contrib.Extensions;
using Translator.Backend.Data.Entities;

namespace Translator.Backend.Data.Repositories
{
    public class SegmentRepository : ISegmentRepository
    {
        private readonly DapperContext _context;

        public SegmentRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task Add(int sessionId, string value)
        {
            using var connection = _context.CreateConnection();

            await connection.InsertAsync(new Segment
            {
                Timestamp = DateTime.Now,
                SessionId = sessionId,
                Value = value
            });
        }

        public async Task<string> AggregateAllSegments(string connectionId)
        {
            using var connection = _context.CreateConnection();

            var segments = await connection.QueryAsync<Segment>("select * from Segments as s inner join Sessions as ses " +
                "on s.SessionId = ses.Id where ses.ConnectionId = @connectionId", new
                {
                    connectionId
                });

            return segments.OrderByDescending(s => s.Timestamp)
                           .Select(s => s.Value)
                           .Aggregate(string.Empty, (s, res) => res += " " + s);
        }
    }
}
