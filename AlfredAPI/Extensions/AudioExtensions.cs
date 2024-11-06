using Serilog;
using System.Diagnostics;

namespace AlfredAPI.Extensions
{
    public static class AudioExtensions
    {
        public static bool saveAudio(string filePath, string ext, byte[] data)
        {
            // Make Sure We Can Locate "binkadec.exe"
            string binkadecPath = Path.Combine("C:\\Users\\ethan\\Desktop\\Coding\\Frameworks\\", "binkadec.exe");
            if (!File.Exists(binkadecPath))
                return false;

            // Create Binka And Wav FilePaths
            string wavFilePath = Path.ChangeExtension(filePath, ".wav");
            string binkaFilePath = filePath + "." + ext;

            try
            {
                // Write Data To Binka File
                var stream = new FileStream(binkaFilePath, FileMode.Create, FileAccess.Write);
                var writer = new BinaryWriter(stream);
                writer.Write(data);
                writer.Flush();

                Log.Information($"Successfully Saved BINKA Audio File: {filePath.Split('/').Last()}!");
                Log.Information("Working On BINKA To Wav Conversion...");

                // Start The Binka To Wav Conversion Process
                var binkadecProcess = Process.Start(new ProcessStartInfo
                {
                    FileName = binkadecPath,
                    Arguments = $"-i \"{binkaFilePath}\" -o \"{wavFilePath}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                binkadecProcess.Start();
                binkadecProcess?.WaitForExit(5000);
                // File.Delete(binkaFilePath); // Optional! Im Currently Working On C# Binka To Wav So I Keep BINKA Files!

                Log.Information($"Successfully Saved Wav Audio File: {wavFilePath.Split('/').Last()}!");
                return binkadecProcess?.ExitCode == 0 && File.Exists(wavFilePath);
            }
            
            catch (Exception e)
            {
#if DEBUG
                Log.Error(e.Message);
#endif
                Log.Error($"Error While Saving Audio For: {filePath.Split('/').Last()}");
                return false;
            }
        }
    }
}