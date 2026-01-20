@description('The name for all resources.')
param name string

@description('The display name for the Bot Service.')
param displayName string = name

@description('The Microsoft 365 Agent ID for Teams SSO.')
param microsoft365AgentId string

@description('The Microsoft 365 Agent Password for Teams SSO.')
@secure()
param microsoft365AgentPassword string

@description('The Microsoft 365 Agent Tenant ID for Teams SSO.')
param microsoft365AgentTenantId string

module logAnalyticsWorkspace './modules/log-analytics-workspace.bicep' = {
  name: '${name}-log-analytics-workspace'
  params: {
    name: 'log-${name}'
  }
}

module appInsights './modules/app-insights.bicep' = {
  name: '${name}-app-insights'
  params: {
    name: 'appi-${name}'
    logAnalyticsWorkspaceId: logAnalyticsWorkspace.outputs.logAnalyticsWorkspaceId
  }
}

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
    appInsightsConnectionString: appInsights.outputs.applicationInsightsConnectionString
    userAssignedIdentityResourceId: userAssignedIdentity.outputs.userAssignedIdentityResourceId
    userAssignedIdentityClientId: userAssignedIdentity.outputs.userAssignedIdentityClientId
    openAIServiceEndpoint: openAIService.outputs.openAIServiceEndpoint
    openAIServiceDeploymentName: openAIService.outputs.openAIServiceDeploymentName
    storageBlobsContainerName: storageAccount.outputs.storageBlobsContainerName
    storageBlobsEndpoint: storageAccount.outputs.storageBlobsEndpoint
    microsoft365AgentId: microsoft365AgentId
    microsoft365AgentPassword: microsoft365AgentPassword
    microsoft365AgentTenantId: microsoft365AgentTenantId
  }
}

module functionApp './modules/function-app.bicep' = {
  name: '${name}-function-app'
  params: {
    name: 'func-${name}'
    appServicePlanResourceId: appServicePlan.outputs.appServicePlanResourceId
    appInsightsConnectionString: appInsights.outputs.applicationInsightsConnectionString
    userAssignedIdentityResourceId: userAssignedIdentity.outputs.userAssignedIdentityResourceId
    userAssignedIdentityClientId: userAssignedIdentity.outputs.userAssignedIdentityClientId
    storageAccountName: storageAccount.outputs.storageAccountName
    storageBlobsContainerName: storageAccount.outputs.storageBlobsContainerName
    storageBlobsEndpoint: storageAccount.outputs.storageBlobsEndpoint
    microsoft365AgentId: microsoft365AgentId
    microsoft365AgentPassword: microsoft365AgentPassword
    microsoft365AgentTenantId: microsoft365AgentTenantId
  }
}

module botService './modules/bot-service.bicep' = {
  name: '${name}-bot-service'
  params: {
    name: 'bot-${name}'
    displayName: displayName
    endpoint: 'https://${webApp.outputs.webAppHostName}/api/messages'
    microsoft365AgentId: microsoft365AgentId
    microsoft365AgentTenantId: microsoft365AgentTenantId
  }
}
