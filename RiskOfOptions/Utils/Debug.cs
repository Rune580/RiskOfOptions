using BepInEx.Logging;

namespace RiskOfOptions.Utils;

internal static class Debug
{
    internal static ManualLogSource? Logger { get; set; }

    public static void Error(object data) => Error(data.ToString());
    
    public static void Warn(object data) => Warn(data.ToString());
    
    public static void Info(object data) => Info(data.ToString());

    public static void Error(string msg)
    {
        if (Logger is null)
        {
            UnityEngine.Debug.LogError($"[{MyPluginInfo.PLUGIN_NAME}] [Error] {msg}");
        }
        else
        {
            Logger.LogError(msg);
        }
    }
    
    public static void Warn(string msg)
    {
        if (Logger is null)
        {
            UnityEngine.Debug.LogWarning($"[{MyPluginInfo.PLUGIN_NAME}] [Warning] {msg}");
        }
        else
        {
            Logger.LogWarning(msg);
        }
    }
    
    public static void Info(string msg)
    {
        if (Logger is null)
        {
            UnityEngine.Debug.Log($"[{MyPluginInfo.PLUGIN_NAME}] [Info] {msg}");
        }
        else
        {
            Logger.LogInfo(msg);
        }
    }
}