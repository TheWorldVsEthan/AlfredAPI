﻿using Serilog;
using Serilog.Events;
using AlfredAPI.AlfredAPI;
using AlfredAPI.Properties;
using Serilog.Sinks.SystemConsole.Themes;

namespace AlfredAPI
{
    public class Program
    {
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

            // await LoadAsset("FortniteGame/Plugins/GameFeatures/BRCosmetics/Content/Athena/Items/Cosmetics/Characters/Character_SteakSting");
            await Task.Delay(-1);
        }

        /*
        private static async Task LoadAsset(string assetPath)
        {
            if (assetPath == null)
            {
                Log.Error("Invalid Asset Path!");
            }
             
            else
            {
                if (!Global.Provider.TryLoadObject(assetPath, out UObject package))
                {
                    Log.Error($"Failed To Locate: {assetPath}");
                }

                else
                {
                    var fullJson = JsonConvert.SerializeObject(package, Formatting.Indented);
                    Console.WriteLine(fullJson);
                    // CosmeticModel cos = new CosmeticModel(package);
                    // Log.Information($"Parsing: {assetPath}...");
                    // Console.WriteLine("DisplayName: " + cos.DisplayName + " Description: " + cos.Description + " ShortDescription: " + cos.ShortDescription);
                }
            }
        }
        */


    }


    // Will Prolly Add A Task TO Pre-Load Files -_('')-_
    /*
                       foreach (var file in Global.Provider.Files)
            {
                if (file.Value.ToString().StartsWith("FortniteGame/Content/Athena/Items/Cosmetics/Characters/"))
                {
                    var gameFile = file.Value.ToString();
                    var allObjects = Global.Provider.LoadAllObjects(gameFile);
                    // var fullJson = JsonConvert.SerializeObject(allObjects, Formatting.Indented);
                    Console.WriteLine("Loaded: " + gameFile);
                }
            }
           */
}