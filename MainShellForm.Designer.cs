// MainShellForm.Designer.cs
using System.Drawing;
using System.Windows.Forms;

namespace billing_system
{
    partial class MainShellForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel navPanel;
        private Label lblAppTitle;
        private Panel btnDashboard;
        private PictureBox iconDashboard;
        private Label lblDashboard;

        private Panel btnInventory;
        private PictureBox iconInventory;
        private Label lblInventory;

        private Panel btnUsers;
        private PictureBox iconUsers;
        private Label lblUsers;

        private Panel btnReports;
        private PictureBox iconReports;
        private Label lblReports;

        private Panel btnLogout;
        private PictureBox iconLogout;
        private Label lblLogout;

        private Panel contentPanel;
        private Panel navTopSpacer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            this.navPanel = new Panel();
            this.lblAppTitle = new Label();

            this.btnDashboard = new Panel();
            this.iconDashboard = new PictureBox();
            this.lblDashboard = new Label();

            this.btnInventory = new Panel();
            this.iconInventory = new PictureBox();
            this.lblInventory = new Label();

            this.btnUsers = new Panel();
            this.iconUsers = new PictureBox();
            this.lblUsers = new Label();

            this.btnReports = new Panel();
            this.iconReports = new PictureBox();
            this.lblReports = new Label();

            this.btnLogout = new Panel();
            this.iconLogout = new PictureBox();
            this.lblLogout = new Label();

            this.contentPanel = new Panel();
            this.navTopSpacer = new Panel();

            // 
            // MainShellForm (Form)
            // 
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1200, 700);
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MinimumSize = new Size(900, 600);
            this.Name = "MainShellForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "BIMS";
            this.WindowState = FormWindowState.Maximized;


            // 
            // navPanel
            // 
            this.navPanel.BackColor = Color.FromArgb(45, 52, 71);
            this.navPanel.Dock = DockStyle.Left;
            this.navPanel.Width = 250;
            this.navPanel.Name = "navPanel";

            // 
            // lblAppTitle
            // 
            this.lblAppTitle.Text = "BIMS";
            this.lblAppTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblAppTitle.ForeColor = Color.White;
            this.lblAppTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.lblAppTitle.Dock = DockStyle.Top;
            this.lblAppTitle.Height = 90;
            this.lblAppTitle.Name = "lblAppTitle";

            // 
            // navTopSpacer
            // 
            this.navTopSpacer.Dock = DockStyle.Top;
            this.navTopSpacer.Height = 10;
            this.navTopSpacer.BackColor = Color.FromArgb(45, 52, 71);

            //
            // btnDashboard
            //
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Height = 55;
            this.btnDashboard.Dock = DockStyle.Top;
            this.btnDashboard.BackColor = Color.FromArgb(45, 52, 71);
            this.btnDashboard.Padding = new Padding(20, 0, 20, 0);
            this.btnDashboard.Cursor = Cursors.Hand;

            // iconDashboard
            this.iconDashboard.Name = "iconDashboard";
            this.iconDashboard.Size = new Size(24, 24);
            this.iconDashboard.Location = new Point(20, 16);
            this.iconDashboard.SizeMode = PictureBoxSizeMode.CenterImage;
            this.iconDashboard.BackColor = Color.Transparent; // placeholder (no image)

            // lblDashboard
            this.lblDashboard.Name = "lblDashboard";
            this.lblDashboard.Text = "Dashboard";
            this.lblDashboard.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblDashboard.ForeColor = Color.White;
            this.lblDashboard.AutoSize = false;
            this.lblDashboard.TextAlign = ContentAlignment.MiddleLeft;
            this.lblDashboard.Location = new Point(60, 0);
            this.lblDashboard.Size = new Size(170, 55);
            this.lblDashboard.BackColor = Color.Transparent;

            this.btnDashboard.Controls.Add(this.iconDashboard);
            this.btnDashboard.Controls.Add(this.lblDashboard);

            //
            // btnInventory
            //
            this.btnInventory.Name = "btnInventory";
            this.btnInventory.Height = 55;
            this.btnInventory.Dock = DockStyle.Top;
            this.btnInventory.BackColor = Color.FromArgb(45, 52, 71);
            this.btnInventory.Padding = new Padding(20, 0, 20, 0);
            this.btnInventory.Cursor = Cursors.Hand;

            // iconInventory
            this.iconInventory.Name = "iconInventory";
            this.iconInventory.Size = new Size(24, 24);
            this.iconInventory.Location = new Point(20, 16);
            this.iconInventory.SizeMode = PictureBoxSizeMode.CenterImage;
            this.iconInventory.BackColor = Color.Transparent;

            // lblInventory
            this.lblInventory.Name = "lblInventory";
            this.lblInventory.Text = "Inventory";
            this.lblInventory.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblInventory.ForeColor = Color.White;
            this.lblInventory.AutoSize = false;
            this.lblInventory.TextAlign = ContentAlignment.MiddleLeft;
            this.lblInventory.Location = new Point(60, 0);
            this.lblInventory.Size = new Size(170, 55);
            this.lblInventory.BackColor = Color.Transparent;

            this.btnInventory.Controls.Add(this.iconInventory);
            this.btnInventory.Controls.Add(this.lblInventory);

            //
            // btnUsers
            //
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Height = 55;
            this.btnUsers.Dock = DockStyle.Top;
            this.btnUsers.BackColor = Color.FromArgb(45, 52, 71);
            this.btnUsers.Padding = new Padding(20, 0, 20, 0);
            this.btnUsers.Cursor = Cursors.Hand;

            // iconUsers
            this.iconUsers.Name = "iconUsers";
            this.iconUsers.Size = new Size(24, 24);
            this.iconUsers.Location = new Point(20, 16);
            this.iconUsers.SizeMode = PictureBoxSizeMode.CenterImage;
            this.iconUsers.BackColor = Color.Transparent;

            // lblUsers
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Text = "Users";
            this.lblUsers.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblUsers.ForeColor = Color.White;
            this.lblUsers.AutoSize = false;
            this.lblUsers.TextAlign = ContentAlignment.MiddleLeft;
            this.lblUsers.Location = new Point(60, 0);
            this.lblUsers.Size = new Size(170, 55);
            this.lblUsers.BackColor = Color.Transparent;

            this.btnUsers.Controls.Add(this.iconUsers);
            this.btnUsers.Controls.Add(this.lblUsers);

            //
            // btnReports
            //
            this.btnReports.Name = "btnReports";
            this.btnReports.Height = 55;
            this.btnReports.Dock = DockStyle.Top;
            this.btnReports.BackColor = Color.FromArgb(45, 52, 71);
            this.btnReports.Padding = new Padding(20, 0, 20, 0);
            this.btnReports.Cursor = Cursors.Hand;

            // iconReports
            this.iconReports.Name = "iconReports";
            this.iconReports.Size = new Size(24, 24);
            this.iconReports.Location = new Point(20, 16);
            this.iconReports.SizeMode = PictureBoxSizeMode.CenterImage;
            this.iconReports.BackColor = Color.Transparent;

            // lblReports
            this.lblReports.Name = "lblReports";
            this.lblReports.Text = "Reports";
            this.lblReports.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblReports.ForeColor = Color.White;
            this.lblReports.AutoSize = false;
            this.lblReports.TextAlign = ContentAlignment.MiddleLeft;
            this.lblReports.Location = new Point(60, 0);
            this.lblReports.Size = new Size(170, 55);
            this.lblReports.BackColor = Color.Transparent;

            this.btnReports.Controls.Add(this.iconReports);
            this.btnReports.Controls.Add(this.lblReports);

            //
            // btnLogout
            //
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Height = 55;
            this.btnLogout.Dock = DockStyle.Bottom;
            this.btnLogout.BackColor = Color.FromArgb(45, 52, 71);
            this.btnLogout.Padding = new Padding(20, 0, 20, 0);
            this.btnLogout.Cursor = Cursors.Hand;

            // iconLogout
            this.iconLogout.Name = "iconLogout";
            this.iconLogout.Size = new Size(24, 24);
            this.iconLogout.Location = new Point(20, 16);
            this.iconLogout.SizeMode = PictureBoxSizeMode.CenterImage;
            this.iconLogout.BackColor = Color.Transparent;

            // lblLogout
            this.lblLogout.Name = "lblLogout";
            this.lblLogout.Text = "Logout";
            this.lblLogout.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblLogout.ForeColor = Color.White;
            this.lblLogout.AutoSize = false;
            this.lblLogout.TextAlign = ContentAlignment.MiddleLeft;
            this.lblLogout.Location = new Point(60, 0);
            this.lblLogout.Size = new Size(170, 55);
            this.lblLogout.BackColor = Color.Transparent;

            this.btnLogout.Controls.Add(this.iconLogout);
            this.btnLogout.Controls.Add(this.lblLogout);

            // 
            // contentPanel
            // 
            this.contentPanel.Dock = DockStyle.Fill;
            this.contentPanel.BackColor = Color.FromArgb(245, 246, 250);
            this.contentPanel.Name = "contentPanel";

            // --- Build navigation stack order ---
            // top area
            this.navPanel.Controls.Add(this.btnReports);
            this.navPanel.Controls.Add(this.btnUsers);
            this.navPanel.Controls.Add(this.btnInventory);
            this.navPanel.Controls.Add(this.btnDashboard);
            this.navPanel.Controls.Add(this.navTopSpacer);
            this.navPanel.Controls.Add(this.lblAppTitle);
            // bottom
            this.navPanel.Controls.Add(this.btnLogout);

            // 
            // wire events (only assignments; logic lives in .cs)
            // 
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            this.lblDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            this.iconDashboard.Click += new System.EventHandler(this.btnDashboard_Click);

            this.btnInventory.Click += new System.EventHandler(this.btnInventory_Click);
            this.lblInventory.Click += new System.EventHandler(this.btnInventory_Click);
            this.iconInventory.Click += new System.EventHandler(this.btnInventory_Click);

            this.btnUsers.Click += new System.EventHandler(this.btnUsers_Click);
            this.lblUsers.Click += new System.EventHandler(this.btnUsers_Click);
            this.iconUsers.Click += new System.EventHandler(this.btnUsers_Click);

            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            this.lblReports.Click += new System.EventHandler(this.btnReports_Click);
            this.iconReports.Click += new System.EventHandler(this.btnReports_Click);

            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            this.lblLogout.Click += new System.EventHandler(this.btnLogout_Click);
            this.iconLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // Hover handlers (assigned here; implemented in code-behind)
            this.btnDashboard.MouseEnter += new System.EventHandler(this.NavButton_MouseEnter);
            this.btnDashboard.MouseLeave += new System.EventHandler(this.NavButton_MouseLeave);
            this.lblDashboard.MouseEnter += new System.EventHandler(this.NavButton_MouseEnter);
            this.lblDashboard.MouseLeave += new System.EventHandler(this.NavButton_MouseLeave);
            this.iconDashboard.MouseEnter += new System.EventHandler(this.NavButton_MouseEnter);
            this.iconDashboard.MouseLeave += new System.EventHandler(this.NavButton_MouseLeave);

            this.btnInventory.MouseEnter += new System.EventHandler(this.NavButton_MouseEnter);
            this.btnInventory.MouseLeave += new System.EventHandler(this.NavButton_MouseLeave);
            this.lblInventory.MouseEnter += new System.EventHandler(this.NavButton_MouseEnter);
            this.lblInventory.MouseLeave += new System.EventHandler(this.NavButton_MouseLeave);
            this.iconInventory.MouseEnter += new System.EventHandler(this.NavButton_MouseEnter);
            this.iconInventory.MouseLeave += new System.EventHandler(this.NavButton_MouseLeave);

            this.btnUsers.MouseEnter += new System.EventHandler(this.NavButton_MouseEnter);
            this.btnUsers.MouseLeave += new System.EventHandler(this.NavButton_MouseLeave);
            this.lblUsers.MouseEnter += new System.EventHandler(this.NavButton_MouseEnter);
            this.lblUsers.MouseLeave += new System.EventHandler(this.NavButton_MouseLeave);
            this.iconUsers.MouseEnter += new System.EventHandler(this.NavButton_MouseEnter);
            this.iconUsers.MouseLeave += new System.EventHandler(this.NavButton_MouseLeave);

            this.btnReports.MouseEnter += new System.EventHandler(this.NavButton_MouseEnter);
            this.btnReports.MouseLeave += new System.EventHandler(this.NavButton_MouseLeave);
            this.lblReports.MouseEnter += new System.EventHandler(this.NavButton_MouseEnter);
            this.lblReports.MouseLeave += new System.EventHandler(this.NavButton_MouseLeave);
            this.iconReports.MouseEnter += new System.EventHandler(this.NavButton_MouseEnter);
            this.iconReports.MouseLeave += new System.EventHandler(this.NavButton_MouseLeave);

            this.btnLogout.MouseEnter += new System.EventHandler(this.NavButton_MouseEnter);
            this.btnLogout.MouseLeave += new System.EventHandler(this.NavButton_MouseLeave);
            this.lblLogout.MouseEnter += new System.EventHandler(this.NavButton_MouseEnter);
            this.lblLogout.MouseLeave += new System.EventHandler(this.NavButton_MouseLeave);
            this.iconLogout.MouseEnter += new System.EventHandler(this.NavButton_MouseEnter);
            this.iconLogout.MouseLeave += new System.EventHandler(this.NavButton_MouseLeave);

            // Allow dragging from empty space on the left panel
            this.navPanel.MouseDown += new MouseEventHandler(this.navPanel_MouseDown);

            // 
            // Add to Form
            // 
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.navPanel);
        }

        #endregion
    }
}
