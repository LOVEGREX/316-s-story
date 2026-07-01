# 《316熄灯档案》路线 3 实现说明

本目录是一个 Unity + Yarn Spinner 的第一章实现包，不是完整 Unity 工程。你需要先用 Unity Hub 创建一个 2D 项目，再把 `Assets/316Case` 复制到 Unity 项目的 `Assets` 目录下。

## 推荐版本

- Unity：2022.3 LTS 或更新的 LTS 版本
- 剧情插件：Yarn Spinner for Unity
- UI：Unity UI + TextMeshPro
- 目标平台：先做 PC / Mac Demo，稳定后再考虑 Steam 或移动端

## 为什么选 Yarn Spinner

《316熄灯档案》的核心不是战斗或复杂物理，而是：

- 大量对话
- 选项分支
- 变量变化
- 条件解锁
- 线索收集
- 多结局

Yarn Spinner 很适合把这些内容和 Unity UI 分开管理。剧情写在 `.yarn` 文件里，Unity 负责界面、音效、动画、调查点、存档。

## 当前实现包内容

```text
Assets/316Case/
  Dialogues/
    chapter1.yarn
  Scripts/
    GameState.cs
    YarnCommandBridge.cs
    CluePanelUI.cs
    InvestigationPoint.cs
    PasswordGate.cs
    ChapterOneResultUI.cs
    ChapterBootstrap.cs
  Docs/
    技术路线3_Unity_YarnSpinner_实现说明.md
```

## 脚本职责

### GameState.cs

全局游戏状态。负责保存：

- 真相值
- 危险值
- 愧疚值
- 陈笑存在感
- 圆语信任
- 舆论扩散度
- 已获得线索
- 关键旗标

后续做存档时，也从这个脚本扩展。

### YarnCommandBridge.cs

连接 Yarn 和 Unity。

在 Yarn 剧情里可以这样写：

```yarn
<<add_clue "异常换锁">>
<<stat "truth" 1>>
<<flag "accepted_responsibility">>
```

这些命令会更新 Unity 里的 `GameState`。

### CluePanelUI.cs

负责刷新线索栏。玩家获得线索后，线索栏自动更新。

### InvestigationPoint.cs

用于宿舍调查点。比如圆语床位、纸箱、门后海报、毕业纪念册。  
点击按钮后，进入指定 Yarn 节点。

### PasswordGate.cs

负责旧群备份密码输入。

第一章密码是：

```text
23160427
```

输入正确后跳转 Yarn 节点：

```text
OldGroupUnlocked
```

### ChapterOneResultUI.cs

第一章结算。根据线索数量和愧疚值，分为：

- 普通线
- 标准线
- 深线

### ChapterBootstrap.cs

章节启动器。场景开始时重置状态，并从 `Chapter1_Start` 节点启动对话。

## Unity 场景搭建

### 1. 创建基础对象

在 Unity 场景中创建：

```text
GameRoot
  GameState
  DialogueSystem
  Canvas
```

`GameState` 对象挂：

```text
GameState.cs
```

`DialogueSystem` 对象挂：

```text
DialogueRunner
YarnCommandBridge.cs
ChapterBootstrap.cs
```

### 2. 配置 Yarn Spinner

导入 `chapter1.yarn` 后，创建 Yarn Project。

在 `DialogueRunner` 上配置：

- Yarn Project：你的 Yarn Project
- Start Node：`Chapter1_Start`
- Dialogue Views：使用 Yarn Spinner 自带的 Line View / Options List View

`ChapterBootstrap` 里也填入同一个 `DialogueRunner`。

### 3. 搭建 UI

建议第一章先用 4 个区域：

```text
Canvas
  BackgroundPanel      316宿舍/红果园氛围背景
  DialoguePanel        Yarn 对话文本
  ChoicePanel          Yarn 选项按钮
  CluePanel            线索列表
  PhonePanel           手机消息/群聊视觉层
  PasswordPanel        旧群密码输入
```

第一版可以先不做复杂动画，重点是让流程跑通。

### 4. 线索栏

`CluePanel` 下放两个 TextMeshProUGUI：

```text
ClueCountText
ClueListText
```

给 `CluePanel` 挂 `CluePanelUI.cs`，把两个文本拖进去。

### 5. 密码输入

`PasswordPanel` 下放：

```text
TMP_InputField
SubmitButton
FeedbackText
```

给 `PasswordPanel` 挂 `PasswordGate.cs`，填：

- Expected Password：`23160427`
- Success Node：`OldGroupUnlocked`
- Dialogue Runner：场景中的 DialogueRunner

在 Yarn 走到 `PasswordWait` 时，显示这个面板。第一版可以手动把面板做成常驻隐藏，走到密码段时再用简单按钮显示；正式版再写一个 Yarn 命令 `show_password_gate`。

## 第一章当前可玩流程

当前 `chapter1.yarn` 已包含：

1. 开场：毕业夜和圆语消息
2. 选择：上楼 / 打电话 / 回复未知号码 / 班级群询问
3. 316 门口：王哥和异常换锁
4. 316 调查：圆语床位、纸箱、海报、纪念册
5. 陈啸宇来电
6. 旧群密码
7. 316 旧群记录
8. 三人私聊：陈徐洋、吴小鸡、王小鸡
9. 证据判断
10. 郭女英旧截图
11. 陈笑敲门
12. 第一章结算

## 后续必须补的功能

### 必做

1. 存档 / 读档
2. 自动保存章节进度
3. 密码面板由 Yarn 命令自动弹出
4. 手机聊天 UI 独立样式
5. 调查点可视化
6. 第一章结束后进入第二章

### 强烈建议

1. 消息提示音
2. 来电震动 / 黑屏效果
3. 熄灯闪烁
4. 敲门声
5. 背景音乐分层
6. 线索获得弹窗

## 视觉方向

不要做成普通电子书。

第一章最好有三种界面状态：

1. 手机状态：群聊、未知号码、来电、旧群备份
2. 宿舍状态：316调查点、门锁、纸箱、纪念册
3. 黑暗状态：熄灯、敲门、陈笑在门外

这样玩家会感觉自己真的在“操作郭女英的夜晚”，而不是只是在翻文本。

## 推荐下一步

1. 用 Unity Hub 创建 2D 项目：`316Case_Unity`
2. 安装 Yarn Spinner for Unity
3. 复制 `Assets/316Case` 到项目
4. 创建 `Chapter1` 场景
5. 搭基础 Canvas 和 DialogueRunner
6. 跑通 `chapter1.yarn`
7. 再补手机 UI 和宿舍调查 UI

先让剧情能从开场跑到陈笑敲门。只要这个通了，后续章节就是继续加 Yarn 节点、线索和 UI 演出。

