using LevelUp.FinancialReports.PDF.PDFReports.BalanceSheet;
using LevelUp.FinancialReports.PDF.SharedModels;
using QuestPDF.Infrastructure;

QuestPDF.Settings.License = LicenseType.Community;

var balanceSheetReport = new BalanceSheetReport("Jellystone Park", ReportMonth.April, 2023);

var currentAssetsAccount = new BalanceSheetAccount("Current Assets", BalanceSheetAccountType.Asset);
currentAssetsAccount.AddChildAccount(new BalanceSheetAccount("Petty Cash", BalanceSheetAccountType.Asset, 5500.21M));
currentAssetsAccount.AddChildAccount(new BalanceSheetAccount("Picnic Basket Fund", BalanceSheetAccountType.Asset, 756.21M, 2100));
balanceSheetReport.AddAccount(currentAssetsAccount);

balanceSheetReport.AddAccount(new BalanceSheetAccount("Park Ranger Fines", BalanceSheetAccountType.Liability, 2752.63M));

var pdf = balanceSheetReport.Compose();
var userHomePath = Environment.GetEnvironmentVariable("HOMEPATH");
File.WriteAllBytes($"{userHomePath}\\Downloads\\TestPdf.pdf", pdf);
