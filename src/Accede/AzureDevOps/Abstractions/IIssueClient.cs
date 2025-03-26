using System;
using Accede.AzureDevOps.Models;
using Microsoft.AspNetCore.Http;

namespace Accede.AzureDevOps.Accede.AzureDevOps.Abstractions;

public interface IIssueClient
{
    Task<int?> CreateIssueAsync(IssueInput input);
    Task<int?> CreateIssueWithAttachmentAsync(IssueInput input, IFormFile[] file);
}
