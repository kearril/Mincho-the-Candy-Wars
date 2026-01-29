using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace MinchoCandyWars.MinchoCandyAbility
{
    public class MinchoAbilityDefModExtension : DefModExtension
    {
        public CandyType candyType = CandyType.None;

        public int requiredMinchoCoreGrade = 0;

        public int requiredMinchoBodyGrade = 0;

        public List<HediffDef> requiredHediffDefs = new List<HediffDef>();

    }
}
