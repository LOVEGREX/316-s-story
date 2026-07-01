using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Case316
{
    public sealed class Case316UiController : MonoBehaviour
    {
        public GameObject mainMenu;
        public Button startButton;
        public Button loadButton;
        public Button saveButton;
        public Button hudLoadButton;
        public Text clockText;
        public Text locationText;
        public Text actionText;
        public Image actionFill;
        public Text clueListText;
        public Text itemListText;
        public Text promptText;
        public Text dialogueTitleText;
        public Text dialogueBodyText;
        public GameObject interactionPanel;
        public Image interactionImage;
        public Text interactionKindText;
        public Text interactionTitleText;
        public Text interactionBodyText;
        public Transform optionRoot;
        public Button optionButtonPrefab;
        public GameObject failurePanel;
        public Text failureTitleText;
        public Text failureBodyText;
        public Button failureRestartButton;
        public Text toastText;

        ChapterOneGameManager manager;
        Coroutine toastRoutine;

        public bool IsInteractionOpen => interactionPanel != null && interactionPanel.activeSelf;
        public bool IsMainMenuOpen => mainMenu != null && mainMenu.activeSelf;

        public void Bind(ChapterOneGameManager gameManager)
        {
            manager = gameManager;
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(manager.StartNewGame);
            loadButton.onClick.RemoveAllListeners();
            loadButton.onClick.AddListener(manager.LoadGame);
            saveButton.onClick.RemoveAllListeners();
            saveButton.onClick.AddListener(manager.SaveGame);
            hudLoadButton.onClick.RemoveAllListeners();
            hudLoadButton.onClick.AddListener(manager.LoadGame);
            failureRestartButton.onClick.RemoveAllListeners();
            failureRestartButton.onClick.AddListener(manager.StartNewGame);
        }

        public void ShowMainMenu(bool hasSave)
        {
            mainMenu.SetActive(true);
            loadButton.interactable = hasSave;
        }

        public void HideMainMenu()
        {
            mainMenu.SetActive(false);
        }

        public void Refresh(TimeSegmentData segment, int actionPoints, IReadOnlyCollection<string> clues, IReadOnlyCollection<string> items, ChapterInteractable target, bool hasSave)
        {
            clockText.text = segment.clock;
            locationText.text = segment.location;
            actionText.text = $"{Mathf.Max(0, actionPoints)}/{segment.actionPoints}";
            actionFill.fillAmount = Mathf.Clamp01((float)actionPoints / segment.actionPoints);
            clueListText.text = BuildList(clues, "尚未发现线索");
            itemListText.text = BuildList(items, "背包为空");
            promptText.text = target != null ? $"按 E 交互：{target.Data.displayName}" : "靠近可疑物品或人物，按 E 交互";
            loadButton.interactable = hasSave;
            hudLoadButton.interactable = hasSave;
        }

        public void SetDialogue(string title, string body)
        {
            dialogueTitleText.text = title;
            dialogueBodyText.text = body;
        }

        public void ShowInteraction(InteractableData data)
        {
            interactionPanel.SetActive(true);
            interactionKindText.text = data.kind == InteractableKind.Npc ? "人物" : "线索";
            interactionTitleText.text = data.displayName;
            interactionBodyText.text = data.body;
            interactionImage.color = data.kind == InteractableKind.Npc ? new Color(0.64f, 0.43f, 0.3f) : new Color(0.78f, 0.63f, 0.36f);

            foreach (Transform child in optionRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (var label in data.options)
            {
                CreateOption(label);
            }
            CreateOption("继续调查");
        }

        public void HideInteraction()
        {
            interactionPanel.SetActive(false);
        }

        public void ShowFailure(string title, string body)
        {
            failurePanel.SetActive(true);
            failureTitleText.text = title;
            failureBodyText.text = body;
        }

        public void HideFailure()
        {
            failurePanel.SetActive(false);
        }

        public void ShowToast(string message)
        {
            if (toastRoutine != null)
            {
                StopCoroutine(toastRoutine);
            }
            toastRoutine = StartCoroutine(ToastRoutine(message));
        }

        void CreateOption(string label)
        {
            var button = Instantiate(optionButtonPrefab, optionRoot);
            button.gameObject.SetActive(true);
            button.GetComponentInChildren<Text>().text = label;
            button.onClick.AddListener(manager.CloseInteraction);
        }

        IEnumerator ToastRoutine(string message)
        {
            toastText.gameObject.SetActive(true);
            toastText.text = message;
            yield return new WaitForSeconds(1.8f);
            toastText.gameObject.SetActive(false);
        }

        static string BuildList(IEnumerable<string> values, string emptyText)
        {
            var builder = new StringBuilder();
            foreach (var value in values)
            {
                builder.Append("· ").Append(value).AppendLine();
            }
            return builder.Length == 0 ? emptyText : builder.ToString();
        }
    }
}
