using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Timers;
using SimpleJSON;
using UnityEngine.Events;
using System.Threading;
using On.Unity;
using UnityEngine;

namespace RiskOfOptions
{
    internal static class Thunderstore
    {
        private static readonly Mutex IconMutex = new Mutex();

        internal static bool doneFetching = false;

        private static List<ModIconInfo> _modIcons;

        public static Sprite defaultIcon;

        internal static void Init()
        {
            _modIcons = new List<ModIconInfo>();


            defaultIcon = Resources.Load<Sprite>("@RiskOfOptions:assets/RiskOfOptions/missing_icon.png");

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
                string path = $"{GetIconsPath()}{modSearchEntry.modGuid.Replace(".", "_")}-Icon.png";
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

            if (json == typeof(JSONNull))
            {
                icons.AddRange(modStrings.Select(modSearchEntry => new ModIconInfo() {Icon = "", modGuid = modSearchEntry.modGuid}));

                return icons;
            }

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

                //Debug.Log($"Downloading icon for {icons[i].modGuid}, from: {downloadLink}");

                using (WebClient client = new WebClient())
                {
                    string localPath = $"{GetIconsPath()}{icons[i].modGuid.Replace(".", "_")}-Icon.png";
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

        private static string GetIconsPath()
        {
            string path = GetMyGamesPath();

            if (!Directory.Exists($"{path}\\icons"))
                Directory.CreateDirectory($"{path}\\icons");

            return $"{path}icons\\";
        }

        private static string GetMyGamesPath()
        {
            string documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

            if (!Directory.Exists($"{documents}\\My Games"))
                Directory.CreateDirectory($"{documents}\\My Games");

            if (!Directory.Exists($"{documents}\\My Games\\Risk Of Options"))
                Directory.CreateDirectory($"{documents}\\My Games\\Risk Of Options");

            return $"{documents}\\My Games\\Risk Of Options\\";
        }

        private static JSONNode FetchModList()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://thunderstore.io/api/v1/");

            request.Method = WebRequestMethods.Http.Head;

            JSONNode json = new JSONNull();

            if (request.GetResponse() is HttpWebResponse statusResponse && statusResponse.StatusCode == HttpStatusCode.OK)
            {
                Debug.Log("Thunderstore API is online! Fetching package list.");

                request = (HttpWebRequest)WebRequest.Create("https://thunderstore.io/api/v1/package/");

                request.ContentType = "application/json";
                request.Accept = "application/json";

                request.Method = WebRequestMethods.Http.Get;

                WebResponse response = request.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    string jsonText = reader.ReadToEnd();

                    Debug.Log("Finished downloading the package list from the Thunderstore API.");

                    using (StreamWriter sw = new StreamWriter($"{GetMyGamesPath()}thunderstore-packagelist.json", false))
                    {
                        sw.Write(jsonText);
                    }

                    Debug.Log("Cached package list for later use.");

                    json = JSON.Parse(jsonText);
                }
            }
            else
            {
                Debug.Log("Thunderstore API couldn't be accessed! Attempting to use cached package list.");

                if (!File.Exists($"{GetMyGamesPath()}thunderstore-packagelist.json"))
                {
                    Debug.Log("Failed to load cached package list and Thunderstore API is inaccessible. All non-cached auto generated icons will be replaced with placeholders.");
                    return json;
                }
                using (StreamReader reader = new StreamReader($"{GetMyGamesPath()}thunderstore-packagelist.json"))
                {
                    json = JSON.Parse(reader.ReadToEnd());
                }
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
