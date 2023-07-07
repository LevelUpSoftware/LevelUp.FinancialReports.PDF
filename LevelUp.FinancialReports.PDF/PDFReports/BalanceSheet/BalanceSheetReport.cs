using System.Diagnostics.CodeAnalysis;
using System.Text;
using LevelUp.FinancialReports.PDF.SharedModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LevelUp.FinancialReports.PDF.PDFReports.BalanceSheet;

public class BalanceSheetReport
{
    private Document? _pdfDocument;
    private readonly float _pageMarginInches = (float).5;

    public string OrganizationName { get; }
    public ReportMonth ReportMonth { get; }
    public int ReportYear { get; }
    public int AccountFontSize { get; set; } = 11;
    public List<BalanceSheetAccount> Accounts { get; } = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationName"></param>
    /// <param name="reportMonth"></param>
    /// <param name="reportYear"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public BalanceSheetReport(string organizationName, ReportMonth reportMonth, int reportYear)
    {
        if (string.IsNullOrEmpty(organizationName))
        {
            throw new ArgumentNullException(nameof(organizationName), "A valid company name is required.");
        }

        validateReportYear(reportYear);

        ReportMonth = reportMonth;
        OrganizationName = organizationName;
        ReportYear = reportYear;
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
                    text.CurrentPageNumber();
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
            column.Item().AlignCenter().Text(OrganizationName).Bold();
            column.Item().AlignCenter().Text("Balance Sheet");
            column.Item().AlignCenter().Text($"{ReportMonth.ToString()} {ReportYear.ToString()}");
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
                    .Underline();
            });
            foreach (var account in Accounts.Where(x => x.AccountType == BalanceSheetAccountType.Asset))
            {
                column.Item().Row(row =>
                {
                    if (Accounts.Any(x => x.AccountNumber is > 0))
                    {
                        var maxAccountNumberLength = 75;
                        row.ConstantItem(maxAccountNumberLength)
                            .Text(account.AccountNumber?.ToString() ?? "--");
                    }
                    row.ConstantItem(125).AlignLeft().Text(account.Name);
                    row.ConstantItem(50).AlignLeft().Text(account.Balance.ToString());
                    row.RelativeItem().EnsureSpace();
                });
            }

            column.Item()
                .PaddingVertical(10)
                .Row(row =>
            {
                row.RelativeItem()
                    .Text("Liabilities")
                    .Bold()
                    .Underline();
            });

            foreach (var account in Accounts.Where(x => x.AccountType == BalanceSheetAccountType.Liability))
            {
                column.Item().Row(row =>
                {
                    if (Accounts.Any(x => x.AccountNumber is > 0))
                    {
                        var maxAccountNumberLength = 75;
                        row.ConstantItem(maxAccountNumberLength)
                            .Text(account.AccountNumber?.ToString() ?? "--");
                    }
                    row.ConstantItem(125).AlignLeft().Text(account.Name);
                    row.ConstantItem(50).AlignLeft().Text(account.Balance.ToString());
                    row.RelativeItem().EnsureSpace();
                });
            }

            column.Item()
                .PaddingVertical(10)
                .Row(row =>
                {
                    var netWorth =
                        (Accounts.Where(x => x.AccountType == BalanceSheetAccountType.Asset).Select(x => x.Balance)
                            .Sum()) - (Accounts.Where(x => x.AccountType == BalanceSheetAccountType.Liability)
                            .Select(x => x.Balance).Sum());
                    row.RelativeItem()
                        .Text($"Balance: ${Math.Round(netWorth), 2}")
                        .Underline();
                });
        });
    }
    private void validateReportYear(int year)
    {
        if (year.ToString().Length < 4)
        {
            throw new ArgumentException("Year must be four digits in length.");
        }
    }
}