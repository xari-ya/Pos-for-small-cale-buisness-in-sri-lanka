// UserDetailForm.cs
using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace billing_system
{
    public partial class UserDetailForm : Form
    {
        public enum Mode { Add, Edit }

        private readonly Mode _mode;
        private readonly UserRepository _repo = new UserRepository();
        private readonly int _userId;
        private bool _resetPasswordRequested = false;

        public UserDetailForm(Mode mode = Mode.Add)
        {
            _mode = mode;
            InitializeComponent();

            // Dialog behavior
            this.AcceptButton = this.btnSave;
            this.CancelButton = this.btnCancel;
            this.KeyPreview = true;

            EnsureRoleItems();
            InitUiForMode();

            // Wire events
            this.btnSave.Click += btnSave_Click;
            this.btnCancel.Click += btnCancel_Click;
            this.btnResetPassword.Click += btnResetPassword_Click;
        }

        public UserDetailForm(Mode mode, int userId, string username, string fullName, string role, bool isActive, DateTime createdAt)
            : this(mode)
        {
            _userId = userId;

            this.txtUsername.Text = username ?? string.Empty;
            this.txtFullName.Text = fullName ?? string.Empty;

            var normalizedRole = (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase)) ? "Admin" : "Cashier";
            if (this.cmbRole.Items.Count == 0) EnsureRoleItems();
            this.cmbRole.SelectedItem = normalizedRole;

            this.chkIsActive.Checked = isActive;
        }

        private void EnsureRoleItems()
        {
            if (this.cmbRole.Items.Count == 0)
            {
                this.cmbRole.Items.Add("Admin");
                this.cmbRole.Items.Add("Cashier");
            }
            if (this.cmbRole.SelectedIndex < 0)
                this.cmbRole.SelectedItem = "Cashier";
        }

        private void InitUiForMode()
        {
            this.Text = (_mode == Mode.Add) ? "Add New User" : "Edit User";
            this.lblTitle.Text = "User Management";

            if (_mode == Mode.Add)
            {
                this.txtPassword.Enabled = true;
                this.txtConfirmPassword.Enabled = true;
                this.txtPassword.Text = string.Empty;
                this.txtConfirmPassword.Text = string.Empty;

                // 👇 ADD THIS LINE HERE
                this.btnResetPassword.Visible = false;
            }
            else
            {
                this.txtPassword.Enabled = false;
                this.txtConfirmPassword.Enabled = false;
                this.txtPassword.Text = string.Empty;
                this.txtConfirmPassword.Text = string.Empty;

                // 👇 AND THIS LINE HERE
                this.btnResetPassword.Visible = true;

                _resetPasswordRequested = false;
            }
        }


        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            // Allow entering a new password when editing
            _resetPasswordRequested = true;
            this.txtPassword.Enabled = true;
            this.txtConfirmPassword.Enabled = true;
            this.txtPassword.Text = string.Empty;
            this.txtConfirmPassword.Text = string.Empty;
            this.txtPassword.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var username = (this.txtUsername.Text ?? string.Empty).Trim();
                var fullName = (this.txtFullName.Text ?? string.Empty).Trim();
                var role = (this.cmbRole.SelectedItem?.ToString() ?? "Cashier");
                var isActive = this.chkIsActive.Checked;

                if (string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show("Username is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(fullName))
                {
                    MessageBox.Show("Full Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
                    role = "Cashier";

                if (_mode == Mode.Add)
                {
                    var pwd = this.txtPassword.Text;
                    var confirm = this.txtConfirmPassword.Text;

                    if (string.IsNullOrWhiteSpace(pwd))
                    {
                        MessageBox.Show("Password is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (!string.Equals(pwd, confirm, StringComparison.Ordinal))
                    {
                        MessageBox.Show("Passwords do not match.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (_repo.UsernameExists(username))
                    {
                        MessageBox.Show("Username already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _repo.CreateUser(username, pwd, fullName, role, isActive);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
                else
                {
                    // Update basic details
                    _repo.UpdateUser(_userId, username, fullName, role, isActive);

                    // If reset was requested, validate & update password
                    if (_resetPasswordRequested)
                    {
                        var pwd = this.txtPassword.Text;
                        var confirm = this.txtConfirmPassword.Text;

                        if (string.IsNullOrWhiteSpace(pwd))
                        {
                            MessageBox.Show("New password is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (!string.Equals(pwd, confirm, StringComparison.Ordinal))
                        {
                            MessageBox.Show("Passwords do not match.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        _repo.UpdatePassword(_userId, pwd);
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("exists"))
            {
                MessageBox.Show("Username already exists. Choose a different username.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SQLiteException ex) when (ex.ResultCode == SQLiteErrorCode.Constraint || ex.Message.Contains("UNIQUE"))
            {
                MessageBox.Show("Username already exists. Choose a different username.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Save failed:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
