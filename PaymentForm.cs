// Filename: PaymentForm.cs
// Purpose: Finalize sale: validate payment, delegate DB save to TransactionManager,
//          generate a receipt image via Python using your JSON, then print it.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace billing_system
{
    public partial class PaymentForm : Form
    {
        private readonly TransactionManager _transaction;

        // Explicitly use WinForms timer to avoid ambiguity with System.Threading.Timer
        private System.Windows.Forms.Timer _statusTimer;
        private Label _statusBanner;

        private string _selectedPaymentMethod = "Cash";
        private readonly CultureInfo _lk = new CultureInfo("en-LK");

        // Python integration (adjust if you place files elsewhere)
        private static readonly string PythonExe = "python";               // or "py"
        private static readonly string PythonScriptName = "generate_bill.py";
        private static readonly string PythonTemplateName = "bill_template.html";

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        [DllImport("user32.dll")] public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")] public static extern bool ReleaseCapture();

        public PaymentForm(TransactionManager transaction)
        {
            InitializeComponent();

            _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));

            BuildStatusBanner();
            WireEvents();

            lblTotalAmount.Text = FormatN2(_transaction.GrandTotal);
            SetCashMode();

            Console.WriteLine($"[PAYMENT] Opened PaymentForm | invoice={_transaction.InvoiceNumber} total={_transaction.GrandTotal:N2} cashier={AppSession.CurrentUser?.Username}");
        }

        private void WireEvents()
        {
            // Hook the buttons you already wired in Designer
            btnCash.Click += btnCash_Click;
            btnCard.Click += btnCard_Click;
            btnCalculateChange.Click += btnCalculateChange_Click;
            btnConfirm.Click += async (_, __) => await ConfirmAsync();
            linkBackToBill.LinkClicked += linkBackToBill_LinkClicked;

            // Enter in txtAmountPaid triggers calculate (manual-only)
            txtAmountPaid.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    CalculateChange();
                }
            };

            // Close button on the title bar
            btnClose.Click += btnClose_Click;

            // Support dragging the custom title bar
            pnlTitleBar.MouseDown += pnlTitleBar_MouseDown;
        }

        private void BuildStatusBanner()
        {
            _statusBanner = new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Visible = false
            };
            Controls.Add(_statusBanner);
            _statusBanner.BringToFront();

            _statusTimer = new System.Windows.Forms.Timer { Interval = 5000 };
            _statusTimer.Tick += (_, __) =>
            {
                _statusTimer.Stop();
                _statusBanner.Visible = false;
            };
        }

        // ==== Modes ====
        private void SetCashMode()
        {
            _selectedPaymentMethod = "Cash";
            txtAmountPaid.ReadOnly = false;
            txtAmountPaid.Clear();
            txtAmountPaid.Focus();
            btnCalculateChange.Enabled = true;
            txtChangeDue.Text = "0.00";
        }

        private void SetCardMode()
        {
            _selectedPaymentMethod = "Card";
            txtAmountPaid.ReadOnly = true;
            txtAmountPaid.Text = FormatN2(_transaction.GrandTotal);
            btnCalculateChange.Enabled = false;
            txtChangeDue.Text = "0.00";
        }

        // ==== Formatting / Parsing ====
        private string FormatN2(decimal value) => value.ToString("N2", _lk);

        private bool TryParseAmount(string input, out decimal value)
        {
            if (string.IsNullOrWhiteSpace(input)) { value = 0m; return false; }
            var styles = NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint;
            if (decimal.TryParse(input, styles, _lk, out var parsed))
            {
                value = Math.Round(parsed, 2, MidpointRounding.ToEven);
                return true;
            }
            value = 0m;
            return false;
        }

        // ==== Calculate Change (manual only) ====
        private void CalculateChange()
        {
            if (_selectedPaymentMethod != "Cash")
            {
                ShowBanner("Change is not applicable for card payments.", false);
                return;
            }

            if (!TryParseAmount(txtAmountPaid.Text, out var amountPaid))
            {
                ShowBanner("Enter a valid amount (e.g., 1,234.56).", false);
                txtAmountPaid.Focus();
                txtAmountPaid.SelectAll();
                return;
            }

            var change = amountPaid - _transaction.GrandTotal;
            if (change < 0m) change = 0m;
            change = Math.Round(change, 2, MidpointRounding.ToEven);

            txtChangeDue.Text = FormatN2(change);
            Console.WriteLine($"[PAYMENT] Change | paid={amountPaid:N2} total={_transaction.GrandTotal:N2} change={change:N2}");
        }

        // ==== Confirm ====
        private async Task ConfirmAsync()
        {
            try
            {
                btnConfirm.Enabled = false; // double-submit guard

                if (!TryParseAmount(txtAmountPaid.Text, out var amountPaid))
                {
                    ShowBanner("Invalid amount. Use 1,234.56 format.", false);
                    txtAmountPaid.Focus();
                    txtAmountPaid.SelectAll();
                    btnConfirm.Enabled = true;
                    Console.WriteLine("[PAYMENT] ERROR: Invalid amount format");
                    return;
                }

                var total = _transaction.GrandTotal;

                if (_selectedPaymentMethod == "Cash")
                {
                    if (amountPaid < total)
                    {
                        ShowBanner("Insufficient payment for cash sale.", false);
                        txtAmountPaid.Focus();
                        txtAmountPaid.SelectAll();
                        btnConfirm.Enabled = true;
                        Console.WriteLine($"[PAYMENT] ERROR: Cash {amountPaid:N2} < Total {total:N2}");
                        return;
                    }
                }
                else // Card
                {
                    if (amountPaid != total)
                    {
                        ShowBanner("Card payments must equal the total.", false);
                        btnConfirm.Enabled = true;
                        Console.WriteLine($"[PAYMENT] ERROR: Card {amountPaid:N2} != Total {total:N2}");
                        return;
                    }
                }

                Console.WriteLine($"[DB] SaveTransactionToDatabase START | invoice={_transaction.InvoiceNumber} method={_selectedPaymentMethod} amountPaid={amountPaid:N2} items={_transaction.CurrentBillItems.Count} cashier={AppSession.CurrentUser?.Username}");
                var (success, errorMessage) = _transaction.SaveTransactionToDatabase(_selectedPaymentMethod, amountPaid);
                if (!success)
                {
                    ShowBanner($"Transaction failed: {errorMessage}", false);
                    Console.WriteLine($"[DB] ERROR: {errorMessage}");
                    btnConfirm.Enabled = true;
                    return;
                }
                Console.WriteLine("[DB] SUCCESS: Transaction committed");

                var printed = await Task.Run(() => TryGenerateAndPrintReceiptSafe(amountPaid));
                if (!printed) Console.WriteLine("[PRINT] WARNING: Receipt generation/printing failed");

                ShowBanner("Payment successful. Finishing up…", true);
                await Task.Delay(5000);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                ShowBanner("Unexpected error. See terminal for details.", false);
                Console.WriteLine("[PAYMENT] FATAL: " + ex);
                btnConfirm.Enabled = true;
            }
        }

        // ==== JSON payload (manual writer: no extra packages needed) ====
        private string EscapeJson(string s)
        {
            if (s == null) return "";
            var sb = new StringBuilder(s.Length + 16);
            foreach (var ch in s)
            {
                switch (ch)
                {
                    case '\"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '\b': sb.Append("\\b"); break;
                    case '\f': sb.Append("\\f"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    default:
                        if (ch < 32) sb.Append("\\u").Append(((int)ch).ToString("x4"));
                        else sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }

        private string WriteReceiptJson(decimal amountPaid)
        {
            // Map to your actual BillItem shape:
            // BillItem.ProductDetails.Name, BillItem.UnitPriceAtSale, BillItem.Quantity, BillItem.LineTotal
            var jsonPath = Path.Combine(Path.GetTempPath(), $"invoice_{_transaction.InvoiceNumber}_{Guid.NewGuid():N}.json");
            var sb = new StringBuilder();
            sb.Append('{');

            sb.Append("\"invoice_id\":\"").Append(EscapeJson(_transaction.InvoiceNumber?.ToString() ?? "")).Append("\",");
            sb.Append("\"date_time\":\"").Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).Append("\",");
            sb.Append("\"cashier\":\"").Append(EscapeJson(AppSession.CurrentUser?.Username ?? "unknown")).Append("\",");
            sb.Append("\"payment_method\":\"").Append(EscapeJson(_selectedPaymentMethod)).Append("\",");
            sb.Append("\"amount_paid\":").Append(amountPaid.ToString("0.00", CultureInfo.InvariantCulture)).Append(',');
            sb.Append("\"grand_total\":").Append(_transaction.GrandTotal.ToString("0.00", CultureInfo.InvariantCulture)).Append(',');

            sb.Append("\"items\":[");
            for (int i = 0; i < _transaction.CurrentBillItems.Count; i++)
            {
                var it = _transaction.CurrentBillItems[i];
                var name = it.ProductDetails?.Name ?? "";
                var unit = it.UnitPriceAtSale;
                var qty = it.Quantity;
                var line = Math.Round(it.LineTotal, 2, MidpointRounding.ToEven);

                if (i > 0) sb.Append(',');
                sb.Append('{');
                sb.Append("\"name\":\"").Append(EscapeJson(name)).Append("\",");
                sb.Append("\"unit_price\":").Append(unit.ToString("0.00", CultureInfo.InvariantCulture)).Append(',');
                sb.Append("\"quantity\":").Append(qty.ToString(CultureInfo.InvariantCulture)).Append(',');
                sb.Append("\"line_total\":").Append(line.ToString("0.00", CultureInfo.InvariantCulture));
                sb.Append('}');
            }
            sb.Append(']');

            sb.Append('}');

            File.WriteAllText(jsonPath, sb.ToString(), new UTF8Encoding(false));
            Console.WriteLine($"[PRINT] JSON written: {jsonPath}");
            return jsonPath;
        }

        // ==== Python + Print ====
        private bool TryGenerateAndPrintReceiptSafe(decimal amountPaid)
        {
            try
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var scriptPath = Path.Combine(baseDir, PythonScriptName);
                var templatePath = Path.Combine(baseDir, PythonTemplateName);

                Console.WriteLine($"[PRINT] script={scriptPath}");
                Console.WriteLine($"[PRINT] template={templatePath}");

                if (!File.Exists(scriptPath) || !File.Exists(templatePath))
                {
                    Console.WriteLine("[PRINT] Missing python script/template. Skipping.");
                    return false;
                }

                var jsonPath = WriteReceiptJson(amountPaid);
                var outPng = Path.Combine(Path.GetTempPath(), $"invoice_{_transaction.InvoiceNumber}_{Guid.NewGuid():N}.png");
                Console.WriteLine($"[PRINT] out={outPng}");

                var psi = new ProcessStartInfo
                {
                    FileName = PythonExe,
                    Arguments = $"\"{scriptPath}\" --data \"{jsonPath}\" --out \"{outPng}\" --template \"{templatePath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WorkingDirectory = baseDir,
                    CreateNoWindow = true
                };

                using (var proc = Process.Start(psi))
                {
                    string stdOut = proc.StandardOutput.ReadToEnd();
                    string stdErr = proc.StandardError.ReadToEnd();
                    proc.WaitForExit();

                    Console.WriteLine("[PRINT][python][stdout] " + stdOut);
                    if (!string.IsNullOrWhiteSpace(stdErr))
                        Console.WriteLine("[PRINT][python][stderr] " + stdErr);

                    if (proc.ExitCode != 0)
                    {
                        Console.WriteLine($"[PRINT] Python exit code={proc.ExitCode}");
                        return false;
                    }
                }

                if (!File.Exists(outPng))
                {
                    Console.WriteLine("[PRINT] Python ran, but image not found.");
                    return false;
                }

                using (var img = Image.FromFile(outPng))
                using (var pd = new PrintDocument())
                {
                    pd.DocumentName = $"Invoice_{_transaction.InvoiceNumber}";
                    pd.PrintPage += (s, e) =>
                    {
                        var margin = e.MarginBounds;
                        float ratio = Math.Min((float)margin.Width / img.Width, (float)margin.Height / img.Height);
                        int w = (int)(img.Width * ratio);
                        int h = (int)(img.Height * ratio);
                        int x = margin.Left + (margin.Width - w) / 2;
                        int y = margin.Top + (margin.Height - h) / 2;
                        e.Graphics.DrawImage(img, new Rectangle(x, y, w, h));
                        e.HasMorePages = false;
                    };

                    Console.WriteLine("[PRINT] Sending to default printer…");
                    pd.Print();
                }

                Console.WriteLine("[PRINT] SUCCESS");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[PRINT] ERROR: " + ex);
                return false;
            }
        }

        // ==== Inline banner ====
        private void ShowBanner(string message, bool success)
        {
            _statusBanner.Text = message;
            _statusBanner.BackColor = success ? Color.FromArgb(30, 135, 76) : Color.FromArgb(200, 39, 55);
            _statusBanner.Visible = true;

            _statusTimer.Stop();
            _statusTimer.Start();
        }

        // ==== Event stubs that your Designer is already wiring ====
        private void btnCash_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[PAYMENT] Method -> Cash");
            SetCashMode();
        }

        private void btnCard_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[PAYMENT] Method -> Card");
            SetCardMode();
        }

        private void btnCalculateChange_Click(object sender, EventArgs e)
        {
            CalculateChange();
        }

        private void linkBackToBill_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void pnlTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
