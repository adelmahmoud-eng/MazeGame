using System.Drawing;
using System.Windows.Forms;

namespace MazeGame.Theme;

/// <summary>
/// Visual identity: university colors (white + sky/milky blue), laid out with the
/// clean, compact, small-font typography style of large e-commerce sites (e.g. Amazon):
/// a slim dark navbar, small readable body text, subtle card borders, generous whitespace.
/// </summary>
public static class AppTheme
{
    public static readonly Color White      = Color.White;
    public static readonly Color OffWhite    = Color.FromArgb(250, 250, 250);
    public static readonly Color MilkyBlue   = Color.FromArgb(214, 236, 248);
    public static readonly Color SkyBlue     = Color.FromArgb(133, 199, 232);
    public static readonly Color PrimaryBlue = Color.FromArgb(35, 84, 128);   // navbar / headings
    public static readonly Color DeepBlue    = Color.FromArgb(19, 47, 72);    // darkest navy, hover states
    public static readonly Color AccentBlue  = Color.FromArgb(41, 128, 185);  // CTA buttons / links
    public static readonly Color CardBorder  = Color.FromArgb(222, 226, 230); // subtle e-commerce card border
    public static readonly Color TextGray    = Color.FromArgb(85, 85, 85);
    public static readonly Color FinishGreen = Color.FromArgb(39, 174, 96);
    public static readonly Color CrashRed    = Color.FromArgb(192, 57, 43);

    // Small, tidy typography — nothing oversized, matching a clean commercial site.
    public static readonly Font LogoFont     = new("Segoe UI", 12.5f, FontStyle.Bold);
    public static readonly Font HeroFont     = new("Segoe UI", 17f, FontStyle.Bold);
    public static readonly Font SectionFont  = new("Segoe UI", 10.5f, FontStyle.Bold);
    public static readonly Font BodyFont     = new("Segoe UI", 9f);
    public static readonly Font SmallFont    = new("Segoe UI", 8f);
    public static readonly Font NavFont      = new("Segoe UI", 9f);
    public static readonly Font ButtonFont   = new("Segoe UI", 9f, FontStyle.Bold);

    /// <summary>يطبّق مظهر الزر الأساسي (خلفية زرقاء) على أي زر — عادي أو LocalizedButton.</summary>
    public static void StylePrimary(Button btn)
    {
        btn.Font = ButtonFont;
        btn.BackColor = AccentBlue;
        btn.ForeColor = White;
        btn.FlatStyle = FlatStyle.Flat;
        btn.Height = 30;
        btn.Cursor = Cursors.Hand;
        btn.UseVisualStyleBackColor = false;
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = DeepBlue;
    }

    /// <summary>يطبّق مظهر الزر الثانوي (إطار رفيع، خلفية بيضاء) على أي زر — عادي أو LocalizedButton.</summary>
    public static void StyleSecondary(Button btn)
    {
        btn.Font = ButtonFont;
        btn.BackColor = White;
        btn.ForeColor = PrimaryBlue;
        btn.FlatStyle = FlatStyle.Flat;
        btn.Height = 28;
        btn.Cursor = Cursors.Hand;
        btn.UseVisualStyleBackColor = false;
        btn.FlatAppearance.BorderSize = 1;
        btn.FlatAppearance.BorderColor = CardBorder;
        btn.FlatAppearance.MouseOverBackColor = MilkyBlue;
    }

    public static Button MakePrimaryButton(string text)
    {
        var btn = new Button { Text = text };
        StylePrimary(btn);
        return btn;
    }

    public static Button MakeSecondaryButton(string text)
    {
        var btn = new Button { Text = text };
        StyleSecondary(btn);
        return btn;
    }

    public static void StyleAsCard(Panel panel)
    {
        panel.BackColor = White;
        panel.Paint += (_, e) =>
        {
            using var pen = new Pen(CardBorder, 1f);
            e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
        };
    }
}
