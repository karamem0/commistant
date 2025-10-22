@description('The name of the Storage Account.')
param name string

@description('The location of the Storage Account.')
param location string = resourceGroup().location

@description('The SKU of the Storage Account. Default is Standard_LRS.')
param sku string = 'Standard_LRS'

@description('The object ID of the User Assigned Identity to associate with the OpenAI Service.')
param userAssignedIdentityObjectId string

resource storageAccount 'Microsoft.Storage/storageAccounts@2025-01-01' = {
  name: name
  location: location
  sku: {
    name: sku
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Hot'
    publicNetworkAccess: 'Enabled'
    allowBlobPublicAccess: false
    networkAcls: {
      defaultAction: 'Allow'
    }
  }
}

resource storageBlobs 'Microsoft.Storage/storageAccounts/blobServices@2025-01-01' = {
  parent: storageAccount
  name: 'default'
  properties: {}
}

resource storageBlobsContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2025-01-01' = {
  parent: storageBlobs
  name: 'azure-bot-states'
  properties: {}
}

@description('This is the built-in Storage Blobs Contributor role.')
resource storageBlobsRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  scope: subscription()
  name: 'ba92f5b4-2d11-453d-a403-e96b0029c9fe'
}

resource storageBlobsRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: storageAccount
  name: guid(storageAccount.id, userAssignedIdentityObjectId, storageBlobsRoleDefinition.id)
  properties: {
    roleDefinitionId: storageBlobsRoleDefinition.id
    principalId: userAssignedIdentityObjectId
    principalType: 'ServicePrincipal'
  }
}

@description('This is the built-in Storage Tables Contributor role.')
resource storageTablesRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  scope: subscription()
  name: '0a9a7e1f-b9d0-4cc4-a60d-0319b160aaa3'
}

resource storageTablesAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: storageAccount
  name: guid(storageAccount.id, userAssignedIdentityObjectId, storageTablesRoleDefinition.id)
  properties: {
    roleDefinitionId: storageTablesRoleDefinition.id
    principalId: userAssignedIdentityObjectId
    principalType: 'ServicePrincipal'
  }
}

@description('The name of the Storage Account.')
output storageAccountName string = storageAccount.name

@description('The endpoint URL of the Storage Blobs.')
output storageBlobsEndpoint string = storageAccount.properties.primaryEndpoints.blob

@description('The container name of the Storage Blobs.')
output storageBlobsContainerName string = storageBlobsContainer.name
