using System;

namespace BudgetTracker.Models
{
    // Inheritance: Derived class for income transactions
    public class IncomeTransaction : Transaction
    {
        public IncomeTransaction(string id, decimal amount, DateTime date, string category, string note)
            : base(id, amount, date, category, note) { }

        // Polymorphism: Positive balance impact
        public override decimal GetBalanceEffect()
        {
            return Amount;
        }
    }
}
