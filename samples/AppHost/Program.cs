var builder = DistributedApplication.CreateBuilder(args);

var environment = builder.AddParameter("environment");

builder.AddProject<Projects.BlazorApp>("blazor-app")
.WithEnvironment("ASPNETCORE_ENVIRONMENT",environment);

builder.Build().Run();
