using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using MazeGame.Localization;
using MazeGame.Theme;
using Microsoft.Web.WebView2.WinForms;

namespace MazeGame.Forms;

/// <summary>
/// Hosts the ORIGINAL blockly.games Maze website inside a WebView2 browser control,
/// so the game itself is pixel-for-pixel identical to the real thing — this app only
/// adds the university-branded chrome (header, level picker, back/translate buttons)
/// around the embedded page.
/// </summary>
public sealed class MazeForm : Form
{
    private readonly WebView2 _webView = new() { Dock = DockStyle.Fill };
    private readonly ComboBox _levelPicker = new() { DropDownStyle = ComboBoxStyle.DropDownList, Font = AppTheme.SmallFont, Width = 90 };
    private readonly Label _statusLabel = new();
    private int _currentLevel;
    private bool _webViewReady;

    public MazeForm(int levelNumber)
    {
        _currentLevel = levelNumber;

        Text = "Maze — Luxor National University";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1200, 800);
        MinimumSize = new Size(860, 600);
        BackColor = AppTheme.White;
        Font = AppTheme.BodyFont;

        BuildUi();
        LanguageToggleButton.ApplyFormDirection(this);

        Load += async (_, _) => await NavigateToLevelAsync(_currentLevel);
    }

    private void BuildUi()
    {
        var topBar = new Panel { Dock = DockStyle.Top, Height = 42, BackColor = AppTheme.DeepBlue };

        var backBtn = new LocalizedButton("⟵ Home", "⟵ الرئيسية") { Location = new Point(10, 6), Size = new Size(90, 28), Font = AppTheme.SmallFont };
        AppTheme.StyleSecondary(backBtn);
        backBtn.Click += (_, _) =>
        {
            var home = new HomeForm();
            home.Show();
            Close();
        };

        var levelLabel = new LocalizedLabel("Level:", "المستوى:")
        {
            ForeColor = AppTheme.White,
            Font = AppTheme.SmallFont,
            AutoSize = true,
            Location = new Point(112, 13)
        };

        for (int i = 1; i <= 9; i++) _levelPicker.Items.Add(i);
        _levelPicker.Location = new Point(160, 8);
        _levelPicker.SelectedIndexChanged += async (_, _) =>
        {
            int chosen = (int)_levelPicker.SelectedItem!;
            if (chosen != _currentLevel) await NavigateToLevelAsync(chosen);
        };

        var langBtn = new LanguageToggleButton
        {
            Size = new Size(100, 28),
            FlatStyle = FlatStyle.Flat,
            BackColor = AppTheme.PrimaryBlue,
            ForeColor = AppTheme.White,
            Font = AppTheme.SmallFont,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        langBtn.FlatAppearance.BorderSize = 0;

        var titleLabel = new LocalizedLabel("Blockly Games — Maze (original site)", "Blockly Games — متاهة (الموقع الأصلي)")
        {
            ForeColor = AppTheme.SkyBlue,
            Font = AppTheme.SmallFont,
            AutoSize = true,
            Anchor = AnchorStyles.Top
        };

        topBar.Controls.Add(langBtn);
        topBar.Controls.Add(titleLabel);
        topBar.Controls.Add(_levelPicker);
        topBar.Controls.Add(levelLabel);
        topBar.Controls.Add(backBtn);

        void RepositionTopBar()
        {
            langBtn.Location = new Point(topBar.Width - langBtn.Width - 10, 7);
            titleLabel.Location = new Point(topBar.Width / 2 - titleLabel.Width / 2, 13);
        }
        topBar.Resize += (_, _) => RepositionTopBar();
        RepositionTopBar();

        _statusLabel.Dock = DockStyle.Bottom;
        _statusLabel.Height = 24;
        _statusLabel.Font = AppTheme.SmallFont;
        _statusLabel.ForeColor = AppTheme.TextGray;
        _statusLabel.TextAlign = ContentAlignment.MiddleCenter;
        _statusLabel.BackColor = AppTheme.OffWhite;
        _statusLabel.Text = "Loading the original Maze game from blockly.games ...";

        Controls.Add(_webView);
        Controls.Add(_statusLabel);
        Controls.Add(topBar);
    }

    private async Task NavigateToLevelAsync(int level)
    {
        _currentLevel = level;
        _levelPicker.SelectedItem = level;
        _statusLabel.Text = $"Loading level {level} from blockly.games ...";

        try
        {
            await _webView.EnsureCoreWebView2Async();
            if (!_webViewReady)
            {
                _webViewReady = true;
                // Subscribed once (not per-navigation) to avoid stacking duplicate handlers.
                _webView.CoreWebView2.NavigationCompleted += (_, e) =>
                {
                    _statusLabel.Text = e.IsSuccess
                        ? $"Level {_currentLevel} loaded from the original blockly.games site."
                        : "Could not load the page. Check your internet connection.";
                };
            }
            string url = $"https://blockly.games/maze?lang=en&level={level}&skin=0";
            _webView.CoreWebView2.Navigate(url);
        }
        catch (Exception ex)
        {
            _statusLabel.Text = "WebView2 Runtime not found. Please install it from Microsoft's website.";
            MessageBox.Show(this,
                "This game embeds the real blockly.games website using the Microsoft Edge WebView2 control.\n" +
                "It looks like the WebView2 Runtime is not installed on this machine.\n\n" +
                "Download it (free) from: https://developer.microsoft.com/microsoft-edge/webview2/\n\n" +
                $"Technical detail: {ex.Message}",
                "WebView2 Runtime required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
