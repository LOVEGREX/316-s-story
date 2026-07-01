const state = {
  nodeId: "start",
  clues: new Set(),
  flags: new Set(),
  stats: { truth: 0, danger: 0, guilt: 0, chenXiao: 0, yuanYu: 0, publicOpinion: 0 },
  phone: [],
  entered: new Set()
};

const $ = (id) => document.getElementById(id);
const els = {
  scene: $("sceneImage"),
  characterBox: $("characterBox"),
  avatar: $("avatar"),
  speakerName: $("speakerName"),
  speakerRole: $("speakerRole"),
  kicker: $("nodeKicker"),
  title: $("nodeTitle"),
  story: $("storyText"),
  choices: $("choices"),
  passwordBox: $("passwordBox"),
  passwordInput: $("passwordInput"),
  passwordButton: $("passwordButton"),
  passwordHint: $("passwordHint"),
  phoneLog: $("phoneLog"),
  clueCount: $("clueCount"),
  clueList: $("clueList"),
  statBars: $("statBars"),
  restart: $("restartButton")
};

function resetGame() {
  state.nodeId = "start";
  state.clues.clear();
  state.flags.clear();
  state.phone = [];
  state.entered.clear();
  state.stats = { truth: 0, danger: 0, guilt: 0, chenXiao: 0, yuanYu: 0, publicOpinion: 0 };
  render();
}

function render(result = []) {
  const node = STORY[state.nodeId];
  if (!node) throw new Error(`Missing node: ${state.nodeId}`);

  if (!state.entered.has(state.nodeId)) {
    state.entered.add(state.nodeId);
    apply(node.enter);
    addPhone(node.phone);
  }

  els.scene.className = `scene-image scene-${node.scene || "campus"}`;
  const character = CHARACTERS[node.character];
  if (character) {
    els.characterBox.classList.remove("is-hidden");
    els.avatar.className = `avatar ${character[2]}`;
    els.speakerName.textContent = character[0];
    els.speakerRole.textContent = character[1];
  } else {
    els.characterBox.classList.add("is-hidden");
  }

  els.kicker.textContent = node.kicker || "";
  els.title.textContent = node.title;
  els.passwordBox.classList.toggle("is-hidden", !node.password);
  els.passwordHint.textContent = "";
  els.passwordInput.value = "";

  const lines = [];
  if (result.length) {
    lines.push({ type: "message", text: "刚才的选择：" }, ...result);
  }
  lines.push(...(node.lines || []));
  if (node.ending) lines.push({ type: "message", text: endingSummary() });

  els.story.innerHTML = lines.map(renderLine).join("");
  els.story.scrollTop = 0;
  renderChoices(node);
  renderSidebars();
}

function renderLine(line) {
  if (typeof line === "string") return `<p>${esc(line)}</p>`;
  return `<p class="${line.type || "message"}">${esc(line.text)}</p>`;
}

function renderChoices(node) {
  els.choices.innerHTML = "";
  for (const raw of node.choices || []) {
    const choice = normalizeChoice(raw);
    if (choice.once && state.flags.has(choice.once)) continue;
    if (choice.showIf && !choice.showIf(state)) continue;
    const button = document.createElement("button");
    button.type = "button";
    button.textContent = choice.text;
    button.addEventListener("click", () => choose(choice));
    els.choices.appendChild(button);
  }
}

function normalizeChoice(raw) {
  if (Array.isArray(raw)) {
    return {
      text: raw[0],
      next: raw[1],
      effects: raw[2],
      result: raw[3] || [],
      once: raw[4],
      showIf: raw[5]
    };
  }
  return raw;
}

function choose(choice) {
  if (choice.effects?.restart) return resetGame();
  apply(choice.effects);
  state.nodeId = choice.next;
  render(choice.result);
}

function apply(effects) {
  if (!effects) return;
  if (effects.clue) state.clues.add(effects.clue);
  if (effects.flag) state.flags.add(effects.flag);
  if (effects.phone) addPhone(effects.phone);
  if (effects.stat) {
    Object.entries(effects.stat).forEach(([key, delta]) => {
      state.stats[key] = (state.stats[key] || 0) + delta;
    });
  }
}

function addPhone(messages) {
  if (!messages) return;
  messages.forEach((message) => state.phone.push(message));
}

function renderSidebars() {
  els.phoneLog.innerHTML = state.phone.slice().reverse().map(([from, text]) => (
    `<div class="phone-msg"><strong>${esc(from)}</strong>${esc(text)}</div>`
  )).join("");

  els.clueCount.textContent = String(state.clues.size);
  els.clueList.innerHTML = Array.from(state.clues).map((clue) => `<li>${esc(clue)}</li>`).join("");

  els.statBars.innerHTML = STATS.map(([key, label]) => {
    const value = state.stats[key] || 0;
    const width = Math.max(0, Math.min(100, value * 18));
    return `<div class="stat-row"><div class="stat-label"><span>${esc(label)}</span><span>${value}</span></div><div class="bar"><span style="width:${width}%"></span></div></div>`;
  }).join("");
}

function submitPassword() {
  const value = els.passwordInput.value.trim();
  if (value === "23160427") {
    state.nodeId = "oldGroup";
    state.entered.delete("oldGroup");
    render(["备份文件已解锁。"]);
    return;
  }
  const hints = { "2316": "时间不够。", "0427": "日期不够。", "3160427": "房间记得你，但那晚不是从房间开始。" };
  els.passwordHint.textContent = hints[value] || "密码错误。提示在旧群消息和海报线索里。";
}

function endingSummary() {
  if (state.clues.size >= 8 && state.flags.has("acceptedResponsibility")) {
    return "结算：深线。你收集了足够多线索，并开始承认自己的旧事责任。第二章中，陈笑会主动留下更接近真相的证据。";
  }
  if (state.clues.size >= 4) {
    return "结算：标准线。第二章将开放匿名墙、快递柜和离校登记。";
  }
  return "结算：普通线。第二章仍可继续，但旧群备份会缺失部分内容。";
}

function esc(value) {
  return String(value)
    .replaceAll("&", "&amp;")
    .replaceAll("<", "&lt;")
    .replaceAll(">", "&gt;")
    .replaceAll('"', "&quot;")
    .replaceAll("'", "&#039;");
}

els.passwordButton.addEventListener("click", submitPassword);
els.passwordInput.addEventListener("keydown", (event) => {
  if (event.key === "Enter") submitPassword();
});
els.restart.addEventListener("click", resetGame);

render();
