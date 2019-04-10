using Harmony;

#pragma warning disable IDE0051
namespace LampCheat
{
    class Harmony
    {
        [HarmonyPatch(typeof(CheatMenu), "InitializeVirtual")]
        class CheatMenu_Patch
        {
            static void Postfix(CheatMenu __instance)
            {
                AchievementManager am = G.Sys.Achievements_;

                EAchievements achievement = EAchievements.Rampage;

                string cheatname = "THE MIGHTY LAMP";

                string cheatlocked = string.Format("{0} Complete: {1}", "To Unlock:".Colorize(Colors.tomato), am.GetAchievement(achievement).name_);
                string cheatunlocked = string.Format("{0}: Enlighten your way trough the Array", "Visual".Colorize(Colors.yellowGreen));

                if (!am.HasAchieved(achievement)) {
                    __instance.TweakAction(GUtils.GetLockedText(cheatname).Colorize(Colors.gray), null, cheatlocked);
                }
                else
                {
                    __instance.TweakBool(cheatname, Entry.GetKey("lamp.cheat", false), (value) => {
                        Entry.SetKey("lamp.cheat", value);
                    }, cheatunlocked);
                }
            }
        }
    }
}
