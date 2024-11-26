
using Microsoft.Extensions.Configuration;
using ReportBugInAzureDevOps;

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

var pat = configuration.GetSection("pat")?.Value ?? throw new ArgumentException("No Personal Access Token found");
var orgName = configuration.GetSection("orgName")?.Value ?? throw new ArgumentException("Organization Name was not specified");
var orgProject = configuration.GetSection("orgProject")?.Value ?? throw new ArgumentException("Project Name was not specified");

var issue = new AzureDevOpsIssuesClient(
    orgName,
    orgProject,
    pat);

issue.CreateIssueUsingClientLib(
    "Submit Button Sometimes Inactive on Load",
    """
    <h1>🚨 Submit Button Sometimes Inactive on Load 🛠️</h1>
    <p><strong>Problem Overview:</strong></p>
    <p>The <em>Submit Button</em> occasionally appears <strong>inactive (disabled)</strong> upon page load, preventing users from proceeding without reloading the page.</p>
    
    <h2>🐞 Steps to Reproduce:</h2>
    <ol>
        <li>Navigate to the affected page.</li>
        <li>Observe the <strong>Submit Button</strong> immediately after the page loads.</li>
        <li>Notice that it is sometimes disabled without any interaction.</li>
    </ol>
    
    <h2>✅ Expected Behavior:</h2>
    <p>The <em>Submit Button</em> should be <strong>active</strong> and ready for user interaction as soon as the page loads, provided all prerequisites are met.</p>
    
    <h2>📉 Impact:</h2>
    <ul>
        <li><strong>User frustration:</strong> Inability to complete tasks on the first attempt.</li>
        <li><strong>Reduced efficiency:</strong> Users may need to reload the page or troubleshoot.</li>
        <li><strong>Negative impression:</strong> Affects trust in the app's reliability.</li>
    </ul>
    
    <h2>🌐 Environment:</h2>
    <ul>
        <li>Browser: Google Chrome Version 131.0.6778.86 (Official Build) (arm64)</li>
        <li>OS: macOS 15.1.1 (24B91)</li>
        <li>Application Version: v1.2</li>
    </ul>
    
    <h2>🛠️ Suggested Priority:</h2>
    <p><strong>Low</strong> 🏃‍♂️</p>
    """);


var bug = new AzureDevOpsBugsClient(
    orgName,
    orgProject,
    pat);

bug.CreateBugUsingClientLib(
    "Test Slack Webhook Bug v5",
    
"""
    <ul>
        <li>Complaint 1</li>
        <li>Complaint 2</li>
        <li>Complaint 3</li>
        <li>Complaint 4</li>
        <li>Complaint 5</li>
    </ul>
""",
    "complaints");
