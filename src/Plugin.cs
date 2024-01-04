using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;

namespace LaserCars;

[BepInPlugin("pw.seekr.plugins.laser-cars", PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInProcess("Distance.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static bool Cheated;
    internal static ConfigEntry<bool> Enabled;
        
    private void Awake()
    {
        Enabled = Config.Bind("General", "Enabled", true, "Enable the mod");
            
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }
}
    
[HarmonyPatch(typeof(CarVisuals), "Start")]
[UsedImplicitly]
internal static class CarVisualsPatch
{
    // ReSharper disable once InconsistentNaming
    [UsedImplicitly]
    private static void Postfix(CarVisuals __instance)
    {
        if (__instance.GetComponent<LaserController>() == null)
        {
            __instance.gameObject.AddComponent<LaserController>();
        }
    }
}
    
[HarmonyPatch(typeof(CheatsManager))]
[HarmonyPatch("GameplayCheatsUsedThisLevel_", MethodType.Getter)]
internal static class BlockLeaderboardUpdatingWhenCheating
{
    [UsedImplicitly]
    // ReSharper disable once InconsistentNaming
    private static void Postfix(ref bool __result)
    {
        __result |= Plugin.Cheated;
    }
}

[HarmonyPatch(typeof(GameManager), "SceneLoaded")]
internal static class PatchSceneLoaded
{
    [UsedImplicitly]
    private static void Postfix()
    {
        if (!Plugin.Enabled.Value)
        {
            Plugin.Cheated = false;
        }
    }
}