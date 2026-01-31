using Verse;

namespace MinchoCandyWars.Ability
{
    //minchoAbility数据拓展
    public class MinchoAbilityDefModExtension : DefModExtension
    {
        public CandyType candyType = CandyType.None;

        public int requiredMinchoCoreGrade = 0;

        public int requiredMinchoBodyGrade = 0;

        public List<HediffDef> requiredHediffDefs = new List<HediffDef>();

    }
}
