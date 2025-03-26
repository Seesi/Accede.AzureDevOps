using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ReportBugInAzureDevOps.Accede.AzureDevOps.Abstractions;
using Accede.AzureDevOps.Configuration;
using Accede.AzureDevOps.Models;
using Microsoft.Extensions.Logging;

namespace Accede.AzureDevOps.Clients;


public class AzureDevOpsBugsClient(
    IOptions<AzureDevOpsWorkItemConfiguration> configuration,
    ILogger<AzureDevOpsBugsClient> logger
    )
    : BaseClient(configuration.Value), IBugClient
{
    private readonly ILogger<AzureDevOpsBugsClient> _logger = logger;

    public new async Task<int?> CreateBugWithAttachments(BugInput input, IFormFile[] files)
    {
        try
        {
            var workItemId = await base.CreateBugWithAttachments(input, files);
            _logger.LogInformation("Bug Successfully Created: Bug #{workItemId}", workItemId);
            return workItemId;
        }
        catch (AggregateException ex)
        {
            _logger.LogError("Error creating bug with attachment: {Error.Message}", ex?.InnerException?.Message);
            return null;
        }
    }

    public async Task<int?> CreateBugAsync(BugInput input)
    {
        try
        {
            var workItemId = await base.CreateBugAsync(input);
            _logger.LogInformation("Bug Successfully Created: Bug #{workItemId}", workItemId);
            return workItemId;
        }
        catch (AggregateException ex)
        {
             _logger.LogError("Error creating bug: {Error.Message}", ex?.InnerException?.Message);
            return null;
        }
    }
}
