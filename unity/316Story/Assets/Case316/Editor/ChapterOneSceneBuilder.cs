using System.Collections.Generic;
using Case316;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Case316.Editor
{
    public static class ChapterOneSceneBuilder
    {
        const string DataPath = "Assets/Case316/Data/Chapter01.asset";
        const string ScenePath = "Assets/Case316/Scenes/Chapter01.unity";

        [MenuItem("Case316/Build Chapter 01 Demo")]
        public static void BuildChapterOneDemo()
        {
            var data = CreateDataAsset();
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "Chapter01";

            var camera = new GameObject("Main Camera").AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.orthographic = true;
            camera.orthographicSize = 8f;
            camera.transform.position = new Vector3(0f, 0f, -10f);
            camera.backgroundColor = new Color(0.04f, 0.06f, 0.06f);

            var boardRoot = new GameObject("Board").transform;
            var manager = new GameObject("ChapterOneGameManager").AddComponent<ChapterOneGameManager>();
            manager.chapter = data;
            manager.boardRoot = boardRoot;
            manager.ui = BuildUi();

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void BuildFromBatch()
        {
            BuildChapterOneDemo();
        }

        static ChapterData CreateDataAsset()
        {
            var data = AssetDatabase.LoadAssetAtPath<ChapterData>(DataPath);
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<ChapterData>();
                AssetDatabase.CreateAsset(data, DataPath);
            }

            data.chapterId = "chapter_01";
            data.title = "第一章：红果园的最后一晚";
            data.boardWidth = 20;
            data.boardHeight = 14;
            data.playerStart = new Vector2Int(10, 11);
            data.requiredCluesToClear = 8;
            data.realMinutesTarget = 40f;
            data.segments = new List<TimeSegmentData>
            {
                new TimeSegmentData
                {
                    clock = "23:47",
                    location = "男生宿舍楼下",
                    actionPoints = 18,
                    requiredClues = 1,
                    gameMinutes = 18,
                    failureTitle = "没有进入316",
                    failureText = "你在楼下浪费了太多时间。圆语的定时消息消失了，316门口的毕业照也被人取走。"
                },
                new TimeSegmentData
                {
                    clock = "23:56",
                    location = "男生宿舍316",
                    actionPoints = 20,
                    requiredClues = 5,
                    gameMinutes = 24,
                    failureTitle = "线索不足",
                    failureText = "熄灯前你没有确认足够线索。宿管查寝提前到来，王哥把316重新锁上，圆语留下的包裹被代取。"
                },
                new TimeSegmentData
                {
                    clock = "00:08",
                    location = "316旧群",
                    actionPoints = 18,
                    requiredClues = 8,
                    gameMinutes = 22,
                    failureTitle = "旧群断线",
                    failureText = "你没能及时解开旧群备份。聊天记录被远程撤回，只剩下一条无法验证的消息：不是鲁毛。"
                }
            };
            data.walls = new List<Vector2Int>
            {
                new Vector2Int(6, 2), new Vector2Int(6, 3), new Vector2Int(6, 4), new Vector2Int(6, 5),
                new Vector2Int(13, 2), new Vector2Int(13, 3), new Vector2Int(13, 4),
                new Vector2Int(3, 9), new Vector2Int(4, 9), new Vector2Int(15, 8), new Vector2Int(16, 8),
                new Vector2Int(2, 3), new Vector2Int(17, 3), new Vector2Int(11, 8)
            };
            data.interactables = BuildInteractables();
            EditorUtility.SetDirty(data);
            return data;
        }

        static List<InteractableData> BuildInteractables()
        {
            return new List<InteractableData>
            {
                Interactable("photo", "被涂黑的毕业照", InteractableKind.Clue, 10, 12, "clue_photo", "被涂黑的毕业照", "item_photo", "毕业照", "316毕业合照被夹在门缝里，圆语的脸被黑笔涂掉。背面写着：红果园不会记得我们，但316记得。", "收进背包", "翻看背面"),
                Interactable("lock", "异常门锁", InteractableKind.Clue, 9, 12, "clue_lock", "异常换锁", "", "", "你的钥匙拧不动。316从未申请换锁，但王哥手里有一把写着316的备用钥匙。", "记下锁芯编号", "敲门"),
                Interactable("bed", "圆语床位", InteractableKind.Clue, 3, 2, "clue_album", "圆语标记的纪念册", "item_album", "毕业纪念册", "第一页夹着便利贴：女英，如果你还记得大二那次保研预选，就不要相信崔向阳。陈笑的名字被铅笔圈了很多遍。", "拿走纪念册", "检查枕套"),
                Interactable("box", "未封口纸箱", InteractableKind.Clue, 8, 5, "clue_package", "寄给316的包裹", "item_waybill", "快递底单", "寄件人圆语，收件人316，经手人王小鸡。备注：毕业前勿退回。", "拆开胶带", "拍下底单"),
                Interactable("poster", "门后海报", InteractableKind.Clue, 2, 10, "clue_route", "东校区路线编号", "", "", "海报背面写着：东校区 / 23:16 / 0427 / 王交通。下面还有一句：末班车回来的人，不是去的人。", "记录路线", "对照校历"),
                Interactable("light", "被遮住的灯管", InteractableKind.Clue, 10, 3, "clue_light", "被遮住的灯管", "", "", "灯管边缘贴着一小片黑色胶带。拆下后，纸条上写着：熄灯不是意外，是信号。", "拆下胶带", "保持原样"),
                Interactable("drawer", "郭女英的抽屉", InteractableKind.Clue, 16, 4, "clue_note", "郭女英旧笔记", "item_note", "旧笔记本", "4月27日那页写着：鲁毛说不能算了。下面一行被划掉：我是不是不该发给吴小鸡？", "撕下复印页", "核对字迹"),
                Interactable("wangge", "王哥", InteractableKind.Npc, 17, 11, "clue_wangge", "王哥通风报信", "", "", "王哥说毕业季统一换锁，但你听见他发语音：他来了，一个人。", "追问换锁", "假装离开"),
                Interactable("chenxiao", "陈笑", InteractableKind.Npc, 10, 1, "clue_chenxiao", "不是鲁毛", "", "", "熄灯后，陈笑站在门外。他没有戴学位帽，只问：你还是先问别人在哪？", "问他鲁毛在哪", "问他圆语在哪"),
                Interactable("traffic", "王交通", InteractableKind.Npc, 14, 10, "clue_bus", "末班车证词", "", "", "王交通说大二那晚东校区末班车提前十分钟发车，可监控里有人用316的校园卡刷了闸机。", "追问刷卡人", "记录末班车时间")
            };
        }

        static InteractableData Interactable(string id, string name, InteractableKind kind, int x, int y, string clueId, string clueName, string itemId, string itemName, string body, params string[] options)
        {
            return new InteractableData
            {
                id = id,
                displayName = name,
                kind = kind,
                gridPosition = new Vector2Int(x, y),
                clueId = clueId,
                clueName = clueName,
                itemId = itemId,
                itemName = itemName,
                body = body,
                options = new List<string>(options)
            };
        }

        static Case316UiController BuildUi()
        {
            var font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            var canvas = new GameObject("Canvas").AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.gameObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvas.gameObject.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
            canvas.gameObject.AddComponent<GraphicRaycaster>();
            var ui = canvas.gameObject.AddComponent<Case316UiController>();

            var hud = Panel(canvas.transform, "HUD", new Color(0.04f, 0.06f, 0.06f, 0f), new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero);
            ui.clockText = Text(hud, "Clock", "23:47", font, 30, TextAnchor.UpperLeft, new Vector2(0, 1), new Vector2(0, 1), new Vector2(30, -30), new Vector2(260, 42));
            ui.locationText = Text(hud, "Location", "男生宿舍楼下", font, 24, TextAnchor.UpperLeft, new Vector2(0, 1), new Vector2(0, 1), new Vector2(30, -76), new Vector2(360, 36));
            ui.actionText = Text(hud, "Action", "18/18", font, 24, TextAnchor.UpperLeft, new Vector2(0, 1), new Vector2(0, 1), new Vector2(30, -118), new Vector2(220, 34));
            ui.actionFill = Image(hud, "ActionFill", new Color(0.75f, 0.55f, 0.28f), new Vector2(0, 1), new Vector2(0, 1), new Vector2(120, -114), new Vector2(240, 10));
            ui.actionFill.type = Image.Type.Filled;
            ui.actionFill.fillMethod = Image.FillMethod.Horizontal;
            ui.saveButton = Button(hud, "Save", "存档", font, new Vector2(1, 1), new Vector2(1, 1), new Vector2(-320, -35), new Vector2(110, 42));
            ui.hudLoadButton = Button(hud, "Load", "读档", font, new Vector2(1, 1), new Vector2(1, 1), new Vector2(-190, -35), new Vector2(110, 42));
            ui.promptText = Text(hud, "Prompt", "靠近可疑物品或人物，按 E 交互", font, 24, TextAnchor.MiddleCenter, new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 40), new Vector2(780, 44));

            var side = Panel(canvas.transform, "RightPanel", new Color(0.08f, 0.11f, 0.11f, 0.94f), new Vector2(1, 0), new Vector2(1, 1), new Vector2(-420, 0), new Vector2(420, 1080));
            ui.dialogueTitleText = Text(side, "DialogueTitle", "调查目标", font, 28, TextAnchor.UpperLeft, new Vector2(0, 1), new Vector2(0, 1), new Vector2(26, -24), new Vector2(360, 38));
            ui.dialogueBodyText = Text(side, "DialogueBody", "在行动力耗尽前收集足够线索。", font, 22, TextAnchor.UpperLeft, new Vector2(0, 1), new Vector2(0, 1), new Vector2(26, -70), new Vector2(360, 160));
            ui.clueListText = Text(side, "Clues", "尚未发现线索", font, 20, TextAnchor.UpperLeft, new Vector2(0, 1), new Vector2(0, 1), new Vector2(26, -260), new Vector2(360, 310));
            ui.itemListText = Text(side, "Items", "背包为空", font, 20, TextAnchor.UpperLeft, new Vector2(0, 1), new Vector2(0, 1), new Vector2(26, -600), new Vector2(360, 220));

            BuildMainMenu(canvas.transform, ui, font);
            BuildInteraction(canvas.transform, ui, font);
            BuildFailure(canvas.transform, ui, font);
            ui.toastText = Text(canvas.transform, "Toast", "", font, 22, TextAnchor.MiddleCenter, new Vector2(1, 0), new Vector2(1, 0), new Vector2(-260, 60), new Vector2(420, 52));
            ui.toastText.color = Color.white;
            ui.toastText.gameObject.AddComponent<Image>().color = new Color(0.09f, 0.13f, 0.13f, 0.95f);
            ui.toastText.gameObject.SetActive(false);
            return ui;
        }

        static void BuildMainMenu(Transform root, Case316UiController ui, Font font)
        {
            ui.mainMenu = Panel(root, "MainMenu", new Color(0.04f, 0.06f, 0.06f, 0.98f), new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero).gameObject;
            Text(ui.mainMenu.transform, "Title", "316熄灯档案", font, 64, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 160), new Vector2(720, 90));
            Text(ui.mainMenu.transform, "Subtitle", "第一章：红果园的最后一晚", font, 28, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 90), new Vector2(720, 50));
            ui.startButton = Button(ui.mainMenu.transform, "StartButton", "开始游戏", font, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, -10), new Vector2(260, 58));
            ui.loadButton = Button(ui.mainMenu.transform, "LoadButton", "读取存档", font, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, -86), new Vector2(260, 58));
        }

        static void BuildInteraction(Transform root, Case316UiController ui, Font font)
        {
            ui.interactionPanel = Panel(root, "InteractionPanel", new Color(0.02f, 0.03f, 0.03f, 0.72f), new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero).gameObject;
            var card = Panel(ui.interactionPanel.transform, "Card", new Color(0.08f, 0.11f, 0.11f, 0.98f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(920, 420));
            ui.interactionImage = Image(card, "Image", new Color(0.55f, 0.42f, 0.24f), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(175, 0), new Vector2(300, 320));
            ui.interactionKindText = Text(card, "Kind", "线索", font, 18, TextAnchor.UpperLeft, new Vector2(0, 1), new Vector2(0, 1), new Vector2(350, -38), new Vector2(420, 34));
            ui.interactionTitleText = Text(card, "Title", "标题", font, 34, TextAnchor.UpperLeft, new Vector2(0, 1), new Vector2(0, 1), new Vector2(350, -78), new Vector2(500, 46));
            ui.interactionBodyText = Text(card, "Body", "正文", font, 24, TextAnchor.UpperLeft, new Vector2(0, 1), new Vector2(0, 1), new Vector2(350, -136), new Vector2(500, 160));
            ui.optionRoot = Panel(card, "Options", new Color(0, 0, 0, 0), new Vector2(0, 0), new Vector2(1, 0), new Vector2(350, 34), new Vector2(520, 90));
            ui.optionButtonPrefab = Button(ui.optionRoot, "OptionButton", "继续调查", font, new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(80, 0), new Vector2(160, 46));
            ui.optionButtonPrefab.gameObject.SetActive(false);
            ui.interactionPanel.SetActive(false);
        }

        static void BuildFailure(Transform root, Case316UiController ui, Font font)
        {
            ui.failurePanel = Panel(root, "FailurePanel", new Color(0.02f, 0.03f, 0.03f, 0.78f), new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero).gameObject;
            var card = Panel(ui.failurePanel.transform, "FailureCard", new Color(0.1f, 0.1f, 0.09f, 0.98f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(620, 330));
            ui.failureTitleText = Text(card, "FailureTitle", "线索不足", font, 38, TextAnchor.UpperLeft, new Vector2(0, 1), new Vector2(0, 1), new Vector2(38, -34), new Vector2(540, 54));
            ui.failureBodyText = Text(card, "FailureBody", "", font, 24, TextAnchor.UpperLeft, new Vector2(0, 1), new Vector2(0, 1), new Vector2(38, -108), new Vector2(540, 120));
            ui.failureRestartButton = Button(card, "RestartButton", "重新开始第一章", font, new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 48), new Vector2(240, 52));
            ui.failurePanel.SetActive(false);
        }

        static RectTransform Panel(Transform parent, string name, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 size)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;
            go.GetComponent<Image>().color = color;
            return rect;
        }

        static Image Image(Transform parent, string name, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 size)
        {
            var rect = Panel(parent, name, color, anchorMin, anchorMax, anchoredPosition, size);
            return rect.GetComponent<Image>();
        }

        static Text Text(Transform parent, string name, string text, Font font, int size, TextAnchor alignment, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 rectSize)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = rectSize;
            var label = go.GetComponent<Text>();
            label.font = font;
            label.text = text;
            label.fontSize = size;
            label.alignment = alignment;
            label.color = new Color(0.93f, 0.96f, 0.95f);
            label.horizontalOverflow = HorizontalWrapMode.Wrap;
            label.verticalOverflow = VerticalWrapMode.Overflow;
            return label;
        }

        static Button Button(Transform parent, string name, string text, Font font, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 size)
        {
            var rect = Panel(parent, name, new Color(0.1f, 0.15f, 0.14f, 0.98f), anchorMin, anchorMax, anchoredPosition, size);
            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = rect.GetComponent<Image>();
            Text(rect, "Text", text, font, 22, TextAnchor.MiddleCenter, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            return button;
        }
    }
}
