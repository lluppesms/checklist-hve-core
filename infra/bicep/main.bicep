// ------------------------------------------------------------------------------------------------------------------------
// Main Bicep Template: Checklist Application Azure Infrastructure
// ------------------------------------------------------------------------------------------------------------------------
targetScope = 'resourceGroup'

@description('Application name prefix')
param appName string = 'checklist'

@description('Short environment code (dev, qa, prod)')
param environmentCode string = 'dev'

@description('Azure region for all resources')
param location string = resourceGroup().location

@description('Instance number suffix for resource naming')
param instanceNumber string = '1'

@description('Deployment type -- controls which resources are deployed')
@allowed(['webapp'])
param deploymentType string = 'webapp'

@description('SQL Database name')
param sqlDatabaseName string = 'checklistdb'

@description('Key Vault owner object ID (user or service principal)')
param adminUserId string = ''

@description('SQL admin Entra ID login display name')
param sqlAdminLoginUserId string = ''

@description('SQL admin Entra ID login object SID')
param sqlAdminLoginUserSid string = ''

@description('SQL admin Entra ID tenant ID')
param sqlAdminLoginTenantId string = ''

@description('Entra ID login instance endpoint')
param adInstance string = 'https://login.microsoftonline.com/'

@description('Entra ID tenant ID')
param adTenantId string = ''

@description('App registration client ID')
param adClientId string = ''

@description('Entra ID domain')
param adDomain string = ''

// ------------------------------------------------------------------------------------------------------------------------
// Resource Names
// ------------------------------------------------------------------------------------------------------------------------
module names 'resourcenames.bicep' = {
  name: 'resourcenames'
  params: {
    appName: appName
    environmentCode: environmentCode
    instanceNumber: instanceNumber
  }
}

// ------------------------------------------------------------------------------------------------------------------------
// Monitoring
// ------------------------------------------------------------------------------------------------------------------------
module logAnalytics 'modules/logAnalytics.bicep' = {
  name: 'logAnalytics'
  params: {
    location: location
    workspaceName: names.outputs.logAnalyticsWorkspaceName
  }
}

module appInsights 'modules/appInsights.bicep' = {
  name: 'appInsights'
  params: {
    location: location
    appInsightsName: names.outputs.appInsightsName
    logAnalyticsWorkspaceId: logAnalytics.outputs.workspaceId
  }
}

// ------------------------------------------------------------------------------------------------------------------------
// Key Vault
// ------------------------------------------------------------------------------------------------------------------------
module keyVault 'modules/keyVault.bicep' = {
  name: 'keyVault'
  params: {
    location: location
    keyVaultName: names.outputs.keyVaultName
  }
}

// ------------------------------------------------------------------------------------------------------------------------
// Azure SQL
// ------------------------------------------------------------------------------------------------------------------------
module sqlServer 'modules/sqlServer.bicep' = {
  name: 'sqlServer'
  params: {
    location: location
    serverName: names.outputs.sqlServerName
    sqlAdminLoginUserId: sqlAdminLoginUserId
    sqlAdminLoginUserSid: sqlAdminLoginUserSid
    sqlAdminLoginTenantId: sqlAdminLoginTenantId
  }
}

module sqlDatabase 'modules/sqlDatabase.bicep' = {
  name: 'sqlDatabase'
  params: {
    serverName: sqlServer.outputs.serverName
    databaseName: sqlDatabaseName
  }
}

// ------------------------------------------------------------------------------------------------------------------------
// App Service (Web App)
// ------------------------------------------------------------------------------------------------------------------------
module appServicePlan 'modules/appServicePlan.bicep' = {
  name: 'appServicePlan'
  params: {
    location: location
    planName: names.outputs.webSiteAppServicePlanName
    sku: 'S1'
  }
}

module appService 'modules/appService.bicep' = {
  name: 'appService'
  params: {
    location: location
    appName: names.outputs.webSiteName
    planId: appServicePlan.outputs.planId
    appInsightsConnectionString: appInsights.outputs.connectionString
    keyVaultName: keyVault.outputs.keyVaultName
    adInstance: adInstance
    adTenantId: adTenantId
    adClientId: adClientId
    adDomain: adDomain
  }
}

// ------------------------------------------------------------------------------------------------------------------------
// RBAC: App Service managed identity -> Key Vault Secrets User
// ------------------------------------------------------------------------------------------------------------------------
module keyVaultRoleAssignment 'modules/keyVaultRoleAssignment.bicep' = {
  name: 'keyVaultRoleAssignment'
  params: {
    keyVaultName: keyVault.outputs.keyVaultName
    principalId: appService.outputs.appServicePrincipalId
  }
}

// ------------------------------------------------------------------------------------------------------------------------
// Outputs
// ------------------------------------------------------------------------------------------------------------------------
output appServiceName string = appService.outputs.appServiceDefaultHostname
output sqlServerFqdn string = sqlServer.outputs.serverFqdn
output keyVaultUri string = keyVault.outputs.keyVaultUri
output appInsightsConnectionString string = appInsights.outputs.connectionString
