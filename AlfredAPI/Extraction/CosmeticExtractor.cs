using Serilog;
using AlfredAPI.Models;
using AlfredAPI.AlfredAPI;
using AlfredAPI.Properties;
using CUE4Parse_Conversion.Textures;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports.Texture;
using SkiaSharp;

namespace AlfredAPI.Extraction
{
    // Will Log Parsed Data To The Console! 
    public static class CosmeticExtractor
    {
        public static async Task LoadAsset(string? assetPath)
        {
            // If No Asset Path Return Error
            if (assetPath == null)
                Log.Error("Invalid Asset Path!");

            else
            {
                // Try To Load The Object, If False Return Error
                if (!Global.Provider.TryLoadObject(assetPath, out UObject package))
                    Log.Error($"Failed To Locate: {assetPath}");

                else
                {
                    // Pass Our UObject Threw Our Model To Parse JSON Data
                    // STILL WORKING ON MODEL For All Export Types, Therefor May Break!
                    CosmeticModel cos = new CosmeticModel(package);
                    Log.Information($"Parsing: {assetPath}...");
                    Console.WriteLine("ExportType: " + cos.ExportType);
                    Console.WriteLine("DisplayName: " + cos.DisplayName);
                    Console.WriteLine("Description: " + cos.Description);
                    Console.WriteLine("ShortDescription: " + cos.ShortDescription);
                    Console.WriteLine("Rarity: " + cos.PreviewRarity);
                    Console.WriteLine("Icon Path: " + cos.imagePath);
                    
                    // Save Icon To Exports Folder
                    await SaveIcon(cos.Icon, cos.DisplayName);
                }
            }
        }

        // MEMORY SAFE PRIORITY!!!!!!
        private static async Task SaveIcon(SKImage? icon, string displayName)
        {
            // Save Icon To Exports Folder
            if (icon != null)
            {
                using var image = SKImage.FromBitmap(SKBitmap.FromImage(icon));
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);
                var path = Path.Combine(Settings.exportsFolder, $"{displayName}.png");

                try
                {
                    await using var stream = File.OpenWrite(path);
                    data.SaveTo(stream);
                    Log.Information($"Saved Icon To: {path}");
                }
                catch (Exception ex)
                {
                    Log.Error($"Failed to save icon to {path}: {ex.Message}");
                }
            }
        }
    }
}
