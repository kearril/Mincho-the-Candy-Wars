using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using HarmonyLib;

namespace MinchoCandyWars.Patch
{
    [StaticConstructorOnStartup]
    public static class MinchoCandyWarsPatch
    {
        public static Harmony harmony;
        static MinchoCandyWarsPatch()
        {
            harmony = new Harmony("MinchoCandyWarsPatch");
            harmony.PatchAll();
        }
    }
}
