using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BudgetTracker.Models;

namespace BudgetTracker.Data
{
    // Abstraction: Encapsulates file operations away from UI and logic
    public class TransactionRepository
    {
        private readonly string _filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "BudgetTracker", "transactions.json");

        public List<Transaction> LoadTransactions()
        {
            try
            {
                // Read older data written to the working directory if present.
                string path = File.Exists(_filePath) ? _filePath : "transactions.json";
                if (!File.Exists(path))
                    return new List<Transaction>();

                string json = File.ReadAllText(path);
                var dtos = JsonSerializer.Deserialize<List<TransactionDto>>(json);
                var list = new List<Transaction>();

                if (dtos == null) return list;

                foreach (var dto in dtos)
                {
                    if (dto.Type == "Income")
                        list.Add(new IncomeTransaction(dto.Id, dto.Amount, dto.Date, dto.Category, dto.Note));
                    else if (dto.Type == "Expense")
                        list.Add(new ExpenseTransaction(dto.Id, dto.Amount, dto.Date, dto.Category, dto.Note));
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error loading data file: {ex.Message}");
            }
        }

        public void SaveTransactions(List<Transaction> transactions)
        {
            try
            {
                var dtos = transactions.Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Type = t is IncomeTransaction ? "Income" : "Expense",
                    Amount = t.Amount,
                    Date = t.Date,
                    Category = t.Category,
                    Note = t.Note
                }).ToList();

                string json = JsonSerializer.Serialize(dtos, new JsonSerializerOptions { WriteIndented = true });
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error saving data file: {ex.Message}");
            }
        }
    }
}
