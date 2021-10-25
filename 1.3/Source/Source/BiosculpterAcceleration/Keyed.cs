using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace BiosculpterAcceleration
{
    public static class Keyed
    {
        public static string OSTooltip(string percent) => "OB.OSTooltip".Translate(percent);


        public static readonly string OverloadSetting = "OB.OverloadSetting".Translate();
        public static readonly string AdditionalNutrition = "OB.AdditionalNutrition".Translate();
        public static readonly string CurrentCycleAcceleration = "OB.CurrentCycleAcceleration".Translate();



    }
}
