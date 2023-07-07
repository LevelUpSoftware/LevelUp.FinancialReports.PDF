namespace LevelUp.FinancialReports.PDF.SharedModels;

public class BalanceSheetReportOptions
{
    public AccountOrder AccountOrder { get; set; } = AccountOrder.OriginalOrder;
    public short AccountFontSize { get; set; } = 11;
    public short HeaderFontSize { get; set; } = 14;
    public short FooterFontSize { get; set; } = 10;

}