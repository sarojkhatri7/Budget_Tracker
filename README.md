# Personal Budget Tracker

A C# Windows Forms application for recording income and expenses. It saves transactions as JSON and shows monthly totals and category spending.

## Run

On Windows, install Visual Studio 2022 with the **.NET desktop development** workload and the .NET 8 SDK. Open `BudgetTracker.csproj`, then press **F5**. Alternatively, run `dotnet run --project BudgetTracker.csproj` from this folder on Windows.

Choose an Income or Expense type, enter a positive amount and category, then click Add. Select a row to edit or delete it. The list initially shows all dates; check **Date range** and click Filter to restrict it. The summary month and year selectors control the totals beneath the list.

The JSON file is stored at `%LOCALAPPDATA%\BudgetTracker\transactions.json`. If an older `transactions.json` is present in the working directory, it is read and migrated to that folder on the next save.

## References and Tools Used

This project builds on the student's existing source code. ChatGPT assisted with debugging the form Load event, filtering, persistence path, and Visual Studio project setup. 