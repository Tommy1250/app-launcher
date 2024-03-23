using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace app_launcher
{
    public class Utils
    {
        public static bool IsCustomProtocol(string input)
        {
            Regex regex = new Regex(@"^[a-z.]+://");
            return regex.IsMatch(input);
        }

        public static void RunApp(JObject data, string name)
        {
            string location = data[name]["location"].ToString();
            string launchArgs = "";

            if (data[name]["args"] != null)
                launchArgs = data[name]["args"].ToString();

            if (!File.Exists(location))
            {
                Console.WriteLine($"File not found: {location}");
                return;
            }
            else
            {
                DirectoryInfo parentPath = Directory.GetParent(location);
                Directory.SetCurrentDirectory(parentPath.FullName);
            }

            if (launchArgs == "")
            {
                System.Diagnostics.Process.Start(location);
            }
            else
            {
                System.Diagnostics.Process.Start(location, launchArgs);
            }
        }
    }
}
