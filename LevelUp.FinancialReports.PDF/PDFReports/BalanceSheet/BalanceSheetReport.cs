using LevelUp.FinancialReports.PDF.Extensions;
using LevelUp.FinancialReports.PDF.SharedModels;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace LevelUp.FinancialReports.PDF.PDFReports.BalanceSheet;

public class BalanceSheetReport
{
    private Document? _pdfDocument;

    public string OrganizationName { get; }
    public ReportMonth ReportMonth { get; }
    public int ReportYear { get; }
    public List<BalanceSheetAccount> Accounts { get; }
    public BalanceSheetReportOptions ReportOptions { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationName"></param>
    /// <param name="reportMonth"></param>
    /// <param name="reportYear"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public BalanceSheetReport(string organizationName, ReportMonth reportMonth, int reportYear, BalanceSheetReportOptions? options = null)
    {
        if (string.IsNullOrEmpty(organizationName))
        {
            throw new ArgumentNullException(nameof(organizationName), "A valid company name is required.");
        }

        validateReportYear(reportYear);

        ReportMonth = reportMonth;
        OrganizationName = organizationName;
        ReportYear = reportYear;

        Accounts = new();
        ReportOptions = options ?? new BalanceSheetReportOptions();
    }

    public BalanceSheetReport AddAccount(BalanceSheetAccount account)
    {
        Accounts.Add(account);
        return this;
    }

    public byte[] Compose()
    {
        _pdfDocument = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(50);
                composeHeader(page.Header());
                composeBalanceTable(page.Content().PaddingVertical(20));
                page.Footer().AlignCenter().Text(text =>
                {
                    text.CurrentPageNumber().FontSize(ReportOptions.FooterFontSize);
                });
            });

        });

        return _pdfDocument.GeneratePdf();
    }

    private void composeHeader(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(1);
            column.Item()
                .AlignCenter()
                .Text(OrganizationName)
                .Bold()
                .FontSize(ReportOptions.HeaderFontSize);
            column.Item()
                .AlignCenter()
                .Text("Balance Sheet")
                .FontSize(ReportOptions.HeaderFontSize);
            column.Item()
                .AlignCenter()
                .Text($"{ReportMonth.ToString()} {ReportYear.ToString()}")
                .FontSize(ReportOptions.HeaderFontSize);
        });
    }

    private void composeBalanceTable(IContainer container)
    {
        container.Padding(10);
        container.Column(column =>
        {
            column.Spacing(1);
            column.Item()
                .PaddingVertical(10)
                .Row(row =>
            {
                row.RelativeItem()
                    .Text("Assets")
                    .Bold()
                    .Underline()
                    .FontSize(ReportOptions.AccountFontSize);
            });
            foreach (var account in Accounts.Where(x => x.AccountType == BalanceSheetAccountType.Asset))
            {
                column.CreateRow(account, Accounts.Any(x => x.AccountNumber is > 0), ReportOptions.AccountFontSize);
            }

            column.Item()
                .PaddingVertical(10)
                .Row(row =>
            {
                row.RelativeItem()
                    .Text("Liabilities")
                    .Bold()
                    .Underline()
                    .FontSize(ReportOptions.AccountFontSize);
            });

            foreach (var account in Accounts.Where(x => x.AccountType == BalanceSheetAccountType.Liability))
            {
                column.CreateRow(account, Accounts.Any(x => x.AccountNumber is > 0), ReportOptions.AccountFontSize);
            }

            column.Item()
                .PaddingVertical(10)
                .Row(row =>
                {
                    var netWorth = getNetWorth();
                        row.RelativeItem()
                        .Text($"Balance: ${Math.Round(netWorth, 2)}")
                        .Underline().FontSize(ReportOptions.AccountFontSize);
                });
        });
    }

    private decimal getNetWorth()
    {
        var assets = Accounts.Where(x => x.AccountType == BalanceSheetAccountType.Asset).ToList();
        var assetSum = getAccountTotals(assets);

        var liabilities = Accounts.Where(x => x.AccountType == BalanceSheetAccountType.Liability).ToList();
        var liabilitySum = getAccountTotals(liabilities);

        var netWorth = assetSum - liabilitySum;
        return netWorth;
    }

    private decimal getAccountTotals(List<BalanceSheetAccount> accounts)
    {
        var accountSum = accounts.Where(x => x.Balance != null && x.Balance != 0).Select(x => x.Balance!.Value).Sum();
        var totalSum = accountSum;
        foreach (var account in accounts)
        {
            var childAccountSum = getAccountTotals(account.ChildAccounts);
            totalSum += childAccountSum;
        }

        return totalSum;
    }
    private void validateReportYear(int year)
    {
        if (year.ToString().Length < 4)
        {
            throw new ArgumentException("Year must be four digits in length.");
        }
    }
}