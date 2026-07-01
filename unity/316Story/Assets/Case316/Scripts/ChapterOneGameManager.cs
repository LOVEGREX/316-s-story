using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Case316
{
    public sealed class ChapterOneGameManager : MonoBehaviour
    {
        public ChapterData chapter;
        public Case316UiController ui;
        public Transform boardRoot;
        public GridPlayerController playerPrefab;
        public ChapterInteractable interactablePrefab;

        readonly HashSet<string> clues = new HashSet<string>();
        readonly HashSet<string> items = new HashSet<string>();
        readonly HashSet<string> usedInteractables = new HashSet<string>();
        readonly Dictionary<Vector2Int, ChapterInteractable> interactablesByPosition = new Dictionary<Vector2Int, ChapterInteractable>();
        readonly HashSet<Vector2Int> walls = new HashSet<Vector2Int>();

        GridPlayerController player;
        int segmentIndex;
        int actionPoints;
        bool failed;
        bool completed;

        public bool InputLocked => failed || completed || ui.IsInteractionOpen || ui.IsMainMenuOpen;
        public Vector2Int PlayerGrid => player != null ? player.GridPosition : chapter.playerStart;

        void Start()
        {
            ui.Bind(this);
            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            ui.ShowMainMenu(SaveSystem.HasSave);
        }

        public void StartNewGame()
        {
            clues.Clear();
            items.Clear();
            usedInteractables.Clear();
            segmentIndex = 0;
            actionPoints = chapter.segments[0].actionPoints;
            failed = false;
            completed = false;
            BuildBoard();
            ui.HideMainMenu();
            ui.HideFailure();
            ui.SetDialogue("调查目标", "在行动力耗尽前收集足够线索。第一章最低目标：8条线索。");
            RefreshUi();
        }

        public void SaveGame()
        {
            if (player == null)
            {
                return;
            }

            SaveSystem.Save(new SaveData
            {
                chapterId = chapter.chapterId,
                segmentIndex = segmentIndex,
                actionPoints = actionPoints,
                playerX = player.GridPosition.x,
                playerY = player.GridPosition.y,
                failed = failed,
                completed = completed,
                clues = clues.ToList(),
                items = items.ToList(),
                usedInteractables = usedInteractables.ToList()
            });
            ui.ShowToast("已保存第一章进度");
            RefreshUi();
        }

        public void LoadGame()
        {
            var data = SaveSystem.Load();
            if (data == null)
            {
                ui.ShowToast("暂无存档");
                return;
            }

            clues.Clear();
            items.Clear();
            usedInteractables.Clear();
            foreach (var clue in data.clues) clues.Add(clue);
            foreach (var item in data.items) items.Add(item);
            foreach (var used in data.usedInteractables) usedInteractables.Add(used);

            segmentIndex = Mathf.Clamp(data.segmentIndex, 0, chapter.segments.Count - 1);
            actionPoints = data.actionPoints;
            failed = data.failed;
            completed = data.completed;
            BuildBoard(new Vector2Int(data.playerX, data.playerY));
            ui.HideMainMenu();
            ui.SetDialogue("读取存档", "调查进度已恢复。继续在本时间段内寻找足够线索。");
            if (failed) ui.ShowFailure(CurrentSegment.failureTitle, CurrentSegment.failureText);
            RefreshUi();
        }

        public bool TryMove(Vector2Int next)
        {
            if (InputLocked || IsBlocked(next))
            {
                return false;
            }

            SpendAction(1);
            RefreshUi();
            return !failed;
        }

        public void TryInteract()
        {
            if (InputLocked)
            {
                return;
            }

            var target = FindAdjacentInteractable();
            if (target == null)
            {
                ui.ShowToast("附近没有可交互目标");
                return;
            }

            SpendAction(2);
            if (failed)
            {
                RefreshUi();
                return;
            }

            usedInteractables.Add(target.Data.id);
            if (!string.IsNullOrWhiteSpace(target.Data.clueId)) clues.Add(target.Data.clueName);
            if (!string.IsNullOrWhiteSpace(target.Data.itemId)) items.Add(target.Data.itemName);
            target.gameObject.SetActive(false);
            ui.ShowInteraction(target.Data);
            ui.SetDialogue(target.Data.displayName, target.Data.body);
            RefreshUi();
        }

        public void CloseInteraction()
        {
            ui.HideInteraction();
            RefreshUi();
        }

        void SpendAction(int amount)
        {
            actionPoints -= amount;
            if (actionPoints <= 0)
            {
                AdvanceTime();
            }
        }

        void AdvanceTime()
        {
            if (clues.Count < CurrentSegment.requiredClues)
            {
                failed = true;
                ui.ShowFailure(CurrentSegment.failureTitle, CurrentSegment.failureText);
                return;
            }

            if (segmentIndex < chapter.segments.Count - 1)
            {
                segmentIndex += 1;
                actionPoints = CurrentSegment.actionPoints;
                ui.SetDialogue("时间推进", $"现在是{CurrentSegment.clock}。本时间段结束前至少需要{CurrentSegment.requiredClues}条线索。");
                return;
            }

            completed = true;
            actionPoints = 0;
            ui.SetDialogue("第一章完成", "你在行动力耗尽前拿到了足够线索。下一章将进入东校区旧群深线。");
            ui.ShowToast("第一章完成");
        }

        void BuildBoard()
        {
            BuildBoard(chapter.playerStart);
        }

        void BuildBoard(Vector2Int playerStart)
        {
            if (boardRoot == null)
            {
                boardRoot = new GameObject("Board").transform;
            }

            foreach (Transform child in boardRoot)
            {
                Destroy(child.gameObject);
            }

            walls.Clear();
            interactablesByPosition.Clear();
            for (var x = 0; x < chapter.boardWidth; x += 1)
            {
                walls.Add(new Vector2Int(x, 0));
                walls.Add(new Vector2Int(x, chapter.boardHeight - 1));
            }
            for (var y = 0; y < chapter.boardHeight; y += 1)
            {
                walls.Add(new Vector2Int(0, y));
                walls.Add(new Vector2Int(chapter.boardWidth - 1, y));
            }
            foreach (var wall in chapter.walls) walls.Add(wall);

            for (var y = 0; y < chapter.boardHeight; y += 1)
            {
                for (var x = 0; x < chapter.boardWidth; x += 1)
                {
                    var grid = new Vector2Int(x, y);
                    CreateTile(grid, walls.Contains(grid));
                }
            }

            foreach (var data in chapter.interactables)
            {
                if (usedInteractables.Contains(data.id))
                {
                    continue;
                }

                var instance = interactablePrefab != null
                    ? Instantiate(interactablePrefab, boardRoot)
                    : new GameObject("Interactable").AddComponent<ChapterInteractable>();
                instance.transform.SetParent(boardRoot);
                instance.Init(data, GridToWorld(data.gridPosition));
                interactablesByPosition[data.gridPosition] = instance;
            }

            player = playerPrefab != null
                ? Instantiate(playerPrefab, boardRoot)
                : new GameObject("Player").AddComponent<GridPlayerController>();
            player.transform.SetParent(boardRoot);
            player.Init(this, playerStart, GridToWorld(playerStart));
        }

        void CreateTile(Vector2Int grid, bool wall)
        {
            var go = new GameObject(wall ? "Wall" : "Floor");
            go.transform.SetParent(boardRoot);
            go.transform.position = GridToWorld(grid);
            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteFactory.Square;
            renderer.color = wall ? new Color(0.15f, 0.22f, 0.2f) : ((grid.x + grid.y) % 2 == 0 ? new Color(0.1f, 0.15f, 0.14f) : new Color(0.12f, 0.18f, 0.16f));
            renderer.sortingOrder = wall ? 1 : 0;
        }

        bool IsBlocked(Vector2Int grid)
        {
            return grid.x < 0 || grid.y < 0 || grid.x >= chapter.boardWidth || grid.y >= chapter.boardHeight || walls.Contains(grid);
        }

        ChapterInteractable FindAdjacentInteractable()
        {
            var directions = new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right, Vector2Int.zero };
            foreach (var direction in directions)
            {
                if (interactablesByPosition.TryGetValue(PlayerGrid + direction, out var target) && target.gameObject.activeSelf)
                {
                    return target;
                }
            }

            return null;
        }

        Vector3 GridToWorld(Vector2Int grid)
        {
            return new Vector3(grid.x - chapter.boardWidth / 2f, chapter.boardHeight / 2f - grid.y, 0f);
        }

        TimeSegmentData CurrentSegment => chapter.segments[segmentIndex];

        void RefreshUi()
        {
            ui.Refresh(CurrentSegment, actionPoints, clues, items, FindAdjacentInteractable(), SaveSystem.HasSave);
        }
    }
}
