using Accede.AzureDevOps.Configuration;
using Accede.AzureDevOps.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace Accede.AzureDevOps.Clients;

public abstract class BaseClient(AzureDevOpsWorkItemConfiguration configuration)
{
    private readonly AzureDevOpsWorkItemConfiguration config = configuration;
    public WorkItemTrackingHttpClient? Client { get; set; }

    public WorkItemTrackingHttpClient CreateClient()
    {
        Uri uri = new($"https://dev.azure.com/{config.OrganizationName}");
        VssBasicCredential credentials = new("", config.PersonalAccessToken);
        VssConnection connection = new(uri, credentials);
        return connection.GetClient<WorkItemTrackingHttpClient>();
    }

    public async Task<int?> CreateBugAsync(BugInput input, bool hasAttachment = false)
    {
        var client = Client ?? CreateClient();

        JsonPatchDocument patchDocument =
        [
            //add fields and their values to your patch document
            new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/System.Title",
                Value = input.Title
            },
            new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/Microsoft.VSTS.TCM.ReproSteps",
                Value = input.StepToReproduce
            },
            new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/Microsoft.VSTS.Common.Priority",
                Value = $"{(int)input.Priority}"
            },
            new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/Microsoft.VSTS.Common.Severity",
                Value = input.Severity switch {
                    Severity.Critical => "1 - Critical",
                    Severity.High => "2 - High",
                    Severity.Medium => "3 - Medium",
                    _ => "4 - Low"
                }
            },
            new JsonPatchOperation(){
                Operation = Operation.Add,
                Path = "/fields/System.AssignedTo",
                Value = ""
            }
        ];

        if (string.IsNullOrEmpty(input.AssignTo))
        {
            patchDocument.Add(new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/System.AssignedTo",
                Value = input.AssignTo
            });
        }

        if (!string.IsNullOrEmpty(input.Tags?.Trim()))
        {
            patchDocument.Add(
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.Tags",
                    Value = input.Tags
                }
            );
        }

        if (hasAttachment)
        {
            patchDocument.Add(new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/Microsoft.VSTS.TCM.ReproSteps",
                Value = "Please find attached screenshots of the issue reported"
            });
        }

        WorkItem? workItem = await client.CreateWorkItemAsync(patchDocument, config.ProjectName, "Bug");
        return workItem?.Id;
    }

    public async Task<int?> CreateBugWithAttachment(BugInput input, IFormFile file)
    {
        var workItemId = await CreateBugAsync(input, true);
        if (workItemId == null) return null;

        var attachmentLinkUrl = await CreateAttachment(file);
        if (string.IsNullOrEmpty(attachmentLinkUrl)) return null;

        return await AddAttachmentToWorkItem(workItemId.Value, attachmentLinkUrl, input.Comments);
    }

    public async Task<int?> CreateIssueAsync(IssueInput input, bool hasAttachment = false)
    {
        var client = Client ?? CreateClient();

        JsonPatchDocument patchDocument =
       [
           //add fields and their values to your patch document
           new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/System.Title",
                Value = input.Title
            }
,
            new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/System.Description",
                Value = input.Description
            }
,
        ];

        if (!string.IsNullOrEmpty(input.Tags))
        {

            patchDocument.Add(
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.Tags",
                    Value = input.Tags
                }
            );
        }

        if (string.IsNullOrEmpty(input.AssignTo))
        {
            patchDocument.Add(new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/System.AssignedTo",
                Value = input.AssignTo
            });
        }

        WorkItem workItem = await client.CreateWorkItemAsync(patchDocument, config.ProjectName, "Issue");
        return workItem?.Id;
    }

    public async Task<int?> CreateIssueWithAttachmentAsync(IssueInput input, IFormFile file)
    {
        var workItemId = await CreateIssueAsync(input, true);
        if (workItemId == null) return null;

        var attachmentLinkUrl = await CreateAttachment(file);
        if (string.IsNullOrEmpty(attachmentLinkUrl)) return null;

        return await AddAttachmentToWorkItem(workItemId.Value, attachmentLinkUrl, input.Comments);
    }

    #region Helper Methods

    private async Task<string?> CreateAttachment(IFormFile file)
    {

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0;
        var client = Client ?? CreateClient();
        AttachmentReference? attachment = await client.CreateAttachmentAsync(
            uploadStream: stream,
            project: config.ProjectName,
            fileName: file.FileName,
            areaPath: null,
            uploadType: UploadType.Simple.ToString(),
             userState: null);
        return attachment?.Url;
    }

    private async Task<int?> AddAttachmentToWorkItem(int workItemId, string attachmentLinkUrl, string? comments = "")
    {
        var client = Client ?? CreateClient();
        JsonPatchDocument? patchDocument =
        [
            new()
            {
                Operation = Operation.Add,
                Path = "/relations/-",
                Value = new
                {
                    Rel = "AttachedFile",
                    Url = attachmentLinkUrl,
                    Attributes = new
                    {
                        Comment = comments
                    }
                }
            }
        ];
        var workItem = await client.UpdateWorkItemAsync(
            document: patchDocument,
            project: config.ProjectName,
            id: workItemId,
            validateOnly: false,
            bypassRules: false,
            suppressNotifications: true,
            expand: null);
        return workItem.Id;
    }

    private static IFormFile CreateIFormFile(string filePath)
    {
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        return new FormFile(fileStream, 0, fileStream.Length, "file", Path.GetFileName(filePath))
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png" // Change if needed
        };
    }
    #endregion
}

