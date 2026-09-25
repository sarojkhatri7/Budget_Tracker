using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BudgetTracker.Logic;

namespace BudgetTracker.Forms
{
    public partial class MainForm : Form
    {
        private readonly BudgetManager _budgetManager;
        private string _selectedTransactionId = null;
        private bool _bindingTransactions;

        public MainForm()
        {
            InitializeComponent();
            Load += MainForm_Load;
            _budgetManager = new BudgetManager();
        }

        private void MainForm_Load(object sender, EventArgs args)
        {
            SetupDefaults();
            RefreshData();
        }

        private void SetupDefaults()
        {
            cmbType.Items.AddRange(new string[] { "Income", "Expense" });
            cmbCategory.Items.AddRange(new string[] { "Salary", "Food", "Transport", "Subscriptions", "Study", "Other" });
            
            cmbFilterType.Items.AddRange(new string[] { "All", "Income", "Expense" });
            cmbFilterType.SelectedIndex = 0;

            cmbFilterCategory.Items.AddRange(new string[] { "All", "Salary", "Food", "Transport", "Subscriptions", "Study", "Other" });
            cmbFilterCategory.SelectedIndex = 0;

            for (int i = 1; i <= 12; i++) cmbFilterMonth.Items.Add(i);
            cmbFilterMonth.SelectedItem = DateTime.Now.Month;
            numSummaryYear.Value = DateTime.Now.Year;
            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTo.Value = DateTime.Now.Date;
        }

        private void RefreshData()
        {
            try
            {
                // Make categories entered by the user available as filter choices.
                var categories = _budgetManager.GetFilteredTransactions(null, null, "All", "All")
                    .Select(t => t.Category).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct();
                foreach (string category in categories)
                    if (!cmbFilterCategory.Items.Contains(category)) cmbFilterCategory.Items.Add(category);

                int selectedMonth = cmbFilterMonth.SelectedItem != null ? Convert.ToInt32(cmbFilterMonth.SelectedItem) : DateTime.Now.Month;
                int currentYear = (int)numSummaryYear.Value;

                // The list shows all dates unless the date range is enabled.
                var list = _budgetManager.GetFilteredTransactions(
                    null,
                    null,
                    cmbFilterType.SelectedItem?.ToString(),
                    cmbFilterCategory.SelectedItem?.ToString(),
                    chkDateRange.Checked ? dtpFrom.Value.Date : (DateTime?)null,
                    chkDateRange.Checked ? dtpTo.Value.Date : (DateTime?)null
                );

                _bindingTransactions = true;
                dgvTransactions.DataSource = null;
                dgvTransactions.DataSource = list;
                if (dgvTransactions.Columns["Id"] != null) dgvTransactions.Columns["Id"].Visible = false;
                dgvTransactions.ClearSelection();
                _selectedTransactionId = null;
                _bindingTransactions = false;

                // Load Summary (FR5)
                var summary = _budgetManager.GetMonthlySummary(selectedMonth, currentYear);
                lblTotalIncome.Text = $"Total Income: ${summary.TotalIncome:F2}";
                lblTotalExpense.Text = $"Total Expense: ${summary.TotalExpense:F2}";
                lblNetBalance.Text = $"Net Balance: ${summary.NetBalance:F2}";

                // Load Category Spending (FR6)
                var catExpenses = _budgetManager.GetCategoryExpenses(selectedMonth, currentYear);
                lstCategorySummary.Items.Clear();
                foreach (var item in catExpenses)
                {
                    lstCategorySummary.Items.Add($"{item.Key}: ${item.Value:F2}");
                }
            }
            catch (Exception ex)
            {
                _bindingTransactions = false;
                MessageBox.Show($"Error refreshing data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // FR1 - Add Transaction with Exception Handling
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtAmount.Text, out decimal amount))
                    throw new FormatException("Please enter a valid numeric amount.");

                _budgetManager.AddTransaction(
                    cmbType.SelectedItem?.ToString(),
                    amount,
                    dtpDate.Value,
                    cmbCategory.Text.Trim(),
                    txtNote.Text
                );

                MessageBox.Show("Transaction saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // FR3 - Edit Transaction with Exception Handling
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_selectedTransactionId))
                {
                    MessageBox.Show("Please select a transaction from the table to edit.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!decimal.TryParse(txtAmount.Text, out decimal amount))
                    throw new FormatException("Please enter a valid numeric amount.");

                _budgetManager.EditTransaction(
                    _selectedTransactionId,
                    cmbType.SelectedItem?.ToString(),
                    amount,
                    dtpDate.Value,
                    cmbCategory.Text.Trim(),
                    txtNote.Text
                );

                MessageBox.Show("Transaction updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // FR4 - Delete Transaction with Confirmation
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_selectedTransactionId))
                {
                    MessageBox.Show("Please select a transaction from the table to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var result = MessageBox.Show("Are you sure you want to delete this transaction?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _budgetManager.DeleteTransaction(_selectedTransactionId);
                    ClearForm();
                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvTransactions_SelectionChanged(object sender, EventArgs e)
        {
            if (_bindingTransactions) return;
            _selectedTransactionId = null;
            if (dgvTransactions.SelectedRows.Count > 0 &&
                dgvTransactions.SelectedRows[0].DataBoundItem is TransactionViewDto)
            {
                var row = dgvTransactions.SelectedRows[0];
                _selectedTransactionId = row.Cells["Id"].Value?.ToString();
                cmbType.SelectedItem = row.Cells["Type"].Value?.ToString();
                txtAmount.Text = row.Cells["Amount"].Value?.ToString();
                cmbCategory.Text = row.Cells["Category"].Value?.ToString();
                txtNote.Text = row.Cells["Note"].Value?.ToString();
                if (DateTime.TryParse(row.Cells["Date"].Value?.ToString(), out DateTime dateVal))
                    dtpDate.Value = dateVal;
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            RefreshData();
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
            cmbType.SelectedIndex = -1;
            cmbCategory.SelectedIndex = -1;
            cmbCategory.Text = "";
            dtpDate.Value = DateTime.Now;
            dgvTransactions.ClearSelection();
        }
    }
}
