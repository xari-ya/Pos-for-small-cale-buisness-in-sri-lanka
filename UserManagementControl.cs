using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace billing_system
{
    public partial class UserManagementControl : UserControl
    {
        private readonly UserRepository _repo = new UserRepository();
        private readonly BindingSource _bs = new BindingSource();
        private DataTable _usersTable = new DataTable();

        private const string Placeholder = "Search by username...";
        private bool _isBound = false; // helps avoid early filtering before bind

        public UserManagementControl()
        {
            InitializeComponent();

            // Placeholder UX
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
                // prevent firing on initial designer-set text
                if (txtSearchUsers.Focused && txtSearchUsers.Text != Placeholder)
                    ApplyFilter(txtSearchUsers.Text);
            };

            // Grid events
            dgvUsers.CellContentClick += DgvUsers_CellContentClick;
            dgvUsers.CellFormatting += DgvUsers_CellFormatting;

            // Sortable headers
            colUserId.SortMode = DataGridViewColumnSortMode.Automatic;
            colUsername.SortMode = DataGridViewColumnSortMode.Automatic;
            colRole.SortMode = DataGridViewColumnSortMode.Automatic;
            colCreated.SortMode = DataGridViewColumnSortMode.Automatic;

            // NOTE: Designer already wires btnAddUser_Click, so do NOT re-wire here.

            // Load data once control is ready
            this.Load += (_, __) => RefreshUsers();
        }

        private void RefreshUsers()
        {
            // Fetch all and bind to a DataView (via BindingSource) to enable RowFilter + sorting.
            _usersTable = _repo.GetAllUsers();

            _bs.DataSource = _usersTable.DefaultView;

            // Use designer-defined columns
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.DataSource = _bs;

            // Map DataPropertyNames to DataTable fields
            colUserId.DataPropertyName = "user_id";
            colUsername.DataPropertyName = "username";
            colRole.DataPropertyName = "role";
            colCreated.DataPropertyName = "created_at";

            _isBound = true;

            // Initialize placeholder after binding
            if (string.IsNullOrWhiteSpace(txtSearchUsers.Text))
            {
                txtSearchUsers.Text = Placeholder;
                txtSearchUsers.ForeColor = Color.Gray;
            }
        }

        // Defensive, fixed version (prevents NRE before binding)
        private void ApplyFilter(string raw)
        {
            if (_bs.DataSource is not DataView view) return;
            var term = (raw ?? "").Trim();
            view.RowFilter = string.IsNullOrEmpty(term)
                ? ""
                : $"username LIKE '%{term.Replace("'", "''")}%'";  // client-side filter by username
        }

        private void DgvUsers_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvUsers.Rows[e.RowIndex].DataBoundItem is not DataRowView drv) return;
            if (_usersTable == null || !_usersTable.Columns.Contains("is_active")) return;

            // Row styling: gray out inactive users
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

            // Friendly created_at formatting
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

        // Handles clicks on the "Edit" action button column
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

            using var frm = new UserDetailForm(
                mode: UserDetailForm.Mode.Edit,
                userId: userId,
                username: username,
                fullName: fullName,
                role: role,
                isActive: isActive,
                createdAt: createdAt
            );

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
                RefreshUsers(); // no success dialog; just refresh
        }

        // This name matches the Designer wiring (avoid CS0103)
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            using var frm = new UserDetailForm(UserDetailForm.Mode.Add);
            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
                RefreshUsers();
        }
    }
}
