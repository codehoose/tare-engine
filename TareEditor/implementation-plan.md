# TareEditor — Implementation Plan

Derived from `architecture.md`. Grounded in the current state of the repo:
`TareEditor` is a bare WPF scaffold (default `App.xaml`/`MainWindow.xaml`, no
project reference to `TareEngine`, no MVVM toolkit). `TareEngine` already has
a working runtime model (`Room`, `RoomExit`) and a separate serialization DTO
layer (`SerializedRoom`, `SerializedRoomCollection`, `SerializedGameData`)
that Newtonsoft.Json reads/writes directly.

## 0. Decisions this plan makes (flag any disagreement before starting)

1. **The editor binds to the serialization DTOs, not the runtime model.**
   `Room`/`RoomExit` require a live `ParserDictionary` (`RoomExit.Exit` is a
   `Word` instance) to construct — that's a runtime/engine-side concern, not
   an editor one. `SerializedRoom` (slug, short, description, graphic[],
   graphicFlag, `Dictionary<string,string> exits`, `Dictionary<string,string>
   blockers`) is already a plain JSON-shaped POCO and is the natural editor
   round-trip type. The editor's `RoomViewModel` wraps `SerializedRoom`
   directly (or a thin editor-side clone of it), not `Room`.
2. **Direction keys are full lowercase words** (`"north"`, `"south"`,
   `"east"`, `"west"`, `"up"`, `"down"`), matching
   `ParserDictionaryFactory` (`DirectionWord("North","n")`, etc.) and the
   existing `thedata.json`. The 6 connector nodes on a room map to exactly
   these 6 dictionary keys.
3. **Room position (X/Y) has no home in the engine's JSON schema.**
   `SerializedRoom` has no coordinate fields, and adding them would leak
   editor-only concerns into the engine format. Per `architecture.md`'s note
   about "a separate format... held for development purposes," the plan is:
   an **editor-native project file** (e.g. `*.tare-project.json`) is the
   thing actually opened/saved/autosaved while working. It contains the full
   room list (as `SerializedRoom[]` plus `startRoom`) **and** a per-room
   `X`/`Y`. "Export to engine JSON" is a one-way projection that drops X/Y
   and writes exactly the `SerializedRoomCollection` shape the engine already
   consumes (e.g. into `thedata.json`'s `rooms` node). "Import" reads engine
   JSON and lays out rooms needing positions (grid fallback, see Phase 2).
4. **Blockers are in scope for round-trip fidelity.** `thedata.json` already
   uses `blockers` (e.g. `"blockers": { "north": "tardis-key-held" }`) on
   real content. If the editor doesn't preserve/edit them it will silently
   corrupt existing rooms on save. Minimal UI: a per-exit optional "blocked
   by flag" text field; full flag-picker UI is a later nicety.
5. **Self-connections (mazes) are real exits**, not a special case in the
   data model — a `RoomViewModel` can be its own `ExitViewModel.Target`. Only
   the *rendering* of that connection is special (Phase 5).

---

## Phase 1 — Solution & project wiring

- [ ] Add `<ProjectReference Include="..\TareEngine\TareEngine.csproj" />`
  to `TareEditor.csproj`.
- [ ] Add `CommunityToolkit.Mvvm` package reference to `TareEditor.csproj`
  (source generators for `ObservableObject`/`[ObservableProperty]`/
  `RelayCommand` — avoids hand-rolling `INotifyPropertyChanged` boilerplate
  across every ViewModel). Confirm the user is fine pulling this dependency
  in rather than hand-rolling; it's the standard low-friction choice for a
  small WPF MVVM app.
- [ ] Folder skeleton under `TareEditor/`:
  ```
  Models/            (editor-only models, e.g. EditorProject, layout DTOs)
  ViewModels/         RoomViewModel, ExitViewModel, MapViewModel, MainViewModel
  Views/              MainWindow, MapCanvasView, RoomNodeControl
  Converters/         multi-binding converters for line geometry
  Services/           ProjectIo (load/save/export), DialogService
  ```
- [ ] Confirm `net10.0-windows` + WPF still builds after adding the
  reference (`TareEngine` targets plain `net10.0`, no conflict expected).

## Phase 2 — Domain/data layer

- [ ] `EditorRoomLayout` — tiny POCO: `Slug`, `X`, `Y`. This is the only new
  persisted concept beyond what `TareEngine.Serialization` already defines.
- [ ] `EditorProject` — the file actually opened/saved while working:
  ```csharp
  class EditorProject {
      string StartRoom;
      List<SerializedRoom> Rooms;
      List<EditorRoomLayout> Layout; // keyed by slug
  }
  ```
  Saved/loaded as its own JSON via Newtonsoft (reuse the `SerializedRoom`
  type directly — no need to invent a parallel shape).
- [ ] `ProjectIoService`:
  - `LoadProject(path) -> EditorProject` / `SaveProject(EditorProject, path)`.
  - `ImportEngineJson(path) -> EditorProject` — reads a
    `SerializedRoomCollection` (or full `SerializedGameData`, keep whichever
    is closer to the real file being targeted — check whether rooms live
    standalone or always inside the full game-data file before committing)
    and synthesizes `EditorRoomLayout` entries for any room lacking one
    (simple grid: `x = (index % cols) * spacing`, `y = (index / cols) *
    spacing`).
  - `ExportEngineJson(EditorProject, path)` — projects to
    `SerializedRoomCollection { startRoom, rooms }` and writes it, dropping
    layout. This is what round-trips into `TARE/Content/thedata.json`.
  - `ExportEncrypted(...)` — stretch, see Phase 9.
- [ ] Validation helpers (used by Phase 8 UI, written now since they're pure
  functions over `List<SerializedRoom>`):
  - duplicate slug detection,
  - exit target slug doesn't exist,
  - (warning, not error) exit without a reciprocal reverse exit — common
    adventure-game authoring mistake, worth flagging not blocking.

## Phase 3 — MVVM layer

- [ ] `RoomViewModel : ObservableObject` — wraps one `SerializedRoom` +
  its `EditorRoomLayout`. Exposes `Slug`, `Short`, `Description`, `Graphic`
  (array editing can stay a simple text list for now), `GraphicFlag`, `X`,
  `Y` (settable, drives node dragging), and
  `ObservableCollection<ExitViewModel> Exits`.
- [ ] `ExitViewModel : ObservableObject` — `Direction` (enum `N/S/E/W/U/D`
  mapped to the six lowercase words from decision #2), `SourceRoom`
  (`RoomViewModel`), `TargetRoom` (`RoomViewModel`, nullable while
  dragging/unconnected), `BlockedByFlag` (nullable string, decision #4).
- [ ] `MapViewModel : ObservableObject` — `ObservableCollection<RoomViewModel>
  Rooms`, `StartRoom` (selected `RoomViewModel`), `PanX/PanY/Zoom`
  (Phase 6). Commands: `AddRoomCommand`, `RemoveRoomCommand`,
  `ConnectExitCommand(source, direction, target)`,
  `DisconnectExitCommand(exit)`. Owns translation to/from `EditorProject`
  (`ToProject()` / `MapViewModel.FromProject(project)`).
- [ ] `MainViewModel` — owns the current file path, dirty flag, and wraps
  `ProjectIoService` calls behind `NewCommand`/`OpenCommand`/`SaveCommand`/
  `SaveAsCommand`/`ImportEngineJsonCommand`/`ExportEngineJsonCommand`.

## Phase 4 — Node canvas & dragging

- [ ] `RoomNodeControl` (UserControl): room name/slug header, six small
  connector "ports" positioned on the edges of the rectangle — N/S on
  top/bottom-center, E/W on left/right-middle, U/D as a smaller pair (e.g.
  stacked in a corner, since they don't map to a 2D compass position).
  Each port is a small `Ellipse`/`Thumb`-like element with `Tag` = direction,
  used as both the drag-source for creating a connection and the drop-target
  for completing one.
- [ ] `MapCanvasView`: `ItemsControl` bound to `MapViewModel.Rooms`,
  `ItemsPanelTemplate` = `Canvas`. `ItemContainerStyle` binds
  `Canvas.Left -> X`, `Canvas.Top -> Y` via attached-property setters (plain
  `Setter`+`Binding`, no converter needed since X/Y are already canvas
  units).
- [ ] Node dragging: handle `PreviewMouseLeftButtonDown` /
  `PreviewMouseMove` / `PreviewMouseLeftButtonUp` on `RoomNodeControl`'s
  header/body (not on the ports — those are reserved for connection
  drag-drop, see Phase 5). Track drag-start mouse position and starting
  `X`/`Y`, update the bound VM properties on move, release capture on
  mouse-up. Must account for the canvas `ScaleTransform` (Phase 6) when
  converting mouse-delta to model-space delta — divide screen delta by
  current zoom factor.

## Phase 5 — Connection (exit) rendering & authoring

- [ ] Second `Canvas` layer in `MapCanvasView`, same coordinate space as the
  node canvas (either z-ordered above/below, or overlaid via a `Grid` with
  both canvases in the same cell), bound to a flattened list of all
  `ExitViewModel`s across all rooms.
- [ ] Each connection renders as a `Path` (or `Line` for the simple case).
  Endpoint depends on both `SourceRoom` position + port offset and
  `TargetRoom` position + port offset — use an `IMultiValueConverter` bound
  to `SourceRoom.X`, `SourceRoom.Y`, `TargetRoom.X`, `TargetRoom.Y` (and the
  direction, to pick which edge/port offset to use) that outputs a
  `PathGeometry`. This recalculates automatically whenever either endpoint's
  `X`/`Y` changes, since `MultiBinding` re-evaluates on any source change —
  no manual "on move, update all connected lines" bookkeeping needed.
- [ ] Self-connections (maze rooms linking to themselves): same converter,
  special-cased when `Source == Target` — route the path out from one port
  and back into another port on the same node (a small loop, e.g. a
  quarter-circle bulge) rather than degenerating to a zero-length line.
- [ ] Arrowhead or direction indicator at the target end so exit direction
  is visually legible (a room can have a one-way exit — nothing in the
  schema requires symmetry).
- [ ] Authoring a connection: drag from a source port; while dragging, render
  a temporary rubber-band line following the mouse (bound to a
  `MapViewModel.PendingConnectionEnd` point updated on `MouseMove`); on
  drop over a valid target port, call `ConnectExitCommand`; dropping
  elsewhere cancels. Reuse the same port hit-testing for both ends.
- [ ] Deleting a connection: click-select a `Path` -> highlight -> `Delete`
  key or context-menu "Remove exit" -> `DisconnectExitCommand`.

## Phase 6 — Zoom & pan

- [ ] Wrap both canvas layers in a single `Grid` (so node and connection
  layers stay perfectly aligned) and apply one shared
  `TransformGroup { ScaleTransform, TranslateTransform }` to that `Grid`,
  bound to `MapViewModel.Zoom`/`PanX`/`PanY`.
- [ ] Mouse wheel over the canvas adjusts `Zoom` (clamp e.g. 0.25–3.0),
  zooming around the cursor position (compute the point-under-cursor in
  canvas space before/after the scale change and adjust pan to compensate,
  so zoom doesn't feel like it's "sliding" the view).
- [ ] Middle-click (or space+drag) pan adjusts `PanX`/`PanY` directly from
  mouse delta — no zoom-factor division needed here since pan is already in
  screen space.
- [ ] Since this is built now (per architecture.md's explicit call-out),
  Phase 4's drag-delta-to-model-space conversion already divides by `Zoom`
  from day one — nothing to retrofit later.

## Phase 7 — Application shell & file I/O

- [ ] `MainWindow.xaml`: menu bar (`File > New/Open/Save/Save As/Import
  Engine JSON/Export Engine JSON/Exit`), toolbar (zoom reset, add room),
  main content = `MapCanvasView`, a side panel bound to the selected
  `RoomViewModel` for editing `Short`/`Description`/`Graphic`/`GraphicFlag`
  and per-exit `BlockedByFlag`.
- [ ] Standard `Microsoft.Win32.OpenFileDialog`/`SaveFileDialog` behind a
  small `IDialogService` (keeps `MainViewModel` testable/UI-free).
- [ ] Dirty-tracking: any mutation through `MapViewModel` commands or
  property changes sets a dirty flag; window title shows `*`; closing with
  unsaved changes prompts save/discard/cancel.
- [ ] Decide the on-disk default: does "Save" write the editor-native
  project file (`*.tare-project.json`, includes layout) and "Export" is a
  separate explicit action to push to the engine's `thedata.json`? (Yes —
  matches decision #3. Surface this explicitly to the user so they don't
  expect `Save` to touch `thedata.json` directly.)

## Phase 8 — Validation & polish

- [ ] Wire Phase 2's validation helpers into the UI: a "Problems" panel or
  status-bar count, listing duplicate slugs / dangling exits / one-way exits,
  clickable to select the offending room.
- [ ] Prevent duplicate slugs at the "add room" / rename point rather than
  only flagging after the fact.
- [ ] Keyboard affordances: `Delete` removes selected room (with confirm,
  since it also severs all exits pointing at it — decide whether those
  become dangling-with-warning or are auto-removed), arrow keys nudge
  selected node.

## Phase 9 — Stretch: encrypted export

- Only after Phases 1–8 are solid. Simple approach: AES-encrypt the
  exported engine JSON bytes with a passphrase-derived key (e.g.
  `Rfc2898DeriveBytes` + `Aes`), write `.json.enc`. Needs a matching loader
  on the `TareEngine` side to actually be useful at runtime — confirm
  whether that engine-side counterpart is wanted before investing here, since
  it's orthogonal to the editor itself.

---

## Suggested build order (maps to architecture.md's checklist, expanded)

1. Phase 1 (wiring) + Phase 2 (data layer, no UI yet — verify with a quick
   console/unit-test round-trip: load `thedata.json` -> `EditorProject` ->
   export -> byte-for-byte-equivalent JSON modulo formatting).
2. Phase 3 (ViewModels) with no visuals — sanity-check via a temporary
   `ListBox` of room names before building the canvas.
3. Phase 4 (nodes + dragging) on a fixed zoom/pan (identity transform) first.
4. Phase 5 (connections) — this is the trickiest binding logic; get plain
   N/S/E/W straight lines working before tackling self-loops and the
   drag-to-connect interaction.
5. Phase 6 (zoom/pan) — retrofit onto the now-working canvas is *possible*
   here since Phase 4/5 already divide by `Zoom` per architecture.md's
   guidance, but building it before Phase 7 avoids doing so twice.
6. Phase 7 (shell/file I/O) ties it into a usable app.
7. Phase 8 (validation) hardens it against bad data.
8. Phase 9 (encryption) only if still wanted.

## Open questions to confirm before/while building

- Does `SerializedRoomCollection` ever live outside a full
  `SerializedGameData` file in practice, or is `thedata.json`'s
  top-level `rooms` node the only real target? Determines whether
  Import/Export should round-trip the *whole* game-data file (preserving
  `flags`/`items`/`actions` untouched) or just the `rooms` sub-tree in
  isolation. Given `thedata.json` is one combined file, Import/Export should
  almost certainly load/preserve the full `SerializedGameData` and only
  touch the `rooms` portion — otherwise exporting will clobber flags/items/
  actions on save.
- Multiple graphics per room (`Room.GetGraphic(index)` implies rotation/
  variants) — how much editing UI does this need beyond a simple string
  list?
- Should removing a room auto-delete exits that target it, or leave them
  dangling with a validation warning (Phase 8)? Affects whether "delete
  room" needs a confirmation dialog listing affected exits.
