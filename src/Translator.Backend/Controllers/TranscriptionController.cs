using Microsoft.AspNetCore.Mvc;
using Translator.Backend.Data.Repositories;

namespace Translator.Backend.Controllers
{
    [ApiController]
    public class TranscriptionController : ControllerBase
    {
        private readonly ILogger<TranscriptionController> _logger;
        private readonly ISegmentRepository _segmentRepository;

        public TranscriptionController(ILogger<TranscriptionController> logger, ISegmentRepository segmentRepository)
        {
            _logger = logger;
            _segmentRepository = segmentRepository;
        }

        [HttpGet("transcription/fullTranscription/{connectionId}")]
        public async Task<IActionResult> FullTranscription(string connectionId)
        {
            var result = await _segmentRepository.AggregateAllSegments(connectionId);
            return Ok(new { transcription = result });
        }
    }
}