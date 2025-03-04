using System;
using System.ComponentModel;
using System.Security.Policy;

namespace Accede.AzureDevOps.Configuration;

public class AzureDevOpsWorkItemConfiguration
{
    public const string WorkItemConfiguration = "AzureDevOpsWorkItemConfiguration";
    public required string OrganizationName { get; set; }
    public required string ProjectName { get; set; }
    public required string PersonalAccessToken { get; set; }
}

