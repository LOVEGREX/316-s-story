using UnityEngine;
using Yarn.Unity;

namespace Case316
{
    public sealed class ChapterBootstrap : MonoBehaviour
    {
        [SerializeField] DialogueRunner dialogueRunner;
        [SerializeField] string startNode = "Chapter1_Start";
        [SerializeField] bool resetStateOnStart = true;

        void Start()
        {
            if (GameState.Instance != null && resetStateOnStart)
            {
                GameState.Instance.ResetState();
            }

            if (dialogueRunner != null)
            {
                dialogueRunner.StartDialogue(startNode);
            }
        }
    }
}
