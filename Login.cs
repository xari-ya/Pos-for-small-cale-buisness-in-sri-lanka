using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace billing_system
{
    /// <summary>
    /// Represents the login form of the application.
    /// </summary>
    /// <remarks>
    /// This is the first form displayed to the user, responsible for authenticating their credentials.
    /// It interacts with the <see cref="AuthService"/> to verify the user's identity and, upon success,
    /// opens the appropriate main application window (<see cref="MainShellForm"/> for administrators
    /// or <see cref="CashierPOSForm"/> for cashiers).
    /// </remarks>
    public partial class Login : Form
    {
        private const string UsernamePlaceholder = "Enter your username";
        private const string PasswordPlaceholder = "Enter your password";

        private readonly Color BorderColor = Color.FromArgb(189, 195, 199);
        private readonly Color BorderFocusColor = Color.FromArgb(44, 62, 80);
        private readonly Color PlaceholderColor = Color.Gray;
        private readonly Color TextColor = Color.Black;

        private readonly AuthService _auth = new AuthService();

        /// <summary>
        /// Initializes a new instance of the <see cref="Login"/> class.
        /// </summary>
        /// <remarks>
        /// The constructor initializes the form's components and sets up custom styling and event handlers.
        /// </remarks>
        public Login()
        {
            InitializeComponent();
            InitializeCustomComponents();
            InitializeEventHandlers();
            SetPlaceholders();
            ConfigureInputs();
        }

        /// <summary>
        /// Initializes custom visual components and styles for the form.
        /// </summary>
        /// <remarks>
        /// This method sets up custom border painting for the form and applies a circular region to the logo panel.
        /// </remarks>
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

        /// <summary>
        /// Opens the main application form based on the user's role.
        /// </summary>
        /// <param name="user">The authenticated user.</param>
        /// <remarks>
        /// If the user's role is "Admin", it opens the <see cref="MainShellForm"/>. Otherwise, it opens the <see cref="CashierPOSForm"/>.
        /// This form is hidden, and the new form is shown. The application will close when the new form is closed.
        /// </remarks>
        private void OpenShell(User user)
        {
            Form shell = string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase)
                ? new MainShellForm()
                : new CashierPOSForm();

            shell.FormClosed += (s, args) => this.Close();
            this.Hide();
            shell.Show();
        }

        /// <summary>
        /// Configures the initial state and behavior of the input text boxes.
        /// </summary>
        /// <remarks>
        /// This method sets the password box to use the system password character and initializes the placeholder text.
        /// It also wires up focus and lost-focus events to manage the placeholder visibility.
        /// </remarks>
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

        /// <summary>
        /// Initializes the event handlers for form controls.
        /// </summary>
        /// <remarks>
        /// This method wires up click and mouse-hover events for buttons, focus events for text boxes,
        /// and mouse-down events for the header panel to enable window dragging.
        /// </remarks>
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

        /// <summary>
        /// Sets the initial placeholder text for the input fields.
        /// </summary>
        private void SetPlaceholders()
        {
            HandleLostFocus(UsernameTextBox, UsernamePanel, UsernamePlaceholder);
            HandleLostFocus(PasswordTextBox, PasswordPanel, PasswordPlaceholder);
        }

        /// <summary>
        /// Handles the GotFocus event for input text boxes.
        /// </summary>
        /// <param name="textBox">The text box that received focus.</param>
        /// <param name="panel">The panel surrounding the text box, used for visual feedback.</param>
        /// <param name="placeholder">The placeholder text to remove.</param>
        /// <remarks>
        /// This method changes the panel's back color to indicate focus and clears the placeholder text.
        /// If the text box is the password field, it enables the system password character.
        /// </remarks>
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

        /// <summary>
        /// Handles the LostFocus event for input text boxes.
        /// </summary>
        /// <param name="textBox">The text box that lost focus.</param>
        /// <param name="panel">The panel surrounding the text box, used for visual feedback.</param>
        /// <param name="placeholder">The placeholder text to display if the field is empty.</param>
        /// <remarks>
        /// This method resets the panel's back color. If the text box is empty, it restores the placeholder text
        /// and, for the password field, disables the system password character so the placeholder is visible.
        /// </remarks>
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

        /// <summary>
        /// Handles the Click event for the Login button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method retrieves the username and password, validates them, and calls the <see cref="AuthService.Login"/> method.
        /// If authentication is successful, it proceeds to open the main application form via <see cref="OpenShell"/>.
        /// If it fails, it displays an error message and may invoke the <see cref="AuthTroubleshooter"/> for diagnostics.
        /// </remarks>
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

        /// <summary>
        /// Windows message constant for a non-client left mouse button down event.
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;

        /// <summary>
        /// Windows message constant indicating the event occurred in the caption area (title bar).
        /// </summary>
        public const int HT_CAPTION = 0x2;

        /// <summary>
        /// Sends a message to the specified window.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose window procedure will receive the message.</param>
        /// <param name="Msg">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing and depends on the message sent.</returns>
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        /// <summary>
        /// Releases the mouse capture from a window in the current thread.
        /// </summary>
        /// <returns><c>true</c> if the function succeeds; otherwise, <c>false</c>.</returns>
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        /// <summary>
        /// Handles the MouseDown event for the header panel to enable window dragging.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method captures the mouse and sends a message to the OS, tricking it into thinking the user
        /// is clicking on the title bar, which allows the borderless form to be moved.
        /// </remarks>
        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        /// <summary>
        /// An unused event handler likely generated by the Windows Forms Designer.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method is not wired to any event in the code and can be safely ignored or removed.
        /// The primary handler for the login button is <see cref="LoginButton_Click"/>.
        /// </remarks>
        private void LoginButton_Click_1(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// An unused event handler for the main title label's click event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        private void MainTitleLabel_Click(object sender, EventArgs e)
        {
        }
    }
}
