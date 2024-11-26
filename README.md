## A demo console app to report bugs and issues to Azure DevOps boards.
### Requirement
- Owner of Azure DevOps account
- Generate Personal Access Token with only Read/Write permissions for Azure Boards

### How to run app
```bash
pat=<your-personal-access-token> orgName=<your-devOps-Organization-name> orgProject=<name-of-project> dotnet run
