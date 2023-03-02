namespace Translator.Backend.Data.Repositories
{
    public interface ISegmentRepository
    {
        public Task Add(int sessionId, string value);
        public Task<string> AggregateAllSegments(string connectionId);
    }
}
