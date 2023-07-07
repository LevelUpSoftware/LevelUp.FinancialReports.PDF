using LevelUp.FinancialReports.PDF.PDFReports.BalanceSheet;
using QuestPDF.Fluent;
using System.Runtime.Serialization;

namespace LevelUp.FinancialReports.PDF.Extensions;

public static class ColumnDescriptorExtensions
{
    public static void CreateRow(this ColumnDescriptor column, BalanceSheetAccount account, bool accountNumbersExist, short fontSize, int nestLevel = 0)
    {
        column.Item().Row(row =>
        {
            if (accountNumbersExist)
            {
                var maxAccountNumberLength = 75;
                row.ConstantItem(maxAccountNumberLength)
                    .Text(account.AccountNumber?.ToString() ?? "--").FontSize(fontSize);
            }

            if (nestLevel > 0)
            {
                row.ConstantItem(5 * nestLevel).AlignLeft().Text(string.Empty);
            }

            if (account.Balance.HasValue)
            {
                row.ConstantItem(125).AlignLeft().Text(account.Name).FontSize(fontSize);
            }
            else
            {
                row.ConstantItem(125).AlignLeft().Text(account.Name).FontSize(fontSize).Bold();
            }

           

            row.ConstantItem(50).AlignLeft().Text(account.Balance.ToString()).FontSize(fontSize);
            row.RelativeItem().EnsureSpace();

            foreach (var childAccount in account.ChildAccounts)
            {
                CreateRow(column, childAccount, accountNumbersExist, fontSize, nestLevel + 1);
            }
        });
    }
}