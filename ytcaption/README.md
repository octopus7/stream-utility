# YT Caption Utility (WPF)

한 줄로 요약: 항상 위에 떠 있는 투명 오버레이에 텍스트를 표시하고, 좌측 빈영역 클릭으로 조정/편집/리사이즈를 제어하는 WPF 유틸리티.

One‑liner: A WPF utility that shows text on a topmost transparent overlay, controlled via a left blank area to toggle Adjust Mode/editing/resizing.

## 개요 / Overview
- 오버레이 텍스트를 표시하는 경량 WPF 앱입니다. 평상시에는 텍스트만 보이고, 좌측의 얇은 빈영역을 클릭하면 조정모드로 전환되어 편집/리사이즈/설정 버튼을 사용할 수 있습니다.
- A lightweight WPF app for overlaying text. In normal mode only the text shows; clicking the thin left blank area toggles Adjust Mode for editing/resizing/opening settings.

## 특징 / Features
- 항상 위(Topmost), 테두리 없음(WindowStyle=None), 투명 허용(AllowsTransparency=True)
- 좌측 빈영역(약 한 글자 폭) 항상 표시, 클릭으로 조정모드 토글
- 조정모드에서:
  - 오버레이 위치에서 바로 텍스트 편집
  - 창 테두리(상/하/좌/우) 드래그로 리사이즈, 보더/핸들 표시
  - 빈영역 왼쪽의 세로 툴바에 버튼 배치: 닫기(X), 설정
- 툴바 영역은 고정 폭(예: 56px)으로 항상 예약되어 조정모드 전환 시 레이아웃이 밀리지 않음
- 오버레이 배경은 약 10% 투명(기본 #1A000000), ARGB로 변경 가능
- 좌측 빈영역 배경 색상/투명도도 설정 가능(기본 #22FFFFFF)
- 설정(배경색/투명도, 폰트)과 오버레이 텍스트는 앱 재실행 시에도 유지
- 창 위치(Left/Top)와 창 크기(Width/Height)도 다음 실행 시 복원하며, 디스플레이 변경 시 화면 안쪽으로 자동 보정/클램프

In English:
- Topmost, borderless, with transparency enabled
- A thin blank area on the left (about one character wide), click to toggle Adjust Mode
- In Adjust Mode:
  - Inline text editing directly on the overlay
  - Resize by dragging window edges with visible border/handles
  - Vertical toolbar to the left of the blank area: Close (X), Settings
- The toolbar area uses a fixed reserved width (e.g., 56px) so the layout does not shift when toggling Adjust Mode
- Overlay background ~10% opacity (default #1A000000), configurable via ARGB
- Left blank area background color/opacity is configurable as well (default #22FFFFFF)
- Settings (background/opacity, font) and overlay text persist across restarts
- Window position (Left/Top) and size (Width/Height) also persist; clamped to visible area if display configuration changes

## 조작 / Controls
- 좌측 빈영역 클릭(마우스 버튼 업 시점): 조정모드 토글 (편집/툴바/리사이즈 보더 표시/숨김)
- 초기 상태에서 좌측 빈영역 드래그: 창 위치 이동
- 설정 버튼: 설정 창 열기 (색상 선택 UI, 투명도 슬라이더, 글꼴/크기)
- 닫기(X) 버튼: 앱 종료
- 조정모드에서 테두리 드래그: 창 크기 조절

Controls in English:
- Click left blank area (on mouse button up): toggle Adjust Mode (edit/toolbar/resize borders)
- Drag left blank area in normal state: move the window
- Settings button: open Settings window (color picker, opacity slider, font family/size)
  - 표시: 텍스트 대신 톱니바퀴(⚙) 아이콘
  - Display: gear (⚙) icon instead of text
- Close (X) button: quit the app
- Drag window edges in Adjust Mode: resize the window

## 설정 / Settings
- 설정 창에서 ‘색상 선택’ UI로 오버레이/좌측 빈영역 배경 색상을 고르고, 투명도는 슬라이더(0–100%)로 각각 조정합니다. 시스템 글꼴과 폰트 크기 선택 가능, 미리보기 제공
- 설정 및 텍스트 저장 위치: `%AppData%/YtCaption.Wpf/settings.json`

In English:
- Use a color picker UI to configure overlay/left-blank-area background colors and per-area opacity (0–100%); choose system font and size with live preview
- Settings and text are stored at `%AppData%/YtCaption.Wpf/settings.json`

## 빌드 및 실행 / Build & Run
- 필요: .NET 8 SDK (실행만 할 경우 .NET 8 Windows Desktop Runtime)
- CLI:
  - `dotnet build YtCaption.Wpf/YtCaption.Wpf.csproj -c Release`
  - 실행 파일: `YtCaption.Wpf/bin/Release/net8.0-windows/YtCaption.Wpf.exe`
- Visual Studio:
  - `YtCaption.Wpf/YtCaption.Wpf.csproj` 열기 → 시작 프로젝트로 설정 → 실행

Requirements and commands in English:
- Requires .NET 8 SDK (Desktop Runtime for running only)
- CLI: `dotnet build YtCaption.Wpf/YtCaption.Wpf.csproj -c Release`, run the generated EXE above
- Visual Studio: open the `.csproj`, set as startup project, run

## 배포 방법 / Distribution
- 선택: Self-contained 단일 파일(SCD Single-File) ZIP 배포.
- 퍼블리시(Windows x64):
  - `dotnet publish YtCaption.Wpf -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:PublishTrimmed=false -p:DebugType=None -p:DebugSymbols=false`
- 산출물: `YtCaption.Wpf/bin/Release/net8.0-windows/win-x64/publish/YtCaption.Wpf.exe`
- ZIP 패키징: 실행 파일과 `dist/README.txt`를 묶어 `ytcaption-v<semver>-win-x64-scd-single.zip` 생성.
- 자동화: `scripts/release.ps1 -Version 0.1.0` 실행 시 퍼블리시+ZIP 생성.

In English:
- Choice: Self-contained single-file (SCD) ZIP.
- Publish (Windows x64): `dotnet publish ... --self-contained true -p:PublishSingleFile=true`
- Output: single EXE under `.../publish/`.
- Package: zip the EXE with `dist/README.txt` as `ytcaption-v<semver>-win-x64-scd-single.zip`.
- Automation: run `scripts/release.ps1 -Version 0.1.0`.

## 주요 파일 / Key Files
- `YtCaption.Wpf/App.xaml` · 리소스(`OverlayBackgroundColor`, `OverlayBackgroundBrush`)
- `YtCaption.Wpf/MainWindow.xaml(.cs)` · 오버레이/빈영역/툴바/편집/리사이즈 로직
- `YtCaption.Wpf/SettingsWindow.xaml(.cs)` · ARGB·폰트 설정 및 미리보기
- `YtCaption.Wpf/Models/AppSettings.cs` · 설정 모델
- `YtCaption.Wpf/Services/SettingsStore.cs` · 설정 로드/저장(JSON, `%AppData%`)
- `Instructions.md` · 요구사항(한국어/영어 병기)

## 참고 / Notes
- 오버레이 배경색은 설정 창에서 변경하거나 `App.xaml` 리소스(ARGB)로 초기값을 조정할 수 있습니다.
- Overlay background can be changed in the Settings window or by editing the ARGB resource in `App.xaml`.

## 변경 기록 / Change Log
- 2025-09-02
  - 초기 공개: 오버레이 텍스트, 좌측 빈영역 기반 조정모드 토글, 인라인 편집, 수동 리사이즈 핸들, 설정 창(색상/투명도/폰트), 설정·텍스트·창 위치/크기 영속화(`%AppData%/YtCaption.Wpf/settings.json`), DPI 안정화된 리사이즈 반영.
  - 문서: `AGENTS.md` 추가(기여 가이드·문서/응답 정책), `Instructions.md`에 문서 정책(한국어 응답, 한/영 병기) 병기, 본 `README.md`에 변경 기록 섹션 생성.
  - 기능: 좌측 빈영역 배경 색상/투명도 설정 추가 및 영속화. 리소스/바인딩으로 오버레이와 동일 방식 적용.

In English:
- 2025-09-02
  - Initial release: overlay text, left blank-area Adjust Mode toggle, inline editing, manual resize handles, Settings window (color/opacity/font), persistence of settings/text/window bounds (`%AppData%/YtCaption.Wpf/settings.json`), DPI-stable resize.
  - Docs: added `AGENTS.md` (contributor guide + doc/response policy), appended documentation policy to `Instructions.md`, created this Change Log section in `README.md`.
  - Feature: added left blank-area background color/opacity settings with persistence, applied via resource/binding same as overlay.
