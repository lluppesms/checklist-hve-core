@description('SQL Server name')
param serverName string

@description('Database name')
param databaseName string

resource server 'Microsoft.Sql/servers@2023-08-01-preview' existing = {
  name: serverName
}

resource database 'Microsoft.Sql/servers/databases@2023-08-01-preview' = {
  parent: server
  name: databaseName
  location: resourceGroup().location
  sku: {
    name: 'S0'
    tier: 'Standard'
  }
}

output databaseName string = database.name
