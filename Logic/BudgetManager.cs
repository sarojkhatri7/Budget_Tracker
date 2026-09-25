using System;
using System.Collections.Generic;
using System.Linq;
using BudgetTracker.Data;
using BudgetTracker.Models;

namespace BudgetTracker.Logic
{
    public class BudgetManager
    {
        private readonly List<Transaction> _transactions;
        private readonly TransactionRepository _repository;

        public BudgetManager()
        {
            _repository = new TransactionRepository();
            _transactions = _repository.LoadTransactions();
        }

        // FR1 - Add & Validation (FR8)
        public void AddTransaction(string type, decimal amount, DateTime date, string category, string note)
        {
            ValidateInput(amount, type, category);

            Transaction t = type == "Income"
                ? new IncomeTransaction(null, amount, date, category, note)
                : new ExpenseTransaction(null, amount, date, category, note);

            _transactions.Add(t);
            _repository.SaveTransactions(_transactions);
        }

        // FR3 - Edit
        public void EditTransaction(string id, string type, decimal amount, DateTime date, string category, string note)
        {
            ValidateInput(amount, type, category);

            var existing = _transactions.FirstOrDefault(t => t.Id == id);
            if (existing == null)
                throw new KeyNotFoundException("Transaction not found.");

            _transactions.Remove(existing);

            Transaction updated = type == "Income"
                ? new IncomeTransaction(id, amount, date, category, note)
                : new ExpenseTransaction(id, amount, date, category, note);

            _transactions.Add(updated);
            _repository.SaveTransactions(_transactions);
        }

        // FR4 - Delete
        public void DeleteTransaction(string id)
        {
            var t = _transactions.FirstOrDefault(x => x.Id == id);
            if (t != null)
            {
                _transactions.Remove(t);
                _repository.SaveTransactions(_transactions);
            }
        }

        // FR2 & FR7 - View & Filter (Newest first)
        public List<TransactionViewDto> GetFilteredTransactions(int? month, int? year, string type, string category,
            DateTime? fromDate = null, DateTime? toDate = null)
        {
            IEnumerable<Transaction> query = _transactions;

            if (year.HasValue && year.Value > 0)
                query = query.Where(t => t.Date.Year == year.Value);

            if (month.HasValue && month.Value > 0)
                query = query.Where(t => t.Date.Month == month.Value);

            if (fromDate.HasValue && toDate.HasValue && fromDate.Value.Date > toDate.Value.Date)
                throw new ArgumentException("Start date must be on or before end date.");
            if (fromDate.HasValue)
                query = query.Where(t => t.Date.Date >= fromDate.Value.Date);
            if (toDate.HasValue)
                query = query.Where(t => t.Date.Date <= toDate.Value.Date);

            if (!string.IsNullOrEmpty(type) && type != "All")
            {
                if (type == "Income") query = query.OfType<IncomeTransaction>();
                else if (type == "Expense") query = query.OfType<ExpenseTransaction>();
            }

            if (!string.IsNullOrEmpty(category) && category != "All")
                query = query.Where(t => string.Equals(t.Category, category, StringComparison.OrdinalIgnoreCase));

            return query.OrderByDescending(t => t.Date)
                .Select(t => new TransactionViewDto
                {
                    Id = t.Id,
                    Type = t is IncomeTransaction ? "Income" : "Expense",
                    Amount = t.Amount,
                    Date = t.Date.ToShortDateString(),
                    Category = t.Category,
                    Note = t.Note,
                    BalanceEffect = t.GetBalanceEffect() // Demonstrating polymorphism
                }).ToList();
        }

        // FR5 - Monthly Summary
        public (decimal TotalIncome, decimal TotalExpense, decimal NetBalance) GetMonthlySummary(int month, int year)
        {
            var monthly = _transactions.Where(t => t.Date.Month == month && t.Date.Year == year).ToList();
            decimal income = monthly.OfType<IncomeTransaction>().Sum(t => t.Amount);
            decimal expense = monthly.OfType<ExpenseTransaction>().Sum(t => t.Amount);
            decimal net = monthly.Sum(t => t.GetBalanceEffect()); // Polymorphic call

            return (income, expense, net);
        }

        // FR6 - Categorize Total Expenses
        public Dictionary<string, decimal> GetCategoryExpenses(int month, int year)
        {
            return _transactions
                .Where(t => t.Date.Month == month && t.Date.Year == year)
                .OfType<ExpenseTransaction>()
                .GroupBy(t => t.Category)
                .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));
        }

        // FR8 - Validation
        private void ValidateInput(decimal amount, string type, string category)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");
            if (type != "Income" && type != "Expense")
                throw new ArgumentException("Please select a transaction type (Income/Expense).");
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Please select or enter a category.");
        }
    }

    public class TransactionViewDto
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Date { get; set; }
        public string Category { get; set; }
        public string Note { get; set; }
        public decimal BalanceEffect { get; set; }
    }
}
