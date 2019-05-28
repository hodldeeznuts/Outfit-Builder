using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Outfit_Builder
{
    public class OutfitPolicyGameComponent : GameComponent
    {
        public OutfitPolicyGameComponent(Game game)
        {
            List<Pawn> pawnsToRemove = new List<Pawn>();

            foreach (Pawn pawn in storedOutfits.Keys)
            {
                if (pawn.DestroyedOrNull())
                {
                    pawnsToRemove.Add(pawn);
                }
            }

            foreach (Pawn pawn in pawnsToRemove)
            {
                storedOutfits.Remove(pawn);
            }
        }

        public Outfit OutfitAssignedToPawn(Pawn pawn)
        {
            if (storedOutfits == null)
            {
                storedOutfits = new Dictionary<Pawn, int>();
            }

            if (storedOutfits.ContainsKey(pawn))
            {
                return Current.Game.outfitDatabase.AllOutfits.Find(s => s.uniqueId == storedOutfits[pawn]);
            }

            return null;
        }

        public Outfit CreateNewOutfit(Pawn pawn)
        {
            Outfit newOutfit = Current.Game.outfitDatabase.MakeNewOutfit();
            newOutfit.label = pawn.Name.ToStringShort.CapitalizeFirst();

            storedOutfits.Add(pawn, newOutfit.uniqueId);

            return newOutfit;
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref storedOutfits, "storedOutfits", LookMode.Reference, LookMode.Value, ref allPawns, ref outfitIdentifiers);
        }

        static Dictionary<Pawn, int> storedOutfits = new Dictionary<Pawn, int>();
        static List<Pawn> allPawns = new List<Pawn>();
        static List<int> outfitIdentifiers = new List<int>();
    }
}
