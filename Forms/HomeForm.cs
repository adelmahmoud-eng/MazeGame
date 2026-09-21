using System;
using System.Drawing;
using System.Windows.Forms;
using MazeGame.Localization;
using MazeGame.Theme;

namespace MazeGame.Forms;

public sealed class HomeForm : Form
{
    public HomeForm()
    {
        Text = "Luxor National University — Problem Solving Community";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1100, 780);
        MinimumSize = new Size(900, 650);
        BackColor = AppTheme.OffWhite;
        Font = AppTheme.BodyFont;

        BuildUi();
        LanguageToggleButton.ApplyFormDirection(this);
    }

    private void BuildUi()
    {
        // ================= Top navbar (slim, small text — like a retail site header) =================
        var navBar = new Panel { Dock = DockStyle.Top, Height = 46, BackColor = AppTheme.DeepBlue };

        var logo = new LocalizedLabel("Luxor National University", "جامعة الأقصر الأهلية")
        {
            Font = AppTheme.LogoFont,
            ForeColor = AppTheme.White,
            AutoSize = true,
            Location = new Point(14, 12)
        };
        var logoTag = new LocalizedLabel("Problem Solving Community", "كوميونتي حل المشكلات")
        {
            Font = AppTheme.SmallFont,
            ForeColor = AppTheme.SkyBlue,
            AutoSize = true
        };

        var langBtn = new LanguageToggleButton
        {
            Size = new Size(110, 28),
            FlatStyle = FlatStyle.Flat,
            BackColor = AppTheme.PrimaryBlue,
            ForeColor = AppTheme.White,
            Font = AppTheme.SmallFont,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        langBtn.FlatAppearance.BorderSize = 0;

        navBar.Controls.Add(langBtn);
        navBar.Controls.Add(logoTag);
        navBar.Controls.Add(logo);

        void RepositionNavBar()
        {
            logoTag.Location = new Point(logo.Right + 12, 16);
            langBtn.Location = new Point(navBar.Width - langBtn.Width - 14, 9);
        }
        navBar.Resize += (_, _) => RepositionNavBar();
        RepositionNavBar();

        // ================= Hero strip (short, moderate headline — not oversized) =================
        var hero = new Panel { Dock = DockStyle.Top, Height = 96, BackColor = AppTheme.PrimaryBlue };
        var heroTitle = new LocalizedLabel(
            "Learn programming logic by solving mazes",
            "تعلّم منطق البرمجة عبر حل المتاهات")
        {
            Font = AppTheme.HeroFont,
            ForeColor = AppTheme.White,
            AutoSize = false,
            Dock = DockStyle.Top,
            Height = 48,
            TextAlign = ContentAlignment.MiddleCenter
        };
        var heroSubtitle = new LocalizedLabel(
            "9 progressive levels • block-based programming • the original Blockly Games Maze, embedded",
            "9 مستويات متدرّجة • برمجة بالكتل • لعبة Blockly Games Maze الأصلية بالضبط")
        {
            Font = AppTheme.BodyFont,
            ForeColor = AppTheme.MilkyBlue,
            AutoSize = false,
            Dock = DockStyle.Top,
            Height = 24,
            TextAlign = ContentAlignment.MiddleCenter
        };
        hero.Controls.Add(heroSubtitle);
        hero.Controls.Add(heroTitle);

        // ================= Scrollable content =================
        var scroller = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = AppTheme.OffWhite, Padding = new Padding(24, 16, 24, 16) };
        var content = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 1 };
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        // ---- Info cards row (3 small cards, Amazon "info tile" style) ----
        var infoRow = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, FlowDirection = FlowDirection.LeftToRight, WrapContents = true };
        infoRow.Controls.Add(MakeInfoCard(
            "What is Problem Solving?", "ما هو حل المشكلات؟",
            "The skill of breaking a challenge into small logical steps and designing a solution for it — the foundation of every programmer and engineer.",
            "مهارة تفكيك التحدي إلى خطوات منطقية صغيرة وابتكار حل له — وهي أساس عمل أي مبرمج ومهندس."));
        infoRow.Controls.Add(MakeInfoCard(
            "What is Competitive Programming?", "ما هي البرمجة التنافسية؟",
            "A mental sport where programmers race to solve algorithmic problems quickly and accurately — think ICPC, Codeforces, and LeetCode contests.",
            "رياضة ذهنية يتسابق فيها المبرمجون لحل مسائل خوارزمية بسرعة ودقة — مثل مسابقات ICPC وCodeforces وLeetCode."));
        infoRow.Controls.Add(MakeInfoCard(
            "Why start with a Maze?", "لماذا نبدأ بالمتاهة؟",
            "Before advanced algorithms, every competitor needs sequencing, loops, and conditionals — exactly what these 9 levels teach, hands-on.",
            "قبل الخوارزميات المتقدمة، يحتاج كل متسابق إتقان الترتيب والتكرار والشروط — وهذا بالضبط ما تعلّمه المستويات التسعة."));
        content.Controls.Add(infoRow);

        // ---- Levels section (Amazon "product grid" style small tiles) ----
        var levelsSection = new Panel { Dock = DockStyle.Top, AutoSize = true, Margin = new Padding(0, 16, 0, 0) };
        var levelsHeader = new LocalizedLabel("Maze Levels", "مستويات المتاهة")
        {
            Font = AppTheme.SectionFont,
            ForeColor = AppTheme.DeepBlue,
            Dock = DockStyle.Top,
            Height = 26
        };
        levelsSection.Controls.Add(levelsHeader);

        var levelsGrid = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
            Padding = new Padding(0, 6, 0, 0)
        };
        for (int i = 1; i <= 9; i++)
        {
            int level = i;
            levelsGrid.Controls.Add(MakeLevelTile(level));
        }
        levelsSection.Controls.Add(levelsGrid);
        levelsSection.Controls.SetChildIndex(levelsGrid, 0);
        levelsSection.Controls.SetChildIndex(levelsHeader, 1);
        content.Controls.Add(levelsSection);

        // ---- Footer ----
        var footer = new LocalizedLabel(
            "Organized by the Problem Solving Community — Luxor National University. Colors follow the official university identity (white & sky blue).",
            "بتنظيم كوميونتي حل المشكلات — جامعة الأقصر الأهلية. الألوان مطابقة للهوية الرسمية للجامعة (الأبيض والأزرق اللبني).")
        {
            Font = AppTheme.SmallFont,
            ForeColor = AppTheme.TextGray,
            Dock = DockStyle.Top,
            Height = 40,
            TextAlign = ContentAlignment.MiddleCenter,
            Margin = new Padding(0, 18, 0, 0)
        };
        content.Controls.Add(footer);

        scroller.Controls.Add(content);

        Controls.Add(scroller);
        Controls.Add(hero);
        Controls.Add(navBar);
    }

    private static Panel MakeInfoCard(string titleEn, string titleAr, string bodyEn, string bodyAr)
    {
        var card = new Panel { Width = 300, Height = 150, Margin = new Padding(0, 0, 14, 14), Padding = new Padding(14) };
        AppTheme.StyleAsCard(card);

        var title = new LocalizedLabel(titleEn, titleAr)
        {
            Font = AppTheme.SectionFont,
            ForeColor = AppTheme.PrimaryBlue,
            Dock = DockStyle.Top,
            Height = 22
        };
        var body = new LocalizedLabel(bodyEn, bodyAr)
        {
            Font = AppTheme.SmallFont,
            ForeColor = AppTheme.TextGray,
            Dock = DockStyle.Fill,
        };
        card.Controls.Add(body);
        card.Controls.Add(title);
        return card;
    }

    private Panel MakeLevelTile(int level)
    {
        var tile = new Panel { Width = 108, Height = 108, Margin = new Padding(0, 0, 10, 10), Padding = new Padding(8) };
        AppTheme.StyleAsCard(tile);

        var numberLabel = new Label
        {
            Text = $"Lv. {level}",
            Font = AppTheme.SectionFont,
            ForeColor = AppTheme.DeepBlue,
            Dock = DockStyle.Top,
            Height = 26,
            TextAlign = ContentAlignment.MiddleCenter
        };
        var playBtn = new LocalizedButton("Play", "العب") { Dock = DockStyle.Bottom, Height = 28 };
        AppTheme.StylePrimary(playBtn);
        playBtn.Click += (_, _) =>
        {
            var mazeForm = new MazeForm(level);
            mazeForm.Show();
            Hide();
            mazeForm.FormClosed += (_, _) => Close();
        };

        tile.Controls.Add(playBtn);
        tile.Controls.Add(numberLabel);
        return tile;
    }
}
