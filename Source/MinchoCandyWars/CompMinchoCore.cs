using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace MinchoCandyWars
{
    public class CompMinchoCore : ThingComp
    {
        public CompProperties_MinchoCore Props => (CompProperties_MinchoCore)this.props;
        public Pawn pawn => this.parent as Pawn;

        private int minchoCoreGrade = 0;

        private int minchoBodyGrade = 0;

        private CandyType currentCandyType = CandyType.None;

        //数据存档
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref minchoCoreGrade, "minchoCoreGrade", 0);
            Scribe_Values.Look(ref minchoBodyGrade, "minchoBodyGrade", 0);
            Scribe_Values.Look(ref currentCandyType, "currentCandyType", CandyType.None);
        }

        //核心等级
        public int MinchoCoreGrade
        {
            get => minchoCoreGrade;
            set
            {
                if (minchoCoreGrade < 5)
                {
                    minchoCoreGrade++;
                }
            }
        }

        //躯体等级
        public int MinchoBodyGrade
        {
            get => minchoBodyGrade;
            set
            {
                if (minchoBodyGrade < 5)
                {
                    minchoBodyGrade++;
                }
            }
        }

        //总等级
        public int MinchoTotalGrade => minchoCoreGrade + minchoBodyGrade;


        //当前糖饰种类
        public CandyType CurrentCandyType
        {
            get => currentCandyType;
            set
            {
                currentCandyType = value;
            }
        }


    }

    public class CompProperties_MinchoCore : CompProperties
    {
        public CompProperties_MinchoCore()
        {
            this.compClass = typeof(CompMinchoCore);
        }
    }
}
