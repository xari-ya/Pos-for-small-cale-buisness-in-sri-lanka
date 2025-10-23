namespace billing_system
{
    partial class Login
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            HeaderPanel = new Panel();
            TitleLabel = new Label();
            MinimizeButton = new Button();
            CloseButton = new Button();
            LogoPanel = new Panel();
            MainTitleLabel = new Label();
            SubtitleLabel = new Label();
            UsernameLabel = new Label();
            PasswordLabel = new Label();
            LoginButton = new Button();
            ErrorMessageLabel = new Label();
            UsernamePanel = new Panel();
            UsernameTextBox = new TextBox();
            PasswordPanel = new Panel();
            PasswordTextBox = new TextBox();
            HeaderPanel.SuspendLayout();
            UsernamePanel.SuspendLayout();
            PasswordPanel.SuspendLayout();
            SuspendLayout();
            // 
            // HeaderPanel
            // 
            HeaderPanel.BackColor = Color.FromArgb(44, 62, 80);
            HeaderPanel.Controls.Add(TitleLabel);
            HeaderPanel.Controls.Add(MinimizeButton);
            HeaderPanel.Controls.Add(CloseButton);
            HeaderPanel.Dock = DockStyle.Top;
            HeaderPanel.Location = new Point(0, 0);
            HeaderPanel.Margin = new Padding(6, 7, 6, 7);
            HeaderPanel.Name = "HeaderPanel";
            HeaderPanel.Size = new Size(800, 92);
            HeaderPanel.TabIndex = 10;
            // 
            // TitleLabel
            // 
            TitleLabel.AutoSize = true;
            TitleLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TitleLabel.ForeColor = Color.White;
            TitleLabel.Location = new Point(30, 21);
            TitleLabel.Margin = new Padding(6, 0, 6, 0);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new Size(178, 38);
            TitleLabel.TabIndex = 2;
            TitleLabel.Text = "BIMS – Login";
            // 
            // MinimizeButton
            // 
            MinimizeButton.Dock = DockStyle.Right;
            MinimizeButton.FlatAppearance.BorderSize = 0;
            MinimizeButton.FlatStyle = FlatStyle.Flat;
            MinimizeButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            MinimizeButton.ForeColor = Color.White;
            MinimizeButton.Location = new Point(680, 0);
            MinimizeButton.Margin = new Padding(6, 7, 6, 7);
            MinimizeButton.Name = "MinimizeButton";
            MinimizeButton.Size = new Size(60, 92);
            MinimizeButton.TabIndex = 12;
            MinimizeButton.Text = "—";
            MinimizeButton.UseVisualStyleBackColor = true;
            // 
            // CloseButton
            // 
            CloseButton.Dock = DockStyle.Right;
            CloseButton.FlatAppearance.BorderSize = 0;
            CloseButton.FlatStyle = FlatStyle.Flat;
            CloseButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CloseButton.ForeColor = Color.White;
            CloseButton.Location = new Point(740, 0);
            CloseButton.Margin = new Padding(6, 7, 6, 7);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(60, 92);
            CloseButton.TabIndex = 13;
            CloseButton.Text = "X";
            CloseButton.UseVisualStyleBackColor = true;
            // 
            // LogoPanel
            // 
            LogoPanel.BackColor = Color.FromArgb(44, 62, 80);
            LogoPanel.Location = new Point(320, 150);
            LogoPanel.Margin = new Padding(6, 7, 6, 7);
            LogoPanel.Name = "LogoPanel";
            LogoPanel.Size = new Size(160, 185);
            LogoPanel.TabIndex = 11;
            // 
            // MainTitleLabel
            // 
            MainTitleLabel.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            MainTitleLabel.ForeColor = Color.FromArgb(52, 73, 94);
            MainTitleLabel.Location = new Point(0, 358);
            MainTitleLabel.Margin = new Padding(6, 0, 6, 0);
            MainTitleLabel.Name = "MainTitleLabel";
            MainTitleLabel.Size = new Size(800, 92);
            MainTitleLabel.TabIndex = 14;
            MainTitleLabel.Text = "Billing && Inventory Management";
            MainTitleLabel.TextAlign = ContentAlignment.MiddleCenter;
            MainTitleLabel.Click += MainTitleLabel_Click;
            // 
            // SubtitleLabel
            // 
            SubtitleLabel.Font = new Font("Segoe UI", 11F);
            SubtitleLabel.ForeColor = Color.FromArgb(127, 140, 141);
            SubtitleLabel.Location = new Point(0, 450);
            SubtitleLabel.Margin = new Padding(6, 0, 6, 0);
            SubtitleLabel.Name = "SubtitleLabel";
            SubtitleLabel.Size = new Size(800, 46);
            SubtitleLabel.TabIndex = 15;
            SubtitleLabel.Text = "Please log in to continue";
            SubtitleLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // UsernameLabel
            // 
            UsernameLabel.AutoSize = true;
            UsernameLabel.Font = new Font("Segoe UI", 10F);
            UsernameLabel.ForeColor = Color.FromArgb(52, 73, 94);
            UsernameLabel.Location = new Point(76, 554);
            UsernameLabel.Margin = new Padding(6, 0, 6, 0);
            UsernameLabel.Name = "UsernameLabel";
            UsernameLabel.Size = new Size(121, 32);
            UsernameLabel.TabIndex = 16;
            UsernameLabel.Text = "Username";
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Font = new Font("Segoe UI", 10F);
            PasswordLabel.ForeColor = Color.FromArgb(52, 73, 94);
            PasswordLabel.Location = new Point(76, 738);
            PasswordLabel.Margin = new Padding(6, 0, 6, 0);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(111, 32);
            PasswordLabel.TabIndex = 17;
            PasswordLabel.Text = "Password";
            // 
            // LoginButton
            // 
            LoginButton.BackColor = Color.FromArgb(46, 204, 113);
            LoginButton.Cursor = Cursors.Hand;
            LoginButton.FlatAppearance.BorderSize = 0;
            LoginButton.FlatStyle = FlatStyle.Flat;
            LoginButton.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            LoginButton.ForeColor = Color.White;
            LoginButton.Location = new Point(84, 992);
            LoginButton.Margin = new Padding(6, 7, 6, 7);
            LoginButton.Name = "LoginButton";
            LoginButton.Size = new Size(632, 104);
            LoginButton.TabIndex = 2;
            LoginButton.Text = "LOGIN";
            LoginButton.UseVisualStyleBackColor = false;
            LoginButton.Click += LoginButton_Click_1;
            // 
            // ErrorMessageLabel
            // 
            ErrorMessageLabel.Font = new Font("Segoe UI", 9F);
            ErrorMessageLabel.ForeColor = Color.FromArgb(231, 76, 60);
            ErrorMessageLabel.Location = new Point(0, 912);
            ErrorMessageLabel.Margin = new Padding(6, 0, 6, 0);
            ErrorMessageLabel.Name = "ErrorMessageLabel";
            ErrorMessageLabel.Size = new Size(800, 46);
            ErrorMessageLabel.TabIndex = 18;
            ErrorMessageLabel.Text = "Invalid username or password.";
            ErrorMessageLabel.TextAlign = ContentAlignment.MiddleCenter;
            ErrorMessageLabel.Visible = false;
            // 
            // UsernamePanel
            // 
            UsernamePanel.BackColor = Color.White;
            UsernamePanel.Controls.Add(UsernameTextBox);
            UsernamePanel.Location = new Point(84, 612);
            UsernamePanel.Margin = new Padding(6, 7, 6, 7);
            UsernamePanel.Name = "UsernamePanel";
            UsernamePanel.Padding = new Padding(2);
            UsernamePanel.Size = new Size(632, 92);
            UsernamePanel.TabIndex = 0;
            // 
            // UsernameTextBox
            // 
            UsernameTextBox.BorderStyle = BorderStyle.None;
            UsernameTextBox.Dock = DockStyle.Fill;
            UsernameTextBox.Font = new Font("Segoe UI", 12F);
            UsernameTextBox.Location = new Point(2, 2);
            UsernameTextBox.Margin = new Padding(6, 7, 6, 7);
            UsernameTextBox.Multiline = true;
            UsernameTextBox.Name = "UsernameTextBox";
            UsernameTextBox.Size = new Size(628, 88);
            UsernameTextBox.TabIndex = 0;
            // 
            // PasswordPanel
            // 
            PasswordPanel.BackColor = Color.White;
            PasswordPanel.Controls.Add(PasswordTextBox);
            PasswordPanel.Location = new Point(84, 796);
            PasswordPanel.Margin = new Padding(6, 7, 6, 7);
            PasswordPanel.Name = "PasswordPanel";
            PasswordPanel.Padding = new Padding(2);
            PasswordPanel.Size = new Size(632, 92);
            PasswordPanel.TabIndex = 1;
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.BorderStyle = BorderStyle.None;
            PasswordTextBox.Dock = DockStyle.Fill;
            PasswordTextBox.Font = new Font("Segoe UI", 12F);
            PasswordTextBox.Location = new Point(2, 2);
            PasswordTextBox.Margin = new Padding(6, 7, 6, 7);
            PasswordTextBox.Multiline = true;
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.Size = new Size(628, 88);
            PasswordTextBox.TabIndex = 0;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 246, 247);
            ClientSize = new Size(800, 1269);
            Controls.Add(PasswordPanel);
            Controls.Add(UsernamePanel);
            Controls.Add(ErrorMessageLabel);
            Controls.Add(LoginButton);
            Controls.Add(PasswordLabel);
            Controls.Add(UsernameLabel);
            Controls.Add(SubtitleLabel);
            Controls.Add(MainTitleLabel);
            Controls.Add(LogoPanel);
            Controls.Add(HeaderPanel);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(6, 7, 6, 7);
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LoginForm";
            HeaderPanel.ResumeLayout(false);
            HeaderPanel.PerformLayout();
            UsernamePanel.ResumeLayout(false);
            UsernamePanel.PerformLayout();
            PasswordPanel.ResumeLayout(false);
            PasswordPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Button MinimizeButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Panel LogoPanel;
        private System.Windows.Forms.Label MainTitleLabel;
        private System.Windows.Forms.Label SubtitleLabel;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.Label ErrorMessageLabel;
        private System.Windows.Forms.Panel UsernamePanel;
        private System.Windows.Forms.TextBox UsernameTextBox;
        private System.Windows.Forms.Panel PasswordPanel;
        private System.Windows.Forms.TextBox PasswordTextBox;
    }
}