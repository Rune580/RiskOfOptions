using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using UnityEngine;

namespace RiskOfOptions.Config;

public class BepInExModConfigProvider : IModConfigProvider
{
    public IEnumerable<IModConfig> GetModConfigs()
    {
        var modConfigs = new HashSet<BepInExModConfig>();
        foreach (var (modGuid, pluginInfo) in Chainloader.PluginInfos)
        {
            var configItems = new List<BepInExConfigItem>();

            foreach (var (_, configEntryBase) in pluginInfo.Instance.Config)
            {
                // var id = new OptionId(modGuid, definition.Section, definition.Key);

                switch (configEntryBase)
                {
                    case ConfigEntry<bool> boolConfigEntry:
                        configItems.Add(new BepInExConfigItem<bool>(boolConfigEntry));
                        break;
                    case ConfigEntry<float> floatConfigEntry:
                        configItems.Add(new BepInExConfigItem<float>(floatConfigEntry));
                        break;
                    case ConfigEntry<int> intConfigEntry:
                        configItems.Add(new BepInExConfigItem<int>(intConfigEntry));
                        break;
                    case ConfigEntry<KeyboardShortcut> keyConfigEntry:
                        configItems.Add(new BepInExConfigItem<KeyboardShortcut>(keyConfigEntry));
                        break;
                    case ConfigEntry<Color> colorConfigEntry:
                        configItems.Add(new BepInExConfigItem<Color>(colorConfigEntry));
                        break;
                    case ConfigEntry<string> stringConfigEntry:
                        configItems.Add(new BepInExConfigItem<string>(stringConfigEntry));
                        break;
                    default:
                    {
                        if (configEntryBase.SettingType.IsEnum)
                        {
                            configItems.Add(new BepInExObjectConfigItem(configEntryBase));
                        }

                        break;
                    }
                }
            }
            
            var searchDir = Path.GetFullPath(pluginInfo.Location);
            var parent = Directory.GetParent(searchDir);
            while (parent is not null && !string.Equals(parent.Name, "plugins", StringComparison.OrdinalIgnoreCase))
            {
                searchDir = parent.FullName;
                parent = Directory.GetParent(searchDir);
            }

            Sprite? icon = null;
            
            var iconPath = Directory.EnumerateFiles(searchDir, "icon.png", SearchOption.AllDirectories).FirstOrDefault();
            if (iconPath is not null)
            {
                var texture = new Texture2D(256, 256);
                if (texture.LoadImage(File.ReadAllBytes(iconPath)) && texture)
                {
                    icon = Sprite.Create(
                        texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f),
                        100
                    );
                }
            }

            modConfigs.Add(new BepInExModConfig(modGuid, pluginInfo.Metadata.Name, [.. configItems], icon, ""));
        }

        return modConfigs;
    }
}