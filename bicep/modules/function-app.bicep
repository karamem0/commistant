@description('The name of the Function App.')
param name string

@description('The location of the Function App.')
param location string = resourceGroup().location

@description('The resource ID of the App Service Plan to associate with the Function App.')
param appServicePlanResourceId string

@description('The connection string of the Application Insights to associate with the Function App.')
param appInsightsConnectionString string

@description('The resource ID of the User Assigned Identity to associate with the Function App.')
param userAssignedIdentityResourceId string

@description('The client ID of the User Assigned Identity to associate with the Function App.')
param userAssignedIdentityClientId string

@description('The name of the Storage Account to associate with the Function App.')
param storageAccountName string

@description('The endpoint URL of the Storage Blobs to associate with the Function App.')
param storageBlobsEndpoint string

@description('The container name of the Storage Blobs to associate with the Function App.')
param storageBlobsContainerName string

@description('The Microsoft 365 Agent ID for the bot authentication.')
param microsoft365AgentId string

@description('The Microsoft 365 Agent Password for the bot authentication.')
@secure()
param microsoft365AgentPassword string

@description('The Microsoft 365 Agent Tenant ID for the bot authentication.')
param microsoft365AgentTenantId string

resource functionApp 'Microsoft.Web/sites@2024-11-01' = {
  name: name
  location: location
  kind: 'functionapp,linux'
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${userAssignedIdentityResourceId}': {}
    }
  }
  properties: {
    serverFarmId: appServicePlanResourceId
    siteConfig: {
      linuxFxVersion: 'DOTNET-ISOLATED|10.0'
      alwaysOn: true
      ftpsState: 'Disabled'
      http20Enabled: true
      appSettings: [
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsightsConnectionString
        }
        {
          name: 'AZURE_CLIENT_ID'
          value: userAssignedIdentityClientId
        }
        {
          name: 'AzureStorageBlobs__ClientId'
          value: userAssignedIdentityClientId
        }
        {
          name: 'AzureStorageBlobs__ContainerName'
          value: storageBlobsContainerName
        }
        {
          name: 'AzureStorageBlobs__Endpoint'
          value: storageBlobsEndpoint
        }
        {
          name: 'AzureWebJobsStorage__accountName'
          value: storageAccountName
        }
        {
          name: 'AzureWebJobsStorage__clientId'
          value: userAssignedIdentityClientId
        }
        {
          name: 'AzureWebJobsStorage__credential'
          value: 'managedidentity'
        }
        {
          name: 'Connections__ServiceConnection__Settings__AuthType'
          value: 'ClientSecret'
        }
        {
          name: 'Connections__ServiceConnection__Settings__ClientId'
          value: microsoft365AgentId
        }
        {
          name: 'Connections__ServiceConnection__Settings__ClientSecret'
          value: microsoft365AgentPassword
        }
        {
          name: 'Connections__ServiceConnection__Settings__TenantId'
          value: microsoft365AgentTenantId
        }
        {
          name: 'ConnectorClient__ClientId'
          value: microsoft365AgentId
        }
        {
          name: 'ConnectorClient__TenantId'
          value: microsoft365AgentTenantId
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        {
          name: 'MicrosoftIdentity__ClientId'
          value: microsoft365AgentId
        }
        {
          name: 'MicrosoftIdentity__ClientSecret'
          value: microsoft365AgentPassword
        }
        {
          name: 'MicrosoftIdentity__TenantId'
          value: microsoft365AgentTenantId
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
        {
          name: 'WEBSITE_USE_PLACEHOLDER_DOTNETISOLATED'
          value: '1'
        }
      ]
    }
  }
}
