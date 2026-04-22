@description('Azure region')
param location string

@description('App Service name')
param appName string

@description('App Service Plan resource ID')
param planId string

@description('Application Insights connection string')
param appInsightsConnectionString string

@description('Key Vault name for secret references')
param keyVaultName string

@description('Entra ID login instance endpoint')
param adInstance string = 'https://login.microsoftonline.com/'

@description('Entra ID tenant ID')
param adTenantId string = ''

@description('App registration client ID')
param adClientId string = ''

@description('Entra ID domain')
param adDomain string = ''

resource app 'Microsoft.Web/sites@2023-12-01' = {
  name: appName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: planId
    httpsOnly: true
    siteConfig: {
      webSocketsEnabled: true   // required for SignalR
      alwaysOn: true            // prevent cold start breaking persistent connections
      minTlsVersion: '1.2'
      appSettings: [
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsightsConnectionString
        }
        {
          name: 'ApplicationInsightsAgent_EXTENSION_VERSION'
          value: '~3'
        }
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Production'
        }
        {
          name: 'KeyVaultName'
          value: keyVaultName
        }
        {
          name: 'AzureAd__Instance'
          value: adInstance
        }
        {
          name: 'AzureAd__TenantId'
          value: adTenantId
        }
        {
          name: 'AzureAd__ClientId'
          value: adClientId
        }
        {
          name: 'AzureAd__Domain'
          value: adDomain
        }
      ]
    }
  }
}

output appServiceId string = app.id
output appServicePrincipalId string = app.identity.principalId
output appServiceDefaultHostname string = app.properties.defaultHostName
