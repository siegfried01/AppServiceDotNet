## Configure App Config

- [Add Azure App Configuration by using Connected Services](https://docs.microsoft.com/en-us/visualstudio/azure/vs-azure-tools-connected-services-app-configuration?view=vs-2022)

## Deploy the app
The `deploy.parameters.json` file is a tempalate to be used for converting the AzureADB2C in [appsettings.json](https://github.com/Azure-Samples/ms-identity-blazor-server/blob/main/WebApp-your-API/B2C/Client/appsettings.json) to be stored in App Config and Key Vault

``` powershell
az deployment group create \
  --name BlazorAppDeployment \
  --resource-group YourResourceGroup \
  --template-file deploy.bicep \
  --parameters @deploy.parameters.json
```
