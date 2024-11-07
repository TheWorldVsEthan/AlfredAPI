using Serilog;
using Serilog.Events;
using AlfredAPI.AlfredAPI;
using AlfredAPI.Properties;
using Serilog.Sinks.SystemConsole.Themes;
using AlfredAPI.Extraction;

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

            // UI
            Console.Clear();
            Console.WriteLine("  Welcome To AlfredAPI!");
            Console.WriteLine("  Please Enter Exports Folder Path: ");

            string exportsFolder = Console.ReadLine().ToString();
            Settings.exportsFolder = exportsFolder;

            Console.WriteLine($"  Exports Will be Sent To: {Settings.exportsFolder}");
            Console.Clear();

            Console.WriteLine("  Welcome To AlfredAPI!");
            Console.WriteLine("  Please Select From The Following Options!");
            Console.WriteLine("  1: Extract Cosmetic JSON Data(Currently Logs Parsed Data To Console And Saves Texture2D To Exports Folder!)");
            // Console.WriteLine("  2: Extract Cosmetic Image");
            // Console.WriteLine("  3: Extract Audio(Only Supports USoundWave ATM)");
            
            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("  Please Enter Asset Path: ");
                    string assetPath = Console.ReadLine().ToString();
                    await CosmeticExtractor.LoadAsset(assetPath);
                    break;

            }

            // await CosmeticExtractor.LoadAsset("FortniteGame/Content/Athena/Items/Cosmetics/Backpacks/BID_755_Hardwood_4KH3V");
            // await AudioExtractor.LoadAudio("FortniteGame/Content/Athena/Sounds/Emotes/LemonCart/Emote_LemonCart_Music_Loop.uasset");
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