using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace billing_system
{
    /// <summary>
    /// Represents the main application shell for administrators.
    /// </summary>
    /// <remarks>
    /// This form serves as the primary interface for users with the "Admin" role. It features a navigation panel
    /// on the left to switch between different management views, which are loaded as <see cref="UserControl"/> instances
    /// into a central content panel. It provides access to the dashboard, inventory, user management, and reports.
    /// </remarks>
    public partial class MainShellForm : Form
    {
        private readonly Color NavBg = Color.FromArgb(45, 52, 71);
        private readonly Color HoverBg = Color.FromArgb(59, 68, 92);
        private readonly Color ActiveBg = Color.FromArgb(41, 213, 146);

        private Panel _activeButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainShellForm"/> class.
        /// </summary>
        /// <remarks>
        /// The constructor initializes the form's components and sets the initial state, with the Dashboard view
        /// loaded by default.
        /// </remarks>
        public MainShellForm()
        {
            InitializeComponent();

            // Default: mark Dashboard active and load it
            SetActive(btnDashboard);
            this.Load += (s, e) => btnDashboard_Click(s, e);
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
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        /// <summary>
        /// Releases the mouse capture from a window in the current thread.
        /// </summary>
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        /// <summary>
        /// Handles the MouseDown event for the navigation panel to enable window dragging.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This allows the borderless form to be moved by clicking and dragging the empty space in the navigation panel.
        /// </remarks>
        private void navPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender == navPanel && e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        /// <summary>
        /// Handles the MouseEnter event for navigation buttons to provide visual feedback.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// Changes the background color of the button's container panel to a hover color, unless it's the currently active button.
        /// </remarks>
        private void NavButton_MouseEnter(object sender, EventArgs e)
        {
            Panel container = GetContainerPanel(sender);
            if (container != _activeButton)
                container.BackColor = HoverBg;
        }

        /// <summary>
        /// Handles the MouseLeave event for navigation buttons to remove visual feedback.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// Reverts the background color of the button's container panel to the default, unless it's the currently active button.
        /// </remarks>
        private void NavButton_MouseLeave(object sender, EventArgs e)
        {
            Panel container = GetContainerPanel(sender);
            if (container != _activeButton)
                container.BackColor = NavBg;
        }

        /// <summary>
        /// Gets the container panel for a given control.
        /// </summary>
        /// <param name="sender">The control whose container panel is to be found.</param>
        /// <returns>The container <see cref="Panel"/>.</returns>
        /// <remarks>
        /// This helper method is used to find the parent panel of a navigation button, which is used for hover and active state effects.
        /// </remarks>
        private Panel GetContainerPanel(object sender)
        {
            if (sender is Panel p) return p;
            if (sender is Control c && c.Parent is Panel pp) return pp;
            return btnDashboard;
        }

        /// <summary>
        /// Sets the visual state of a navigation button to active.
        /// </summary>
        /// <param name="panel">The panel of the navigation button to be set as active.</param>
        /// <remarks>
        /// This method updates the background colors to visually distinguish the active navigation option.
        /// It also keeps track of the currently active button.
        /// </remarks>
        private void SetActive(Panel panel)
        {
            if (_activeButton != null && !_activeButton.IsDisposed)
                _activeButton.BackColor = NavBg;

            _activeButton = panel;
            _activeButton.BackColor = ActiveBg;
        }

        /// <summary>
        /// Loads a <see cref="UserControl"/> into the main content area.
        /// </summary>
        /// <param name="userControl">The user control to load.</param>
        /// <remarks>
        /// This method clears the content panel and adds the specified user control, docking it to fill the available space.
        /// </remarks>
        private void LoadUserControl(UserControl userControl)
        {
            if (userControl == null) return;

            this.contentPanel.SuspendLayout();
            this.contentPanel.Controls.Clear();

            userControl.Dock = DockStyle.Fill;
            this.contentPanel.Controls.Add(userControl);

            this.contentPanel.ResumeLayout();
        }

        /// <summary>
        /// Handles the Click event for the Dashboard navigation button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// Sets the Dashboard button as active and loads the <see cref="DashboardControl"/> into the content panel.
        /// </remarks>
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            SetActive(btnDashboard);
            LoadUserControl(new DashboardControl());
        }

        /// <summary>
        /// Handles the Click event for the Inventory navigation button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// Sets the Inventory button as active and loads the <see cref="InventoryControl"/> into the content panel.
        /// </remarks>
        private void btnInventory_Click(object sender, EventArgs e)
        {
            SetActive(btnInventory);
            LoadUserControl(new InventoryControl());
        }

        /// <summary>
        /// Handles the Click event for the Users navigation button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// Sets the Users button as active and loads the <see cref="UserManagementControl"/> into the content panel.
        /// </remarks>
        private void btnUsers_Click(object sender, EventArgs e)
        {
            SetActive(btnUsers);
            LoadUserControl(new UserManagementControl());
        }

        /// <summary>
        /// Handles the Click event for the Reports navigation button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// Sets the Reports button as active and loads the <see cref="ReportsControl"/> into the content panel.
        /// </remarks>
        private void btnReports_Click(object sender, EventArgs e)
        {
            SetActive(btnReports);
            LoadUserControl(new ReportsControl());
        }

        /// <summary>
        /// Handles the Click event for the Logout button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method prompts the user for confirmation, and if confirmed, logs out the current user via the
        /// <see cref="AuthService"/> and displays the <see cref="Login"/> form.
        /// </remarks>
        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to log out?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var authService = new AuthService();
                authService.Logout();

                this.Hide();

                var loginForm = new Login();
                loginForm.Show();
            }
        }
    }
}
