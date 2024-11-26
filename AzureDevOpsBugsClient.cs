using System;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace ReportBugInAzureDevOps;

public class AzureDevOpsBugsClient
{
    readonly string _uri;
    readonly string _personalAccessToken;
    readonly string _project;

    /// <summary>
    /// Constructor. Manually set values to match your organization. 
    /// </summary>
    public AzureDevOpsBugsClient(string organizationName,string projectName,string personalAccessToken)
    {
        _uri = $"https://dev.azure.com/{organizationName}";
        _project = projectName;
        _personalAccessToken = personalAccessToken;
    }

     
    /// <summary>
    /// Create a bug in Azure DevOps
    /// </summary>
    /// <param name="title">Title of bug</param>
    /// <param name="stepToReproduce">HTML string of steps to reproduce bug</param>
    /// <returns></returns>
    public WorkItem? CreateBugUsingClientLib(string title,string stepToReproduce)
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
                Path = "/fields/Microsoft.VSTS.TCM.ReproSteps",
                Value = stepToReproduce
            }
        );

        patchDocument.Add(
            new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/Microsoft.VSTS.Common.Priority",
                Value = "1"
            }
        );

        patchDocument.Add(
            new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/Microsoft.VSTS.Common.Severity",
                Value = "2 - High"
            }
        );
        VssConnection connection = new VssConnection(uri, credentials);
        WorkItemTrackingHttpClient workItemTrackingHttpClient = connection.GetClient<WorkItemTrackingHttpClient>();

        try
        {
            WorkItem? result = workItemTrackingHttpClient.CreateWorkItemAsync(patchDocument, project, "Bug").Result;

            Console.WriteLine("Bug Successfully Created: Bug #{0}", result.Id);

            return result;
        }
        catch (AggregateException ex)
        {
            Console.WriteLine("Error creating bug: {0}", ex?.InnerException?.Message);
            return null;
        }
    }
}
