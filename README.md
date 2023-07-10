# LevelUp.FinancialReports.PDF
A .NET Core library for automatically generating financial reports in PDF format.

## BalanceSheetReport
```
var balanceSheetReport = new BalanceSheetReport("Jellystone Park", ReportMonth.February, 2008);

var assetAccount = new BalanceSheetAccount("CurrentAssets", BalanceSheetAccountType.Asset);
assetaccount.AddChildAccount(new BalanceSheetAccount("Petty Cash", BalanceSheetAccountType.Asset, 2522.71M, 1031791);
balanceSheetReport.AddAccount(assetAccount);

var pdfByteArray = balanceSheetReport.Compose();


```
