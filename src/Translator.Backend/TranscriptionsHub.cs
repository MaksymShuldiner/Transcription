using Google.Cloud.Speech.V1;
using Microsoft.AspNetCore.SignalR;
using Translator.Backend.Data.Repositories;

namespace Translator.Backend
{
    public class TranscriptionsHub : Hub
    {
        private readonly ISegmentRepository _segmentRepository;
        private readonly ISessionRepository _sessionRepository;

        public TranscriptionsHub(ISegmentRepository segmentRepository, ISessionRepository sessionRepository)
        {
            _segmentRepository = segmentRepository;
            _sessionRepository = sessionRepository;
        }

        public async Task BroadcastTranscriptionData(string content, string language)
        {
            var connectionId = Context.ConnectionId;
            int? sessionId = (await _sessionRepository.Get(connectionId))?.Id;
            sessionId ??= await _sessionRepository.Add(Context.ConnectionId, language);

            var speechClient = await SpeechClient.CreateAsync();
            byte[] bytes = Convert.FromBase64String(content);
            var audio = RecognitionAudio.FromBytes(bytes);

            var speech = SpeechClient.Create();
            var streamingCall = speech.StreamingRecognize();

            await streamingCall.WriteAsync(
                new StreamingRecognizeRequest()
                {
                    StreamingConfig = new StreamingRecognitionConfig()
                    {
                        Config = new RecognitionConfig()
                        {
                            Encoding =
                            RecognitionConfig.Types.AudioEncoding.WebmOpus,
                            SampleRateHertz = 48000,
                            LanguageCode = language,
                            EnableAutomaticPunctuation = false,
                        }
                    }
                });

            Task process = Task.Run(async () =>
            {
                var stream = streamingCall.GetResponseStream();

                while (await stream.MoveNextAsync(default))
                {
                    foreach (var result in stream.Current.Results)
                    {
                        var segment = result.Alternatives?.OrderBy(a => a.Confidence).FirstOrDefault()?.Transcript;

                        if (segment != null)
                        {
                            await Clients.Caller.SendAsync("broadcastTranscriptionData", segment);
                                await _segmentRepository.Add(sessionId.Value, segment);
                        }
                    }   
                }
            });

            await streamingCall.WriteAsync(
                new StreamingRecognizeRequest()
                {
                    AudioContent = Google.Protobuf.ByteString.FromBase64(content),
                });

            await streamingCall.WriteCompleteAsync();
            await process;
        }
    }
}
