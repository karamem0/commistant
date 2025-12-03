@description('The name for all resources.')
param name string

@description('The display name for the Bot Service.')
param displayName string = name

@description('The Microsoft App ID for Teams SSO.')
param microsoftAppId string

@description('The Microsoft App Password for Teams SSO.')
@secure()
param microsoftAppPassword string

@description('The Microsoft App Tenant ID for Teams SSO.')
param microsoftAppTenantId string

module userAssignedIdentity './modules/user-assigned-identity.bicep' = {
  name: '${name}-user-assigned-identity'
  params: {
    name: 'id-${name}'
  }
}

module openAIService './modules/openai-service.bicep' = {
  name: '${name}-openai-service'
  params: {
    name: 'oai-${name}'
    deploymentName: name
    userAssignedIdentityObjectId: userAssignedIdentity.outputs.userAssignedIdentityObjectId
  }
}

module storageAccount './modules/storage-account.bicep' = {
  name: '${name}-storage-account'
  params: {
    name: 'st${toLower(replace(replace(replace(name, '-', ''), '_', ''), ' ', ''))}'
    userAssignedIdentityObjectId: userAssignedIdentity.outputs.userAssignedIdentityObjectId
  }
}

module appServicePlan './modules/app-service-plan.bicep' = {
  name: '${name}-app-service-plan'
  params: {
    name: 'asp-${name}'
  }
}

module webApp './modules/web-app.bicep' = {
  name: '${name}-web-app'
  params: {
    name: 'app-${name}'
    appServicePlanResourceId: appServicePlan.outputs.appServicePlanResourceId
    userAssignedIdentityResourceId: userAssignedIdentity.outputs.userAssignedIdentityResourceId
    userAssignedIdentityClientId: userAssignedIdentity.outputs.userAssignedIdentityClientId
    openAIServiceEndpoint: openAIService.outputs.openAIServiceEndpoint
    openAIServiceDeploymentName: openAIService.outputs.openAIServiceDeploymentName
    storageBlobsContainerName: storageAccount.outputs.storageBlobsContainerName
    storageBlobsEndpoint: storageAccount.outputs.storageBlobsEndpoint
    microsoftAppId: microsoftAppId
    microsoftAppPassword: microsoftAppPassword
    microsoftAppTenantId: microsoftAppTenantId
  }
}

module functionApp './modules/function-app.bicep' = {
  name: '${name}-function-app'
  params: {
    name: 'func-${name}'
    appServicePlanResourceId: appServicePlan.outputs.appServicePlanResourceId
    userAssignedIdentityResourceId: userAssignedIdentity.outputs.userAssignedIdentityResourceId
    userAssignedIdentityClientId: userAssignedIdentity.outputs.userAssignedIdentityClientId
    storageAccountName: storageAccount.outputs.storageAccountName
    storageBlobsContainerName: storageAccount.outputs.storageBlobsContainerName
    storageBlobsEndpoint: storageAccount.outputs.storageBlobsEndpoint
    microsoftAppId: microsoftAppId
    microsoftAppPassword: microsoftAppPassword
    microsoftAppTenantId: microsoftAppTenantId
  }
}

module botService './modules/bot-service.bicep' = {
  name: '${name}-bot-service'
  params: {
    name: 'bot-${name}'
    displayName: displayName
    endpoint: 'https://${webApp.outputs.webAppHostName}/api/messages'
    msaAppId: microsoftAppId
    msaAppTenantId: microsoftAppTenantId
  }
}
