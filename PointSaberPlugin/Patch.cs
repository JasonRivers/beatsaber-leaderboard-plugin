using Harmony;
using System;
using System.Reflection;

[HarmonyPatch(typeof(LocalLeaderboardsModel))]
[HarmonyPatch("AddScore")]
[HarmonyPatch(new Type[] { typeof(string), typeof(string), typeof(int), typeof(bool) })]
class LocalLeaderboardsModel_AddScore_Patch
{
    public static void Postfix(LocalLeaderboardsModel __instance)
    {
 
        try
        {
            PostScoreBehavior.PostScores();
        }
        catch
        {

        }
    }
}