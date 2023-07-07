using LevelUp.FinancialReports.PDF.SharedModels;

namespace LevelUp.FinancialReports.PDF.PDFReports.BalanceSheet;

public class BalanceSheetAccount
{
    public string Name { get; }
    public decimal Balance { get; }
    public BalanceSheetAccountType AccountType { get; }
    public int? AccountNumber { get; }

    public BalanceSheetAccount(string name, BalanceSheetAccountType accountType, decimal balance, int? accountNumber = null)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name), "A valid account name is required.");
        }

        Name = name;
        AccountType = accountType;
        Balance = balance;
        AccountNumber = accountNumber;
    }
}