using Harmony;
using JetBrains.Annotations;
using UnityEngine;

namespace LaserCars
{
    [UsedImplicitly]
    internal class Harmony
    {
        [HarmonyPatch(typeof(CarVisuals), "Start")]
        [UsedImplicitly]
        private class CarVisualsPatch
        {
            // ReSharper disable once InconsistentNaming
            [UsedImplicitly]
            private static void Postfix(CarVisuals __instance)
            {
                if (__instance.GetComponent<Scripts.LaserController>() == null)
                {
                    __instance.gameObject.AddComponent<Scripts.LaserController>();
                }
            }
        }
        
        // [HarmonyPatch(typeof(CarSplit), "Split")]
        // [UsedImplicitly]
        // private class DoNotCloneLaserWhenSplit
        // {
        //     // ReSharper disable once InconsistentNaming
        //     [UsedImplicitly]
        //     private static void Postfix(CarSplit __instance)
        //     {
        //         var rootGameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        //         foreach (var gameObject in rootGameObjects)
        //         {
        //             if (gameObject.name != "MyCarSplit") continue;
        //             
        //             var laser = gameObject.transform.Find("CarLaser");
        //             if (laser != null)
        //             {
        //                 Object.DestroyImmediate(laser.gameObject);
        //             }
        //         }
        //     }
        // }

        [HarmonyPatch(typeof(CheatMenu), "InitializeVirtual")]
        [UsedImplicitly]
        private class CheatMenuPatch
        {
            // ReSharper disable once InconsistentNaming
            [UsedImplicitly]
            private static void Postfix(CheatMenu __instance)
            {
                var achievementManager = G.Sys.Achievements_;

                const EAchievements achievement = EAchievements.Rampage;

                const string cheatName = "THE MIGHTY LAMP";

                var cheatLocked =
                    $"{"To Unlock:".Colorize(Colors.tomato)} Complete: {achievementManager.GetAchievement(achievement).name_}";
                var cheatUnlocked = $"{"Visual".Colorize(Colors.yellowGreen)}: Enlighten your way through the Array";

                if (!achievementManager.HasAchieved(achievement))
                {
                    __instance.TweakAction(GUtils.GetLockedText(cheatName).Colorize(Colors.gray), null, cheatLocked);
                }
                else
                {
                    __instance.TweakBool(cheatName, Entry.GetKey("lamp.cheat", false), (value) =>
                    {
                        Entry.SetKey("lamp.cheat", value);
                        foreach (var controller in UnityEngine.Object.FindObjectsOfType<Scripts.LaserController>())
                            controller.Laser.SetActive(Entry.GetKey("lamp.cheat", false));
                    }, cheatUnlocked);
                }
            }
        }
    }
}