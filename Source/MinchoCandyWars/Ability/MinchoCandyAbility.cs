using MinchoCandyWars.Interface;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace MinchoCandyWars.Ability
{
    public class MinchoCandyAbility : RimWorld.Ability, IInitalizable
    {
        public MinchoAbilityDefModExtension minchoAbilityDef = null!;
        private int requiredMinchoCoreGrade => minchoAbilityDef.requiredMinchoCoreGrade;
        private int requiredMinchoBodyGrade => minchoAbilityDef.requiredMinchoBodyGrade;
        private CandyType candyType => minchoAbilityDef.candyType;
        private List<HediffDef> requiredHediffDefs => minchoAbilityDef.requiredHediffDefs;
        private CompMinchoCore compMinchoCore = null!;

        //初始化数据
        void IInitalizable.Initialize()
        {
            minchoAbilityDef = def.GetModExtension<MinchoAbilityDefModExtension>();
            compMinchoCore = pawn.GetComp<CompMinchoCore>();
            if (minchoAbilityDef == null || compMinchoCore == null)
            {
                Log.ErrorOnce($"MinchoCandyWars: Ability {def.defName} is missing required MinchoAbilityDefModExtension or CompMinchoCore on pawn {pawn.LabelCap}.", 112421);
                pawn.abilities.RemoveAbility(this.def);
            }
        }

        //检查是否显示Gizmo
        private bool ShouldShowGizmo()
        {
            //检查核心等级和躯体等级
            if (compMinchoCore.MinchoCoreGrade < requiredMinchoCoreGrade || compMinchoCore.MinchoBodyGrade < requiredMinchoBodyGrade)
            {
                return false;
            }
            //检查糖饰种类
            if (compMinchoCore.CurrentCandyType != candyType)
            {
                return false;
            }
            //检查所需的Hediff,如果不需要hediff则不检查
            if (requiredHediffDefs.Count > 0)
            {
                foreach (var hediffDef in requiredHediffDefs)
                {
                    if (!pawn.health.hediffSet.HasHediff(hediffDef))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //控制Gizmo显示
        public override IEnumerable<Command> GetGizmos()
        {
            if (SettingUtility.IsDebugMode())
            {
                foreach (var gizmo in base.GetGizmos())
                {
                    yield return gizmo;
                }
                yield break;
            }
            if (ShouldShowGizmo())
            {
                foreach (var gizmo in base.GetGizmos())
                {
                    yield return gizmo;
                }
            }
        }

        //控制Gizmo显示
        public override IEnumerable<Gizmo> GetGizmosExtra()
        {
            if (SettingUtility.IsDebugMode())
            {
                foreach (var gizmo in base.GetGizmosExtra())
                {
                    yield return gizmo;
                }
                yield break;
            }
            if (ShouldShowGizmo())
            {
                foreach (var gizmo in base.GetGizmosExtra())
                {
                    yield return gizmo;
                }
            }
        }
    }
}
