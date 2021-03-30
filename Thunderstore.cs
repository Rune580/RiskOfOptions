using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Timers;
using SimpleJSON;
using UnityEngine.Events;
using System.Threading;

namespace RiskOfOptions
{
    internal static class Thunderstore
    {
        private static readonly Mutex IconMutex = new Mutex();

        internal static bool doneFetching = false;

        private static List<ModIconInfo> _modIcons;

        internal static void AddStartupEvent()
        {
            _modIcons = new List<ModIconInfo>();


            Debug.Log($"Adding Startup Event");
            ModSettingsManager.addStartupListener(GrabIcons);
        }

        internal static void GrabIcons()
        {
            Thread iconFetchThread = new Thread(new ThreadStart(ThreadedIconFetching));

            iconFetchThread.Start();
        }

        internal static void AddIcon(ModIconInfo modIconInfo)
        {
            _modIcons.Add(modIconInfo);
        }


        internal static ModIconInfo[] GetModIcons()
        {
            ModIconInfo[] modIcons = _modIcons.ToArray();

            return modIcons;
        }

        internal static ModIconInfo GetModIcon(string modGuid)
        {
            foreach (var modIconInfo in _modIcons)
            {
                if (modIconInfo.modGuid == modGuid)
                {
                    return modIconInfo;
                }
            }


            throw new Exception($"No mod icon could be found for: {modGuid}");
        }

        internal static ModSearchEntry[] RemoveIfIconExists(ModSearchEntry[] modStrings)
        {
            List<ModSearchEntry> searchEntries = new List<ModSearchEntry>();
            foreach (var modSearchEntry in modStrings)
            {
                string path = $"{GetMyGamesPath()}{modSearchEntry.modGuid.Replace(".", "_")}-Icon.png";
                if (!File.Exists(path) && !_modIcons.Contains(modSearchEntry))
                {
                    searchEntries.Add(modSearchEntry);
                }
                else
                {
                    _modIcons.Add(new ModIconInfo()
                    {
                        modGuid = modSearchEntry.modGuid,
                        Icon = path
                    });
                }
            }

            return searchEntries.ToArray();
        }

        internal static void ThreadedIconFetching()
        {
            IconMutex.WaitOne();

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            Debug.Log($"Now Fetching Icons...");

            ModSearchEntry[] searchEntries = RemoveIfIconExists(ModSettingsManager.GetIconSearchEntries());

            if (searchEntries.Length > 0)
            {
                _modIcons = FetchModIconsFromList(FetchModList(), searchEntries);
            }

            stopwatch.Stop();
            Debug.Log($"Finished after {stopwatch.Elapsed} seconds");

            doneFetching = true;

            IconMutex.ReleaseMutex();
        }

        private static List<ModIconInfo> FetchModIconsFromList(JSONNode json, ModSearchEntry[] modStrings)
        {
            List<ModIconInfo> icons = new List<ModIconInfo>();

            for (int i = 0; i < json.Count; i++)
            {
                if (!json[i][0].IsString)
                    continue;

                
                foreach (var modString in modStrings)
                {
                    string fullName = json[i]["full_name"];
                    string name = json[i]["name"];

                    // Exact Match Search
                    if (string.Equals(fullName, modString.fullName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ModIconInfo modIconInfo = new ModIconInfo
                        {
                            modGuid = modString.modGuid,
                            Icon = json[i]["versions"][0]["icon"]
                        };

                        icons.Add(modIconInfo);

                        break;
                    }
                    if (string.Equals(fullName, modString.fullNameWithUnderscores, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ModIconInfo modIconInfo = new ModIconInfo
                        {
                            modGuid = modString.modGuid,
                            Icon = json[i]["versions"][0]["icon"]
                        };

                        icons.Add(modIconInfo);

                        break;
                    }
                    if (string.Equals(fullName, modString.fullNameWithoutSpaces, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ModIconInfo modIconInfo = new ModIconInfo
                        {
                            modGuid = modString.modGuid,
                            Icon = json[i]["versions"][0]["icon"]
                        };

                        icons.Add(modIconInfo);

                        break;
                    }

                    // More loose search, only checking mod name and not owner;
                    if (string.Equals(name, modString.nameWithUnderscores, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ModIconInfo modIconInfo = new ModIconInfo
                        {
                            modGuid = modString.modGuid,
                            Icon = json[i]["versions"][0]["icon"]
                        };

                        icons.Add(modIconInfo);

                        break;
                    }
                    if (string.Equals(name, modString.nameWithoutSpaces, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ModIconInfo modIconInfo = new ModIconInfo
                        {
                            modGuid = modString.modGuid,
                            Icon = json[i]["versions"][0]["icon"]
                        };

                        icons.Add(modIconInfo);

                        break;
                    }
                }
            }

            for (int i = 0; i < icons.Count; i++)
            {
                string downloadLink = icons[i].Icon;

                using (WebClient client = new WebClient())
                {
                    string localPath = $"{GetMyGamesPath()}{icons[i].modGuid.Replace(".", "_")}-Icon.png";
                    client.DownloadFile(new Uri(downloadLink), localPath);

                    ModIconInfo modIconInfo = new ModIconInfo()
                    {
                        Icon = localPath,
                        modGuid = icons[i].modGuid
                    };

                    icons[i] = modIconInfo;
                }
            }

            return icons;
        }

        private static string GetMyGamesPath()
        {
            string documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

            if (!Directory.Exists($"{documents}\\My Games"))
                Directory.CreateDirectory($"{documents}\\My Games");

            if (!Directory.Exists($"{documents}\\My Games\\Risk Of Options"))
                Directory.CreateDirectory($"{documents}\\My Games\\Risk Of Options");

            if (!Directory.Exists($"{documents}\\My Games\\Risk Of Options\\icons"))
                Directory.CreateDirectory($"{documents}\\My Games\\Risk Of Options\\icons");

            return $"{documents}\\My Games\\Risk Of Options\\icons\\";
        }

        private static JSONNode FetchModList()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://thunderstore.io/api/v1/package/");

            request.ContentType = "application/json";
            request.Accept = "application/json";

            request.Method = WebRequestMethods.Http.Get;

            WebResponse response = request.GetResponse();


            JSONNode json;

            using (StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
            {
                string jsonText = reader.ReadToEnd();

                json = JSON.Parse(jsonText);
            }

            return json;
        }

        private static bool Contains(this List<ModIconInfo> modIconInfos, ModSearchEntry searchEntry)
        {
            return modIconInfos.Any(modIconInfo => modIconInfo.modGuid == searchEntry.modGuid);
        }

        internal struct ModSearchEntry
        {
            internal string fullName;
            internal string fullNameWithUnderscores;
            internal string fullNameWithoutSpaces;
            internal string nameWithUnderscores;
            internal string nameWithoutSpaces;
            internal string modName;
            internal string modGuid;
        }

        internal struct ModIconInfo
        {
            internal string modGuid;
            internal string Icon;
        }
    }
}
