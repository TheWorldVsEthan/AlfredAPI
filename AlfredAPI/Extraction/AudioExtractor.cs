using Serilog;
using AlfredAPI.AlfredAPI;
using AlfredAPI.Extensions;
using CUE4Parse_Conversion.Sounds;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports.Sound;

namespace AlfredAPI.Extraction
{
    // The Start Of A Headache
    public static class AudioExtractor
    {
        public static async Task LoadAudio(string assetPath)
        {
            string newAssetPath = null;

            // If No Asset Path Return Error
            if (assetPath == null)
                Log.Error("Invalid Asset Path!");

            else
            {
                // Try To Load The Object, If False Return Error
                if (assetPath.EndsWith(".uasset"))
                    newAssetPath = assetPath.Replace(".uasset", "");

                var _obj = Global.Provider.TryLoadObject(newAssetPath, out UObject export);
                if (export == null)
                    Log.Error($"Failed To Locate: {newAssetPath}");


                else
                {
                    // TESTING 
                    CheckExport(export);

                    // var fullJson = JsonConvert.SerializeObject(export, Formatting.Indented);
                    // Console.WriteLine(fullJson);
                }
            }
        }
        public static bool CheckExport(UObject uobject)
        {
            switch (uobject)
            {
                case USoundWave audio:
                    audio.Decode(true, out var format, out var audioData);
                    if (audioData == null || string.IsNullOrEmpty(format) || audio.Owner == null)
                        return false;

                    Global.soundWave = audio; // So We Can Access "audio.Name" Globally (If Needed In The Future)
                    return AudioExtensions.saveAudio(Path.Combine("C:\\Users\\ethan\\Desktop\\AlfredTesting", audio.Name), format, audioData);
                default:
                    return true;
            }
        }
    }
}