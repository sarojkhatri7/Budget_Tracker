using System;

namespace BudgetTracker.Models
{
    // Inheritance: Derived class for expense transactions
    public class ExpenseTransaction : Transaction
    {
        public ExpenseTransaction(string id, decimal amount, DateTime date, string category, string note)
            : base(id, amount, date, category, note) { }

        // Polymorphism: Negative balance impact
        public override decimal GetBalanceEffect()
        {
            return -Amount;
        }
    }
}
