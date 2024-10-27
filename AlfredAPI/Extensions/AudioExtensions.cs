using Serilog;

namespace AlfredAPI.Extensions
{
    public static class AudioExtensions
    {
        public static bool saveAudio(string filePath, string ext, byte[] data)
        {
            // Currently Saves .BINKA, How Convert To .wave.exe?
            try
            {
                var stream = new FileStream(filePath + "." + ext, FileMode.Create, FileAccess.Write);
                var writer = new BinaryWriter(stream);
                writer.Write(data);
                writer.Flush();

                Log.Information($"Saved Audio: {filePath.Split('/').Last()} In Audios Folder!");
                return true;
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