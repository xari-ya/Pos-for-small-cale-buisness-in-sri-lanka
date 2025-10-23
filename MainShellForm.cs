// MainShellForm.cs
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace billing_system
{
    public partial class MainShellForm : Form
    {
        private readonly Color NavBg = Color.FromArgb(45, 52, 71);
        private readonly Color HoverBg = Color.FromArgb(59, 68, 92);
        private readonly Color ActiveBg = Color.FromArgb(41, 213, 146);

        private Panel _activeButton;

        public MainShellForm()
        {
            InitializeComponent();

            // Default: mark Dashboard active and load it
            SetActive(btnDashboard);
            this.Load += (s, e) => btnDashboard_Click(s, e);
        }

        // --- Borderless window dragging from empty navPanel area ---
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private void navPanel_MouseDown(object sender, MouseEventArgs e)
        {
            // Only when clicking empty space on navPanel itself (not child buttons)
            if (sender == navPanel && e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        // --- Hover effects for all nav panels & their children ---
        private void NavButton_MouseEnter(object sender, EventArgs e)
        {
            Panel container = GetContainerPanel(sender);
            if (container != _activeButton)
                container.BackColor = HoverBg;
        }

        private void NavButton_MouseLeave(object sender, EventArgs e)
        {
            Panel container = GetContainerPanel(sender);
            if (container != _activeButton)
                container.BackColor = NavBg;
        }

        private Panel GetContainerPanel(object sender)
        {
            if (sender is Panel p) return p;
            if (sender is Control c && c.Parent is Panel pp) return pp;
            return btnDashboard;
        }

        private void SetActive(Panel panel)
        {
            if (_activeButton != null && !_activeButton.IsDisposed)
                _activeButton.BackColor = NavBg;

            _activeButton = panel;
            _activeButton.BackColor = ActiveBg;
        }

        // --- Content host helper ---
        private void LoadUserControl(UserControl userControl)
        {
            if (userControl == null) return;

            this.contentPanel.SuspendLayout();
            this.contentPanel.Controls.Clear();

            userControl.Dock = DockStyle.Fill;
            this.contentPanel.Controls.Add(userControl);

            this.contentPanel.ResumeLayout();
        }

        // --- Navigation Clicks ---
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            SetActive(btnDashboard);
            LoadUserControl(new DashboardControl());
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            SetActive(btnInventory);
            LoadUserControl(new InventoryControl());
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            SetActive(btnUsers);
            LoadUserControl(new UserManagementControl());
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            SetActive(btnReports);
            LoadUserControl(new ReportsControl());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to log out?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                // Assume AuthService and Login form exist
                var authService = new AuthService();
                authService.Logout();

                this.Hide();

                var loginForm = new Login();
                loginForm.Show();
            }
        }
    }
}
