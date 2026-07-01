using TMPro;
using UnityEngine;

namespace Case316.Pixel
{
    public sealed class DialoguePopupUI : MonoBehaviour
    {
        [SerializeField] CanvasGroup group;
        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] TextMeshProUGUI bodyText;

        public void Show(string title, string body)
        {
            if (titleText != null)
            {
                titleText.text = title;
            }

            if (bodyText != null)
            {
                bodyText.text = body;
            }

            if (group != null)
            {
                group.alpha = 1f;
                group.interactable = true;
                group.blocksRaycasts = true;
            }
        }

        public void Hide()
        {
            if (group != null)
            {
                group.alpha = 0f;
                group.interactable = false;
                group.blocksRaycasts = false;
            }
        }
    }
}
