using Accede.AzureDevOps.Accede.AzureDevOps.Abstractions;
using Accede.AzureDevOps.Configuration;
using Accede.AzureDevOps.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace  Accede.AzureDevOps.Clients;

public class AzureDevOpsIssuesClient(
    IOptions<AzureDevOpsWorkItemConfiguration> configuration,
    ILogger<AzureDevOpsIssuesClient> logger
    )
    : BaseClient(configuration.Value), IIssueClient
{
    private readonly ILogger<AzureDevOpsIssuesClient> _logger = logger;

    public async Task<int?> CreateIssueAsync(IssueInput input)
    {
        try
        {
            var workItemId = await base.CreateIssueAsync(input);

            _logger.LogInformation("Issue Successfully Created: Issue: #{workItemId}", workItemId);

            return workItemId;
        }
        catch (AggregateException ex)
        {
            _logger.LogError("Error creating issue: {Error.Message}", ex?.InnerException?.Message);
            return null;
        }
    }

    public async new Task<int?> CreateIssueWithAttachmentAsync(IssueInput input, IFormFile file)
    {
        try
        {
            var workItemId = await base.CreateIssueWithAttachmentAsync(input, file);

            _logger.LogInformation("Issue Successfully Created: Issue: #{workItemId}", workItemId);

            return workItemId;
        }
        catch (AggregateException ex)
        {
            _logger.LogError("Error creating issue with attachment: {Error.Message}", ex?.InnerException?.Message);
            return null;
        }
    }
}
