using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using HarmonyLib;
using UnityEngine;

namespace BiosculpterAcceleration
{
    [HarmonyPatch(typeof(CompBiosculpterPod))]
    public static class Patch_CompBiosculpterPod
    {
        public static float NutritionRequired
        {
            get
            {
                return 5f;
            }
        }


        [HarmonyPatch("get_RequiredNutritionRemaining")]
        [HarmonyPrefix]
        public static bool RequiredNutritionRemaining(ref float __result, CompBiosculpterPod __instance, float ___liquifiedNutrition)
        {
            CompBiosculpterOverload comp = __instance.parent.TryGetComp<CompBiosculpterOverload>();
            if (comp != null)
            {
                __result = Mathf.Max(NutritionRequired * comp.LoadMultiplier - ___liquifiedNutrition, 0f);
                return false;
            }
            return true;
        }

        [HarmonyPatch("get_CycleSpeedFactor")]
        [HarmonyPostfix]
        public static void CycleSpeedFactor(ref float __result, CompBiosculpterPod __instance, float ___liquifiedNutrition, float ___currentCycleTicksRemaining)
        {
            CompBiosculpterOverload comp = __instance.parent.TryGetComp<CompBiosculpterOverload>();
            if (comp != null)
            {
                if (___currentCycleTicksRemaining == 0) comp.CycleFactor = ___liquifiedNutrition / NutritionRequired;
                __result *= comp.CycleFactor;
            }
        }

        [HarmonyPatch("LiquifyNutrition")]
        [HarmonyPrefix]
        public static bool LiquifyNutrition(CompBiosculpterPod __instance, ThingOwner ___innerContainer, ref float ___liquifiedNutrition, float ___currentCycleTicksRemaining)
        {
            CompBiosculpterOverload comp = __instance.parent.TryGetComp<CompBiosculpterOverload>();
            if (comp != null)
            {
                foreach (Thing thing in ___innerContainer)
                {
                    float num = thing.GetStatValue(StatDefOf.Nutrition, true) * thing.stackCount;
                    if (num > 0f && !(thing is Pawn))
                    {
                        
                        ___liquifiedNutrition += num;
                        thing.Destroy(DestroyMode.Vanish);
                    }
                }
                if (___currentCycleTicksRemaining == 0) comp.CycleFactor = ___liquifiedNutrition / NutritionRequired;
                return false;
            }

            return true;
        }


    }
}
