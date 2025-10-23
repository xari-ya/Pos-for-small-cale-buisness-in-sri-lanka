namespace billing_system
{
    // In PaymentForm.Designer.cs

    partial class PaymentForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlTitleBar = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTotalAmountDueHeader = new System.Windows.Forms.Label();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.lblPaymentMethod = new System.Windows.Forms.Label();
            this.btnCash = new System.Windows.Forms.Button();
            this.btnCard = new System.Windows.Forms.Button();
            this.lblAmountPaid = new System.Windows.Forms.Label();
            this.txtAmountPaid = new System.Windows.Forms.TextBox();
            this.lblChangeDue = new System.Windows.Forms.Label();
            this.txtChangeDue = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.linkBackToBill = new System.Windows.Forms.LinkLabel();
            this.btnCalculateChange = new System.Windows.Forms.Button();
            this.pnlTitleBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTitleBar
            // 
            this.pnlTitleBar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlTitleBar.Controls.Add(this.lblTitle);
            this.pnlTitleBar.Controls.Add(this.btnClose);
            this.pnlTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTitleBar.Name = "pnlTitleBar";
            this.pnlTitleBar.Size = new System.Drawing.Size(450, 60);
            this.pnlTitleBar.TabIndex = 14;
            this.pnlTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlTitleBar_MouseDown);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(23, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(169, 28);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Process Payment";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(400, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(38, 38);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTotalAmountDueHeader
            // 
            this.lblTotalAmountDueHeader.AutoSize = true;
            this.lblTotalAmountDueHeader.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTotalAmountDueHeader.ForeColor = System.Drawing.Color.Gray;
            this.lblTotalAmountDueHeader.Location = new System.Drawing.Point(24, 85);
            this.lblTotalAmountDueHeader.Name = "lblTotalAmountDueHeader";
            this.lblTotalAmountDueHeader.Size = new System.Drawing.Size(147, 23);
            this.lblTotalAmountDueHeader.TabIndex = 1;
            this.lblTotalAmountDueHeader.Text = "Total Amount Due";
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.Location = new System.Drawing.Point(19, 108);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(149, 54);
            this.lblTotalAmount.TabIndex = 2;
            this.lblTotalAmount.Text = "$45.50";
            // 
            // lblPaymentMethod
            // 
            this.lblPaymentMethod.AutoSize = true;
            this.lblPaymentMethod.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentMethod.Location = new System.Drawing.Point(24, 185);
            this.lblPaymentMethod.Name = "lblPaymentMethod";
            this.lblPaymentMethod.Size = new System.Drawing.Size(141, 23);
            this.lblPaymentMethod.TabIndex = 3;
            this.lblPaymentMethod.Text = "Payment Method";
            // 
            // btnCash
            // 
            this.btnCash.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnCash.FlatAppearance.BorderSize = 0;
            this.btnCash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCash.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCash.ForeColor = System.Drawing.Color.White;
            this.btnCash.Location = new System.Drawing.Point(28, 221);
            this.btnCash.Name = "btnCash";
            this.btnCash.Size = new System.Drawing.Size(190, 45);
            this.btnCash.TabIndex = 0;
            this.btnCash.Text = "Cash";
            this.btnCash.UseVisualStyleBackColor = false;
            this.btnCash.Click += new System.EventHandler(this.btnCash_Click);
            // 
            // btnCard
            // 
            this.btnCard.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCard.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCard.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCard.ForeColor = System.Drawing.Color.Black;
            this.btnCard.Location = new System.Drawing.Point(232, 221);
            this.btnCard.Name = "btnCard";
            this.btnCard.Size = new System.Drawing.Size(190, 45);
            this.btnCard.TabIndex = 1;
            this.btnCard.Text = "Card";
            this.btnCard.UseVisualStyleBackColor = false;
            this.btnCard.Click += new System.EventHandler(this.btnCard_Click);
            // 
            // lblAmountPaid
            // 
            this.lblAmountPaid.AutoSize = true;
            this.lblAmountPaid.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmountPaid.Location = new System.Drawing.Point(24, 290);
            this.lblAmountPaid.Name = "lblAmountPaid";
            this.lblAmountPaid.Size = new System.Drawing.Size(217, 23);
            this.lblAmountPaid.TabIndex = 6;
            this.lblAmountPaid.Text = "Amount Paid by Customer";
            // 
            // txtAmountPaid
            // 
            this.txtAmountPaid.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtAmountPaid.Location = new System.Drawing.Point(28, 316);
            this.txtAmountPaid.Name = "txtAmountPaid";
            this.txtAmountPaid.Size = new System.Drawing.Size(394, 34);
            this.txtAmountPaid.TabIndex = 2;
            // 
            // lblChangeDue
            // 
            this.lblChangeDue.AutoSize = true;
            this.lblChangeDue.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChangeDue.Location = new System.Drawing.Point(24, 434);
            this.lblChangeDue.Name = "lblChangeDue";
            this.lblChangeDue.Size = new System.Drawing.Size(103, 23);
            this.lblChangeDue.TabIndex = 8;
            this.lblChangeDue.Text = "Change Due";
            // 
            // txtChangeDue
            // 
            this.txtChangeDue.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtChangeDue.Location = new System.Drawing.Point(28, 460);
            this.txtChangeDue.Name = "txtChangeDue";
            this.txtChangeDue.ReadOnly = true;
            this.txtChangeDue.Size = new System.Drawing.Size(394, 34);
            this.txtChangeDue.TabIndex = 4;
            this.txtChangeDue.Text = "$0.00";
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(164)))), ((int)(((byte)(79)))));
            this.btnConfirm.FlatAppearance.BorderSize = 0;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Location = new System.Drawing.Point(28, 520);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(394, 55);
            this.btnConfirm.TabIndex = 5;
            this.btnConfirm.Text = "Confirm Payment && Print Receipt";
            this.btnConfirm.UseVisualStyleBackColor = false;
            // 
            // linkBackToBill
            // 
            this.linkBackToBill.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.linkBackToBill.AutoSize = true;
            this.linkBackToBill.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkBackToBill.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.linkBackToBill.Location = new System.Drawing.Point(171, 591);
            this.linkBackToBill.Name = "linkBackToBill";
            this.linkBackToBill.Size = new System.Drawing.Size(95, 23);
            this.linkBackToBill.TabIndex = 6;
            this.linkBackToBill.TabStop = true;
            this.linkBackToBill.Text = "Back to Bill";
            this.linkBackToBill.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.linkBackToBill.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkBackToBill_LinkClicked);
            // 
            // btnCalculateChange
            // 
            this.btnCalculateChange.BackColor = System.Drawing.Color.Gainsboro;
            this.btnCalculateChange.FlatAppearance.BorderSize = 0;
            this.btnCalculateChange.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCalculateChange.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCalculateChange.ForeColor = System.Drawing.Color.Black;
            this.btnCalculateChange.Location = new System.Drawing.Point(28, 365);
            this.btnCalculateChange.Name = "btnCalculateChange";
            this.btnCalculateChange.Size = new System.Drawing.Size(394, 45);
            this.btnCalculateChange.TabIndex = 3;
            this.btnCalculateChange.Text = "Calculate Change";
            this.btnCalculateChange.UseVisualStyleBackColor = false;
            this.btnCalculateChange.Click += new System.EventHandler(this.btnCalculateChange_Click);
            // 
            // PaymentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(450, 635);
            this.Controls.Add(this.btnCalculateChange);
            this.Controls.Add(this.linkBackToBill);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.txtChangeDue);
            this.Controls.Add(this.lblChangeDue);
            this.Controls.Add(this.txtAmountPaid);
            this.Controls.Add(this.lblAmountPaid);
            this.Controls.Add(this.btnCard);
            this.Controls.Add(this.btnCash);
            this.Controls.Add(this.lblPaymentMethod);
            this.Controls.Add(this.lblTotalAmount);
            this.Controls.Add(this.lblTotalAmountDueHeader);
            this.Controls.Add(this.pnlTitleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PaymentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PaymentForm";
            this.pnlTitleBar.ResumeLayout(false);
            this.pnlTitleBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTitleBar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTotalAmountDueHeader;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.Label lblPaymentMethod;
        private System.Windows.Forms.Button btnCash;
        private System.Windows.Forms.Button btnCard;
        private System.Windows.Forms.Label lblAmountPaid;
        private System.Windows.Forms.TextBox txtAmountPaid;
        private System.Windows.Forms.Label lblChangeDue;
        private System.Windows.Forms.TextBox txtChangeDue;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.LinkLabel linkBackToBill;
        private System.Windows.Forms.Button btnCalculateChange;
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>


        #endregion
    }
}