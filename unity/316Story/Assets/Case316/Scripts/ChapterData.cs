using System;
using System.Collections.Generic;
using UnityEngine;

namespace Case316
{
    [CreateAssetMenu(menuName = "Case316/Chapter Data", fileName = "ChapterData")]
    public sealed class ChapterData : ScriptableObject
    {
        public string chapterId = "chapter_01";
        public string title = "第一章：红果园的最后一晚";
        public int boardWidth = 20;
        public int boardHeight = 14;
        public Vector2Int playerStart = new Vector2Int(10, 11);
        public int requiredCluesToClear = 8;
        public float realMinutesTarget = 40f;
        public List<TimeSegmentData> segments = new List<TimeSegmentData>();
        public List<Vector2Int> walls = new List<Vector2Int>();
        public List<InteractableData> interactables = new List<InteractableData>();
    }

    [Serializable]
    public sealed class TimeSegmentData
    {
        public string clock = "23:47";
        public string location = "男生宿舍楼下";
        public int actionPoints = 18;
        public int requiredClues = 1;
        public int gameMinutes = 18;
        public string failureTitle = "线索不足";
        [TextArea(2, 5)] public string failureText;
    }

    [Serializable]
    public sealed class InteractableData
    {
        public string id;
        public string displayName;
        public InteractableKind kind;
        public Vector2Int gridPosition;
        public string clueId;
        public string clueName;
        public string itemId;
        public string itemName;
        [TextArea(3, 8)] public string body;
        public List<string> options = new List<string>();
    }

    public enum InteractableKind
    {
        Clue,
        Npc
    }
}
