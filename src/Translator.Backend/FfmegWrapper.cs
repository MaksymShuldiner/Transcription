using System.Diagnostics;

namespace Translator.Backend
{
    public class FfmegWrapper
    {
        public static void Launch(string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe");
            startInfo.Arguments = arguments;
            startInfo.RedirectStandardOutput = true;
            //startInfo.RedirectStandardError = true;

            Console.WriteLine(string.Format(
                "Executing \"{0}\" with arguments \"{1}\".\r\n",
                startInfo.FileName,
                startInfo.Arguments));

            using Process process = Process.Start(startInfo)!;
            process.WaitForExit();
        }
    }
}
