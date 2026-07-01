using TMPro;
using UnityEngine;

namespace Case316
{
    public sealed class CluePanelUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI clueListText;
        [SerializeField] TextMeshProUGUI clueCountText;

        void OnEnable()
        {
            if (GameState.Instance != null)
            {
                GameState.Instance.ClueAdded += HandleClueAdded;
                GameState.Instance.StateChanged += Refresh;
                Refresh();
            }
        }

        void OnDisable()
        {
            if (GameState.Instance != null)
            {
                GameState.Instance.ClueAdded -= HandleClueAdded;
                GameState.Instance.StateChanged -= Refresh;
            }
        }

        void HandleClueAdded(string clueName)
        {
            Refresh();
        }

        public void Refresh()
        {
            if (GameState.Instance == null)
            {
                return;
            }

            if (clueCountText != null)
            {
                clueCountText.text = $"线索 {GameState.Instance.ClueCount}";
            }

            if (clueListText == null)
            {
                return;
            }

            if (GameState.Instance.ClueCount == 0)
            {
                clueListText.text = "暂无线索";
                return;
            }

            clueListText.text = string.Join("\n", GameState.Instance.Clues);
        }
    }
}
