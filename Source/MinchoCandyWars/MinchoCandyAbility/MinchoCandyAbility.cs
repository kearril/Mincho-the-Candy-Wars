using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace MinchoCandyWars.MinchoCandyAbility
{
    public class MinchoCandyAbility : Ability
    {
        private int requiredMinchoCoreGrade;
        private int requiredMinchoBodyGrade;
        private CandyType candyType;
        private List<HediffDef> requiredHediffDefs;
        private CompMinchoCore compMinchoCore;
        private bool isInitialized = false;

        //初始化数据
        private void InitializeMinchoCandyAbility()
        {
            var modExt = this.def.GetModExtension<MinchoAbilityDefModExtension>();
            requiredMinchoCoreGrade = modExt.requiredMinchoCoreGrade;
            requiredMinchoBodyGrade = modExt.requiredMinchoBodyGrade;
            candyType = modExt.candyType;
            requiredHediffDefs = modExt.requiredHediffDefs;
            compMinchoCore = pawn.GetComp<CompMinchoCore>();

            isInitialized = true;
        }


        //检查是否显示Gizmo
        private bool ShouldShowGizmo()
        {
            //初始化
            if (!isInitialized)
            {
                InitializeMinchoCandyAbility();
            }
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
