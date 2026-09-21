# 🧩 Maze Game — Luxor National University · Problem Solving Community

A Windows desktop app (C# / .NET 8 / WinForms) built for the **Problem Solving Community at
Luxor National University**. It teaches programming fundamentals (sequencing, loops,
conditionals) through the original **[Blockly Games — Maze](https://blockly.games/maze)**
puzzle game, wrapped in a branded, bilingual home page.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![WinForms](https://img.shields.io/badge/UI-Windows%20Forms-0078D4)
![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)

---

## ✨ Features

| Feature | Details |
|---|---|
| 🎮 Real game, not a clone | The Maze itself runs **inside a WebView2 browser control**, pointed straight at `blockly.games`, so gameplay is byte-for-byte identical to the original — this project never re-implements the puzzle logic. |
| 🏠 Clean home page | Small, e-commerce-style typography (inspired by large retail sites): slim navbar, short hero, bordered info cards, a 1–9 level grid — no oversized headings. |
| 🎓 University branding | Mentions **Luxor National University** and the **Problem Solving Community**, using the university's white & sky-blue color identity throughout. |
| 🌐 One-click translation | English is the default language everywhere; a single toggle button flips **every label in the app** to Arabic instantly, including mirroring the layout to right-to-left. |
| 🧭 9-level picker | Jump directly to any of blockly.games' first 9 Maze levels from the home page or from inside the game screen. |

---

## 🏗️ How it was built

This project went through a few deliberate design iterations — documented here in case you
want to understand *why* it's structured this way before extending it.

### 1. Two ways to give someone "the Maze game" in C#
There were two options for embedding Blockly Games Maze in a native C# app:

- **(a) Re-implement the puzzle logic in C#** — build a grid model, a block-based mini
  language (Move/Turn/Repeat/If blocks), an interpreter, and a canvas renderer, all from
  scratch in WinForms.
- **(b) Embed the real website** — host an actual Chromium browser inside the WinForms window
  (via [WebView2](https://learn.microsoft.com/microsoft-edge/webview2/)) and simply navigate it
  to `https://blockly.games/maze?lang=en&level={n}&skin=0`.

The project started with **(a)** (see the git history / earlier iterations), but was rebuilt to
use **(b)** so the gameplay is guaranteed pixel-identical to the real Blockly Games — the C# app
only supplies the surrounding chrome (navbar, level picker, language toggle), not the puzzle
engine itself.

### 2. Architecture

```
Program.cs                 → entry point, launches HomeForm
Theme/AppTheme.cs          → single source of truth for colors + fonts (small, e-commerce-style),
                              plus StylePrimary()/StyleSecondary()/StyleAsCard() helpers so every
                              button/card looks consistent without repeating style code.
Localization/
  Localizer.cs              → static Current language + a LanguageChanged event (simple pub/sub,
                               no external i18n library needed for just two languages).
  LocalizedControls.cs       → LocalizedLabel / LocalizedButton: WinForms controls that take an
                               (English, Arabic) pair in their constructor and re-render their own
                               .Text whenever Localizer.LanguageChanged fires. LanguageToggleButton
                               is the button that calls Localizer.Toggle() and flips the host
                               Form's RightToLeft/RightToLeftLayout for a full RTL mirror.
Forms/
  HomeForm.cs                → navbar + hero + 3 info cards (Problem Solving / Competitive
                               Programming / why a maze) + a 3×3 grid of level tiles.
  MazeForm.cs                → a WebView2 control docked to fill the window, a small top bar
                               (Back / level dropdown / language toggle), and a status strip.
                               NavigateToLevelAsync() calls EnsureCoreWebView2Async() once, then
                               just re-navigates the same WebView2 instance when the level changes.
```

**Why this split?** Each form only builds UI (`BuildUi()`); all visual decisions (colors, font
sizes, button chrome) live in `AppTheme`, and all translation logic lives in `Localization/` —
so neither form needs to know *how* styling or translation works, just that
`AppTheme.StylePrimary(button)` and `new LocalizedLabel(en, ar)` exist.

### 3. Key implementation details worth knowing
- **No drag-and-drop block engine anymore** — that responsibility now belongs entirely to the
  embedded blockly.games page, so the app has zero puzzle-logic code to maintain.
- **RTL is handled at the Form level only.** WinForms controls inherit `RightToLeft` from their
  parent by default, so toggling `Form.RightToLeft` / `RightToLeftLayout` once is enough to
  mirror the whole page — individual controls don't need to manage it themselves.
- **`ImplicitUsings` is disabled** and every file has explicit `using` statements. This was a
  deliberate fix after hitting SDK-dependent implicit-using inconsistencies that caused cascading
  `CS0246` errors on some machines — see the Troubleshooting section below.
- **WebView2's `NavigationCompleted` handler is subscribed exactly once** (guarded by a
  `_webViewReady` flag) rather than on every level switch, to avoid stacking duplicate event
  handlers over a session.

### 4. Tech stack
- **.NET 8**, Windows Forms (`net8.0-windows`)
- **Microsoft.Web.WebView2** (NuGet) — Chromium-based embedded browser control
- No external UI/theme libraries — all styling is plain GDI+/WinForms (`Panel.Paint` for card
  borders, `FlatStyle.Flat` buttons, etc.)

---

## 🚀 Getting started

### Requirements
- Windows 10/11
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- **WebView2 Runtime** (pre-installed on most Windows 10/11 machines with Edge; the app will
  prompt with a download link if it's missing)
- Internet connection (to restore the WebView2 NuGet package on first build, and at runtime to
  load blockly.games)


## 📁 Project structure
```
.
├── LICENSE
├── README.md
└── MazeGame/
    ├── MazeGame.csproj
    ├── Program.cs
    ├── Theme/
    │   └── AppTheme.cs
    ├── Localization/
    │   ├── Localizer.cs
    │   └── LocalizedControls.cs
    └── Forms/
        ├── HomeForm.cs
        └── MazeForm.cs
```

## 🎨 Customizing
- **Colors** — `Theme/AppTheme.cs`: adjust `AccentBlue`, `PrimaryBlue`, `DeepBlue`, `MilkyBlue`
  to match the university's exact official hex codes.
- **Text / translations** — every `new LocalizedLabel("English text", "النص العربي")` /
  `new LocalizedButton(...)` call in `HomeForm.cs` / `MazeForm.cs` takes English first, Arabic
  second — edit either side directly, no resource files to touch.
- **Levels** — the picker and home tiles are numbered 1–9 to match blockly.games' own level
  range; the URL pattern is `https://blockly.games/maze?lang=en&level={n}&skin=0`.

## 🛠️ Troubleshooting
- **`CS0246` errors everywhere on a fresh clone** → run `dotnet restore` first (or let Visual
  Studio restore automatically on open) — the project needs the WebView2 NuGet package before it
  will compile.
- **`NU1101`/`NU1102` restoring `Microsoft.Web.WebView2`** → right-click the project → **Manage
  NuGet Packages** → search `Microsoft.Web.WebView2` → install the latest version (the exact
  patch pinned in `.csproj` isn't important).
- **"An object reference is required for..." on a method used inside a static helper** → make
  sure any private helper method that calls instance methods (like `Hide()`/`Close()`) isn't
  marked `static`.
- **Blank/blocked game screen** → check your internet connection and that the WebView2 Runtime
  is installed (Settings → Apps → search "WebView2").

## 📄 License
MIT — see [LICENSE](LICENSE). Feel free to fork and adapt for other communities/universities.

---
*Built for the Problem Solving Community — Luxor National University.*
