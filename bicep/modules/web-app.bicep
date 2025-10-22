@description('The name of the Web App.')
param name string

@description('The location of the Web App.')
param location string = resourceGroup().location

@description('The resource ID of the App Service Plan to associate with the Web App.')
param appServicePlanResourceId string

@description('The resource ID of the User Assigned Identity to associate with the Web App.')
param userAssignedIdentityResourceId string

@description('The client ID of the User Assigned Identity to associate with the Web App.')
param userAssignedIdentityClientId string

@description('The tenant ID of the User Assigned Identity to associate with the Web App.')
param userAssignedIdentityTenantId string = tenant().tenantId

@description('The endpoint of the OpenAI Service to associate with the Web App.')
param openAIServiceEndpoint string

@description('The deployment name of the OpenAI Service to associate with the Web App.')
param openAIServiceDeploymentName string

@description('The endpoint URL of the Storage Blobs to associate with the Web App.')
param storageBlobsEndpoint string

@description('The container name of the Storage Blobs to associate with the Web App.')
param storageBlobsContainerName string

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
      linuxFxVersion: 'DOTNETCORE|8.0'
      alwaysOn: true
      ftpsState: 'Disabled'
      http20Enabled: true
      appSettings: [
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
          name: 'BotFramework__MicrosoftAppId'
          value: userAssignedIdentityClientId
        }
        {
          name: 'BotFramework__MicrosoftAppTenantId'
          value: userAssignedIdentityTenantId
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
