using LevelUp.FinancialReports.PDF.PDFReports.BalanceSheet;
using LevelUp.FinancialReports.PDF.SharedModels;
using QuestPDF.Infrastructure;

QuestPDF.Settings.License = LicenseType.Community;

var balanceSheetReport = new BalanceSheetReport("Jellystone Park", ReportMonth.April, 2023);
balanceSheetReport.AddAccount(new BalanceSheetAccount("Petty Cash", BalanceSheetAccountType.Asset, 5500.00M));
balanceSheetReport.AddAccount(new BalanceSheetAccount("Picnic Basket Fund", BalanceSheetAccountType.Asset, 756.21M, 2100));
balanceSheetReport.AddAccount(new BalanceSheetAccount("Park Ranger Fines", BalanceSheetAccountType.Liability, 2752.63M));
var pdf = balanceSheetReport.Compose();
File.WriteAllBytes("C:\\Users\\NathanBurdick\\Downloads\\TestPdf.pdf", pdf);
