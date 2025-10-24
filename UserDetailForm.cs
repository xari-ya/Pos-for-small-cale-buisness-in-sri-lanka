using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace billing_system
{
    /// <summary>
    /// Represents a form for adding a new user or editing an existing one.
    /// </summary>
    /// <remarks>
    /// This form is used by the <see cref="UserManagementControl"/> to manage user details. It operates in two modes,
    /// defined by the <see cref="Mode"/> enum, to handle both the creation and modification of user accounts.
    /// </remarks>
    public partial class UserDetailForm : Form
    {
        /// <summary>
        /// Defines the operational mode of the form (Add or Edit).
        /// </summary>
        public enum Mode { Add, Edit }

        private readonly Mode _mode;
        private readonly UserRepository _repo = new UserRepository();
        private readonly int _userId;
        private bool _resetPasswordRequested = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDetailForm"/> class in a specified mode.
        /// </summary>
        /// <param name="mode">The operational mode of the form (Add or Edit).</param>
        public UserDetailForm(Mode mode = Mode.Add)
        {
            _mode = mode;
            InitializeComponent();
            this.AcceptButton = this.btnSave;
            this.CancelButton = this.btnCancel;
            this.KeyPreview = true;
            EnsureRoleItems();
            InitUiForMode();
            this.btnSave.Click += btnSave_Click;
            this.btnCancel.Click += btnCancel_Click;
            this.btnResetPassword.Click += btnResetPassword_Click;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDetailForm"/> class for editing an existing user.
        /// </summary>
        /// <param name="mode">The operational mode, which should be 'Edit'.</param>
        /// <param name="userId">The ID of the user to edit.</param>
        /// <param name="username">The current username of the user.</param>
        /// <param name="fullName">The current full name of the user.</param>
        /// <param name="role">The current role of the user.</param>
        /// <param name="isActive">The current active status of the user.</param>
        /// <param name="createdAt">The creation date of the user's account.</param>
        public UserDetailForm(Mode mode, int userId, string username, string fullName, string role, bool isActive, DateTime createdAt) : this(mode)
        {
            _userId = userId;
            this.txtUsername.Text = username ?? string.Empty;
            this.txtFullName.Text = fullName ?? string.Empty;
            var normalizedRole = (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase)) ? "Admin" : "Cashier";
            if (this.cmbRole.Items.Count == 0) EnsureRoleItems();
            this.cmbRole.SelectedItem = normalizedRole;
            this.chkIsActive.Checked = isActive;
        }

        /// <summary>
        /// Ensures that the role combo box is populated with the available roles.
        /// </summary>
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

        /// <summary>
        /// Initializes the UI elements based on the current mode (Add or Edit).
        /// </summary>
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
                this.btnResetPassword.Visible = false;
            }
            else
            {
                this.txtPassword.Enabled = false;
                this.txtConfirmPassword.Enabled = false;
                this.txtPassword.Text = string.Empty;
                this.txtConfirmPassword.Text = string.Empty;
                this.btnResetPassword.Visible = true;
                _resetPasswordRequested = false;
            }
        }

        /// <summary>
        /// Handles the Click event for the "Reset Password" button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method enables the password fields, allowing the user to enter a new password for an existing user.
        /// </remarks>
        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            _resetPasswordRequested = true;
            this.txtPassword.Enabled = true;
            this.txtConfirmPassword.Enabled = true;
            this.txtPassword.Text = string.Empty;
            this.txtConfirmPassword.Text = string.Empty;
            this.txtPassword.Focus();
        }

        /// <summary>
        /// Handles the Click event for the "Save" button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method validates the user's input and then either creates a new user or updates an existing one
        /// by calling the appropriate methods in the <see cref="UserRepository"/>.
        /// </remarks>
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
                    _repo.UpdateUser(_userId, username, fullName, role, isActive);
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

        /// <summary>
        /// Handles the Click event for the "Cancel" button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method sets the form's <see cref="DialogResult"/> to <see cref="DialogResult.Cancel"/> and closes the form.
        /// </remarks>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
