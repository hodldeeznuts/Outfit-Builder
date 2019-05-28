using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Outfit_Builder
{
    public class Outfit_Builder : Mod
    {
        public Outfit_Builder(ModContentPack content) : base(content)
        {

        }
    }

    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        private static readonly Type patchType = typeof(HarmonyPatches);

        static HarmonyPatches()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("com.github.harmony.rimworld.mod.outfit_builder");

            harmony.Patch(
                    original: AccessTools.Method(typeof(RimWorld.OutfitForcedHandler), nameof(RimWorld.OutfitForcedHandler.Reset)),
                    prefix: new HarmonyMethod(type: patchType, name: nameof(OutfitResetPrefix))
                );
        }

        static void OutfitResetPrefix(RimWorld.OutfitForcedHandler __instance)
        {
            if (__instance.ForcedApparel == null || __instance.ForcedApparel.Count < 1) return;

            OutfitPolicyGameComponent component = Current.Game.GetComponent<OutfitPolicyGameComponent>();

            Pawn pawn = __instance.ForcedApparel[0].Wearer;

            Outfit outfitAssignedToPawn = component.OutfitAssignedToPawn(pawn);

            if (outfitAssignedToPawn == null)
            {
                outfitAssignedToPawn = component.CreateNewOutfit(pawn);
            }

            outfitAssignedToPawn.filter = new ThingFilter();


            foreach (Apparel apparel in pawn.apparel.WornApparel)
            {
                outfitAssignedToPawn.filter.SetAllow(apparel.def, true);
            }

            pawn.outfits.CurrentOutfit = outfitAssignedToPawn;
        }
    }
}
