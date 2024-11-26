using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace ReportBugInAzureDevOps;

public class AzureDevOpsIssuesClient
{
    readonly string _uri;
    readonly string _personalAccessToken;
    readonly string _project;

    /// <summary>
    /// Constructor. Manually set values to match your organization. 
    /// </summary>
    public AzureDevOpsIssuesClient(
        string organizationName,
        string projectName,
        string personalAccessToken)
    {
        _uri = $"https://dev.azure.com/{organizationName}";
        _project = projectName;
        _personalAccessToken = personalAccessToken;
    }

    /// <summary>
    /// Create an Issue in Azure DevOps
    /// </summary>
    /// <param name="title">Title of issue.</param>
    /// <param name="description">HTML string of description of the issue.</param>
    /// <param name="tags">Tags: e.g. "demo, bug, complaints"</param>
    /// <returns></returns>
    public WorkItem? CreateIssueUsingClientLib(
        string title,
        string description,
        string? tags = "")
    {
        Uri uri = new Uri(_uri);
        string personalAccessToken = _personalAccessToken;
        string project = _project;

        VssBasicCredential credentials = new VssBasicCredential("", _personalAccessToken);
        JsonPatchDocument patchDocument = new JsonPatchDocument();

        //add fields and their values to your patch document
        patchDocument.Add(
            new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/System.Title",
                Value = title
            }
        );

        patchDocument.Add(
            new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/System.Description",
                Value = description
            }
        );
        if (!string.IsNullOrEmpty(tags))
        {

            patchDocument.Add(
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.Tags",
                    Value = tags
                }
            );
        }

        VssConnection connection = new VssConnection(uri, credentials);
        WorkItemTrackingHttpClient workItemTrackingHttpClient = connection.GetClient<WorkItemTrackingHttpClient>();

        try
        {
            WorkItem result = workItemTrackingHttpClient.CreateWorkItemAsync(patchDocument, project, "Issue").Result;

            Console.WriteLine("Issue Successfully Created: Issue: #{0}", result.Id);

            return result;
        }
        catch (AggregateException ex)
        {
            Console.WriteLine("Error creating issue: {0}", ex?.InnerException?.Message);
            return null;
        }
    }
}
