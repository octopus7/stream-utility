using System;
using System.Windows;
using System.Windows.Input;
 

namespace YtCaption.Wpf;

public partial class MainWindow : Window
{
    private bool _isResizeMode;
    private bool _isEditMode;

    private enum ResizeEdge { None, Left, Right, Top, Bottom }
    private bool _isResizing;
    private ResizeEdge _currentEdge = ResizeEdge.None;
    private System.Windows.Point _startMouse;
    private System.Windows.Point _startMouseScreen;
    private double _startLeft, _startTop, _startWidth, _startHeight;
    // Left handle interaction state
    private bool _leftHandlePressed;
    private bool _leftHandleDidDrag;
    private System.Windows.Point _leftHandleDownPos;

    private Models.AppSettings _settings = new();

    public MainWindow()
    {
        InitializeComponent();
        this.MouseMove += Window_OnMouseMove;
        this.MouseLeftButtonUp += Window_OnMouseLeftButtonUp;
        this.Loaded += MainWindow_Loaded;
        this.Closing += MainWindow_Closing;
        
    }
    

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        _settings = Services.SettingsStore.Load() ?? new Models.AppSettings();
        ApplySettingsToUi(_settings);
        // 텍스트 복원
        EditBox.Text = _settings.OverlayText ?? string.Empty;
        // 창 위치/크기 복원
        ApplyWindowPosition(_settings);
    }

    private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        PersistTextIfNeeded();
    }

    private void ApplySettingsToUi(Models.AppSettings s)
    {
        // 오버레이 배경 업데이트 (리소스 컬러 갱신 + 브러시 인스턴스/즉시 반영)
        try
        {
            var c = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(
                string.IsNullOrWhiteSpace(s.OverlayColor) ? "#1A000000" : s.OverlayColor);
            System.Windows.Application.Current.Resources["OverlayBackgroundColor"] = c;
            if (TryFindResource("OverlayBackgroundBrush") is System.Windows.Media.SolidColorBrush ob)
            {
                ob.Color = c;
                OverlayArea.Background = ob; // 즉시 반영
            }
            else
            {
                var newBrush = new System.Windows.Media.SolidColorBrush(c);
                System.Windows.Application.Current.Resources["OverlayBackgroundBrush"] = newBrush;
                OverlayArea.Background = newBrush;
            }
        }
        catch { }

        // 좌측 빈영역(LeftHandle) 배경 업데이트 (리소스 컬러 갱신 + 브러시 인스턴스/즉시 반영)
        try
        {
            var lc = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(
                string.IsNullOrWhiteSpace(s.LeftHandleColor) ? "#22FFFFFF" : s.LeftHandleColor);
            System.Windows.Application.Current.Resources["LeftHandleBackgroundColor"] = lc;
            if (TryFindResource("LeftHandleBackgroundBrush") is System.Windows.Media.SolidColorBrush lb)
            {
                lb.Color = lc;
                LeftHandle.Background = lb; // 즉시 반영
            }
            else
            {
                var newLeftBrush = new System.Windows.Media.SolidColorBrush(lc);
                System.Windows.Application.Current.Resources["LeftHandleBackgroundBrush"] = newLeftBrush;
                LeftHandle.Background = newLeftBrush;
            }
        }
        catch { }

        // 폰트 적용
        try
        {
            var ff = new System.Windows.Media.FontFamily(string.IsNullOrWhiteSpace(s.FontFamily) ? "Segoe UI" : s.FontFamily);
            OverlayText.FontFamily = ff;
            EditBox.FontFamily = ff;
        }
        catch { /* 폰트 적용 실패 시 무시 */ }

        OverlayText.FontSize = s.FontSize > 0 ? s.FontSize : 24;
        EditBox.FontSize = OverlayText.FontSize;
    }

    private void LeftHandle_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _leftHandlePressed = true;
        _leftHandleDidDrag = false;
        _leftHandleDownPos = e.GetPosition(this);
    }

    private void LeftHandle_OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (!_leftHandlePressed) return;
        if (_isEditMode) return; // 조정모드에서는 드래그로 이동하지 않음
        if (e.LeftButton != System.Windows.Input.MouseButtonState.Pressed) return;

        var p = e.GetPosition(this);
        const double threshold = 5.0; // 픽셀
        if (Math.Abs(p.X - _leftHandleDownPos.X) > threshold || Math.Abs(p.Y - _leftHandleDownPos.Y) > threshold)
        {
            try
            {
                _leftHandleDidDrag = true;
                DragMove();
            }
            catch { }
        }
    }

    private void LeftHandle_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (!_leftHandlePressed)
            return;

        if (!_leftHandleDidDrag)
        {
            SetSettingsState(!_isEditMode);
        }

        _leftHandlePressed = false;
        _leftHandleDidDrag = false;
    }

    private void ExitButton_OnClick(object sender, RoutedEventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }

    private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
    {
        var win = new SettingsWindow(_settings);
        win.Owner = this;
        if (win.ShowDialog() == true)
        {
            // 저장된 설정 적용
            _settings = win.ResultSettings;
            ApplySettingsToUi(_settings);
            Services.SettingsStore.Save(_settings);
        }
    }

    private void PersistTextIfNeeded()
    {
        if (_settings is null) return;
        _settings.OverlayText = EditBox.Text ?? string.Empty;
        // 창 위치/크기 저장
        _settings.WindowLeft = this.Left;
        _settings.WindowTop = this.Top;
        _settings.WindowWidth = this.Width;
        _settings.WindowHeight = this.Height;
        Services.SettingsStore.Save(_settings);
    }

    private void SetResizeMode(bool on)
    {
        _isResizeMode = on;
        var vis = on ? Visibility.Visible : Visibility.Collapsed;
        ResizeVisual.Visibility = vis;
        ResizeTop.Visibility = vis;
        ResizeBottom.Visibility = vis;
        ResizeLeft.Visibility = vis;
        ResizeRight.Visibility = vis;
        if (!on)
        {
            // 드래그 중이었다면 해제
            _isResizing = false;
            _currentEdge = ResizeEdge.None;
            System.Windows.Input.Mouse.Capture(null);
        }
    }

    private void ResizeHandle_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!_isResizeMode) return;
        if (sender is FrameworkElement fe && fe.Tag is string tag)
        {
            _currentEdge = tag switch
            {
                "Left" => ResizeEdge.Left,
                "Right" => ResizeEdge.Right,
                "Top" => ResizeEdge.Top,
                "Bottom" => ResizeEdge.Bottom,
                _ => ResizeEdge.None
            };
            _isResizing = _currentEdge != ResizeEdge.None;
            if (_isResizing)
            {
                _startMouse = e.GetPosition(this);
                _startMouseScreen = this.PointToScreen(_startMouse);
                _startLeft = this.Left;
                _startTop = this.Top;
                _startWidth = this.Width;
                _startHeight = this.Height;
                System.Windows.Input.Mouse.Capture(this);
                e.Handled = true;
            }
        }
    }

    private void Window_OnMouseMove(object? sender, System.Windows.Input.MouseEventArgs e)
    {
        if (!_isResizing) return;
        // 화면 좌표 기반 델타 (DPI 보정 포함)로 계산해 요동 방지
        var currentScreen = this.PointToScreen(e.GetPosition(this));
        var dxPx = currentScreen.X - _startMouseScreen.X;
        var dyPx = currentScreen.Y - _startMouseScreen.Y;
        var dpi = System.Windows.Media.VisualTreeHelper.GetDpi(this);
        var dx = dxPx / dpi.DpiScaleX;
        var dy = dyPx / dpi.DpiScaleY;

        switch (_currentEdge)
        {
            case ResizeEdge.Left:
            {
                var newWidth = _startWidth - dx;
                if (newWidth < this.MinWidth)
                {
                    dx = _startWidth - this.MinWidth;
                    newWidth = this.MinWidth;
                }
                this.Left = _startLeft + dx;
                this.Width = newWidth;
                break;
            }
            case ResizeEdge.Right:
            {
                var newWidth = _startWidth + dx;
                this.Width = Math.Max(this.MinWidth, newWidth);
                break;
            }
            case ResizeEdge.Top:
            {
                var newHeight = _startHeight - dy;
                if (newHeight < this.MinHeight)
                {
                    dy = _startHeight - this.MinHeight;
                    newHeight = this.MinHeight;
                }
                this.Top = _startTop + dy;
                this.Height = newHeight;
                break;
            }
            case ResizeEdge.Bottom:
            {
                var newHeight = _startHeight + dy;
                this.Height = Math.Max(this.MinHeight, newHeight);
                break;
            }
        }
    }

    private void Window_OnMouseLeftButtonUp(object? sender, MouseButtonEventArgs e)
    {
        if (_isResizing)
        {
            _isResizing = false;
            _currentEdge = ResizeEdge.None;
            System.Windows.Input.Mouse.Capture(null);
            e.Handled = true;
        }
    }

    private void SetSettingsState(bool on)
    {
        _isEditMode = on;
        EditBox.Visibility = on ? Visibility.Visible : Visibility.Collapsed;
        ToolbarPanel.Visibility = on ? Visibility.Visible : Visibility.Collapsed;
        SetResizeMode(on);
        if (on)
        {
            EditBox.Focus();
            EditBox.CaretIndex = EditBox.Text?.Length ?? 0;
        }
        else
        {
            PersistTextIfNeeded();
        }
    }

    private void ApplyWindowPosition(Models.AppSettings s)
    {
        var vsLeft = SystemParameters.VirtualScreenLeft;
        var vsTop = SystemParameters.VirtualScreenTop;
        var vsRight = vsLeft + SystemParameters.VirtualScreenWidth;
        var vsBottom = vsTop + SystemParameters.VirtualScreenHeight;

        // 크기 먼저 복원 (없으면 현재값 유지)
        var targetWidth = s.WindowWidth ?? (this.ActualWidth > 0 ? this.ActualWidth : this.Width);
        var targetHeight = s.WindowHeight ?? (this.ActualHeight > 0 ? this.ActualHeight : this.Height);

        // 화면 가시 영역 기준으로 크기 클램프
        targetWidth = Math.Max(this.MinWidth, Math.Min(targetWidth, SystemParameters.VirtualScreenWidth));
        targetHeight = Math.Max(this.MinHeight, Math.Min(targetHeight, SystemParameters.VirtualScreenHeight));

        this.Width = targetWidth;
        this.Height = targetHeight;

        // 위치 복원 (둘 다 있는 경우에 한해 적용)
        if (s.WindowLeft.HasValue && s.WindowTop.HasValue)
        {
            var maxLeft = Math.Max(vsLeft, vsRight - targetWidth);
            var maxTop = Math.Max(vsTop, vsBottom - targetHeight);

            var newLeft = Math.Max(vsLeft, Math.Min(s.WindowLeft.Value, maxLeft));
            var newTop = Math.Max(vsTop, Math.Min(s.WindowTop.Value, maxTop));

            this.Left = newLeft;
            this.Top = newTop;
        }
    }
}
