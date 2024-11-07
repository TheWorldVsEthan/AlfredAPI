using AlfredAPI.AlfredAPI.Services;
using Serilog;

namespace AlfredAPI.AlfredAPI
{
    public class Manager
    {
        private static ILogger logger = Log.ForContext("Title", "Services @ Manager");

        public async Task StartAsync()
        {
            logger.Information("Starting Mount...");
            await Provider.Mount();
            await FetchAES();

            // Don't Need This Unless You Wanna Keep This Running For Mining :)
            // RunInBackground(TimeSpan.FromSeconds(5), () =>
            // {
                // Provider.Mount();
                // FetchAES();
            // });
        }

        private async Task FetchAES()
        {
            var aes = await AES.Grab(Global.Manifest?.BuildVersion ?? "none");
            if (aes == null) return;


            foreach (var (guid, key) in aes) 
            {
                await Global.Provider?.SubmitKeyAsync(guid, key)!; 
            }
        }

        private async Task RunInBackground(TimeSpan timeSpan, Action action)
        {
            var periodicTimer = new PeriodicTimer(timeSpan);
            while (await periodicTimer.WaitForNextTickAsync()) action();
        }
    }
}