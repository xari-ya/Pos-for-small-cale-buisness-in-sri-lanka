using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace billing_system
{
    /// <summary>
    /// Represents the user control for managing user accounts.
    /// </summary>
    /// <remarks>
    /// This control displays a list of all users in a data grid, allowing administrators to view, search, and manage user accounts.
    /// It provides functionalities for adding new users and editing existing ones by launching the <see cref="UserDetailForm"/>.
    /// </remarks>
    public partial class UserManagementControl : UserControl
    {
        private readonly UserRepository _repo = new UserRepository();
        private readonly BindingSource _bs = new BindingSource();
        private DataTable _usersTable = new DataTable();
        private const string Placeholder = "Search by username...";
        private bool _isBound = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManagementControl"/> class.
        /// </summary>
        public UserManagementControl()
        {
            InitializeComponent();
            txtSearchUsers.Enter += (_, __) =>
            {
                if (txtSearchUsers.Text == Placeholder)
                {
                    txtSearchUsers.Text = "";
                    txtSearchUsers.ForeColor = Color.Black;
                }
            };
            txtSearchUsers.Leave += (_, __) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearchUsers.Text))
                {
                    txtSearchUsers.Text = Placeholder;
                    txtSearchUsers.ForeColor = Color.Gray;
                    if (_isBound) ApplyFilter("");
                }
            };
            txtSearchUsers.TextChanged += (_, __) =>
            {
                if (txtSearchUsers.Focused && txtSearchUsers.Text != Placeholder)
                    ApplyFilter(txtSearchUsers.Text);
            };
            dgvUsers.CellContentClick += DgvUsers_CellContentClick;
            dgvUsers.CellFormatting += DgvUsers_CellFormatting;
            colUserId.SortMode = DataGridViewColumnSortMode.Automatic;
            colUsername.SortMode = DataGridViewColumnSortMode.Automatic;
            colRole.SortMode = DataGridViewColumnSortMode.Automatic;
            colCreated.SortMode = DataGridViewColumnSortMode.Automatic;
            this.Load += (_, __) => RefreshUsers();
        }

        /// <summary>
        /// Refreshes the user data from the database and updates the data grid.
        /// </summary>
        /// <remarks>
        /// This method fetches all users from the <see cref="UserRepository"/> and binds the result to the data grid view
        /// using a <see cref="BindingSource"/> to enable filtering and sorting.
        /// </remarks>
        private void RefreshUsers()
        {
            _usersTable = _repo.GetAllUsers();
            _bs.DataSource = _usersTable.DefaultView;
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.DataSource = _bs;
            colUserId.DataPropertyName = "user_id";
            colUsername.DataPropertyName = "username";
            colRole.DataPropertyName = "role";
            colCreated.DataPropertyName = "created_at";
            _isBound = true;
            if (string.IsNullOrWhiteSpace(txtSearchUsers.Text))
            {
                txtSearchUsers.Text = Placeholder;
                txtSearchUsers.ForeColor = Color.Gray;
            }
        }

        /// <summary>
        /// Applies a filter to the data grid view based on the search term.
        /// </summary>
        /// <param name="raw">The search term entered by the user.</param>
        /// <remarks>
        /// This method filters the user list by username in a client-side manner using the <see cref="DataView.RowFilter"/> property.
        /// </remarks>
        private void ApplyFilter(string raw)
        {
            if (_bs.DataSource is not DataView view) return;
            var term = (raw ?? "").Trim();
            view.RowFilter = string.IsNullOrEmpty(term) ? "" : $"username LIKE '%{term.Replace("'", "''")}%'";
        }

        /// <summary>
        /// Handles the CellFormatting event for the data grid view to apply custom styles.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method styles rows for inactive users by changing their text and background color.
        /// It also formats the 'created_at' date for better readability.
        /// </remarks>
        private void DgvUsers_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvUsers.Rows[e.RowIndex].DataBoundItem is not DataRowView drv) return;
            if (_usersTable == null || !_usersTable.Columns.Contains("is_active")) return;
            bool isActive = drv["is_active"] != DBNull.Value && Convert.ToInt32(drv["is_active"]) == 1;
            var row = dgvUsers.Rows[e.RowIndex];
            if (!isActive)
            {
                row.DefaultCellStyle.ForeColor = Color.DimGray;
                row.DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            }
            else
            {
                row.DefaultCellStyle.ForeColor = Color.Black;
                row.DefaultCellStyle.BackColor = Color.White;
            }
            if (dgvUsers.Columns[e.ColumnIndex].Name == colCreated.Name)
            {
                var raw = drv["created_at"]?.ToString();
                if (!string.IsNullOrWhiteSpace(raw) && DateTime.TryParse(raw, out var dt))
                {
                    e.Value = dt.ToString("yyyy-MM-dd HH:mm");
                    e.FormattingApplied = true;
                }
            }
        }

        /// <summary>
        /// Handles the CellContentClick event for the data grid view, specifically for the "Edit" button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// When the "Edit" button in a row is clicked, this method retrieves the user's data for that row
        /// and opens the <see cref="UserDetailForm"/> in edit mode. After the form is closed, it refreshes the user list.
        /// </remarks>
        private void DgvUsers_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvUsers.Columns[e.ColumnIndex].Name != colAction.Name) return;
            if (dgvUsers.Rows[e.RowIndex].DataBoundItem is not DataRowView drv) return;
            int userId = Convert.ToInt32(drv["user_id"]);
            string username = drv["username"]?.ToString() ?? "";
            string fullName = drv["full_name"]?.ToString() ?? "";
            string role = drv["role"]?.ToString() ?? "Cashier";
            bool isActive = Convert.ToInt32(drv["is_active"]) == 1;
            var createdAtText = drv["created_at"]?.ToString() ?? "";
            DateTime.TryParse(createdAtText, out var createdAt);
            using var frm = new UserDetailForm(mode: UserDetailForm.Mode.Edit, userId: userId, username: username, fullName: fullName, role: role, isActive: isActive, createdAt: createdAt);
            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
                RefreshUsers();
        }

        /// <summary>
        /// Handles the Click event for the "Add User" button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method opens the <see cref="UserDetailForm"/> in add mode. If a user is successfully added,
        /// it refreshes the user list.
        /// </remarks>
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            using var frm = new UserDetailForm(UserDetailForm.Mode.Add);
            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
                RefreshUsers();
        }
    }
}
