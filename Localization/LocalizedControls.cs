using System;
using System.Windows.Forms;

namespace MazeGame.Localization;

/// <summary>Label يحمل نصّه الإنجليزي والعربي معًا، ويحدّث نفسه تلقائيًا عند تبديل اللغة.</summary>
public class LocalizedLabel : Label
{
    private readonly string _en, _ar;

    public LocalizedLabel(string en, string ar)
    {
        _en = en; _ar = ar;
        RefreshText();
        Localizer.LanguageChanged += RefreshText;
    }

    private void RefreshText() => Text = Localizer.T(_en, _ar);

    protected override void Dispose(bool disposing)
    {
        Localizer.LanguageChanged -= RefreshText;
        base.Dispose(disposing);
    }
}

/// <summary>Button يحمل نصّه الإنجليزي والعربي معًا، ويحدّث نفسه تلقائيًا عند تبديل اللغة.</summary>
public class LocalizedButton : Button
{
    private readonly string _en, _ar;

    public LocalizedButton(string en, string ar)
    {
        _en = en; _ar = ar;
        RefreshText();
        Localizer.LanguageChanged += RefreshText;
    }

    private void RefreshText() => Text = Localizer.T(_en, _ar);

    protected override void Dispose(bool disposing)
    {
        Localizer.LanguageChanged -= RefreshText;
        base.Dispose(disposing);
    }
}

/// <summary>
/// زر التبديل نفسه: يعرض دائمًا اسم اللغة "الأخرى" (اضغط لتتحول إليها)،
/// ويبدّل اتجاه/محاذاة الفورم الحاوي له (RightToLeft) عند كل ضغطة.
/// </summary>
public sealed class LanguageToggleButton : Button
{
    public LanguageToggleButton()
    {
        RefreshText();
        Click += (_, _) =>
        {
            Localizer.Toggle();
            RefreshText();
            ApplyFormDirection(FindForm());
        };
        Localizer.LanguageChanged += RefreshText;
    }

    private void RefreshText() => Text = Localizer.Current == AppLanguage.English ? "🌐 العربية" : "🌐 English";

    public static void ApplyFormDirection(Form? form)
    {
        if (form is null) return;
        bool arabic = Localizer.Current == AppLanguage.Arabic;
        form.RightToLeft = arabic ? RightToLeft.Yes : RightToLeft.No;
        form.RightToLeftLayout = arabic;
    }

    protected override void Dispose(bool disposing)
    {
        Localizer.LanguageChanged -= RefreshText;
        base.Dispose(disposing);
    }
}
