@description('Azure region')
param location string

@description('App Service Plan name')
param planName string

@description('SKU name — S1 minimum for WebSocket/SignalR support')
param sku string = 'S1'

resource plan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: planName
  location: location
  sku: {
    name: sku
    tier: 'Standard'
  }
  properties: {
    reserved: false
  }
}

output planId string = plan.id
