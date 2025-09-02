using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using YtCaption.Wpf.Models;
using WinForms = System.Windows.Forms;

namespace YtCaption.Wpf;

public partial class SettingsWindow : Window
{
    public AppSettings ResultSettings { get; private set; }

    private System.Windows.Media.Color _rgbColor; // A 제외 RGB만 보관
    private byte _alpha;
    private System.Windows.Media.Color _leftRgbColor; // 좌측 빈영역 RGB
    private byte _leftAlpha;

    public SettingsWindow(AppSettings current)
    {
        InitializeComponent();
        // 복사본으로 편집
        ResultSettings = new AppSettings
        {
            OverlayColor = current.OverlayColor,
            LeftHandleColor = current.LeftHandleColor,
            FontFamily = current.FontFamily,
            FontSize = current.FontSize,
            OverlayText = current.OverlayText,
            WindowLeft = current.WindowLeft,
            WindowTop = current.WindowTop,
            WindowWidth = current.WindowWidth,
            WindowHeight = current.WindowHeight,
        };

        // 초기값 채우기
        var c = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(ResultSettings.OverlayColor);
        _rgbColor = System.Windows.Media.Color.FromRgb(c.R, c.G, c.B);
        _alpha = c.A;
        AlphaSlider.Value = Math.Round(_alpha * 100.0 / 255.0);
        ColorPreviewBrush.Color = _rgbColor;

        // 좌측 빈영역 초기값 채우기
        System.Windows.Media.Color lc;
        try
        {
            lc = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(string.IsNullOrWhiteSpace(ResultSettings.LeftHandleColor) ? "#22FFFFFF" : ResultSettings.LeftHandleColor);
        }
        catch
        {
            lc = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#22FFFFFF");
        }
        _leftRgbColor = System.Windows.Media.Color.FromRgb(lc.R, lc.G, lc.B);
        _leftAlpha = lc.A;
        LeftAlphaSlider.Value = Math.Round(_leftAlpha * 100.0 / 255.0);
        LeftColorPreviewBrush.Color = _leftRgbColor;

        FontFamilyBox.SelectedItem = Fonts.SystemFontFamilies.FirstOrDefault(f => string.Equals(f.Source, ResultSettings.FontFamily, StringComparison.OrdinalIgnoreCase))
                                      ?? new System.Windows.Media.FontFamily(ResultSettings.FontFamily);
        FontSizeBox.Text = (ResultSettings.FontSize > 0 ? ResultSettings.FontSize : 24).ToString(CultureInfo.InvariantCulture);

        ApplyPreview();
    }

    private void Save_OnClick(object sender, RoutedEventArgs e)
    {
        // 값 저장
        var ff = (FontFamilyBox.SelectedItem as System.Windows.Media.FontFamily)?.Source ?? ResultSettings.FontFamily;
        if (!double.TryParse(FontSizeBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var fs) || fs <= 0) fs = 24;

        var a = (byte)Math.Round(AlphaSlider.Value * 255.0 / 100.0);
        ResultSettings.OverlayColor = $"#{a:X2}{_rgbColor.R:X2}{_rgbColor.G:X2}{_rgbColor.B:X2}";
        var la = (byte)Math.Round(LeftAlphaSlider.Value * 255.0 / 100.0);
        ResultSettings.LeftHandleColor = $"#{la:X2}{_leftRgbColor.R:X2}{_leftRgbColor.G:X2}{_leftRgbColor.B:X2}";
        ResultSettings.FontFamily = ff;
        ResultSettings.FontSize = fs;

        DialogResult = true;
        Close();
    }

    private void Close_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void ApplyPreview()
    {
        try
        {
            // 폰트 미리보기 (컨트롤 값 기준)
            var ff = (FontFamilyBox.SelectedItem as System.Windows.Media.FontFamily)?.Source ?? ResultSettings.FontFamily;
            double fs = 24;
            _ = double.TryParse(FontSizeBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out fs);
            if (fs <= 0) fs = 24;
            PreviewText.FontFamily = new System.Windows.Media.FontFamily(ff);
            PreviewText.FontSize = fs;

            // 배경 미리보기 (RGB + 슬라이더 알파)
            var a = (byte)Math.Round(AlphaSlider.Value * 255.0 / 100.0);
            var col = System.Windows.Media.Color.FromArgb(a, _rgbColor.R, _rgbColor.G, _rgbColor.B);
            if (PreviewText.Parent is System.Windows.Controls.Border border)
            {
                border.Background = new SolidColorBrush(col);
            }
            // 상단 컬러 프리뷰 박스는 투명도 제외 RGB만 표시
            ColorPreviewBrush.Color = _rgbColor;
            AlphaValueText.Text = $"{Math.Round(AlphaSlider.Value)}%";

            // 좌측 빈영역 프리뷰 텍스트 (프리뷰 박스는 RGB만)
            LeftColorPreviewBrush.Color = _leftRgbColor;
            LeftAlphaValueText.Text = $"{Math.Round(LeftAlphaSlider.Value)}%";
        }
        catch { }
    }

    private void ChooseColorBtn_OnClick(object sender, RoutedEventArgs e)
    {
        var dlg = new WinForms.ColorDialog
        {
            AllowFullOpen = true,
            FullOpen = true,
            AnyColor = true,
            Color = System.Drawing.Color.FromArgb(_rgbColor.R, _rgbColor.G, _rgbColor.B)
        };
        if (dlg.ShowDialog() == WinForms.DialogResult.OK)
        {
            _rgbColor = System.Windows.Media.Color.FromRgb(dlg.Color.R, dlg.Color.G, dlg.Color.B);
            ApplyPreview();
        }
    }

    private void AlphaSlider_OnValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
    {
        ApplyPreview();
    }

    private void ChooseLeftColorBtn_OnClick(object sender, RoutedEventArgs e)
    {
        var dlg = new WinForms.ColorDialog
        {
            AllowFullOpen = true,
            FullOpen = true,
            AnyColor = true,
            Color = System.Drawing.Color.FromArgb(_leftRgbColor.R, _leftRgbColor.G, _leftRgbColor.B)
        };
        if (dlg.ShowDialog() == WinForms.DialogResult.OK)
        {
            _leftRgbColor = System.Windows.Media.Color.FromRgb(dlg.Color.R, dlg.Color.G, dlg.Color.B);
            ApplyPreview();
        }
    }

    private void LeftAlphaSlider_OnValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
    {
        ApplyPreview();
    }
}
