using System.Drawing;
using System.Windows.Forms;

namespace BudgetTracker.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private ComboBox cmbType;
        private ComboBox cmbCategory;
        private ComboBox cmbFilterMonth;
        private ComboBox cmbFilterType;
        private ComboBox cmbFilterCategory;

        private TextBox txtAmount;
        private TextBox txtNote;

        private DateTimePicker dtpDate;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;

        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnClear;
        private Button btnFilter;
        private Button btnResetFilters;

        private DataGridView dgvTransactions;
        private ListBox lstCategorySummary;

        private Label lblTotalIncome;
        private Label lblTotalExpense;
        private Label lblNetBalance;
        private Label lblDataPath;
        private Label lblRecordCount;

        private CheckBox chkDateRange;
        private NumericUpDown numSummaryYear;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            Text = "Personal Budget Tracker - ITS203";
            Font = new Font("Segoe UI", 10F);
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1180, 760);
            MinimumSize = new Size(1100, 740);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            cmbType = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            txtAmount = new TextBox { Dock = DockStyle.Fill };

            cmbCategory = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDown
            };

            dtpDate = NewDatePicker();

            txtNote = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            btnAdd = NewButton("Add", btnAdd_Click);
            btnEdit = NewButton("Edit", btnEdit_Click);
            btnDelete = NewButton("Delete", btnDelete_Click);
            btnClear = NewButton("Clear", btnClear_Click);
            btnFilter = NewButton("Apply", btnFilter_Click);
            btnResetFilters = NewButton("Reset", btnResetFilters_Click);

            btnEdit.Enabled = false;
            btnDelete.Enabled = false;

            cmbFilterType = new ComboBox
            {
                Width = 130,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbFilterCategory = new ComboBox
            {
                Width = 170,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbFilterMonth = new ComboBox
            {
                Width = 65,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            numSummaryYear = new NumericUpDown
            {
                Width = 85,
                Minimum = 1900,
                Maximum = 9998,
                Value = DateTime.Now.Year
            };

            chkDateRange = new CheckBox
            {
                Text = "Date range",
                AutoSize = true,
                Margin = new Padding(5, 8, 5, 3)
            };
            chkDateRange.CheckedChanged += chkDateRange_CheckedChanged;

            dtpFrom = NewDatePicker();
            dtpFrom.Dock = DockStyle.None;
            dtpFrom.Width = 130;

            dtpTo = NewDatePicker();
            dtpTo.Dock = DockStyle.None;
            dtpTo.Width = 130;

            dgvTransactions = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                AutoGenerateColumns = true
            };
            dgvTransactions.SelectionChanged +=
                dgvTransactions_SelectionChanged;

            lblTotalIncome = NewLabel("Total Income: $0.00");
            lblTotalExpense = NewLabel("Total Expense: $0.00");
            lblNetBalance = NewLabel("Net Balance: $0.00");

            lstCategorySummary = new ListBox
            {
                Dock = DockStyle.Fill,
                IntegralHeight = false,
                HorizontalScrollbar = true
            };

            lblDataPath = new Label
            {
                Dock = DockStyle.Fill,
                AutoEllipsis = true,
                TextAlign = ContentAlignment.MiddleLeft
            };

            lblRecordCount = new Label
            {
                Dock = DockStyle.Fill,
                AutoEllipsis = true,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                RowCount = 3,
                ColumnCount = 1
            };

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 48));
            root.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));
            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 36));

            var title = new Label
            {
                Text = "Personal Budget Tracker",
                Dock = DockStyle.Fill,
                Font = new Font(
                    "Segoe UI", 18F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            root.Controls.Add(title, 0, 0);

            var body = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };

            body.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 340));
            body.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100));
            body.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            root.Controls.Add(body, 0, 1);
            root.Controls.Add(lblDataPath, 0, 2);

            var left = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(0, 0, 12, 0)
            };

            left.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 32));
            left.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 238));
            left.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 86));
            left.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 40));
            left.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            left.Controls.Add(
                NewLabel("Transaction details", true), 0, 0);

            var inputs = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5
            };

            inputs.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 80));
            inputs.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100));

            for (int i = 0; i < 4; i++)
                inputs.RowStyles.Add(
                    new RowStyle(SizeType.Absolute, 38));

            inputs.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            Control[] fields =
            {
                cmbType, txtAmount, cmbCategory, dtpDate, txtNote
            };

            string[] captions =
            {
                "Type", "Amount ($)", "Category", "Date", "Note"
            };

            for (int i = 0; i < fields.Length; i++)
            {
                inputs.Controls.Add(NewLabel(captions[i]), 0, i);
                fields[i].Margin = new Padding(3, 5, 3, 5);
                inputs.Controls.Add(fields[i], 1, i);
            }

            left.Controls.Add(inputs, 0, 1);

            var actions = NewFlow();
            actions.Controls.AddRange(new Control[]
            {
                btnAdd, btnEdit, btnDelete, btnClear
            });
            left.Controls.Add(actions, 0, 2);

            left.Controls.Add(
                NewLabel("Expenses for the summary month", true),
                0, 3);
            left.Controls.Add(lstCategorySummary, 0, 4);

            body.Controls.Add(left, 0, 0);

            var right = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 7
            };

            right.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 32));
            right.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 44));
            right.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 44));
            right.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));
            right.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 40));
            right.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 48));
            right.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 44));

            right.Controls.Add(
                NewLabel("Transactions and table filters", true),
                0, 0);

            var filters = NewFlow();
            filters.Controls.AddRange(new Control[]
            {
                NewLabel("Type"),
                cmbFilterType,
                NewLabel("Category"),
                cmbFilterCategory,
                btnFilter,
                btnResetFilters
            });
            right.Controls.Add(filters, 0, 1);

            var dates = NewFlow();
            dates.Controls.AddRange(new Control[]
            {
                chkDateRange,
                NewLabel("From"),
                dtpFrom,
                NewLabel("To"),
                dtpTo
            });
            right.Controls.Add(dates, 0, 2);

            right.Controls.Add(dgvTransactions, 0, 3);
            right.Controls.Add(lblRecordCount, 0, 4);

            var summarySelectors = NewFlow();
            summarySelectors.Controls.AddRange(new Control[]
            {
                NewLabel("Monthly summary", true),
                NewLabel("Month"),
                cmbFilterMonth,
                NewLabel("Year"),
                numSummaryYear,
                NewLabel("Click Apply to refresh")
            });
            right.Controls.Add(summarySelectors, 0, 5);

            var totals = NewFlow();
            totals.Controls.AddRange(new Control[]
            {
                lblTotalIncome,
                lblTotalExpense,
                lblNetBalance
            });
            right.Controls.Add(totals, 0, 6);

            body.Controls.Add(right, 1, 0);

            Controls.Add(root);
            ResumeLayout(true);
        }

        private static Label NewLabel(
            string text, bool bold = false)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Margin = new Padding(4, 8, 8, 4),
                Font = new Font(
                    "Segoe UI",
                    10F,
                    bold ? FontStyle.Bold : FontStyle.Regular)
            };
        }

        private static FlowLayoutPanel NewFlow()
        {
            return new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                WrapContents = true,
                AutoScroll = true
            };
        }

        private static Button NewButton(
            string text, System.EventHandler handler)
        {
            var button = new Button
            {
                Text = text,
                Size = new Size(76, 34),
                Margin = new Padding(3)
            };

            button.Click += handler;
            return button;
        }

        private static DateTimePicker NewDatePicker()
        {
            return new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd MMM yyyy",
                Dock = DockStyle.Fill
            };
        }
    }
}