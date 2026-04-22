@description('Azure region')
param location string

@description('SQL Server name')
param serverName string

@description('Entra ID SQL admin login display name')
param sqlAdminLoginUserId string = ''

@description('Entra ID SQL admin login object SID')
param sqlAdminLoginUserSid string = ''

@description('Entra ID SQL admin tenant ID')
param sqlAdminLoginTenantId string = ''

resource server 'Microsoft.Sql/servers@2023-08-01-preview' = {
  name: serverName
  location: location
  properties: {
    minimalTlsVersion: '1.2'
    administrators: (sqlAdminLoginUserId != '') ? {
      administratorType: 'ActiveDirectory'
      login: sqlAdminLoginUserId
      sid: sqlAdminLoginUserSid
      tenantId: sqlAdminLoginTenantId
      azureADOnlyAuthentication: false
    } : null
  }
}

// Allow Azure services to connect
resource firewallAzureServices 'Microsoft.Sql/servers/firewallRules@2023-08-01-preview' = {
  parent: server
  name: 'AllowAzureServices'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

output serverName string = server.name
output serverFqdn string = server.properties.fullyQualifiedDomainName
