using System;

namespace BudgetTracker.Data
{
    // Data Transfer Object for JSON persistence
    public class TransactionDto
    {
        public string Id { get; set; }
        public string Type { get; set; } // "Income" or "Expense"
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string Note { get; set; }
    }
}
