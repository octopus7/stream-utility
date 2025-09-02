# 지시사항 / Instructions

## 개요 / Overview
- 이 문서는 현재까지 사용자가 지시한 요구사항을 정리한 문서입니다. 모든 항목은 한국어와 영어로 병기됩니다. 향후 지시사항도 이 파일에 동일한 형식으로 갱신합니다.
- This document summarizes all requirements the user has given so far. Each item is provided in both Korean and English. Future instructions will be appended here in the same format.

## 일반 / General
- 빌드는 하지 않습니다. 사용자가 Windows에서 직접 실행합니다. 대답은 한국어로 제공합니다.
- Do not build locally. The user will run it on Windows. All responses are in Korean.

## 창 속성 / Window Properties
- 창은 항상 최상위(Topmost)로 표시되고, 테두리 없음(WindowStyle=None), 투명 허용(AllowsTransparency=True)입니다.
- The window stays always on top (Topmost), is borderless (WindowStyle=None), and allows transparency (AllowsTransparency=True).

## 기본 상태(평상시) / Normal State
- 평상시에는 오버레이 텍스트 영역만 보입니다. 오버레이 영역 배경은 약 10% 불투명도의 색상으로 희미하게 칠해지며, ARGB 리소스로 나중에 변경 가능해야 합니다.
- In normal state, only the overlay text area is visible. The overlay area has a faint background at about 10% opacity, and the color must be changeable later via an ARGB resource.

- 오버레이 왼쪽에는 한 글자 정도 폭의 ‘빈영역’을 항상 표시합니다.
- A narrow “blank area” about one character wide is always visible on the left side of the overlay.

## 조정모드 / Adjust Mode
- 좌측 빈영역을 마우스 버튼 업 시점에 클릭으로 인식하여 ‘조정모드’를 전환/해제합니다.
- Adjust Mode toggles on mouse button up when clicking the left blank area.

- 초기(조정모드 아님)에서 좌측 빈영역을 드래그하면 창 위치 이동이 됩니다(클릭이 아닌 드래그 시 토글되지 않음).
- In normal (non-adjust) state, dragging the left blank area moves the window (dragging does not toggle the mode).

- 조정모드 버튼(세로 툴바)은 좌측에 고정 폭(예: 56px)을 항상 예약하여 레이아웃이 밀리지 않도록 합니다. 조정모드가 아닐 때도 공간이 확보되어 오버레이가 흔들리지 않습니다.
- The vertical toolbar area on the left uses a fixed width (e.g., 56px) reserved at all times to prevent layout shift; the overlay does not move when entering/exiting Adjust Mode.

- 조정모드에서는 다음이 표시됩니다: (1) 텍스트 편집 가능 상태(오버레이 위치에서 바로 편집), (2) 창 리사이즈 보더/핸들(상/하/좌/우 드래그로 크기 조정), (3) 좌측 빈영역의 왼쪽에 세로 툴바.
- In Adjust Mode, the following appear: (1) inline text editing at the overlay location, (2) resize border/handles to drag on all four edges, and (3) a vertical toolbar to the left of the blank area.

- 세로 툴바 버튼 배치는 위에서 아래로 ‘닫기(X) 버튼’, ‘설정 버튼’ 순서입니다.
- The vertical toolbar buttons are ordered from top to bottom: Close (X) button, then Settings button.

- 닫기(X) 버튼을 누르면 앱이 종료됩니다.
- Clicking the Close (X) button exits the app.

## 설정 창 / Settings Window
- 설정 버튼을 누르면 별도의 설정 창이 열립니다.
- Pressing the Settings button opens a separate Settings window.

- 설정 창에서는 컬러 선택 UI로 색상을 고르고, 투명도는 슬라이더(0–100%)로 조정합니다. 또한 폰트 패밀리와 폰트 크기를 설정할 수 있습니다.
- In the Settings window, choose color via a color picker UI and adjust opacity with a slider (0–100%). You can also set font family and font size.

## 지속성 / Persistence
- 변경한 설정(오버레이 배경색/투명도, 폰트 설정)은 앱을 종료해도 유지되어야 합니다. 오버레이에 입력된 텍스트도 재실행 시 복원되어야 합니다.
- Updated settings (overlay background color/opacity, font settings) must persist across app restarts. The overlay text must also be restored on relaunch.

- 창 위치(Left/Top)도 저장하여 다음 실행 시 복원합니다. 화면 구성이 바뀐 경우에는 가시 영역 안으로 자동 보정합니다.
- Window position (Left/Top) is persisted and restored on next launch. If display configuration changes, the position is clamped inside the visible area.

- 창 크기(Width/Height)도 저장·복원하며, 화면 크기를 넘지 않도록 자동으로 클램프됩니다.
- Window size (Width/Height) is also persisted and restored, and is clamped to fit within the screen.

## 변경 이력(대체된 지시) / Change History (Superseded)
- 초기 지시는 하단에 9pt, 2줄 입력창과 상단 투명 오버레이 표시, 입력창 좌측 드래그 핸들, 창 Topmost 등이었으나 이후 요구사항 변경으로 대체되었습니다.
- The initial request included a 9pt, two-line input box at the bottom, a transparent overlay above, a drag handle left of the input, and Topmost; these were later replaced by updated requirements.

- 상단 오버레이 더블클릭으로 리사이즈 모드 토글 요구도 이후 ‘좌측 빈영역 클릭’으로 통합되어 대체되었습니다.
- The instruction to toggle resize mode by double-clicking the overlay was later superseded by using “left blank area click” instead.

## 문서 정책 / Documentation Policy
- 모든 응답은 한국어로 제공합니다.
- All responses are in Korean.

- 새로운 지시는 이 파일에 한국어/영어 병기로 추가하고, 기존 형식을 따릅니다.
- New instructions are added to this file in both Korean and English, following the existing format.

- 구현 내용과 변경 사항은 `README.md`에 요약하여 지속적으로 기록합니다.
- Implemented features and changes are continuously summarized in `README.md`.
