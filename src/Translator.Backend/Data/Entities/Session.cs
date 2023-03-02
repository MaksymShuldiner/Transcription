using System.ComponentModel.DataAnnotations;

namespace Translator.Backend.Data.Entities
{
    public class Session
    {
        [Key]
        public int Id { get; set; }

        public string? ConnectionId { get; set; }

        public string? Language { get; set; }
    }
}
