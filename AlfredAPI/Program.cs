using Serilog;
using Serilog.Events;
using AlfredAPI.Models;
using AlfredAPI.AlfredAPI;
using AlfredAPI.Properties;
using Serilog.Sinks.SystemConsole.Themes;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.AssetRegistry.Objects;
using AlfredAPI.AlfredAPI.Services;
using CUE4Parse.UE4.AssetRegistry;

namespace AlfredAPI
{
    public class Program
    {

        public readonly List<FAssetData> AssetRegistry = [];

        // Basic Usage, Docs soon? Possibly Read The Code And Learn How to Use It??
        private static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(LogEventLevel.Debug, "[{Timestamp}] [{Level:u3}] {Message}{NewLine}{Exception}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            Log.Information($"[START UP] Stating: {Settings.Name} v{Settings.Version}");
            Log.Information($"[START UP] Hosting Environment: {Settings.Environment}");
            Log.Information($"[START UP] Content Root Path: {Directory.GetCurrentDirectory()}");

            // Start API
            Manager? manager = new Manager();
            await manager.StartAsync();

            var totalFileCount = Global.Provider.Files.Count;
            Log.Information($"Successfully Loaded: {totalFileCount} Files!");

            // await logAllFiles();
            await TestLoadAsset("FortniteGame/Content/Athena/Items/Cosmetics/Characters/CID_A_198_Athena_Commando_M_CerealBox"); // Really Just A Basic Example On How To Use CUE4Parse
            await Task.Delay(-1);
        }

        private static async Task TestLoadAsset(string assetPath)
        {
            // If No Asset Path Return Error
            if (assetPath == null)
            {
                Log.Error("Invalid Asset Path!");
            }

            else
            {
                // Try To Load The Object, If False Return Error
                if (!Global.Provider.TryLoadObject(assetPath, out UObject package))
                {
                    Log.Error($"Failed To Locate: {assetPath}");
                }

                else
                {
                    // Pass Our UObject Threw Our Model To Parse Data
                    // STILL WORKING ON MODEL For All Export Types, Therefor May Break!
                    CosmeticModel cos = new CosmeticModel(package);
                    // Log.Information($"Parsing: {assetPath}...");
                    // Console.WriteLine("ExportType: " + cos.ExportType);
                    // Console.WriteLine("DisplayName: " + cos.DisplayName);
                    // Console.WriteLine("Description: " + cos.Description);
                    // Console.WriteLine("ShortDescription: " + cos.ShortDescription);
                    // Console.WriteLine("Rarity: " + cos.PreviewRarity);
                    // Console.WriteLine("Icon Path: " + cos.imagePath);
                }
            }
        }

        private static async Task logAllFiles()
        {
            foreach (var file in Global.Provider.Files)
            {
                if (file.Value.ToString().StartsWith("FortniteGame/Content/Athena/Items/Cosmetics/Characters/"))
                {
                    var gameFile = file.Value.ToString();
                    // var allObjects = Global.Provider.LoadAllObjects(gameFile);
                    // var fullJson = JsonConvert.SerializeObject(allObjects, Formatting.Indented);
                    Console.WriteLine("Loaded: " + gameFile);
                }
            }
        }
    }
}