using Accede.AzureDevOps.Models;
using Microsoft.AspNetCore.Http;

namespace ReportBugInAzureDevOps.Accede.AzureDevOps.Abstractions;

public interface IBugClient
{
    Task<int?> CreateBugAsync(BugInput input);
    Task<int?> CreateBugWithAttachments(BugInput input, IFormFile[] file);
}
