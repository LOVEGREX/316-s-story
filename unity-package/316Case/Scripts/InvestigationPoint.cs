using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace Case316
{
    [RequireComponent(typeof(Button))]
    public sealed class InvestigationPoint : MonoBehaviour
    {
        [SerializeField] DialogueRunner dialogueRunner;
        [SerializeField] string targetNode;
        [SerializeField] bool disableAfterUse = true;

        Button button;

        void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(StartInvestigation);
        }

        void StartInvestigation()
        {
            if (dialogueRunner == null || string.IsNullOrWhiteSpace(targetNode))
            {
                Debug.LogWarning($"Investigation point {name} is missing a DialogueRunner or target node.");
                return;
            }

            dialogueRunner.StartDialogue(targetNode);

            if (disableAfterUse)
            {
                button.interactable = false;
            }
        }
    }
}
