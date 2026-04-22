targetScope = 'resourceGroup'

@description('Environment name (dev, staging, prod)')
param environmentName string = 'dev'

@description('Azure region for all resources')
param location string = resourceGroup().location

@description('Application name prefix')
param appName string = 'checklist'

var resourcePrefix = '${appName}-${environmentName}'

module logAnalytics 'modules/logAnalytics.bicep' = {
  name: 'logAnalytics'
  params: { location: location, workspaceName: '${resourcePrefix}-law' }
}

module appInsights 'modules/appInsights.bicep' = {
  name: 'appInsights'
  params: {
    location: location
    appInsightsName: '${resourcePrefix}-ai'
    logAnalyticsWorkspaceId: logAnalytics.outputs.workspaceId
  }
}

module keyVault 'modules/keyVault.bicep' = {
  name: 'keyVault'
  params: { location: location, keyVaultName: '${resourcePrefix}-kv' }
}

module sqlServer 'modules/sqlServer.bicep' = {
  name: 'sqlServer'
  params: { location: location, serverName: '${resourcePrefix}-sql', administratorLogin: 'sqladmin' }
}

module sqlDatabase 'modules/sqlDatabase.bicep' = {
  name: 'sqlDatabase'
  params: { serverName: sqlServer.outputs.serverName, databaseName: '${resourcePrefix}-db' }
}

module appServicePlan 'modules/appServicePlan.bicep' = {
  name: 'appServicePlan'
  params: { location: location, planName: '${resourcePrefix}-asp', sku: 'S1' }
}

module appService 'modules/appService.bicep' = {
  name: 'appService'
  params: {
    location: location
    appName: '${resourcePrefix}-app'
    planId: appServicePlan.outputs.planId
    appInsightsConnectionString: appInsights.outputs.connectionString
    keyVaultName: keyVault.outputs.keyVaultName
  }
}

// RBAC: grant App Service managed identity access to Key Vault
// Declared after both modules because it depends on outputs from both
module keyVaultRoleAssignment 'modules/keyVaultRoleAssignment.bicep' = {
  name: 'keyVaultRoleAssignment'
  params: {
    keyVaultName: keyVault.outputs.keyVaultName
    principalId: appService.outputs.appServicePrincipalId
  }
}
