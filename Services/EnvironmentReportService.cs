namespace StudentPortal.Diagnostics.Services;

public class EnvironmentReportService : IEnvironmentReportService
{
    public string GetAppInfo() => "StudentPortal.Diagnostics v1.0 (Core)";
}