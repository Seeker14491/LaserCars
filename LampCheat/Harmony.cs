using Harmony;

#pragma warning disable IDE0051
namespace LampCheat
{
    class Harmony
    {
        [HarmonyPatch(typeof(CarVisuals), "Start")]
        class CarVisuals_Patch
        {
            static void Postfix(CarVisuals __instance)
            {
                Scripts.LampController controller = __instance.gameObject.AddComponent<Scripts.LampController>();
                controller.SetCarLogic(__instance.gameObject.GetComponentInParent<CarLogic>());
            }
        }


        [HarmonyPatch(typeof(CheatMenu), "InitializeVirtual")]
        class CheatMenu_Patch
        {
            static void Postfix(CheatMenu __instance)
            {
                AchievementManager am = G.Sys.Achievements_;

                EAchievements achievement = EAchievements.Rampage;

                string cheatname = "THE MIGHTY LAMP";

                string cheatlocked = string.Format("{0} Complete: {1}", "To Unlock:".Colorize(Colors.tomato), am.GetAchievement(achievement).name_);
                string cheatunlocked = string.Format("{0}: Enlighten your way through the Array", "Visual".Colorize(Colors.yellowGreen));

                if (!am.HasAchieved(achievement)) {
                    __instance.TweakAction(GUtils.GetLockedText(cheatname).Colorize(Colors.gray), null, cheatlocked);
                }
                else
                {
                    __instance.TweakBool(cheatname, Entry.GetKey("lamp.cheat", false), (value) => {
                        Entry.SetKey("lamp.cheat", value);
                        foreach (Scripts.LampController controller in UnityEngine.Object.FindObjectsOfType<Scripts.LampController>())
                            controller.lamp.SetActive(Entry.GetKey("lamp.cheat", false));
                    }, cheatunlocked);
                }
            }
        }
    }
}
