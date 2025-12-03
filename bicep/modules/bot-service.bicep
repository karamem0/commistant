@description('The name of the Bot Service.')
param name string

@description('The location of the Bot Service.')
param location string = 'global'

@description('The SKU of the Bot Service. Default is F0 (Free).')
param sku string = 'F0'

@description('The display name of the Bot Service.')
param displayName string

@description('The endpoint URL where the Bot Service is hosted.')
param endpoint string

@description('The type of Microsoft App used for the bot authentication.')
param msaAppType string = 'SingleTenant'

@description('The Microsoft App ID or User Assigned Managed Identity for the bot authentication.')
param msaAppId string

@description('The Microsoft App Tenant ID for the bot authentication.')
param msaAppTenantId string = tenant().tenantId

resource botService 'Microsoft.BotService/botServices@2022-09-15' = {
  name: name
  location: location
  kind: 'bot'
  sku: {
    name: sku
  }
  properties: {
    displayName: displayName
    endpoint: endpoint
    msaAppType: msaAppType
    msaAppId: msaAppId
    msaAppTenantId: msaAppTenantId
  }
}

@description('The resource ID of the Bot Service.')
output botServiceResourceId string = botService.id
