using TMPro;
using UnityEngine;

namespace Case316
{
    public sealed class ChapterOneResultUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] TextMeshProUGUI bodyText;

        public void ShowResult()
        {
            ChapterResult result = GameState.Instance.EvaluateChapterOne();

            if (titleText != null)
            {
                titleText.text = result switch
                {
                    ChapterResult.DeepLine => "第一章结算：深线",
                    ChapterResult.StandardLine => "第一章结算：标准线",
                    _ => "第一章结算：普通线"
                };
            }

            if (bodyText != null)
            {
                bodyText.text = result switch
                {
                    ChapterResult.DeepLine => "你获得了足够多的线索，并开始承认自己的旧事责任。第二章中，陈笑会主动留下更接近真相的证据。",
                    ChapterResult.StandardLine => "你掌握了进入第二章调查所需的关键线索。匿名墙、快递柜和离校登记将全部开放。",
                    _ => "你的线索不足。第二章仍可继续，但旧群备份会缺失部分内容。",
                };
            }
        }
    }
}
