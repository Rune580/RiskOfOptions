using System.Collections.Generic;
using RiskOfOptions.Lib;
using UnityEngine;

namespace RiskOfOptions.Config;

public interface IModConfig
{
    public string ModGuid { get; }
    
    public string ModName { get; }
    
    public IEnumerable<IConfigItem> GetConfigItems();
    
    public Sprite? ModIcon { get; }
    
    public StringOrToken ModDescription { get; }
}