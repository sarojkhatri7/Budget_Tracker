namespace BudgetTracker.Forms
{
    partial class MainForm
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

        private void InitializeComponent()
        {
            this.lblType = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblNote = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.dgvTransactions = new System.Windows.Forms.DataGridView();
            this.lblTotalIncome = new System.Windows.Forms.Label();
            this.lblTotalExpense = new System.Windows.Forms.Label();
            this.lblNetBalance = new System.Windows.Forms.Label();
            this.cmbFilterMonth = new System.Windows.Forms.ComboBox();
            this.cmbFilterType = new System.Windows.Forms.ComboBox();
            this.cmbFilterCategory = new System.Windows.Forms.ComboBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.lstCategorySummary = new System.Windows.Forms.ListBox();
            this.lblCatHeading = new System.Windows.Forms.Label();
            this.chkDateRange = new System.Windows.Forms.CheckBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.numSummaryYear = new System.Windows.Forms.NumericUpDown();
            this.lblSummaryMonth = new System.Windows.Forms.Label();
            this.lblSummaryYear = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSummaryYear)).BeginInit();
            this.SuspendLayout();
            // 
            // Labels and Inputs Layout
            // 
            this.lblType.Text = "Type:"; this.lblType.Location = new System.Drawing.Point(20, 20);
            this.cmbType.Location = new System.Drawing.Point(100, 17); this.cmbType.Size = new System.Drawing.Size(150, 23);

            this.lblAmount.Text = "Amount:"; this.lblAmount.Location = new System.Drawing.Point(20, 55);
            this.txtAmount.Location = new System.Drawing.Point(100, 52); this.txtAmount.Size = new System.Drawing.Size(150, 23);

            this.lblCategory.Text = "Category:"; this.lblCategory.Location = new System.Drawing.Point(20, 90);
            this.cmbCategory.Location = new System.Drawing.Point(100, 87); this.cmbCategory.Size = new System.Drawing.Size(150, 23);

            this.lblDate.Text = "Date:"; this.lblDate.Location = new System.Drawing.Point(20, 125);
            this.dtpDate.Location = new System.Drawing.Point(100, 122); this.dtpDate.Size = new System.Drawing.Size(150, 23);

            this.lblNote.Text = "Note:"; this.lblNote.Location = new System.Drawing.Point(20, 160);
            this.txtNote.Location = new System.Drawing.Point(100, 157); this.txtNote.Size = new System.Drawing.Size(150, 23);

            // Buttons
            this.btnAdd.Text = "Add"; this.btnAdd.Location = new System.Drawing.Point(20, 200); this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            this.btnEdit.Text = "Edit"; this.btnEdit.Location = new System.Drawing.Point(105, 200); this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            this.btnDelete.Text = "Delete"; this.btnDelete.Location = new System.Drawing.Point(190, 200); this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.btnClear.Text = "Clear"; this.btnClear.Location = new System.Drawing.Point(275, 200); this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // Filters
            this.cmbFilterMonth.Location = new System.Drawing.Point(380, 17); this.cmbFilterMonth.Size = new System.Drawing.Size(80, 23);
            this.cmbFilterType.Location = new System.Drawing.Point(470, 17); this.cmbFilterType.Size = new System.Drawing.Size(90, 23);
            this.cmbFilterCategory.Location = new System.Drawing.Point(570, 17); this.cmbFilterCategory.Size = new System.Drawing.Size(100, 23);
            this.btnFilter.Text = "Filter"; this.btnFilter.Location = new System.Drawing.Point(680, 16); this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            this.lblSummaryMonth.Text = "Summary month"; this.lblSummaryMonth.Location = new System.Drawing.Point(380, 0); this.lblSummaryMonth.AutoSize = true;
            this.lblSummaryYear.Text = "Year"; this.lblSummaryYear.Location = new System.Drawing.Point(780, 0); this.lblSummaryYear.AutoSize = true;
            this.numSummaryYear.Location = new System.Drawing.Point(780, 17); this.numSummaryYear.Size = new System.Drawing.Size(80, 23);
            this.numSummaryYear.Minimum = 1900; this.numSummaryYear.Maximum = 2100; this.numSummaryYear.Value = 2026;
            this.chkDateRange.Text = "Date range:"; this.chkDateRange.Location = new System.Drawing.Point(380, 51); this.chkDateRange.Size = new System.Drawing.Size(105, 23);
            this.dtpFrom.Location = new System.Drawing.Point(490, 51); this.dtpFrom.Size = new System.Drawing.Size(160, 23); this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(680, 51); this.dtpTo.Size = new System.Drawing.Size(160, 23); this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;

            // DataGrid
            this.dgvTransactions.Location = new System.Drawing.Point(380, 85);
            this.dgvTransactions.Size = new System.Drawing.Size(480, 185);
            this.dgvTransactions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTransactions.MultiSelect = false;
            this.dgvTransactions.ReadOnly = true;
            this.dgvTransactions.AllowUserToAddRows = false;
            this.dgvTransactions.SelectionChanged += new System.EventHandler(this.dgvTransactions_SelectionChanged);

            // Summary Labels
            this.lblTotalIncome.Text = "Total Income: $0.00"; this.lblTotalIncome.Location = new System.Drawing.Point(380, 280); this.lblTotalIncome.AutoSize = true;
            this.lblTotalExpense.Text = "Total Expense: $0.00"; this.lblTotalExpense.Location = new System.Drawing.Point(530, 280); this.lblTotalExpense.AutoSize = true;
            this.lblNetBalance.Text = "Net Balance: $0.00"; this.lblNetBalance.Location = new System.Drawing.Point(680, 280); this.lblNetBalance.AutoSize = true;

            // Category Summary
            this.lblCatHeading.Text = "Category Expenses (Selected Month):"; this.lblCatHeading.Location = new System.Drawing.Point(20, 250); this.lblCatHeading.AutoSize = true;
            this.lstCategorySummary.Location = new System.Drawing.Point(20, 275); this.lstCategorySummary.Size = new System.Drawing.Size(330, 95);

            // Form
            this.ClientSize = new System.Drawing.Size(884, 390);
            this.Controls.Add(this.lblType); this.Controls.Add(this.cmbType);
            this.Controls.Add(this.lblAmount); this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.lblCategory); this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblDate); this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.lblNote); this.Controls.Add(this.txtNote);
            this.Controls.Add(this.btnAdd); this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete); this.Controls.Add(this.btnClear);
            this.Controls.Add(this.cmbFilterMonth); this.Controls.Add(this.cmbFilterType);
            this.Controls.Add(this.cmbFilterCategory); this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.chkDateRange); this.Controls.Add(this.dtpFrom); this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.lblSummaryMonth); this.Controls.Add(this.lblSummaryYear); this.Controls.Add(this.numSummaryYear);
            this.Controls.Add(this.dgvTransactions);
            this.Controls.Add(this.lblTotalIncome); this.Controls.Add(this.lblTotalExpense); this.Controls.Add(this.lblNetBalance);
            this.Controls.Add(this.lblCatHeading); this.Controls.Add(this.lstCategorySummary);
            this.Text = "Personal Budget Tracker - ITS203";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSummaryYear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblType, lblAmount, lblCategory, lblDate, lblNote, lblTotalIncome, lblTotalExpense, lblNetBalance, lblCatHeading;
        private System.Windows.Forms.TextBox txtAmount, txtNote;
        private System.Windows.Forms.ComboBox cmbType, cmbCategory, cmbFilterMonth, cmbFilterType, cmbFilterCategory;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Button btnAdd, btnEdit, btnDelete, btnClear, btnFilter;
        private System.Windows.Forms.DataGridView dgvTransactions;
        private System.Windows.Forms.ListBox lstCategorySummary;
        private System.Windows.Forms.CheckBox chkDateRange;
        private System.Windows.Forms.DateTimePicker dtpFrom, dtpTo;
        private System.Windows.Forms.NumericUpDown numSummaryYear;
        private System.Windows.Forms.Label lblSummaryMonth, lblSummaryYear;
    }
}
