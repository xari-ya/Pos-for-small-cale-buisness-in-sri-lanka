using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace billing_system
{
    public partial class Login : Form
    {
        private const string UsernamePlaceholder = "Enter your username";
        private const string PasswordPlaceholder = "Enter your password";

        private readonly Color BorderColor = Color.FromArgb(189, 195, 199);
        private readonly Color BorderFocusColor = Color.FromArgb(44, 62, 80);
        private readonly Color PlaceholderColor = Color.Gray;
        private readonly Color TextColor = Color.Black;

        private readonly AuthService _auth = new AuthService();

        public Login()
        {
            InitializeComponent();
            InitializeCustomComponents();
            InitializeEventHandlers();
            SetPlaceholders();
            ConfigureInputs();
        }

        private void InitializeCustomComponents()
        {
            this.Paint += (sender, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                    Color.FromArgb(189, 195, 199), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(189, 195, 199), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(189, 195, 199), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(189, 195, 199), 1, ButtonBorderStyle.Solid);
            };

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(0, 0, LogoPanel.Width, LogoPanel.Height);
                LogoPanel.Region = new Region(path);
            }
        }

        private void OpenShell(User user)
        {
            Form shell = string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase)
                ? new MainShellForm()
                : new CashierPOSForm();

            shell.FormClosed += (s, args) => this.Close();
            this.Hide();
            shell.Show();
        }

        private void ConfigureInputs()
        {
            UsernameTextBox.Multiline = false;
            PasswordTextBox.Multiline = false;
            PasswordTextBox.UseSystemPasswordChar = true;

            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                UsernameTextBox.Text = UsernamePlaceholder;
                UsernameTextBox.ForeColor = Color.Gray;
            }
            if (string.IsNullOrWhiteSpace(PasswordTextBox.Text))
            {
                PasswordTextBox.UseSystemPasswordChar = false;
                PasswordTextBox.Text = PasswordPlaceholder;
                PasswordTextBox.ForeColor = Color.Gray;
            }

            UsernameTextBox.GotFocus += (s, e) =>
            {
                if (UsernameTextBox.Text == UsernamePlaceholder)
                {
                    UsernameTextBox.Text = "";
                    UsernameTextBox.ForeColor = Color.Black;
                }
            };
            UsernameTextBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
                {
                    UsernameTextBox.Text = UsernamePlaceholder;
                    UsernameTextBox.ForeColor = Color.Gray;
                }
            };

            PasswordTextBox.GotFocus += (s, e) =>
            {
                if (PasswordTextBox.Text == PasswordPlaceholder)
                {
                    PasswordTextBox.Text = "";
                    PasswordTextBox.ForeColor = Color.Black;
                    PasswordTextBox.UseSystemPasswordChar = true;
                }
            };
            PasswordTextBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(PasswordTextBox.Text))
                {
                    PasswordTextBox.UseSystemPasswordChar = false;
                    PasswordTextBox.Text = PasswordPlaceholder;
                    PasswordTextBox.ForeColor = Color.Gray;
                }
            };

            this.AcceptButton = LoginButton;
        }

        private void InitializeEventHandlers()
        {
            CloseButton.Click += (s, e) => Application.Exit();
            MinimizeButton.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

            CloseButton.MouseEnter += (s, e) => CloseButton.BackColor = Color.FromArgb(52, 73, 94);
            CloseButton.MouseLeave += (s, e) => CloseButton.BackColor = Color.FromArgb(44, 62, 80);

            MinimizeButton.MouseEnter += (s, e) => MinimizeButton.BackColor = Color.FromArgb(52, 73, 94);
            MinimizeButton.MouseLeave += (s, e) => MinimizeButton.BackColor = Color.FromArgb(44, 62, 80);

            LoginButton.Click += LoginButton_Click;
            LoginButton.MouseEnter += (s, e) => LoginButton.BackColor = Color.FromArgb(61, 255, 143);
            LoginButton.MouseLeave += (s, e) => LoginButton.BackColor = Color.FromArgb(46, 204, 113);

            HeaderPanel.MouseDown += HeaderPanel_MouseDown;
            TitleLabel.MouseDown += HeaderPanel_MouseDown;

            UsernameTextBox.GotFocus += (s, e) => HandleFocus(UsernameTextBox, UsernamePanel, UsernamePlaceholder);
            UsernameTextBox.LostFocus += (s, e) => HandleLostFocus(UsernameTextBox, UsernamePanel, UsernamePlaceholder);

            PasswordTextBox.GotFocus += (s, e) => HandleFocus(PasswordTextBox, PasswordPanel, PasswordPlaceholder);
            PasswordTextBox.LostFocus += (s, e) => HandleLostFocus(PasswordTextBox, PasswordPanel, PasswordPlaceholder);
        }

        private void SetPlaceholders()
        {
            HandleLostFocus(UsernameTextBox, UsernamePanel, UsernamePlaceholder);
            HandleLostFocus(PasswordTextBox, PasswordPanel, PasswordPlaceholder);
        }

        private void HandleFocus(TextBox textBox, Panel panel, string placeholder)
        {
            panel.BackColor = BorderFocusColor;
            if (textBox.Text == placeholder)
            {
                textBox.Text = "";
                textBox.ForeColor = TextColor;
                if (textBox == PasswordTextBox)
                {
                    textBox.UseSystemPasswordChar = true;
                }
            }
        }

        private void HandleLostFocus(TextBox textBox, Panel panel, string placeholder)
        {
            panel.BackColor = BorderColor;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = placeholder;
                textBox.ForeColor = PlaceholderColor;
                if (textBox == PasswordTextBox)
                {
                    textBox.UseSystemPasswordChar = false;
                }
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string userText = (UsernameTextBox.Text ?? "").Trim();
            string passText = PasswordTextBox.Text ?? "";

            if (userText == UsernamePlaceholder) userText = "";
            if (passText == PasswordPlaceholder) passText = "";

            if (string.IsNullOrWhiteSpace(userText) || string.IsNullOrWhiteSpace(passText))
            {
                ErrorMessageLabel.Text = "Please enter username and password.";
                ErrorMessageLabel.Visible = true;
                return;
            }

            var result = _auth.Login(userText, passText);
            if (!result.IsAuthenticated || result.User == null)
            {
                ErrorMessageLabel.Text = result.Error ?? "Invalid username or password.";
                ErrorMessageLabel.Visible = true;
                AuthTroubleshooter.RunFull(userText, passText);
                return;
            }

            AuthTroubleshooter.CheckCurrentUser("After successful login");

            AppSession.CurrentUser = result.User;

            Console.WriteLine($"Username: {AppSession.CurrentUser.Username}");
            Console.WriteLine($"FullName: {AppSession.CurrentUser.FullName}");
            Console.WriteLine($"Role: {AppSession.CurrentUser.Role}");
            Console.WriteLine($"IsActive: {AppSession.CurrentUser.IsActive}");

            OpenShell(result.User);
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void LoginButton_Click_1(object sender, EventArgs e)
        {
            // Not used. Ensure the Designer's Click is wired to LoginButton_Click.
        }

        private void MainTitleLabel_Click(object sender, EventArgs e)
        {
        }
    }
}
