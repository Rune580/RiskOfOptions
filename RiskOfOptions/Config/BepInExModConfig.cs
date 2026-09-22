using System.Collections.Generic;
using RiskOfOptions.Lib;
using UnityEngine;

namespace RiskOfOptions.Config;

public class BepInExModConfig(string modGuid, string modName, BepInExConfigItem[] configItems, Sprite? icon, StringOrToken description) : IModConfig
{
    public string ModGuid => modGuid;
    public string ModName => modName;

    public IEnumerable<IConfigItem> GetConfigItems() => configItems;

    public Sprite? ModIcon => icon;

    public StringOrToken ModDescription => description;
}