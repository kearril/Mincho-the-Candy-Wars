using MinchoCandyWars.Data;
using RimWorld;
using System.Text;
using Verse;

namespace MinchoCandyWars.Buff
{
    public class CompMinchoCandyBuffApply : ThingComp
    {
        private CompMinchoCore cachedCoreComp;
        private List<MinchoCandyBuffEffect> cachedEffects = new List<MinchoCandyBuffEffect>();
        private bool effectsDirty = true;
        private MinchoCandyBuffGrade cachedCoreGrade;
        private MinchoCandyBuffGrade cachedBodyGrade;

        public Pawn pawn => this.parent as Pawn;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            cachedCoreComp = pawn.GetComp<CompMinchoCore>();
            effectsDirty = true;
        }

        //收信号
        public override void ReceiveCompSignal(string signal)
        {
            // 当核心数据变化时，标记效果为脏
            if (signal == CompSignals.MinchoCoreDataChange)
            {
                effectsDirty = true;
            }
        }

        //获取当前的BuffDef
        private MinchoCandyBuffDef GetCurrentBuffDef()
        {
            // 未选择糖饰时不产生任何 Buff
            if (cachedCoreComp.CurrentCandyType == CandyType.None)
            {
                return null;
            }

            // 按糖饰类型查表拿到对应 BuffDef
            if (DefDataPreloading.MinchoCandyBuffDefs.TryGetValue(cachedCoreComp.CurrentCandyType, out var buffDef))
            {
                return buffDef;
            }

            return null;
        }

        //查找满足条件的最高等级
        private MinchoCandyBuffGrade FindMaxGrade(List<MinchoCandyBuffGrade> grades, int currentGrade)
        {
            // 从所有等级里选出 requiredGrade 不超过当前等级的最大项
            MinchoCandyBuffGrade maxGrade = null;
            int maxGradeLevel = 0;

            foreach (var grade in grades)
            {
                if (grade.requiredGrade <= currentGrade &&
                    grade.requiredGrade > maxGradeLevel)
                {
                    maxGrade = grade;
                    maxGradeLevel = grade.requiredGrade;
                }
            }

            return maxGrade;
        }

        //收集并返回当前适用的所有Buff效果
        private List<MinchoCandyBuffEffect> GetApplicableEffects()
        {
            // 核心数据未变化时直接复用缓存
            if (!effectsDirty)
            {
                return cachedEffects;
            }

            // 重新计算缓存
            cachedEffects.Clear();
            cachedCoreGrade = null;
            cachedBodyGrade = null;


            var buffDef = GetCurrentBuffDef();
            if (buffDef == null)
            {
                effectsDirty = false;
                return cachedEffects;
            }

            // 收集核心等级对应的所有效果，并缓存最高核心等级
            var maxCoreGrade = FindMaxGrade(buffDef.coreGrades, cachedCoreComp.MinchoCoreGrade);
            cachedCoreGrade = maxCoreGrade;
            if (maxCoreGrade != null && maxCoreGrade.effects != null)
            {
                cachedEffects.AddRange(maxCoreGrade.effects);
            }

            // 收集躯体等级对应的所有效果，并缓存最高躯体等级
            var maxBodyGrade = FindMaxGrade(buffDef.bodyGrades, cachedCoreComp.MinchoBodyGrade);
            cachedBodyGrade = maxBodyGrade;
            if (maxBodyGrade != null && maxBodyGrade.effects != null)
            {
                cachedEffects.AddRange(maxBodyGrade.effects);
            }

            effectsDirty = false;
            return cachedEffects;
        }

        //应用属性乘数
        public override float GetStatFactor(StatDef stat)
        {
            // 统一从缓存的效果里筛选当前 stat 的乘数并累乘
            var effects = GetApplicableEffects();

            float totalFactor = 1f;
            foreach (var effect in effects)
            {
                if (effect.statDef == stat)
                {
                    totalFactor *= effect.statFactor;
                }
            }

            return totalFactor;
        }

        //应用属性偏移
        public override float GetStatOffset(StatDef stat)
        {
            // 统一从缓存的效果里筛选当前 stat 的偏移并累加
            var effects = GetApplicableEffects();

            float totalOffset = 0f;
            foreach (var effect in effects)
            {
                if (effect.statDef == stat)
                {
                    totalOffset += effect.statOffset;
                }
            }

            return totalOffset;
        }

        //获取属性说明，例如： "核心等级3（琉璃糖糖饰）"
        public override void GetStatsExplanation(StatDef stat, StringBuilder sb, string whitespace = "")
        {
            
            var buffDef = GetCurrentBuffDef();
            if (buffDef == null)
            {
                return;
            }

            // 确保 cachedCoreGrade/cachedBodyGrade 已被更新
            GetApplicableEffects();

            if (HasStatEffect(cachedCoreGrade, stat))
            {
                sb.AppendLine($"{whitespace}{cachedCoreGrade.label}（{buffDef.label}）");
            }

            if (HasStatEffect(cachedBodyGrade, stat))
            {
                sb.AppendLine($"{whitespace}{cachedBodyGrade.label}（{buffDef.label}）");
            }
        }

        private static bool HasStatEffect(MinchoCandyBuffGrade grade, StatDef stat)
        {
            // 用于判断该等级是否对指定 stat 有影响
            if (grade == null || grade.effects == null)
            {
                return false;
            }

            foreach (var effect in grade.effects)
            {
                if (effect.statDef == stat)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class CompProperties_MinchoCandyBuffApply : CompProperties
    {
        public CompProperties_MinchoCandyBuffApply()
        {
            this.compClass = typeof(CompMinchoCandyBuffApply);
        }
    }
}
