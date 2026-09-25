using System;

namespace BudgetTracker.Models
{
    // Abstraction & Encapsulation: Abstract base class with private setters
    public abstract class Transaction
    {
        public string Id { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Date { get; private set; }
        public string Category { get; private set; }
        public string Note { get; private set; }

        protected Transaction(string id, decimal amount, DateTime date, string category, string note)
        {
            Id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            Amount = amount;
            Date = date;
            Category = category;
            Note = note;
        }

        // Polymorphism: Derived classes override this to alter balance calculations
        public abstract decimal GetBalanceEffect();

        public void UpdateDetails(decimal amount, DateTime date, string category, string note)
        {
            Amount = amount;
            Date = date;
            Category = category;
            Note = note;
        }
    }
}
