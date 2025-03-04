using Accede.AzureDevOps.Accede.AzureDevOps.Abstractions;
using Accede.AzureDevOps.Clients;
using Accede.AzureDevOps.Configuration;
using Microsoft.Extensions.Configuration;
using ReportBugInAzureDevOps.Accede.AzureDevOps.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddAzureDevOpsWorkItemTracking(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureDevOpsWorkItemConfiguration>(configuration.GetSection(AzureDevOpsWorkItemConfiguration.WorkItemConfiguration));
        services.AddScoped<IBugClient, AzureDevOpsBugsClient>();
        services.AddScoped<IIssueClient, AzureDevOpsIssuesClient>();
    }

}
