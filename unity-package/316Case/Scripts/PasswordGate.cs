using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace Case316
{
    public sealed class PasswordGate : MonoBehaviour
    {
        [SerializeField] TMP_InputField input;
        [SerializeField] Button submitButton;
        [SerializeField] TextMeshProUGUI feedbackText;
        [SerializeField] DialogueRunner dialogueRunner;
        [SerializeField] string expectedPassword = "23160427";
        [SerializeField] string successNode = "OldGroupUnlocked";

        void Awake()
        {
            if (submitButton != null)
            {
                submitButton.onClick.AddListener(Submit);
            }
        }

        public void Submit()
        {
            if (input == null)
            {
                return;
            }

            string value = input.text.Trim();
            if (value == expectedPassword)
            {
                if (feedbackText != null)
                {
                    feedbackText.text = "备份文件已解锁";
                }

                gameObject.SetActive(false);

                if (dialogueRunner != null)
                {
                    dialogueRunner.StartDialogue(successNode);
                }
            }
            else
            {
                if (feedbackText != null)
                {
                    feedbackText.text = GetHint(value);
                }
            }
        }

        string GetHint(string value)
        {
            return value switch
            {
                "2316" => "时间不够。",
                "0427" => "日期不够。",
                "3160427" => "房间记得你，但那晚不是从房间开始。",
                _ => "密码错误。"
            };
        }
    }
}
