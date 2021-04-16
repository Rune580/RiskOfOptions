using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RiskOfOptions.Containers;
using RiskOfOptions.Options;
using UnityEngine;

namespace RiskOfOptions
{
    internal static class OptionSerializer
    {
        private static List<OptionData> _cache;
        private static Dictionary<string, StringBuilder> _stringBuilders;
        public static void Save(OptionContainer[] containers)
        {
            _cache ??= new List<OptionData>();

            foreach (var container in containers)
            {
                foreach (var option in container.GetModOptionsCached())
                {
                    _cache.SaveOption(option);
                }
            }

            _cache.Write();
        }

        public static string Load(string consoleToken)
        {
            if (_cache == null)
                ReadAll();

            OptionData option = _cache.LoadOption(consoleToken);

            return option.ConsoleToken == "" ? "" : option.Value;
        }

        private static void ReadAll()
        {
            _cache = new List<OptionData>();

            if (!File.Exists(PathUtils.GetStoragePath()))
                return;

            string[] lines = File.ReadAllLines(PathUtils.GetStoragePath());

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("[") && lines[i].EndsWith("]"))
                {
                    var modGuid = lines[i].Replace("[", "").Replace("]", "");

                    i++;

                    if (lines[i].StartsWith("{"))
                    {
                        i++;

                        while (i < lines.Length && !lines[i].StartsWith("}"))
                        {
                            string[] splitLine = lines[i].Split(':');

                            string consoleToken = splitLine[0].Trim();
                            string value = splitLine[1].Trim();

                            _cache.Add(new OptionData(modGuid, consoleToken, value));

                            i++;
                        }
                    }
                }
            }
        }

        private static void Write(this List<OptionData> cache)
        {
            StringBuilder sb = new StringBuilder();

            _stringBuilders = new Dictionary<string, StringBuilder>();

            foreach (var optionData in cache.Where(optionData => !_stringBuilders.ContainsKey(optionData.ModGuid)))
            {
                _stringBuilders.Add(optionData.ModGuid, new StringBuilder());
            }

            foreach (var modGuid in _stringBuilders.Keys)
            {
                _stringBuilders[modGuid].AppendLine($"[{modGuid}]");
                _stringBuilders[modGuid].AppendLine("{");

                foreach (var optionData in cache.Where(optionData => optionData.ModGuid == modGuid))
                {
                    _stringBuilders[modGuid].AppendLine($"{optionData.ConsoleToken} : {optionData.Value}");
                }

                _stringBuilders[modGuid].AppendLine("}");

                sb.AppendLine(_stringBuilders[modGuid].ToString());
            }

            _stringBuilders.Clear();

            using (StreamWriter writer = File.CreateText(PathUtils.GetStoragePath()))
            {
                writer.Write(sb.ToString());
            }
        }

        private static void SaveOption(this List<OptionData> cache, RiskOfOption option)
        {
            for (int i = 0; i < cache.Count; i++)
            {
                var optionData = cache[i];

                if (!string.Equals(optionData.ConsoleToken, option.ConsoleToken, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                optionData.Value = option.GetInternalValueAsString();
                cache[i] = optionData;

                return;
            }

            cache.Add(new OptionData(option.ModGuid, option.ConsoleToken, option.GetValueAsString()));
        }

        private static OptionData LoadOption(this List<OptionData> cache, string consoleToken)
        {
            for (int i = 0; i < cache.Count; i++)
            {
                if (!string.Equals(cache[i].ConsoleToken, consoleToken, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                return cache[i];
            }

            return default;
        }


        private struct OptionData
        {
            internal OptionData(string modGuid, string consoleToken, string value)
            {
                ModGuid = modGuid;
                ConsoleToken = consoleToken;
                Value = value;
            }

            public string ModGuid;
            public string ConsoleToken;
            public string Value;
        }
    }
}
