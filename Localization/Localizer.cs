using System;

namespace MazeGame.Localization;

public enum AppLanguage { English, Arabic }

/// <summary>
/// نظام ترجمة بسيط: الإنجليزية هي اللغة الافتراضية لكل الواجهة، وزر واحد يبدّل
/// كل النصوص المسجَّلة إلى العربية والعكس فورًا (Event-driven، بدون إعادة تشغيل النموذج).
/// </summary>
public static class Localizer
{
    public static AppLanguage Current { get; private set; } = AppLanguage.English;
    public static event Action? LanguageChanged;

    public static void Toggle()
    {
        Current = Current == AppLanguage.English ? AppLanguage.Arabic : AppLanguage.English;
        LanguageChanged?.Invoke();
    }

    public static string T(string en, string ar) => Current == AppLanguage.English ? en : ar;
}
