namespace YtCaption.Wpf.Models;

public class AppSettings
{
    public string OverlayColor { get; set; } = "#1A000000"; // AARRGGBB (기본 10% 검정)
    public string FontFamily { get; set; } = "Segoe UI";
    public double FontSize { get; set; } = 24;
    public string OverlayText { get; set; } = string.Empty;
}

