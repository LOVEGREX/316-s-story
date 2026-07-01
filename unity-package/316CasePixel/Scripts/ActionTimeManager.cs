using System;
using UnityEngine;

namespace Case316.Pixel
{
    public sealed class ActionTimeManager : MonoBehaviour
    {
        [SerializeField] int maxActionPoints = 18;
        [SerializeField] TimeSegment[] segments;

        int currentSegmentIndex;
        int actionPoints;
        bool isFailed;

        public event Action<int, int> ActionPointsChanged;
        public event Action<TimeSegment> SegmentChanged;
        public event Action<FailureReason> Failed;

        public int ActionPoints => actionPoints;
        public int MaxActionPoints => maxActionPoints;
        public TimeSegment CurrentSegment => segments != null && segments.Length > 0 ? segments[currentSegmentIndex] : null;
        public bool IsFailed => isFailed;

        void Start()
        {
            StartChapter();
        }

        public void StartChapter()
        {
            currentSegmentIndex = 0;
            isFailed = false;
            actionPoints = maxActionPoints;
            SegmentChanged?.Invoke(CurrentSegment);
            ActionPointsChanged?.Invoke(actionPoints, maxActionPoints);
        }

        public bool TrySpend(int amount, string reason)
        {
            if (isFailed)
            {
                return false;
            }

            if (amount <= 0)
            {
                return true;
            }

            if (actionPoints < amount)
            {
                AdvanceTime();
                return false;
            }

            actionPoints -= amount;
            ActionPointsChanged?.Invoke(actionPoints, maxActionPoints);

            if (actionPoints == 0)
            {
                AdvanceTime();
            }

            return true;
        }

        public void AdvanceTime()
        {
            if (isFailed)
            {
                return;
            }

            TimeSegment current = CurrentSegment;
            if (current != null && PixelGameState.Instance.ClueCount < current.minimumCluesRequired)
            {
                TriggerFailure(current.failureTitle, current.failureMessage);
                return;
            }

            currentSegmentIndex += 1;
            if (segments == null || currentSegmentIndex >= segments.Length)
            {
                return;
            }

            actionPoints = maxActionPoints;
            SegmentChanged?.Invoke(CurrentSegment);
            ActionPointsChanged?.Invoke(actionPoints, maxActionPoints);
        }

        public void TriggerFailure(string title, string message)
        {
            isFailed = true;
            Failed?.Invoke(new FailureReason
            {
                title = string.IsNullOrWhiteSpace(title) ? "线索不足" : title,
                message = string.IsNullOrWhiteSpace(message) ? "你错过了本时间段必须确认的关键线索。" : message
            });
        }
    }

    [Serializable]
    public sealed class TimeSegment
    {
        public string timeLabel = "23:47";
        public string locationHint = "男生宿舍316";
        public int minimumCluesRequired;
        public string failureTitle = "线索不足";
        [TextArea(2, 5)] public string failureMessage;
    }

    public struct FailureReason
    {
        public string title;
        public string message;
    }
}
