using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;


namespace BiosculpterAcceleration
{
    [StaticConstructorOnStartup]
    public static class DefInjection
    {
        static DefInjection()
        {
            Injection();
        }

        private static void Injection()
        {
            List<ThingDef> buildings = DefDatabase<ThingDef>.AllDefs.Where(x => x.IsBuildingArtificial).ToList();
            CompProperties comp = new CompProperties(typeof(CompBiosculpterOverload));

            if (!buildings.NullOrEmpty()) for (int i=0; i<buildings.Count; i++)
                {
                    if (buildings[i].comps.Exists(x => x is CompProperties_BiosculpterPod))
                    {
                        buildings[i].comps.Insert(0,comp);
                    }
                }


        }


    }
}
