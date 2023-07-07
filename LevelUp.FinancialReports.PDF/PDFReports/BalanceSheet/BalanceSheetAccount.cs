using LevelUp.FinancialReports.PDF.SharedModels;

namespace LevelUp.FinancialReports.PDF.PDFReports.BalanceSheet;

public class BalanceSheetAccount
{
    public string Name { get; }
    public decimal? Balance { get; }
    public BalanceSheetAccountType AccountType { get; }
    public int? AccountNumber { get; }
    public List<BalanceSheetAccount> ChildAccounts { get; }

    /// <summary>
    /// Create a new balance sheet account for display on the balance sheet.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="accountType"></param>
    /// <param name="balance"></param>
    /// <param name="accountNumber"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public BalanceSheetAccount(string name, BalanceSheetAccountType accountType, decimal? balance = null, int? accountNumber = null)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name), "A valid account name is required.");
        }

        Name = name;
        AccountType = accountType;
        Balance = balance;
        AccountNumber = accountNumber;
        ChildAccounts = new List<BalanceSheetAccount>();
    }

    /// <summary>
    /// Add child accounts to be displayed under a parent account on the balance sheet.
    /// </summary>
    /// <param name="childAccount"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddChildAccount(BalanceSheetAccount childAccount)
    {
        if (childAccount.AccountType != AccountType)
        {
            throw new InvalidOperationException(
                $"Child account {childAccount.Name} type {childAccount.AccountType} does not match type {AccountType} of parent account {Name}.");
        }

        if (ChildAccounts.Any(x => x.Name == childAccount.Name))
        {
            throw new InvalidOperationException($"A child account with name {childAccount.Name} already exists.");
        }

        ChildAccounts.Add(childAccount);
    }
}