# 《316熄灯档案》像素探索版 Unity 制作说明

## 目标

把第一章从纯图文选择，升级为“像素探索 + 图文交互”的结构：

- WASD 控制郭女英移动。
- 靠近人物或物品后按 E 交互。
- 交互会弹出图文信息，并获得线索或物品。
- 玩家有行动力，移动和交互都会消耗行动力。
- 行动力耗尽进入下一个时间段。
- 每个时间段有最低线索要求，线索不足则失败。

## 推荐 Unity 设置

- Unity：2022.3 LTS 或更新 LTS。
- 项目类型：2D。
- Pixels Per Unit：建议先用 16 或 32。
- 图片导入：
  - Filter Mode：Point。
  - Compression：None。
  - Sprite Mode：Multiple。
  - Mesh Type：Full Rect。

像素资产：

```text
Assets/316CasePixel/Art/pixel-map-and-tiles.png
Assets/316CasePixel/Art/pixel-characters.png
```

## 场景结构建议

```text
Chapter1PixelScene
  GameRoot
    PixelGameState
    ActionTimeManager
  Grid
    Tilemap_Floor
    Tilemap_Walls
    Tilemap_Objects
  Player
    SpriteRenderer
    Rigidbody2D
    BoxCollider2D
    PixelPlayerController
  Interactables
    DoorPhoto
    DoorLock
    YuanYuBed
    ParcelBox
    PosterBack
    LightTube
    OldDrawer
    WangGe
    ChenXiaoDoor
  Canvas
    HUD
    Prompt
    DialoguePopup
    FailurePopup
```

## 核心脚本

### PixelPlayerController

负责：

- WASD 移动。
- E 交互。
- 检测附近的 `IInteractable`。
- 消耗行动力。

### ActionTimeManager

负责：

- 当前时间段。
- 当前行动力。
- 行动力耗尽后推进时间。
- 检查当前时间段最低线索要求。
- 触发失败提示。

### PixelGameState

负责：

- 已获得线索。
- 背包物品。
- 全局状态事件。

### ClueInteractable

挂在可调查物上，比如毕业照、纸箱、灯管。交互后：

- 弹出图文说明。
- 添加线索。
- 可添加背包物品。
- 可隐藏该物体。

### NpcInteractable

挂在人物上，比如王哥、陈笑。交互后：

- 弹出对话。
- 可添加线索。

### PixelHudUI

负责显示：

- 行动力。
- 当前时间段。
- 已获得线索。
- 背包。
- 失败提示。

## 第一章时间段

配置见：

```text
Assets/316CasePixel/Data/chapter1_pixel_interactables.json
```

推荐时间段：

1. 23:47 男生宿舍楼下  
   最低线索：1  
   失败：没能进入316，毕业照被取走。

2. 23:56 男生宿舍316  
   最低线索：4  
   失败：宿管查寝提前到来，王哥重新锁门。

3. 00:08 316旧群  
   最低线索：7  
   失败：旧群备份被远程撤回。

## 失败提示设计

失败不是“游戏结束”四个字，而要和剧情结合：

标题：线索不足  
正文：熄灯前你没有确认足够线索。宿管查寝提前到来，王哥把316重新锁上，圆语留下的包裹被代取。

玩家选择：

- 重新开始本时间段。
- 重新开始第一章。

## 下一步

1. 在 Unity 里创建 2D 场景。
2. 导入 `316CasePixel` 文件夹。
3. 切割像素图片为 Sprite。
4. 用 Tilemap 搭 316 宿舍和走廊。
5. 给交互物挂 `ClueInteractable`。
6. 给人物挂 `NpcInteractable`。
7. 给 Player 挂 `PixelPlayerController`。
8. 用 `ActionTimeManager` 配置三个时间段。

当前仓库同时提供 `game-pixel/` 网页原型，用于先体验 WASD/E/行动力/失败条件。

