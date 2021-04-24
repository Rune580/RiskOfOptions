using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using SimpleJSON;
using System.Threading;
using RiskOfOptions.Resources;
using UnityEngine;

namespace RiskOfOptions
{
    internal static class Thunderstore
    {
        private static readonly Mutex IconMutex = new Mutex();

        internal static bool doneFetching = false;

        internal static bool loadingFromQueue = false;

        private static List<ModIconInfo> _modIcons;

        private static List<ModIconInfo> _iconQueue;

        public static Sprite defaultIcon;

        internal static void Init()
        {
            _modIcons = new List<ModIconInfo>();

            defaultIcon = Assets.Load<Sprite>("assets/RiskOfOptions/missing_icon.png");
        }

        internal static void GrabIcons()
        {
            Thread iconFetchThread = new Thread(new ThreadStart(ThreadedIconFetching));

            iconFetchThread.Start();
        }

        internal static void AddIcon(string modGuid, Sprite icon)
        {
            if (!doneFetching && !loadingFromQueue)
            {
                if (_iconQueue == null)
                    _iconQueue = new List<ModIconInfo>();

                _iconQueue.Add(new ModIconInfo()
                {
                    modGuid = modGuid,
                    Icon = icon
                });
            }
            else if (doneFetching && !loadingFromQueue)
            {
                _modIcons.Add(new ModIconInfo()
                {
                    modGuid = modGuid,
                    Icon = icon
                });
            }
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

            return new ModIconInfo();
        }

        internal static ModSearchEntry[] RemoveIfIconExists(ModSearchEntry[] modStrings)
        {
            List<ModSearchEntry> searchEntries = new List<ModSearchEntry>();
            foreach (var modSearchEntry in modStrings)
            {
                string path = $"{PathUtils.GetIconsPath()}{modSearchEntry.modGuid.Replace(".", "_")}-Icon.png";
                if (!File.Exists(path) && !_modIcons.Contains(modSearchEntry))
                {
                    searchEntries.Add(modSearchEntry);
                }
                else
                {
                    //Debug.Log($"Found existing icon for {modSearchEntry.modGuid}, path: {path}");
                    _modIcons.Add(new ModIconInfo()
                    {
                        modGuid = modSearchEntry.modGuid,
                        IconPath = path
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
                List<ModIconInfo> onlineIconInfos = FetchModIconsFromList(FetchModList(), searchEntries);

                //Debug.Log($"{onlineIconInfos.Count} Online Icons found.");

                foreach (var onlineIconInfo in onlineIconInfos)
                {
                    _modIcons.Add(onlineIconInfo);
                }
                onlineIconInfos.Clear();
            }

            //Debug.Log(_modIcons.Count);

            foreach (var modIconInfo in _modIcons)
            {
                //Debug.Log($"modGuid: {modIconInfo.modGuid}, IconPath: {modIconInfo.IconPath}, Icon {modIconInfo.Icon}");
            }

            //Debug.Log(loadingFromQueue);

            

            if (_iconQueue != null)
            {
                loadingFromQueue = true;

                //Debug.Log(loadingFromQueue);

                //Debug.Log($"Loading icons from queue. {_iconQueue.Count} Icons in queue");

                foreach (var modIconInfo in _iconQueue)
                {
                    _modIcons.Add(modIconInfo);
                }

                _iconQueue.Clear();

                //Debug.Log($"Finished loading from queue");

                loadingFromQueue = false;
            }

            doneFetching = true;

            stopwatch.Stop();
            //Debug.Log($"Finished after {stopwatch.Elapsed} seconds");

            IconMutex.ReleaseMutex();
        }

        private static List<ModIconInfo> FetchModIconsFromList(JSONNode json, ModSearchEntry[] modStrings)
        {
            List<ModIconInfo> icons = new List<ModIconInfo>();

            if (json == typeof(JSONNull))
            {
                icons.AddRange(modStrings.Select(modSearchEntry => new ModIconInfo() {IconPath = "", modGuid = modSearchEntry.modGuid}));

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

                    //Debug.Log($"Starting search for {modString}");

                    // Exact Match Search
                    if (string.Equals(fullName, modString.fullName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ModIconInfo modIconInfo = new ModIconInfo
                        {
                            modGuid = modString.modGuid,
                            IconPath = json[i]["versions"][0]["icon"]
                        };

                        icons.Add(modIconInfo);

                        break;
                    }
                    if (string.Equals(fullName, modString.fullNameWithUnderscores, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ModIconInfo modIconInfo = new ModIconInfo
                        {
                            modGuid = modString.modGuid,
                            IconPath = json[i]["versions"][0]["icon"]
                        };

                        icons.Add(modIconInfo);

                        break;
                    }
                    if (string.Equals(fullName, modString.fullNameWithoutSpaces, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ModIconInfo modIconInfo = new ModIconInfo
                        {
                            modGuid = modString.modGuid,
                            IconPath = json[i]["versions"][0]["icon"]
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
                            IconPath = json[i]["versions"][0]["icon"]
                        };

                        icons.Add(modIconInfo);

                        break;
                    }
                    if (string.Equals(name, modString.nameWithoutSpaces, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ModIconInfo modIconInfo = new ModIconInfo
                        {
                            modGuid = modString.modGuid,
                            IconPath = json[i]["versions"][0]["icon"]
                        };

                        icons.Add(modIconInfo);

                        break;
                    }

                    //Debug.Log($"Finished search for {modString}");
                }
            }

            Debug.Log("Downloading Icons");

            for (int i = 0; i < icons.Count; i++)
            {
                string downloadLink = icons[i].IconPath;

                //Debug.Log($"Downloading icon for {icons[i].modGuid}, from: {downloadLink}");

                using (WebClient client = new WebClient())
                {
                    string localPath = $"{PathUtils.GetIconsPath()}{icons[i].modGuid.Replace(".", "_")}-Icon.png";
                    client.DownloadFile(new Uri(downloadLink), localPath);

                    ModIconInfo modIconInfo = new ModIconInfo()
                    {
                        IconPath = localPath,
                        modGuid = icons[i].modGuid
                    };

                    icons[i] = modIconInfo;
                }
            }

            Debug.Log("Finished downloading Icons");

            return icons;
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

                    using (StreamWriter sw = new StreamWriter($"{PathUtils.GetMyGamesPath()}thunderstore-packagelist.json", false))
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

                if (!File.Exists($"{PathUtils.GetMyGamesPath()}thunderstore-packagelist.json"))
                {
                    Debug.Log("Failed to load cached package list and Thunderstore API is inaccessible. All non-cached auto generated icons will be replaced with placeholders.");
                    return json;
                }
                using (StreamReader reader = new StreamReader($"{PathUtils.GetMyGamesPath()}thunderstore-packagelist.json"))
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
            internal string IconPath;
            internal Sprite Icon;
        }
    }
}
