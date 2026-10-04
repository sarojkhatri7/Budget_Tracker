using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BudgetTracker.Models;

namespace BudgetTracker.Data
{
    public class TransactionRepository
    {
        private readonly string _filePath;
        private readonly bool _allowLegacyPath;

        public string DataFilePath => _filePath;

        public TransactionRepository(string filePath = null)
        {
            _allowLegacyPath = filePath == null;

            _filePath = Path.GetFullPath(
                filePath ?? Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData),
                    "BudgetTracker",
                    "transactions.json"));
        }

        public List<Transaction> LoadTransactions()
        {
            try
            {
                string path = _filePath;

                // Read transactions.json from the old working folder if needed.
                if (!File.Exists(path) &&
                    _allowLegacyPath &&
                    File.Exists("transactions.json"))
                {
                    path = Path.GetFullPath("transactions.json");
                }

                if (!File.Exists(path))
                    return new List<Transaction>();

                var dtos =
                    JsonSerializer.Deserialize<List<TransactionDto>>(
                        File.ReadAllText(path));

                if (dtos == null)
                    throw new InvalidDataException(
                        "The data file must contain a JSON array.");

                var transactions = new List<Transaction>();
                var ids = new HashSet<string>(StringComparer.Ordinal);

                foreach (var dto in dtos)
                {
                    if (dto == null ||
                        string.IsNullOrWhiteSpace(dto.Id) ||
                        !ids.Add(dto.Id))
                    {
                        throw new InvalidDataException(
                            "A transaction has a missing or duplicate ID.");
                    }

                    if (dto.Type != "Income" && dto.Type != "Expense")
                    {
                        throw new InvalidDataException(
                            "A transaction has an unknown type.");
                    }

                    Transaction transaction = dto.Type == "Income"
                        ? new IncomeTransaction(
                            dto.Id, dto.Amount, dto.Date,
                            dto.Category, dto.Note)
                        : new ExpenseTransaction(
                            dto.Id, dto.Amount, dto.Date,
                            dto.Category, dto.Note);

                    transactions.Add(transaction);
                }

                return transactions;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Cannot load transactions. The data file has not been "
                    + "changed. " + ex.Message,
                    ex);
            }
        }

        public void SaveTransactions(List<Transaction> transactions)
        {
            string temporaryPath =
                _filePath + "." + Guid.NewGuid().ToString("N") + ".tmp";

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

                string json = JsonSerializer.Serialize(
                    dtos,
                    new JsonSerializerOptions { WriteIndented = true });

                Directory.CreateDirectory(
                    Path.GetDirectoryName(_filePath));

                // Finish the temporary file before replacing the saved file.
                File.WriteAllText(temporaryPath, json);

                if (File.Exists(_filePath))
                {
                    File.Replace(
                        temporaryPath,
                        _filePath,
                        _filePath + ".bak");
                }
                else
                {
                    File.Move(temporaryPath, _filePath);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Cannot save transactions. Your change was not applied. "
                    + ex.Message,
                    ex);
            }
            finally
            {
                try
                {
                    if (File.Exists(temporaryPath))
                        File.Delete(temporaryPath);
                }
                catch
                {
                    // Do not hide the original save error.
                }
            }
        }
    }
}