using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;


namespace BiosculpterAcceleration
{
    public class CompBiosculpterOverload : ThingComp
    {
        protected float loadmultiplier = 1f;
        protected float cyclefactor = 1f;
 

        public float LoadMultiplier
        {
            get
            {
                return loadmultiplier;
            }
            set
            {
                loadmultiplier = value;
            }
        }

        public float CycleFactor
        {
            get
            {
                return cyclefactor;
            }
            set
            {
                cyclefactor = value;
            }
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref loadmultiplier, "loadmul", loadmultiplier, true);
            Scribe_Values.Look(ref cyclefactor, "cyclefactor", cyclefactor, true);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            yield return new Gizmo_BioOverloadConfig(this);
        }

       




    }

    public class Gizmo_BioOverloadConfig : Gizmo
    {
        public const float Width = 212f;
        public const float AdditionalHeight = 8f;

        public static readonly Color DarkGreen = new Color(0,0.4f,0);
        public static readonly Color DarkYellow = new Color(0.75f,0.75f,0);

        protected CompBiosculpterOverload parent;
        protected CompBiosculpterPod compbiopod;
        
        protected static bool clicked = false;
        protected static CompBiosculpterOverload clickedgizmo = null;
        


        public Gizmo_BioOverloadConfig(CompBiosculpterOverload parent)
        {
            this.parent = parent;
            this.compbiopod = parent.parent.GetComp<CompBiosculpterPod>();
            this.order = -100;
        }


        public override float GetWidth(float maxWidth)
        {
            return Width;
        }

        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            Rect outRect = new Rect(topLeft.x, topLeft.y - AdditionalHeight, GetWidth(maxWidth), Height + AdditionalHeight);
            Rect inRect = outRect.ContractedBy(3f);
            Rect row1 = new Rect(inRect.x,inRect.y,inRect.width, inRect.height/4);
            Rect row2 = new Rect(inRect.x,row1.y + row1.height,inRect.width, inRect.height/4);
            Rect row3 = new Rect(inRect.x,row2.y + row2.height,inRect.width, inRect.height/4);
            Rect row4 = new Rect(inRect.x,row3.y + row3.height, inRect.width, inRect.height/4);

            Widgets.DrawWindowBackground(outRect);

            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(row1, Keyed.OverloadSetting);
            Text.Font = GameFont.Tiny;
            Text.Anchor = TextAnchor.UpperLeft;
            Widgets.Label(row2, String.Format(Keyed.AdditionalNutrition + ": {0:0.##}", Patch_CompBiosculpterPod.NutritionRequired * parent.LoadMultiplier - Patch_CompBiosculpterPod.NutritionRequired));
            Widgets.Label(row3, String.Format(Keyed.CurrentCycleAcceleration + ": {0:P2} ", parent.CycleFactor));
            Text.Font = GameFont.Small;
            string label = String.Format("{0:P0}", parent.LoadMultiplier);
            parent.LoadMultiplier = DrawAdjustableBar(row4, parent.LoadMultiplier, parent.CycleFactor, 1.0f, 20f, DarkGreen, DarkYellow, Color.black, label, Keyed.OSTooltip(label));
            
            
            return new GizmoResult(GizmoState.Clear);
        }

        protected float DrawAdjustableBar(Rect rect, float val, float subval, float min, float max, Color barColor, Color subBarColor, Color bgColor, string label = null, string tooltip = null)
        {
            Rect nutritionRect = rect.ContractedBy(2f);
            Rect barRect = rect.ContractedBy(4f);
            float originval = val;


            if (!clicked && Mouse.IsOver(barRect))
            {
                if (Input.GetMouseButton(0))
                {
                    clicked = true;
                    clickedgizmo = parent;
                }
                else if (Event.current.type == EventType.ScrollWheel)
                {
                    float delta = Input.mouseScrollDelta.y;
                    if (delta > 0) val = Mathf.Clamp(val + 0.01f, min, max);
                    else if (delta < 0) val = Mathf.Clamp(val - 0.01f, min, max);
                    Event.current.Use();
                }
            }

            if (clicked && clickedgizmo == parent)
            {
                float posnormalized = Mathf.Clamp01(Input.mousePosition.x.Normalize(barRect.x, barRect.xMax));
                val = (float)Mathf.Floor(posnormalized.DeNormalize(min, max)*100f)/100f;
                if (!Input.GetMouseButton(0))
                {
                    clicked = false;
                    clickedgizmo = null;
                }
            }

            if (val != originval) SoundDefOf.DragSlider.PlayOneShotOnCamera();

            barRect.width *= val.Normalize(min, max);


            GUI.color = bgColor;
            GUI.DrawTexture(rect, Texture2D.whiteTexture, ScaleMode.StretchToFill);

            nutritionRect.width = nutritionRect.width*Mathf.Clamp01(subval.Normalize(min, max)) + 2f;
            GUI.color = subBarColor;
            GUI.DrawTexture(nutritionRect, Texture2D.whiteTexture, ScaleMode.StretchToFill);

            GUI.color = barColor;
            GUI.DrawTexture(barRect, Texture2D.whiteTexture, ScaleMode.StretchToFill);

            GUI.color = Color.white;

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(rect, label);
            Text.Anchor = TextAnchor.UpperLeft;

            Widgets.DrawHighlightIfMouseover(rect);
            if (tooltip != null) TooltipHandler.TipRegion(rect, tooltip);

            return val;
        }



    }


}
