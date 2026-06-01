namespace CarsApp.Domain.Lab7.TemplateMethod;

public record ServiceReportDocument(
    string ReportType,
    IReadOnlyList<string> Sections)
{
    public string Content => string.Join(Environment.NewLine, Sections);
}
