
using Microsoft.Extensions.Configuration;
using ReportBugInAzureDevOps;

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

var pat = configuration.GetSection("pat")?.Value ?? throw new ArgumentException("No Personal Access Token found");
var orgName = configuration.GetSection("orgName")?.Value ?? throw new ArgumentException("Organization Name was not specified");
var orgProject = configuration.GetSection("orgProject")?.Value ?? throw new ArgumentException("Project Name was not specified");

var bug = new AzureDevOpsBugsClient(
    orgName,
    orgProject,
    pat);

bug.CreateBugUsingClientLib(
    "Unable to submit a complaint using the Web portal", 
    """
    <ul>
        <li>Login into app</li>
        <li>Click on File Complaint</li>
        <li>Fill in all fields</li>
        <li>Click on Submit button</li>
    </ul>
    """);



