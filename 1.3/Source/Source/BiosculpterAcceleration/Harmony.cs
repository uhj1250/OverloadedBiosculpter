using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using HarmonyLib;

namespace BiosculpterAcceleration
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
}
