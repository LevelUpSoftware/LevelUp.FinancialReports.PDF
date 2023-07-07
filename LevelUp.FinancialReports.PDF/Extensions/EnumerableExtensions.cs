using LevelUp.FinancialReports.PDF.PDFReports.BalanceSheet;
using LevelUp.FinancialReports.PDF.SharedModels;

namespace LevelUp.FinancialReports.PDF.Extensions;

public static class EnumerableExtensions
{
    public static List<BalanceSheetAccount> OrderBy(this List<BalanceSheetAccount> accounts, AccountOrder order)
    {
        throw new NotImplementedException();
        //switch (order)
        //{
        //    case AccountOrder.AccountNameAscending:
        //        return accounts.OrderBy(x => x.Name).Select(x => x.ChildAccounts.OrderBy(y => y.Name)).ToList();
        //}
    }
}