using UnityEngine;

namespace Case316
{
    public sealed class ChapterInteractable : MonoBehaviour
    {
        public InteractableData Data { get; private set; }

        public void Init(InteractableData data, Vector3 worldPosition)
        {
            Data = data;
            transform.position = worldPosition;
            transform.localScale = data.kind == InteractableKind.Npc ? new Vector3(0.72f, 0.88f, 1f) : new Vector3(0.62f, 0.62f, 1f);
            gameObject.name = data.displayName;
            var renderer = gameObject.GetComponent<SpriteRenderer>() ?? gameObject.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteFactory.Square;
            renderer.color = data.kind == InteractableKind.Npc ? new Color(0.64f, 0.43f, 0.3f) : new Color(0.78f, 0.63f, 0.36f);
            renderer.sortingOrder = data.kind == InteractableKind.Npc ? 8 : 6;
        }
    }
}
