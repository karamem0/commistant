@description('The name of the Web App.')
param name string

@description('The location of the Web App.')
param location string = resourceGroup().location

@description('The resource ID of the App Service Plan to associate with the Web App.')
param appServicePlanResourceId string

@description('The connection string of the Application Insights to associate with the Web App.')
param appInsightsConnectionString string

@description('The resource ID of the User Assigned Identity to associate with the Web App.')
param userAssignedIdentityResourceId string

@description('The client ID of the User Assigned Identity to associate with the Web App.')
param userAssignedIdentityClientId string

@description('The endpoint of the OpenAI Service to associate with the Web App.')
param openAIServiceEndpoint string

@description('The deployment name of the OpenAI Service to associate with the Web App.')
param openAIServiceDeploymentName string

@description('The endpoint URL of the Storage Blobs to associate with the Web App.')
param storageBlobsEndpoint string

@description('The container name of the Storage Blobs to associate with the Web App.')
param storageBlobsContainerName string

@description('The Microsoft 365 Agent ID for the bot authentication.')
param microsoft365AgentId string

@description('The Microsoft 365 Agent Password for the bot authentication.')
@secure()
param microsoft365AgentPassword string

@description('The Microsoft 365 Agent Tenant ID for the bot authentication.')
param microsoft365AgentTenantId string

resource webApp 'Microsoft.Web/sites@2024-11-01' = {
  name: name
  location: location
  kind: 'app,linux'
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${userAssignedIdentityResourceId}': {}
    }
  }
  properties: {
    serverFarmId: appServicePlanResourceId
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|10.0'
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
          name: 'AzureOpenAI__DeploymentName'
          value: openAIServiceDeploymentName
        }
        {
          name: 'AzureOpenAI__Endpoint'
          value: openAIServiceEndpoint
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
          name: 'TokenValidation__Audiences__0'
          value: microsoft365AgentId
        }
        {
          name: 'TokenValidation__TenantId'
          value: microsoft365AgentTenantId
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
      ]
    }
  }
}

output webAppHostName string = webApp.properties.defaultHostName
