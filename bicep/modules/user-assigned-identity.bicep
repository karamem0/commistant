@description('The name of the User Assigned Identity.')
param name string

@description('The location of the User Assigned Identity.')
param location string = resourceGroup().location

resource userAssignedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2024-11-30' = {
  name: name
  location: location
}

@description('The resource ID of the User Assigned Identity.')
output userAssignedIdentityResourceId string = userAssignedIdentity.id

@description('The client ID of the User Assigned Identity.')
output userAssignedIdentityClientId string = userAssignedIdentity.properties.clientId

@description('The object ID of the User Assigned Identity.')
output userAssignedIdentityObjectId string = userAssignedIdentity.properties.principalId
