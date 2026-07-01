using System;
using System.Collections.Generic;

namespace Case316
{
    [Serializable]
    public sealed class SaveData
    {
        public string chapterId;
        public int segmentIndex;
        public int actionPoints;
        public int playerX;
        public int playerY;
        public bool failed;
        public bool completed;
        public List<string> clues = new List<string>();
        public List<string> items = new List<string>();
        public List<string> usedInteractables = new List<string>();
    }
}
