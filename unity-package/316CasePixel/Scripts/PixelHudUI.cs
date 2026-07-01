using System.Linq;
using TMPro;
using UnityEngine;

namespace Case316.Pixel
{
    public sealed class PixelHudUI : MonoBehaviour
    {
        [SerializeField] ActionTimeManager actionTimeManager;
        [SerializeField] TextMeshProUGUI actionText;
        [SerializeField] TextMeshProUGUI timeText;
        [SerializeField] TextMeshProUGUI clueText;
        [SerializeField] TextMeshProUGUI inventoryText;
        [SerializeField] DialoguePopupUI failurePopup;

        void OnEnable()
        {
            if (actionTimeManager != null)
            {
                actionTimeManager.ActionPointsChanged += HandleActionPointsChanged;
                actionTimeManager.SegmentChanged += HandleSegmentChanged;
                actionTimeManager.Failed += HandleFailed;
            }

            if (PixelGameState.Instance != null)
            {
                PixelGameState.Instance.StateChanged += RefreshCollections;
            }
        }

        void OnDisable()
        {
            if (actionTimeManager != null)
            {
                actionTimeManager.ActionPointsChanged -= HandleActionPointsChanged;
                actionTimeManager.SegmentChanged -= HandleSegmentChanged;
                actionTimeManager.Failed -= HandleFailed;
            }

            if (PixelGameState.Instance != null)
            {
                PixelGameState.Instance.StateChanged -= RefreshCollections;
            }
        }

        void HandleActionPointsChanged(int current, int max)
        {
            if (actionText != null)
            {
                actionText.text = $"行动力 {current}/{max}";
            }
        }

        void HandleSegmentChanged(TimeSegment segment)
        {
            if (timeText != null && segment != null)
            {
                timeText.text = $"{segment.timeLabel}  {segment.locationHint}";
            }
        }

        void RefreshCollections()
        {
            if (PixelGameState.Instance == null)
            {
                return;
            }

            if (clueText != null)
            {
                clueText.text = "线索\n" + string.Join("\n", PixelGameState.Instance.Clues);
            }

            if (inventoryText != null)
            {
                inventoryText.text = "背包\n" + string.Join("\n", PixelGameState.Instance.Inventory.Select(item => item.displayName));
            }
        }

        void HandleFailed(FailureReason reason)
        {
            failurePopup?.Show(reason.title, reason.message);
        }
    }
}
