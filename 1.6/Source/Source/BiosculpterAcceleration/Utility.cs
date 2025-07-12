using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;


namespace BiosculpterAcceleration
{
    public static class Utility
    {
        public static float Normalize(this float val, float min, float max)
        {
            return (val - min) / (max - min);
        }

        public static float DeNormalize(this float val, float min, float max)
        {
            return val * (max - min) + min;
        }
    }
}
