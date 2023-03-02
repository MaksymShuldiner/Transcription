using System.ComponentModel.DataAnnotations;

namespace Translator.Backend.Data.Entities
{
    public class Segment
    {
        [Key]
        public long Id { get; set; }

        public string? Value { get; set; }

        public int SessionId { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
