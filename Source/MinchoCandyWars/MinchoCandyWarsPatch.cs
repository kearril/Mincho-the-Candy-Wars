using HarmonyLib;
using Verse;

namespace MinchoCandyWars
{
    [StaticConstructorOnStartup]
    public static class MinchoCandyWarsPatch
    {
        static MinchoCandyWarsPatch()
        {
            var harmony = new Harmony("MinchoCandyWarsPatch");
            harmony.PatchAll();
        }
    }
}
