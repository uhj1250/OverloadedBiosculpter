using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using HarmonyLib;
using BiosculpterAcceleration;
using FasterBiosculpterPod;

namespace BiosculpterAcceleration.Module_FasterBioPod
{
    [StaticConstructorOnStartup]
    internal static class Init
    {
        static Init()
        {
            Harmony harmony = new Harmony("BiosculpterAcceleration");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(Patch_CompBiosculpterPod))]
    public static class Patch_Patch_CompBiosculpterPod
    {
        public static Settings settingcache = LoadedModManager.GetMod<FasterBiosculpterPod.FasterBiosculpterPod>().GetSettings<Settings>();
        

        [HarmonyPatch("get_NutritionRequired")]
        [HarmonyPostfix]
        public static void NutritionRequired(ref float __result)
        {
            __result = settingcache.NutritionRequired;
        }




    }




}
