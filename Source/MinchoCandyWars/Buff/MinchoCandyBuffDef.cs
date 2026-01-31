using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace MinchoCandyWars.Buff
{

    //增益def定义
    public class MinchoCandyBuffDef : Def
    {
        public CandyType candyType = CandyType.None;

        public List<MinchoCandyBuffGrade> coreGrades;

        public List<MinchoCandyBuffGrade> bodyGrades;
    }

    public class MinchoCandyBuffGrade
    {
        public int requiredGrade = 0;

        [MustTranslate]
        public string label;

        public List<MinchoCandyBuffEffect> effects;
    }

    public class MinchoCandyBuffEffect
    {
        public StatDef statDef;
        public float statOffset = 0f;
        public float statFactor = 1f;
    }
}
