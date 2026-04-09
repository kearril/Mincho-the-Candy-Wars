using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MinchoCandyWars.Buildings
{
    public class CompProperties_BedJoyGain : CompProperties
    {

        public JoyKindDef? joyKind;

        public CompProperties_BedJoyGain()
        {
            compClass = typeof(CompBedJoyGain);
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (string item in base.ConfigErrors(parentDef))
            {
                yield return item;
            }

            if (parentDef.tickerType == TickerType.Never)
            {
                yield return "has CompBedJoyGain, but its TickerType is set to Never";
            }
        }
    }

    public class CompBedJoyGain : ThingComp
    {
        private int ticksPassedSinceLastJoy;

        public CompProperties_BedJoyGain Props => (CompProperties_BedJoyGain)props;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref ticksPassedSinceLastJoy, "ticksPassedSinceLastJoy", 0);
        }

        public override void CompTickInterval(int delta)
        {
            TickInterval(delta);
        }

        public override void CompTickRare()
        {
            TickInterval(250);
        }

        public override void CompTickLong()
        {
            TickInterval(2000);
        }

        private void TickInterval(int ticks)
        {
            if (!(parent is Building_Bed bed) || !parent.Spawned)
            {
                return;
            }

            float joyGain = parent.GetStatValue(StatDefOf.JoyGainFactor) * 0.36f / 2500f * (float)ticks;
            if (joyGain <= 0f)
            {
                return;
            }

            foreach (Pawn pawn in bed.CurOccupants)
            {
                pawn?.needs?.joy?.GainJoy(joyGain, Props.joyKind ?? JoyKindDefOf.Meditative);
            }
        }
    }
}
