using TMPro;
using UnityEngine;

namespace Case316.Pixel
{
    public sealed class InteractionPromptUI : MonoBehaviour
    {
        [SerializeField] CanvasGroup group;
        [SerializeField] TextMeshProUGUI promptText;

        public void Show(string text)
        {
            if (promptText != null)
            {
                promptText.text = text;
            }

            if (group != null)
            {
                group.alpha = 1f;
            }
        }

        public void Hide()
        {
            if (group != null)
            {
                group.alpha = 0f;
            }
        }
    }
}
