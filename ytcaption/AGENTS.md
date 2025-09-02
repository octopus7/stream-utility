# Repository Guidelines

## Project Structure & Module Organization
- `YtCaption.Wpf/`: .NET 8 WPF app (single project).
  - `App.xaml`: application resources (e.g., `OverlayBackgroundColor`, brushes).
  - `MainWindow.xaml(.cs)`: overlay UI, left handle, resize/edit logic.
  - `SettingsWindow.xaml(.cs)`: color/opacity/font settings UI with preview.
  - `Models/AppSettings.cs`: persisted settings model.
  - `Services/SettingsStore.cs`: JSON load/save at `%AppData%/YtCaption.Wpf/settings.json`.
- Build outputs: `YtCaption.Wpf/bin/`, `YtCaption.Wpf/obj/` (git‑ignored).

## Build, Test, and Development Commands
- Build (Debug): `dotnet build YtCaption.Wpf/YtCaption.Wpf.csproj -c Debug`
- Build (Release): `dotnet build YtCaption.Wpf/YtCaption.Wpf.csproj -c Release`
- Run (Windows): `dotnet run --project YtCaption.Wpf`
- Output EXE: `YtCaption.Wpf/bin/Release/net8.0-windows/YtCaption.Wpf.exe`
- Optional format: `dotnet format` (if installed) to apply C# style fixes.

## Coding Style & Naming Conventions
- Language: C# 12, .NET 8, WPF/XAML; 4‑space indentation; file‑scoped namespaces.
- Naming: PascalCase for types/properties/methods and XAML names (e.g., `ToolbarPanel`, `ExitButton`).
- Private fields: `_camelCase` (e.g., `_isResizeMode`). Locals/parameters: `camelCase`.
- XAML resources: descriptive keys (e.g., `OverlayBackgroundBrush`). Keep UI logic in code‑behind minimal and focused.

## Testing Guidelines
- No test project yet. If adding tests, prefer `xUnit` or `MSTest` in `YtCaption.Wpf.Tests/`.
- Name tests `ClassNameTests`; method pattern `Method_Scenario_Expected`.
- Run tests with `dotnet test` from the repo root once a test project exists.

## Commit & Pull Request Guidelines
 - Commits: imperative mood with short scope tag; e.g., `feat(ui): add resize handles to overlay`; `fix(settings): persist window bounds on close`.
 - PRs: clear description, linked issues, rationale, and manual test steps; include screenshots/GIFs for UI changes; update `README.md` when behavior changes.

## Security & Configuration Tips
- Never hard‑code user paths; use `%AppData%` for persistence (already implemented).
- Do not commit files under `bin/`, `obj/`, or user‑specific IDE folders.
- Validate settings input (font size, color) and fail safely; keep defaults sane (`#1A000000`, `Segoe UI`, size 24).

## Documentation & Communication
- Response language: Write all answers in Korean.
- Instructions: Log new requirements in `Instructions.md` with Korean/English pairs, following the existing format.
- README updates: Summarize implemented changes and keep `README.md` continuously updated.
- File name: This guide is `AGENTS.md` (not `AGENT.md`) for consistency.
