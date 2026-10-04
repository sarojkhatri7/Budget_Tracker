using System;
using System.Windows.Forms;
using BudgetTracker.Forms;

namespace BudgetTracker
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "The application could not start. "
                    + ex.Message
                    + "\n\nIf the data file is damaged, close the app "
                    + "and restore transactions.json from its .bak backup. "
                    + "Keep a copy of the damaged file first.",
                    "Budget Tracker startup error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}