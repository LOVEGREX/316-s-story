using UnityEngine;

namespace Case316
{
    public sealed class GridPlayerController : MonoBehaviour
    {
        ChapterOneGameManager manager;

        public Vector2Int GridPosition { get; private set; }

        public void Init(ChapterOneGameManager gameManager, Vector2Int startGrid, Vector3 startWorld)
        {
            manager = gameManager;
            GridPosition = startGrid;
            transform.position = startWorld;
            var renderer = gameObject.GetComponent<SpriteRenderer>() ?? gameObject.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteFactory.Square;
            renderer.color = new Color(0.72f, 0.25f, 0.25f);
            renderer.sortingOrder = 10;
            transform.localScale = new Vector3(0.72f, 0.9f, 1f);
            gameObject.name = "郭女英";
        }

        void Update()
        {
            if (manager == null)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                manager.TryInteract();
                return;
            }

            var delta = Vector2Int.zero;
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) delta = Vector2Int.up;
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) delta = Vector2Int.down;
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) delta = Vector2Int.left;
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) delta = Vector2Int.right;
            if (delta == Vector2Int.zero)
            {
                return;
            }

            var next = GridPosition + delta;
            if (manager.TryMove(next))
            {
                GridPosition = next;
                transform.position += new Vector3(delta.x, -delta.y, 0f);
            }
        }
    }
}
