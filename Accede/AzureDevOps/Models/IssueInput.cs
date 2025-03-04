namespace Accede.AzureDevOps.Models;

public record IssueInput
{
    /// <summary>
    /// Title of issue or complaint
    /// </summary>
    public required string Title { get; set; }
    /// <summary>
    /// Description of the issue or complaint
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Optional comments on the bug
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    /// Tags: e.g. "demo, bug, complaints"
    /// </summary>
    public string? Tags { get; internal set; }

    /// <summary>
    /// Optional - The name of the team member the work item should be assigned to
    /// </summary>
    public string? AssignTo { get; set; }
}

