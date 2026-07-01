using UnityEngine;

namespace Case316.Pixel
{
    public sealed class ClueInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] string clueId;
        [SerializeField] string displayName;
        [TextArea(3, 8)] [SerializeField] string description;
        [SerializeField] InventoryItem itemReward;
        [SerializeField] bool disableAfterInteract = true;
        [SerializeField] DialoguePopupUI popupUI;

        public string PromptText => $"按 E 查看：{displayName}";

        public void Interact(PixelPlayerController player)
        {
            PixelGameState.Instance.AddClue(clueId);

            if (itemReward != null)
            {
                PixelGameState.Instance.AddItem(itemReward);
            }

            popupUI?.Show(displayName, description);

            if (disableAfterInteract)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
