using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using BudgetTracker.Logic;

namespace BudgetTracker.Forms
{
    public partial class MainForm : Form
    {
        private readonly BudgetManager _budgetManager;
        private string _selectedTransactionId;
        private bool _bindingTransactions;

        public MainForm()
        {
            InitializeComponent();
            _budgetManager = new BudgetManager();
            Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetupDefaults();
            RefreshData();
            lblDataPath.Text = "Data file: " + _budgetManager.DataFilePath;
        }

        private void SetupDefaults()
        {
            cmbType.Items.AddRange(
                new object[] { "Income", "Expense" });

            cmbCategory.Items.AddRange(
                new object[]
                {
                    "Salary", "Food", "Transport",
                    "Subscriptions", "Study", "Other"
                });

            cmbFilterType.Items.AddRange(
                new object[] { "All", "Income", "Expense" });
            cmbFilterType.SelectedIndex = 0;

            cmbFilterCategory.Items.AddRange(
                new object[]
                {
                    "All", "Salary", "Food", "Transport",
                    "Subscriptions", "Study", "Other"
                });
            cmbFilterCategory.SelectedIndex = 0;

            for (int month = 1; month <= 12; month++)
                cmbFilterMonth.Items.Add(month);

            cmbFilterMonth.SelectedItem = DateTime.Now.Month;
            numSummaryYear.Value = DateTime.Now.Year;

            dtpFrom.Value = new DateTime(
                DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTo.Value = DateTime.Now.Date;
            dtpFrom.Enabled = dtpTo.Enabled = chkDateRange.Checked;

            cmbType.SelectedItem = "Expense";
            cmbCategory.SelectedItem = "Other";
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void RefreshData()
        {
            try
            {
                // Add saved categories to the entry and filter lists.
                var categories = _budgetManager
                    .GetFilteredTransactions(null, null, "All", "All")
                    .Select(t => t.Category)
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .Distinct();

                foreach (string category in categories)
                {
                    bool inFilter = cmbFilterCategory.Items
                        .Cast<object>()
                        .Any(item => string.Equals(
                            item.ToString(),
                            category,
                            StringComparison.OrdinalIgnoreCase));

                    if (!inFilter)
                        cmbFilterCategory.Items.Add(category);

                    bool inEntry = cmbCategory.Items
                        .Cast<object>()
                        .Any(item => string.Equals(
                            item.ToString(),
                            category,
                            StringComparison.OrdinalIgnoreCase));

                    if (!inEntry)
                        cmbCategory.Items.Add(category);
                }

                int selectedMonth = cmbFilterMonth.SelectedItem != null
                    ? Convert.ToInt32(cmbFilterMonth.SelectedItem)
                    : DateTime.Now.Month;

                int selectedYear = (int)numSummaryYear.Value;

                var transactions = _budgetManager.GetFilteredTransactions(
                    null,
                    null,
                    cmbFilterType.SelectedItem?.ToString(),
                    cmbFilterCategory.SelectedItem?.ToString(),
                    chkDateRange.Checked
                        ? dtpFrom.Value.Date
                        : (DateTime?)null,
                    chkDateRange.Checked
                        ? dtpTo.Value.Date
                        : (DateTime?)null);

                _bindingTransactions = true;

                dgvTransactions.DataSource = null;
                dgvTransactions.DataSource = transactions;

                if (dgvTransactions.Columns["Id"] != null)
                    dgvTransactions.Columns["Id"].Visible = false;

                if (dgvTransactions.Columns["Amount"] != null)
                    dgvTransactions.Columns["Amount"]
                        .DefaultCellStyle.Format = "N2";

                if (dgvTransactions.Columns["Date"] != null)
                    dgvTransactions.Columns["Date"]
                        .DefaultCellStyle.Format = "dd MMM yyyy";

                if (dgvTransactions.Columns["BalanceEffect"] != null)
                    dgvTransactions.Columns["BalanceEffect"].Visible = false;

                dgvTransactions.ClearSelection();
                _selectedTransactionId = null;
                _bindingTransactions = false;

                ClearForm();

                lblRecordCount.Text =
                    $"Showing {transactions.Count} transaction(s). "
                    + "Table filters do not change monthly totals.";

                var summary = _budgetManager.GetMonthlySummary(
                    selectedMonth, selectedYear);

                lblTotalIncome.Text =
                    $"Total Income: ${summary.TotalIncome:F2}";
                lblTotalExpense.Text =
                    $"Total Expense: ${summary.TotalExpense:F2}";
                lblNetBalance.Text =
                    $"Net Balance: ${summary.NetBalance:F2}";

                lblNetBalance.ForeColor = summary.NetBalance < 0
                    ? Color.Firebrick
                    : Color.DarkGreen;

                var categoryExpenses =
                    _budgetManager.GetCategoryExpenses(
                        selectedMonth, selectedYear);

                lstCategorySummary.Items.Clear();

                if (categoryExpenses.Count == 0)
                {
                    lstCategorySummary.Items.Add(
                        "No expenses for this month.");
                }

                foreach (var item in categoryExpenses)
                {
                    lstCategorySummary.Items.Add(
                        $"{item.Key}: ${item.Value:F2}");
                }
            }
            catch (Exception ex)
            {
                _bindingTransactions = false;

                MessageBox.Show(
                    "Error refreshing data: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(
                    txtAmount.Text,
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out decimal amount))
                {
                    throw new FormatException(
                        "Please enter a valid numeric amount.");
                }

                _budgetManager.AddTransaction(
                    cmbType.SelectedItem?.ToString(),
                    amount,
                    dtpDate.Value,
                    cmbCategory.Text.Trim(),
                    txtNote.Text);

                MessageBox.Show(
                    "Transaction saved successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearForm();
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_selectedTransactionId))
                {
                    MessageBox.Show(
                        "Select a transaction from the table to edit.",
                        "Selection Required",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                if (!decimal.TryParse(
                    txtAmount.Text,
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out decimal amount))
                {
                    throw new FormatException(
                        "Please enter a valid numeric amount.");
                }

                _budgetManager.EditTransaction(
                    _selectedTransactionId,
                    cmbType.SelectedItem?.ToString(),
                    amount,
                    dtpDate.Value,
                    cmbCategory.Text.Trim(),
                    txtNote.Text);

                MessageBox.Show(
                    "Transaction updated successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearForm();
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_selectedTransactionId))
                {
                    MessageBox.Show(
                        "Select a transaction from the table to delete.",
                        "Selection Required",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this transaction?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _budgetManager.DeleteTransaction(
                        _selectedTransactionId);

                    ClearForm();
                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvTransactions_SelectionChanged(
            object sender, EventArgs e)
        {
            if (_bindingTransactions)
                return;

            _selectedTransactionId = null;

            if (dgvTransactions.SelectedRows.Count == 0)
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                return;
            }

            var row = dgvTransactions.SelectedRows[0];

            if (!(row.DataBoundItem is TransactionViewDto))
                return;

            _selectedTransactionId =
                row.Cells["Id"].Value?.ToString();

            cmbType.SelectedItem =
                row.Cells["Type"].Value?.ToString();

            txtAmount.Text =
                row.Cells["Amount"].Value?.ToString();

            cmbCategory.Text =
                row.Cells["Category"].Value?.ToString();

            txtNote.Text =
                row.Cells["Note"].Value?.ToString();

            if (row.Cells["Date"].Value is DateTime transactionDate)
                dtpDate.Value = transactionDate;

            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void btnResetFilters_Click(object sender, EventArgs e)
        {
            cmbFilterType.SelectedIndex = 0;
            cmbFilterCategory.SelectedIndex = 0;
            chkDateRange.Checked = false;
            cmbFilterMonth.SelectedItem = DateTime.Now.Month;
            numSummaryYear.Value = DateTime.Now.Year;
            RefreshData();
        }

        private void chkDateRange_CheckedChanged(
            object sender, EventArgs e)
        {
            dtpFrom.Enabled = chkDateRange.Checked;
            dtpTo.Enabled = chkDateRange.Checked;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            _selectedTransactionId = null;

            txtAmount.Clear();
            txtNote.Clear();

            cmbType.SelectedItem = "Expense";
            cmbCategory.SelectedItem = "Other";
            dtpDate.Value = DateTime.Now;

            btnEdit.Enabled = false;
            btnDelete.Enabled = false;

            dgvTransactions.ClearSelection();
        }
    }
}