using System.Net;
using CUE4Parse.FileProvider;
using CUE4Parse.UE4.Versions;
using EpicManifestParser.Objects;
using CUE4Parse.UE4.Assets.Exports.Sound;

namespace AlfredAPI.AlfredAPI
{
    public class Global
    {
        public static StreamedFileProvider Provider = new StreamedFileProvider("FortniteLive", true, new VersionContainer(EGame.GAME_UE5_LATEST));
        public static Manifest? Manifest;
        public static HttpClient Client = new(new HttpClientHandler()
        {
            UseProxy = false,
            UseCookies = false,
            AutomaticDecompression = DecompressionMethods.All,
        });

        // So We Can Access "audio.Name" Globally
        public static USoundWave soundWave;
    }
}
