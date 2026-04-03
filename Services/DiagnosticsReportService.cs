namespace CampusRouteLab.Services;

using CampusRouteLab.Services.Interfaces;

public class DiagnosticsReportService
{
    private readonly IAppInfoService _appInfo;
    private readonly IRequestContextService _requestContext;
    private readonly ITransientMarkerService _transient;
    
    public DiagnosticsReportService(
        IAppInfoService appInfo,
        IRequestContextService requestContext,
        ITransientMarkerService transient)
    {
        _appInfo = appInfo;
        _requestContext = requestContext;
        _transient = transient;
    }
    
    public LifetimeReport GetLifetimeReport()
    {
        return new LifetimeReport
        {
            SingletonAppId = _appInfo.AppInstanceId,
            ScopedRequestId = _requestContext.RequestId,
            TransientMarkerId = _transient.MarkerId,
            Source = "DiagnosticsReportService",
            GeneratedAt = DateTime.Now
        };
    }
}

public class LifetimeReport
{
    public Guid SingletonAppId { get; set; }
    public Guid ScopedRequestId { get; set; }
    public Guid TransientMarkerId { get; set; }
    public string Source { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
}