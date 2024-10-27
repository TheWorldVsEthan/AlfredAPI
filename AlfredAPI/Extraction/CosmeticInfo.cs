using Serilog;
using AlfredAPI.Models;
using AlfredAPI.AlfredAPI;
using CUE4Parse.UE4.Assets.Exports;

namespace AlfredAPI.Extraction
{
    // Will Log Parsed Data To The Console! 
    public static class CosmeticInfo
    {
        public static async Task LoadAsset(string assetPath)
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
                }
            }
        }
    }
}