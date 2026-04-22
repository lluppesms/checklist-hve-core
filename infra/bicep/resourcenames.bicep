// ------------------------------------------------------------------------------------------------------------------------
// Resource Names Module
// Generates standardized Azure resource names for the Checklist application.
// ------------------------------------------------------------------------------------------------------------------------
param appName string
param environmentCode string
param instanceNumber string

var resourceAbbreviations = loadJsonContent('./data/resourceAbbreviations.json')

// Sanitize names: lowercase, replace spaces/underscores with dashes
var sanitizedAppName = toLower(replace(replace(appName, ' ', '-'), '_', '-'))
var sanitizedEnv = toLower(environmentCode)
var sanitizedInstance = instanceNumber

// Base name pattern: {appName}{instance}-{env}
var baseNameWithDashes = '${sanitizedAppName}${sanitizedInstance}-${sanitizedEnv}'

// Outputs: all resource names
output logAnalyticsWorkspaceName string = '${baseNameWithDashes}-${resourceAbbreviations.logAnalyticsWorkspace}'
output appInsightsName string = '${baseNameWithDashes}-${resourceAbbreviations.applicationInsights}'
output keyVaultName string = '${sanitizedAppName}${sanitizedInstance}${resourceAbbreviations.keyVault}${sanitizedEnv}'
output sqlServerName string = '${sanitizedAppName}${sanitizedInstance}${resourceAbbreviations.sqlServer}${sanitizedEnv}'
output webSiteName string = '${baseNameWithDashes}-${resourceAbbreviations.appService}'
output webSiteAppServicePlanName string = '${baseNameWithDashes}-${resourceAbbreviations.appServicePlan}'
