using Newtonsoft.Json.Linq;
using app_launcher;
using System;
using System.Reflection;
using System.Xml.Linq;

namespace LaunchApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Tommy's app launcher";

            if (args.Length == 0)
            {
                Console.WriteLine("No launch argument provided.");
                return;
            }

            string shortcutsFile = Path.Combine(AppContext.BaseDirectory, "files.json");
            string aliasesFile = Path.Combine(AppContext.BaseDirectory, "aliases.json");

            if (args[0].ToLower() == "launch")
            {
                if (args.Length == 1)
                {
                    Console.WriteLine("You must provide the application argument!");
                    

                    if (!File.Exists(shortcutsFile))
                    {
                        Console.WriteLine($"File not found: {shortcutsFile}");
                        return;
                    }

                    if (!File.Exists(aliasesFile))
                    {
                        Console.WriteLine("No aliases found");
                    }

                    try
                    {
                        string json = File.ReadAllText(shortcutsFile);
                        JObject data = JObject.Parse(json);

                        Console.WriteLine("Here are the avalable items.");
                        foreach(var item in data)
                        {
                            Console.WriteLine($"{item.Key} statrs at {item.Value["location"].ToString()}");
                        }

                        if (File.Exists(aliasesFile))
                        {
                            string aliasesjson = File.ReadAllText(aliasesFile);
                            JObject aliases = JObject.Parse(aliasesjson);

                            Console.WriteLine("Here are the avalable aliases.");
                            foreach (var item in aliases)
                            {
                                Console.WriteLine($"{item.Key} refers to {item.Value.ToString()}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        return;
                    }
                    return;
                }
                string name = args[1];

                if (!File.Exists(shortcutsFile))
                {
                    Console.WriteLine($"File not found: {shortcutsFile}");
                    return;
                }

                try
                {
                    string json = File.ReadAllText(shortcutsFile);
                    JObject data = JObject.Parse(json);

                    if (File.Exists(aliasesFile))
                    {
                        string aliasesjson = File.ReadAllText(aliasesFile);
                        JObject aliases = JObject.Parse(aliasesjson);

                        if (aliases[name] != null)
                        {
                            if (data[aliases[name].ToString()] != null)
                            {
                                Utils.RunApp(data, aliases[name].ToString());
                                return;
                            }
                        }
                    }

                    if (data[name] == null)
                    {
                        Console.WriteLine($"Location not found for {name}.");
                        return;
                    }

                    Utils.RunApp(data, name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }else if (args[0].ToLower() == "add")
            {
                if (args.Length < 3)
                {
                    Console.WriteLine("The location and the name argument must be provided");
                    return;
                }

                string name = args[1];
                string location = args[2];
                string commandArgs = "";
                try
                {
                    if(!File.Exists(shortcutsFile))
                    {
                        File.WriteAllText(shortcutsFile, "{}");
                    }
                    string json = File.ReadAllText(shortcutsFile);
                    JObject data = JObject.Parse(json);

                    if (!File.Exists(location) && !Utils.IsCustomProtocol(location))
                    {
                        Console.WriteLine("The path provided isn't correct");
                        return;
                    }

                    if (args.Length >= 4)
                    {
                        commandArgs = args[3];
                    }

                    if(commandArgs == "")
                    {
                        if (Utils.IsCustomProtocol(location))
                        {
                            JProperty newProperty = new JProperty(name, new JObject(
                                new JProperty("location", "C:\\Windows\\System32\\rundll32.exe"),
                                new JProperty("args", "url.dll,FileProtocolHandler " + location)
                            ));

                            // Add the new property to the JObject
                            data.Add(newProperty);

                            string outputJson = data.ToString();

                            File.WriteAllText(shortcutsFile, outputJson);
                        }
                        else
                        {
                            // Create a new property to add to the JObject
                            JProperty newProperty = new JProperty(name, new JObject(
                                new JProperty("location", location)));

                            // Add the new property to the JObject
                            data.Add(newProperty);

                            string outputJson = data.ToString();

                            File.WriteAllText(shortcutsFile, outputJson);
                        }
                    }
                    else
                    {
                        // Create a new property to add to the JObject
                        JProperty newProperty = new JProperty(name, new JObject(
                            new JProperty("location", location), 
                            new JProperty("args", commandArgs)
                        ));

                        // Add the new property to the JObject
                        data.Add(newProperty);

                        string outputJson = data.ToString();

                        File.WriteAllText(shortcutsFile, outputJson);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }
            else if (args[0].ToLower() == "remove")
            {
                if (args.Length == 1)
                {
                    Console.WriteLine("You must provide the application argument!");
                    
                    if (!File.Exists(shortcutsFile))
                    {
                        Console.WriteLine($"File not found: {shortcutsFile}");
                        return;
                    }

                    try
                    {
                        string json = File.ReadAllText(shortcutsFile);
                        JObject data = JObject.Parse(json);

                        Console.WriteLine("Here are the avalable items.");
                        foreach (var item in data)
                        {
                            Console.WriteLine($"{item.Key} statrs at {item.Value["location"].ToString()}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        return;
                    }
                    return;
                }

                string name = args[1];

                if (!File.Exists(shortcutsFile))
                {
                    Console.WriteLine($"File not found: {shortcutsFile}");
                    return;
                }

                try
                {
                    string json = File.ReadAllText(shortcutsFile);
                    JObject data = JObject.Parse(json);

                    if (data[name] == null)
                    {
                        Console.WriteLine($"Location not found for {name}.");
                        return;
                    }

                    data.Remove(name);

                    File.WriteAllText(shortcutsFile, data.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }
            else if (args[0].ToLower() == "list")
            {

                if (!File.Exists(shortcutsFile))
                {
                    Console.WriteLine($"File not found: {shortcutsFile}");
                    return;
                }

                try
                {
                    string json = File.ReadAllText(shortcutsFile);
                    JObject data = JObject.Parse(json);

                    foreach (var item in data)
                    {
                        string consoleString = $"App: {item.Key}, Shortcut: {item.Value["location"]} ";
                        if (item.Value["args"] != null)
                            consoleString += item.Value["args"];
                        Console.WriteLine(consoleString);
                    }

                    if (File.Exists(aliasesFile))
                    {
                        string aliasesjson = File.ReadAllText(aliasesFile);
                        JObject aliases = JObject.Parse(aliasesjson);

                        Console.WriteLine("Here are the avalable aliases.");
                        foreach (var item in aliases)
                        {
                            Console.WriteLine($"{item.Key} refers to {item.Value.ToString()}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }
            else if (args[0].ToLower() == "alias")
            {

                if (args.Length < 3)
                {
                    Console.WriteLine("Usage AppLauncher alias (add/remove) [Alias] <shortcutName>");
                    return;
                }

                string type = args[1];
                string alias = args[2];
                

                if(type.ToLower() == "add")
                {
                    if(args.Length >= 4) { 
                        string shortcut = args[3];
                        if (!File.Exists(aliasesFile))
                        {
                            File.WriteAllText(aliasesFile, "{}");
                        }

                        string aliasesJson = File.ReadAllText(aliasesFile);
                        JObject aliasesData = JObject.Parse(aliasesJson);

                        string shortcutsJson = File.ReadAllText(shortcutsFile);
                        JObject shortCutData = JObject.Parse(shortcutsJson);

                        if (shortCutData[shortcut] == null)
                        {
                            Console.WriteLine("The shortcut provided doesn't exist");
                            return;
                        }

                        JProperty newAlias = new JProperty(alias, shortcut);

                        // Add the new property to the JObject
                        aliasesData.Add(newAlias);

                        string outputJson = aliasesData.ToString();

                        File.WriteAllText(aliasesFile, outputJson);
                    }
                    else
                    {
                        Console.WriteLine("You must provide an existing shortcut name");
                    }
                }
                else if(type.ToLower() == "remove")
                {
                    if (!File.Exists(aliasesFile))
                    {
                        Console.WriteLine("You don't have any aliases nor an aliases file for that matter");
                        return;
                    }

                    string aliasesJson = File.ReadAllText(aliasesFile);
                    JObject aliasesData = JObject.Parse(aliasesJson);

                    if (aliasesData[alias] == null)
                    {
                        Console.WriteLine("This alias doesn't exist");
                        return;
                    }

                    aliasesData.Remove(alias);

                    File.WriteAllText (aliasesFile, aliasesData.ToString());
                }
                else
                {
                    Console.WriteLine($"Usage {AppContext.BaseDirectory} alias (add/remove) [Alias] <shortcutName>");
                }
            }
            else if (args[0].ToLower() == "help")
            {
                Console.WriteLine("This a simple launcher made for the cmd by Tommy\nThe commands are:\n1. launch <appName>\nlaunches an app\nIf no appName was provided the launcher will list all avalable apps\n2. add <appName> <appLocation> <args(optional)>\nadds an app shortcut to the list\n3. remove <appName>\nRemoves the provided app name from the list\nThe actual app is not touched\n4. list\nLists all avalable shorcuts in an extensive format\n5. prints the app location");
            }
            else if (args[0].ToLower() == "getlocation")
            {
                Console.WriteLine(AppContext.BaseDirectory);
            }
            else if (args[0].ToLower() == "listjson")
            {
                if (!File.Exists(shortcutsFile))
                {
                    Console.WriteLine($"File not found: {shortcutsFile}");
                    return;
                }

                try
                {
                    string json = File.ReadAllText(shortcutsFile);
                    Console.WriteLine(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Invalid option");
            }
        }
    }
}