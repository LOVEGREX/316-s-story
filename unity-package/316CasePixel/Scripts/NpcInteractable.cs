using UnityEngine;

namespace Case316.Pixel
{
    public sealed class NpcInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] string npcName;
        [TextArea(3, 8)] [SerializeField] string dialogue;
        [SerializeField] string clueReward;
        [SerializeField] DialoguePopupUI popupUI;

        public string PromptText => $"按 E 交谈：{npcName}";

        public void Interact(PixelPlayerController player)
        {
            if (!string.IsNullOrWhiteSpace(clueReward))
            {
                PixelGameState.Instance.AddClue(clueReward);
            }

            popupUI?.Show(npcName, dialogue);
        }
    }
}
