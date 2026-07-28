# Adventure Game Editor — Architecture Notes

## Context

Companion WPF editor for an existing C# adventure game engine. The engine already
has a working JSON file format for game data (rooms, exits, objects, etc.). The
editor's first major feature is a visual room-map canvas — designers connect rooms
via a flowchart-style node graph instead of hand-editing JSON.

## Goals

- Share code between the engine and editor rather than duplicating data models.
- Visual node-graph canvas: rooms as draggable nodes, exits as connecting lines.
- Round-trip cleanly to/from the engine's existing JSON schema.

## Project structure

Split into two projects:

- **`TareEngine`** — the existing game engine. Holds the JSON-serializable data
  model (Room, Exit, GameObject, etc.), enums, and validation logic, alongside
  the engine itself.
- **`TareEditor`** — new WPF app, referencing `TareEngine` directly, MVVM-structured.

## MVVM shape

- `RoomViewModel` — wraps a `Room` model; exposes `X`, `Y`, `Name`, and an
  `ObservableCollection<ExitViewModel>`.
- `ExitViewModel` — references source/target `RoomViewModel`, plus direction
  (N/S/E/W or U/D for up and down); drives the connecting line between two nodes.
- `MapViewModel` — top-level `ObservableCollection<RoomViewModel>`; owns
  add/remove/connect logic and serializes back to the engine's JSON format via
  `TareEngine`. A separate format could be held for development purposes and then the data exported to the engine's JSON format.
  - Nice to have: export to an encrypted format

## Canvas / node-graph implementation

This is a standard node-based graph editor pattern in WPF:

- `ItemsControl` with `Canvas` as the `ItemsPanel`. Bind `Canvas.Left` / `Canvas.Top`
  to each `RoomViewModel`'s `X`/`Y` via attached-property bindings in the item
  container style. 
- Each room node is a `UserControl` (name, maybe a thumbnail/icon). The rectangle for the room should have nodes for the cardinal directions NORTH SOUTH EAST and WEST exits. In addition, it would be nice to have an UP and DOWN exit too. The user can connect nodes to each other by dragging and dropping from one exit in a room to another exit in another room. Rooms can connect to themselves to allow mazes to be created.
- Connections (exits) drawn on a separate `Canvas` layer (above or below the
  nodes), using `Path`/`Line` elements. Line endpoints need to recalculate when a
  node moves — typically via a `MultiBinding` or converter bound to both
  endpoints' positions.
- Dragging: handled manually via `MouseLeftButtonDown` / `MouseMove` /
  `MouseLeftButtonUp` on the node control, updating the bound `X`/`Y`. Canvas has
  no built-in drag support.

## Zoom and pan

Build this in from the start rather than retrofitting — wrap the node canvas in
a `ScrollViewer`, or apply `ScaleTransform` + `TranslateTransform` to the canvas,
driven by mouse wheel (zoom) and middle-click drag (pan). Adding zoom after nodes
are positioned in raw pixel coordinates means touching every existing coordinate
calculation.

## Library vs. hand-rolled

Given the scope (likely tens of rooms, not thousands) and the need for tight
control over the JSON round-trip, hand-rolling the drag/connect/zoom/pan layer is
reasonable and doable quickly — a full graph library (e.g. NodeNetwork-style
packages) is probably more than this needs.

## Open next steps

- [ ] Scaffold `TareEditor` WPF project, wire up reference to `TareEngine`
- [ ] Build `RoomViewModel` / `ExitViewModel` / `MapViewModel`
- [ ] Implement node dragging on the canvas
- [ ] Implement exit line rendering between connected nodes
- [ ] Add zoom/pan transform layer
- [ ] Wire up JSON load/save through `TareEngine`'s serialization