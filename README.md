# LevelUp.FinancialReports.PDF
A .NET Core library for easily generating financial reports in PDF format.

## Dependencies
This library uses QuestPDF to generate PDF documents. Out of respect for their license and usage policies, you will need to set a QuestPDF license type in order to generate PDF reports using this library.
You can read more about the different types of licenses they offer [here](https://www.questpdf.com/license/guide.html) and how to set the license selection [here](https://www.questpdf.com/license/configuration.html).

## Reports

### BalanceSheetReport
_Creates a basic balance sheet report._

Example:
```
var balanceSheetReport = new BalanceSheetReport("Jellystone Park", ReportMonth.February, 2008);

var assetAccount = new BalanceSheetAccount("CurrentAssets", BalanceSheetAccountType.Asset);
assetaccount.AddChildAccount(new BalanceSheetAccount("Petty Cash", BalanceSheetAccountType.Asset, 2522.71M, 1031791);
balanceSheetReport.AddAccount(assetAccount);

var pdfByteArray = balanceSheetReport.Compose();
File.WriteAllBytes($"C:\\Users\\JohnSmith\\Documents\\TestPdf.pdf", pdfByteArray);

```
