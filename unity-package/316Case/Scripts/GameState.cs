using System;
using System.Collections.Generic;
using UnityEngine;

namespace Case316
{
    public sealed class GameState : MonoBehaviour
    {
        public static GameState Instance { get; private set; }

        [Header("Chapter Variables")]
        public int truth;
        public int danger;
        public int guilt;
        public int chenXiaoPresence;
        public int yuanYuTrust;
        public int publicOpinion;

        readonly HashSet<string> clues = new HashSet<string>();
        readonly HashSet<string> flags = new HashSet<string>();

        public event Action<string> ClueAdded;
        public event Action StateChanged;

        public IReadOnlyCollection<string> Clues => clues;
        public int ClueCount => clues.Count;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void ResetState()
        {
            truth = 0;
            danger = 0;
            guilt = 0;
            chenXiaoPresence = 0;
            yuanYuTrust = 0;
            publicOpinion = 0;
            clues.Clear();
            flags.Clear();
            StateChanged?.Invoke();
        }

        public void AddClue(string clueName)
        {
            if (string.IsNullOrWhiteSpace(clueName))
            {
                return;
            }

            if (clues.Add(clueName))
            {
                ClueAdded?.Invoke(clueName);
                StateChanged?.Invoke();
            }
        }

        public bool HasClue(string clueName)
        {
            return clues.Contains(clueName);
        }

        public void SetFlag(string flagName)
        {
            if (string.IsNullOrWhiteSpace(flagName))
            {
                return;
            }

            if (flags.Add(flagName))
            {
                StateChanged?.Invoke();
            }
        }

        public bool HasFlag(string flagName)
        {
            return flags.Contains(flagName);
        }

        public void AddStat(string statName, int delta)
        {
            switch (statName)
            {
                case "truth":
                    truth += delta;
                    break;
                case "danger":
                    danger += delta;
                    break;
                case "guilt":
                    guilt += delta;
                    break;
                case "chen_xiao_presence":
                    chenXiaoPresence += delta;
                    break;
                case "yuan_yu_trust":
                    yuanYuTrust += delta;
                    break;
                case "public_opinion":
                    publicOpinion += delta;
                    break;
                default:
                    Debug.LogWarning($"Unknown stat: {statName}");
                    return;
            }

            StateChanged?.Invoke();
        }

        public int GetStat(string statName)
        {
            return statName switch
            {
                "truth" => truth,
                "danger" => danger,
                "guilt" => guilt,
                "chen_xiao_presence" => chenXiaoPresence,
                "yuan_yu_trust" => yuanYuTrust,
                "public_opinion" => publicOpinion,
                _ => 0
            };
        }

        public ChapterResult EvaluateChapterOne()
        {
            if (ClueCount >= 8 && guilt > 0)
            {
                return ChapterResult.DeepLine;
            }

            if (ClueCount >= 4)
            {
                return ChapterResult.StandardLine;
            }

            return ChapterResult.CommonLine;
        }
    }

    public enum ChapterResult
    {
        CommonLine,
        StandardLine,
        DeepLine
    }
}
