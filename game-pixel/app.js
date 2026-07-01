const canvas = document.getElementById("gameCanvas");
const ctx = canvas.getContext("2d");
ctx.imageSmoothingEnabled = false;

const tile = 32;
const mapWidth = 20;
const mapHeight = 14;

const segments = [
  {
    time: "23:47",
    location: "男生宿舍楼下",
    ap: 18,
    minClues: 1,
    failureTitle: "没有进入316",
    failureText: "你在楼下浪费了太多时间。圆语的定时消息消失了，316门口的毕业照也被人取走。"
  },
  {
    time: "23:56",
    location: "男生宿舍316",
    ap: 18,
    minClues: 4,
    failureTitle: "线索不足",
    failureText: "熄灯前你没有确认足够线索。宿管查寝提前到来，王哥把316重新锁上，圆语留下的包裹被代取。"
  },
  {
    time: "00:08",
    location: "316旧群",
    ap: 16,
    minClues: 7,
    failureTitle: "旧群断线",
    failureText: "你没能及时解开旧群备份。聊天记录被远程撤回，只剩下一条无法验证的消息：不是鲁毛。"
  }
];

const walls = new Set();
for (let x = 0; x < mapWidth; x += 1) {
  walls.add(`${x},0`);
  walls.add(`${x},${mapHeight - 1}`);
}
for (let y = 0; y < mapHeight; y += 1) {
  walls.add(`0,${y}`);
  walls.add(`${mapWidth - 1},${y}`);
}
[
  [6, 2], [6, 3], [6, 4], [6, 5], [13, 2], [13, 3], [13, 4],
  [3, 9], [4, 9], [15, 8], [16, 8], [2, 3], [17, 3]
].forEach(([x, y]) => walls.add(`${x},${y}`));

const interactables = [
  {
    id: "photo",
    x: 10,
    y: 12,
    icon: "▧",
    name: "被涂黑的毕业照",
    clue: "被涂黑的毕业照",
    item: "毕业照",
    text: "316毕业合照被夹在门缝里，圆语的脸被黑笔涂掉。背面写着：红果园不会记得我们，但316记得。"
  },
  {
    id: "lock",
    x: 9,
    y: 12,
    icon: "▣",
    name: "异常门锁",
    clue: "异常换锁",
    text: "你的钥匙拧不动。316从未申请换锁，但王哥手里有一把写着316的备用钥匙。"
  },
  {
    id: "bed",
    x: 3,
    y: 2,
    icon: "▤",
    name: "圆语床位",
    clue: "圆语标记的纪念册",
    item: "毕业纪念册",
    text: "第一页夹着便利贴：女英，如果你还记得大二那次保研预选，就不要相信崔向阳。陈笑的名字被铅笔圈了很多遍。"
  },
  {
    id: "box",
    x: 8,
    y: 5,
    icon: "□",
    name: "未封口纸箱",
    clue: "寄给316的包裹",
    item: "快递底单",
    text: "寄件人圆语，收件人316，经手人王小鸡。备注：毕业前勿退回。"
  },
  {
    id: "poster",
    x: 2,
    y: 10,
    icon: "▥",
    name: "门后海报",
    clue: "东校区路线编号",
    text: "海报背面写着：东校区 / 23:16 / 0427 / 王交通。下面还有一句：末班车回来的人，不是去的人。"
  },
  {
    id: "light",
    x: 10,
    y: 3,
    icon: "✦",
    name: "被遮住的灯管",
    clue: "被遮住的灯管",
    text: "灯管边缘贴着一小片黑色胶带。拆下后，纸条上写着：熄灯不是意外，是信号。"
  },
  {
    id: "drawer",
    x: 16,
    y: 4,
    icon: "▨",
    name: "郭女英的抽屉",
    clue: "郭女英旧笔记",
    item: "旧笔记本",
    text: "4月27日那页写着：鲁毛说不能算了。下面一行被划掉：我是不是不该发给吴小鸡？"
  },
  {
    id: "wangge",
    x: 17,
    y: 11,
    npc: true,
    icon: "王",
    name: "王哥",
    clue: "王哥通风报信",
    text: "王哥说毕业季统一换锁，但你听见他发语音：他来了，一个人。"
  },
  {
    id: "chenxiao",
    x: 10,
    y: 1,
    npc: true,
    icon: "笑",
    name: "陈笑",
    clue: "不是鲁毛",
    text: "熄灯后，陈笑站在门外。他没有戴学位帽，只问：你还是先问别人在哪？"
  }
];

const state = {
  player: { x: 10, y: 11 },
  segment: 0,
  ap: segments[0].ap,
  clues: new Set(),
  inventory: new Set(),
  used: new Set(),
  failed: false
};

function draw() {
  ctx.clearRect(0, 0, canvas.width, canvas.height);
  for (let y = 0; y < mapHeight; y += 1) {
    for (let x = 0; x < mapWidth; x += 1) {
      const isWall = walls.has(`${x},${y}`);
      ctx.fillStyle = isWall ? "#263432" : ((x + y) % 2 === 0 ? "#1b2625" : "#202d2b");
      ctx.fillRect(x * tile, y * tile, tile, tile);
      ctx.strokeStyle = "rgba(237,243,241,.05)";
      ctx.strokeRect(x * tile, y * tile, tile, tile);
    }
  }

  for (const item of interactables) {
    if (state.used.has(item.id)) continue;
    ctx.fillStyle = item.npc ? "#9e6f4f" : "#c7a464";
    ctx.fillRect(item.x * tile + 6, item.y * tile + 6, 20, 20);
    ctx.fillStyle = "#101515";
    ctx.font = "16px sans-serif";
    ctx.textAlign = "center";
    ctx.textBaseline = "middle";
    ctx.fillText(item.icon, item.x * tile + 16, item.y * tile + 17);
  }

  ctx.fillStyle = "#b84949";
  ctx.fillRect(state.player.x * tile + 7, state.player.y * tile + 5, 18, 22);
  ctx.fillStyle = "#edf3f1";
  ctx.fillRect(state.player.x * tile + 11, state.player.y * tile + 8, 10, 8);
}

function move(dx, dy) {
  if (state.failed) return;
  const nx = state.player.x + dx;
  const ny = state.player.y + dy;
  if (walls.has(`${nx},${ny}`)) return;
  state.player.x = nx;
  state.player.y = ny;
  spend(1);
  render();
}

function interact() {
  if (state.failed) return;
  const target = nearestInteractable();
  if (!target) return;
  spend(2);
  state.clues.add(target.clue);
  if (target.item) state.inventory.add(target.item);
  state.used.add(target.id);
  setDialogue(target.name, target.text);
  render();
}

function nearestInteractable() {
  return interactables.find((item) => {
    if (state.used.has(item.id)) return false;
    const distance = Math.abs(item.x - state.player.x) + Math.abs(item.y - state.player.y);
    return distance <= 1;
  });
}

function spend(amount) {
  state.ap -= amount;
  if (state.ap <= 0) advanceTime();
}

function advanceTime() {
  const current = segments[state.segment];
  if (state.clues.size < current.minClues) {
    fail(current.failureTitle, current.failureText);
    return;
  }
  if (state.segment < segments.length - 1) {
    state.segment += 1;
    state.ap = segments[state.segment].ap;
    setDialogue("时间推进", `现在是${segments[state.segment].time}。你必须在本时间段结束前收集至少${segments[state.segment].minClues}条线索。`);
  } else {
    state.ap = 0;
    setDialogue("第一章完成", "你在行动力耗尽前拿到了足够线索。下一步可以进入旧群深线。");
  }
}

function fail(title, text) {
  state.failed = true;
  document.getElementById("failureTitle").textContent = title;
  document.getElementById("failureText").textContent = text;
  document.getElementById("failure").classList.remove("is-hidden");
}

function setDialogue(title, text) {
  document.getElementById("dialogueTitle").textContent = title;
  document.getElementById("dialogueText").textContent = text;
}

function render() {
  draw();
  const segment = segments[state.segment];
  document.getElementById("timeLabel").textContent = segment.time;
  document.getElementById("locationLabel").textContent = segment.location;
  document.getElementById("apLabel").textContent = `${Math.max(0, state.ap)}/${segment.ap}`;
  document.getElementById("apBar").style.width = `${Math.max(0, state.ap / segment.ap * 100)}%`;
  document.getElementById("clueCount").textContent = state.clues.size;
  document.getElementById("clues").innerHTML = [...state.clues].map((clue) => `<li>${clue}</li>`).join("");
  document.getElementById("inventory").innerHTML = [...state.inventory].map((item) => `<li>${item}</li>`).join("");
  const target = nearestInteractable();
  document.getElementById("prompt").textContent = target ? `按 E 交互：${target.name}` : "靠近可疑物品或人物，按 E 交互。";
}

function reset() {
  state.player = { x: 10, y: 11 };
  state.segment = 0;
  state.ap = segments[0].ap;
  state.clues.clear();
  state.inventory.clear();
  state.used.clear();
  state.failed = false;
  document.getElementById("failure").classList.add("is-hidden");
  setDialogue("调查目标", "在行动力耗尽前收集足够线索。第一章最低目标：7条线索。");
  render();
}

window.addEventListener("keydown", (event) => {
  const key = event.key.toLowerCase();
  if (["w", "a", "s", "d", "e", "arrowup", "arrowleft", "arrowdown", "arrowright"].includes(key)) {
    event.preventDefault();
  }
  if (key === "w" || key === "arrowup") move(0, -1);
  if (key === "s" || key === "arrowdown") move(0, 1);
  if (key === "a" || key === "arrowleft") move(-1, 0);
  if (key === "d" || key === "arrowright") move(1, 0);
  if (key === "e") interact();
});

document.getElementById("restart").addEventListener("click", reset);
document.getElementById("failureRestart").addEventListener("click", reset);

window.__pixelDebug = {
  getState: () => ({
    segment: state.segment,
    ap: state.ap,
    clues: [...state.clues],
    inventory: [...state.inventory],
    failed: state.failed
  }),
  forceAdvanceTime: advanceTime,
  forceFail: () => fail(segments[state.segment].failureTitle, segments[state.segment].failureText)
};

render();
