using System;
using System.Collections.Generic;
using UnityEngine;

namespace Case316.Pixel
{
    public sealed class PixelGameState : MonoBehaviour
    {
        public static PixelGameState Instance { get; private set; }

        readonly HashSet<string> clues = new HashSet<string>();
        readonly List<InventoryItem> inventory = new List<InventoryItem>();

        public event Action<string> ClueAdded;
        public event Action<InventoryItem> ItemAdded;
        public event Action StateChanged;

        public IReadOnlyCollection<string> Clues => clues;
        public IReadOnlyList<InventoryItem> Inventory => inventory;
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
            clues.Clear();
            inventory.Clear();
            StateChanged?.Invoke();
        }

        public void AddClue(string clueId)
        {
            if (string.IsNullOrWhiteSpace(clueId))
            {
                return;
            }

            if (clues.Add(clueId))
            {
                ClueAdded?.Invoke(clueId);
                StateChanged?.Invoke();
            }
        }

        public bool HasClue(string clueId)
        {
            return clues.Contains(clueId);
        }

        public void AddItem(InventoryItem item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.itemId))
            {
                return;
            }

            if (inventory.Exists(existing => existing.itemId == item.itemId))
            {
                return;
            }

            inventory.Add(item);
            ItemAdded?.Invoke(item);
            StateChanged?.Invoke();
        }

        public bool HasItem(string itemId)
        {
            return inventory.Exists(item => item.itemId == itemId);
        }
    }

    [Serializable]
    public sealed class InventoryItem
    {
        public string itemId;
        public string displayName;
        [TextArea(2, 5)] public string description;
        public Sprite icon;
    }
}
