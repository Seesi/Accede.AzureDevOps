namespace Accede.AzureDevOps.Models;

public record BugInput
{
    /// <summary>
    /// Title of the bug
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Priority of the bug
    /// </summary>
    public Priority Priority { get; set; }

    /// <summary>
    /// Severity of the bug.
    /// </summary>
    public Severity Severity { get; set; }

    /// <summary>
    /// Html or Markdown text of the steps to reproduce the bug
    /// </summary>
    public required string StepToReproduce { get; set; }

    /// <summary>
    /// Tags: e.g. "demo, bug, complaints"
    /// </summary>
    public string? Tags { get; set; }

    /// <summary>
    /// Optional comments on the bug
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    /// Optional - The name of the team member the work item should be assigned to
    /// </summary>
    public string? AssignTo { get; set; }
}

