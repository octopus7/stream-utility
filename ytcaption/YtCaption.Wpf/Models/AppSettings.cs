namespace YtCaption.Wpf.Models;

public class AppSettings
{
    public string OverlayColor { get; set; } = "#1A000000"; // AARRGGBB (기본 10% 검정)
    public string LeftHandleColor { get; set; } = "#22FFFFFF"; // AARRGGBB (기본 옅은 흰색)
    public string FontFamily { get; set; } = "Segoe UI";
    public double FontSize { get; set; } = 24;
    public string OverlayText { get; set; } = string.Empty;
    public double? WindowLeft { get; set; }
    public double? WindowTop { get; set; }
    public double? WindowWidth { get; set; }
    public double? WindowHeight { get; set; }
}
