using System;
using System.IO;
using System.Text.Json;
using YtCaption.Wpf.Models;

namespace YtCaption.Wpf.Services;

public static class SettingsStore
{
    private static string GetFolder()
    {
        var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YtCaption.Wpf");
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        return dir;
    }

    private static string GetPath() => Path.Combine(GetFolder(), "settings.json");

    public static AppSettings? Load()
    {
        try
        {
            var path = GetPath();
            if (!File.Exists(path)) return null;
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<AppSettings>(json);
        }
        catch
        {
            return null;
        }
    }

    public static void Save(AppSettings settings)
    {
        try
        {
            var path = GetPath();
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
        catch
        {
            // 무시: 저장 실패해도 앱 동작 유지
        }
    }
}

