using UnityEngine;
using Yarn.Unity;

namespace Case316
{
    public sealed class YarnCommandBridge : MonoBehaviour
    {
        [YarnCommand("add_clue")]
        public static void AddClue(string clueName)
        {
            GameState.Instance.AddClue(clueName);
        }

        [YarnCommand("stat")]
        public static void AddStat(string statName, int delta)
        {
            GameState.Instance.AddStat(statName, delta);
        }

        [YarnCommand("flag")]
        public static void SetFlag(string flagName)
        {
            GameState.Instance.SetFlag(flagName);
        }

        [YarnFunction("has_clue")]
        public static bool HasClue(string clueName)
        {
            return GameState.Instance.HasClue(clueName);
        }

        [YarnFunction("stat_value")]
        public static float StatValue(string statName)
        {
            return GameState.Instance.GetStat(statName);
        }

        [YarnFunction("clue_count")]
        public static float ClueCount()
        {
            return GameState.Instance.ClueCount;
        }
    }
}
