using Serilog;
using System.Reflection;

namespace AlfredAPI.Properties
{
    public static class Settings
    {
        public static Assembly AppAssembly = Assembly.GetExecutingAssembly();
        public static string Version = AppAssembly.GetName().Version?.ToString(4)!;
        public static string Name = AppAssembly.GetName().Name!;

        public static string Environment = "Development";
    }
}