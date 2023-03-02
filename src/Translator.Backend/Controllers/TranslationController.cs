using Google.Cloud.Translation.V2;
using Microsoft.AspNetCore.Mvc;

namespace Translator.Backend.Controllers
{
    [ApiController]
    public class TranslationController : ControllerBase
    {
        [HttpGet("translation/translate")]
        public async Task<IActionResult> Translate([FromQuery] TranslationRequest request)
        {
            if (request.Text != null)
            {
                TranslationClient client = await TranslationClient.CreateAsync();

                var response = await client.TranslateTextAsync(request.Text,
                    request.TargetLanguage, request.SourceLanguage);

                return Ok(new { translation = response.TranslatedText });
            }

            return BadRequest();
        }
    }
}
