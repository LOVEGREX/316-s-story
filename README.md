# 316熄灯档案

校园悬疑解谜 RPG。当前仓库已包含第一章网页 Demo、像素探索原型，以及正式 Unity 第一章 Demo 工程骨架。

## 当前 Demo

第一章：《红果园的最后一晚》

设定：

- 学校：红果园大学
- 宿舍：男生宿舍 316
- 时间线：大二发生保研旧事，大四毕业前夜爆发
- 主角：郭女英，男性
- 旧事核心人物：鲁毛

## 直接游玩

正式 Unity 版第一章：

```text
unity/316Story
```

用 Unity Hub 打开该目录后，在 Unity 顶部菜单点击：

```text
Case316 > Build Chapter 01 Demo
```

然后打开：

```text
Assets/Case316/Scenes/Chapter01.unity
```

点击 Play 即可验收第一章 Unity Demo。已实现主菜单、存档、读档、20x14 盘面场景、WASD 移动、E 交互、背包、线索、行动力时间推进、失败提示和第一章通关判定。

像素探索原型，支持 WASD 移动、E 交互、行动力、背包、线索不足失败：

```text
game-pixel/index.html
```

图文选择 Demo：

```text
game/index.html
```

或打开根目录 `index.html`，选择要进入的版本。

第一章 Demo 已实现：

- 像素地图移动探索原型
- WASD 移动和 E 交互
- 行动力和时间段推进
- 线索不足失败提示
- 背包和线索收集
- 开场选择
- 316 门口选择
- 宿舍调查点
- 线索系统
- 手机消息栏
- 隐藏数值栏
- 旧群密码输入，密码为 `23160427`
- 陈笑敲门章末选择
- 第一章结算

## 目录结构

```text
assets/images/          统一风格图片资产
assets/pixel/           像素风地图、角色、物品资产
docs/                   剧情与设定文档
game/                   可直接打开的网页 Demo
game-pixel/             可直接打开的像素探索原型
unity-package/          Unity + Yarn Spinner 实现包
unity/316Story/         正式 Unity 第一章 Demo 工程
```

## 技术路线

短期：用 `unity/316Story/` 完成第一章可玩 Demo。  
长期：沿用当前 Unity 数据结构扩展 8 章，逐步替换程序化色块为正式像素角色、物品和地图 Tilemap。

## 验收重点

- 第一章可以从头玩到结算。
- 所有人物设定为男性。
- 场景是男生宿舍 316。
- 旧事人物统一为鲁毛。
- 图片资产风格统一，已存放在 `assets/images/`。
- 像素资产已存放在 `assets/pixel/`，Unity 包中也有一份副本。
