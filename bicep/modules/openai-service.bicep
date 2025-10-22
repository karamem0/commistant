@description('The name of the OpenAI Service.')
param name string

@description('The location of the OpenAI Service.')
param location string = resourceGroup().location

@description('The SKU of the OpenAI Service. Default is S0.')
param sku string = 'S0'

@description('The deployment name of the OpenAI Service.')
param deploymentName string

@description('The object ID of the User Assigned Identity to associate with the OpenAI Service.')
param userAssignedIdentityObjectId string

resource openAIService 'Microsoft.CognitiveServices/accounts@2025-06-01' = {
  name: name
  location: location
  kind: 'OpenAI'
  sku: {
    name: sku
  }
  properties: {
    customSubDomainName: name
  }
}

resource openAIServiceDeployment 'Microsoft.CognitiveServices/accounts/deployments@2023-05-01' = {
  parent: openAIService
  name: deploymentName
  sku: {
    name: 'GlobalStandard'
    capacity: 100
  }
  properties: {
    model: {
      format: 'OpenAI'
      name: 'gpt-4.1-mini'
      version: '2025-04-14'
    }
  }
}

@description('This is the built-in Cognitive Services OpenAI User role.')
resource roleDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  scope: subscription()
  name: '5e0bd9bd-7b93-4f28-af87-19fc36ad61bd'
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: openAIService
  name: guid(openAIService.id, userAssignedIdentityObjectId, roleDefinition.id)
  properties: {
    roleDefinitionId: roleDefinition.id
    principalId: userAssignedIdentityObjectId
    principalType: 'ServicePrincipal'
  }
}

@description('The endpoint of the OpenAI Service.')
output openAIServiceEndpoint string = 'https://${openAIService.properties.customSubDomainName}.openai.azure.com/'

@description('The deployment name of the OpenAI Service.')
output openAIServiceDeploymentName string = openAIServiceDeployment.name
