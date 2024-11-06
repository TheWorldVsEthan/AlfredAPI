using Serilog;
using Serilog.Events;
using AlfredAPI.AlfredAPI;
using AlfredAPI.Properties;
using Serilog.Sinks.SystemConsole.Themes;
using AlfredAPI.Extraction;
using CUE4Parse.FileProvider.Objects;

namespace AlfredAPI
{
    public class Program
    {
        private static async Task Main()
        {
            // Setup Our Serilog Logger
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

            // Get Total Loaded File Count
            Int32 totalFileCount = Global.Provider.Files.Count;
            Log.Information($"Successfully Loaded: {totalFileCount} Files!");

            // await CosmeticExtractor.LoadAsset("FortniteGame/Content/Athena/Items/Cosmetics/Backpacks/BID_755_Hardwood_4KH3V");
            await AudioExtractor.LoadAudio("FortniteGame/Content/Athena/Sounds/Emotes/LemonCart/Emote_LemonCart_Music_Loop.uasset");
            // await logAllFiles();
            await Task.Delay(-1);
        }

        private static async Task logAllFiles()
        {
            foreach (var file in Global.Provider.Files)
            {
                string fileValue = file.Value.ToString();
                if (fileValue.StartsWith("FortniteGame/Content/Athena/Sounds/Emotes/"))
                {
                    // var allObjects = Global.Provider.LoadAllObjects(fileValue);
                    // var fullJson = JsonConvert.SerializeObject(allObjects, Formatting.Indented);
                    // Console.WriteLine(fullJson);
                    Console.WriteLine("Loaded: " + fileValue);
                }
            }
        }
    }
}