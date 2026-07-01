# 316熄灯档案 Unity 第一章 Demo

## 打开方式

1. 用 Unity Hub 打开 `unity/316Story`。
2. 等待脚本编译完成。
3. 在顶部菜单点击 `Case316 > Build Chapter 01 Demo`。
4. 打开 `Assets/Case316/Scenes/Chapter01.unity`，点击 Play。

如果命令行生成场景，使用：

```bash
/Applications/Unity/Hub/Editor/6000.5.1f1/Unity.app/Contents/MacOS/Unity \
  -batchmode -quit \
  -projectPath /path/to/316-s-story/unity/316Story \
  -executeMethod Case316.Editor.ChapterOneSceneBuilder.BuildFromBatch \
  -logFile /tmp/316-unity-build.log
```

## 已实现玩法

- 主菜单：开始游戏、读取存档。
- HUD：游戏时间、地点、行动力、存档、读档、交互提示。
- 移动：WASD 或方向键按格移动。
- 交互：靠近人物/物品后按 E。
- 背包：获得关键物品后进入背包列表。
- 线索：调查后进入线索列表。
- 图文交互：弹出人物/物品图像色块、标题、正文和选项。
- 时间/行动力：移动消耗 1 点，交互消耗 2 点；行动力耗尽后进入下一时间段。
- 失败：当前时间段线索不足时弹出失败提示。
- 通关：第三时间段结束前收集至少 8 条线索。
- 存档：写入 `Application.persistentDataPath/case316_chapter1_save.json`。

## 第一章节奏

第一章目标通关时长约 40 分钟。Demo 内用 3 个时间段模拟完整章结构：

- 23:47 男生宿舍楼下：进入 316 前的异常门锁、毕业照、王哥。
- 23:56 男生宿舍316：圆语床位、纸箱、灯管、抽屉。
- 00:08 316旧群：陈笑、王交通、东校区末班车证词。

正式扩写时可以把每个时间段拆成 2-3 个小场景，保持同一套数据结构。

## 第一章关键反转种子

玩家以为线索都指向鲁毛，但第一章末尾会拿到“不是鲁毛”的证词。真正的矛盾不是某个人单独作恶，而是大二保研预选时 316 几个人共同修改、隐瞒、转发材料造成的连锁后果。圆语留下包裹，是为了逼大家在毕业前重演当年的选择。
