# Overview
This library provides extensions methods of [azure-iot-sdk](https://github.com/Azure/azure-iot-sdk-csharp) to support the following scenarios:

1. Instantiate a new deployment manifest with default EdgeAgent and EdgeHub values (e.g. create options, restart policy, et. al.).
2. Extract IoT Edge module status (e.g. last updated time, exit code) from $EdgeAgent and $EdgeHub module twin reported properties.

## Installation
The library is available via nuget.org

TBD: nuget link

## How To Use
The following are samples of how to use the extension methods.

### Create a new deployment manifest

You can generate desired properties for edgeAgent with default values using the following snippet:
```csharp
EdgeAgentDesiredProperties edgeAgentDesiredProperties = new EdgeAgentDesiredProperties();
```
If you want to change some of the default values, add registry credentials or custom modules properties, you can configure as the following:
```csharp
EdgeAgentDesiredProperties edgeAgentDesiredProperties = new EdgeAgentDesiredProperties()
{
    SystemModuleVersion = "1.3",
    RegistryCredentials = new List<RegistryCredential>()
    {
       new RegistryCredential("AcrAdmin", "address", "userName", "password"),
    },
    EdgeModuleSpecifications = new List<EdgeModuleSpecification>()
    {
       new EdgeModuleSpecification("custom-module-name", "image-uri"),
    },
};
```
You can generate desired properties for edgeHub with default values using the following snippet:
```csharp
EdgeHubDesiredProperties edgeHubConfig = new EdgeHubDesiredProperties();
```
If you want to change some of the default values, for example the default value of `StoreAndForwardTimeToLiveSecs` is 7200, you can change it to 3600 by configure it as the following:
```csharp
EdgeHubDesiredProperties edgeHubConfig = new EdgeHubDesiredProperties()
{
    StoreAndForwardTimeToLiveSecs = 3600,
};
```
Generate desired properties for custom module:
```csharp
ModuleSpecificationDesiredProperties moduleSpecificationDesiredProperties = new ModuleSpecificationDesiredProperties()
{
    Name = "custom-module",
    DesiredProperty = new
    {
        dummy_a = new
        {
            dummy_b = "dummy"
        },
    }
};
```
Finally, setup all desired properties to ConfigurationContent
```csharp
ConfigurationContent configurationContent = new ConfigurationContent()
                .SetEdgeHub(edgeHubConfig)
                .SetEdgeAgent(edgeAgentDesiredProperties)
                .SetModuleDesiredProperty(moduleSpecificationDesiredProperties);
```
Then the `configurationContent` can be appied to IoTEdge device
```csharp
// apply the configurationContent through SDK.
await registryManager.ApplyConfigurationContentOnDeviceAsync({IoTEdgeId}, configurationContent).ConfigureAwait(false);
```
### Extract IoT Edge Module Status
The reported properties from $EdgeAgent/$EdgeHub module twins can be used to examine the state of a deployment on a device.
```csharp
// get $EdgeHub and $EdgeAgent module twin through SDK.
Twin edgeAgentTwin = await registryManager.GetTwinAsync(IoTEdgeId, "$edgeAgent").ConfigureAwait(false);
Twin edgeHubTwin = await registryManager.GetTwinAsync(IoTEdgeId, "$edgeHub").ConfigureAwait(false);
```
```csharp
// get $EdgeHub and $EdgeAgent module twin through SDK.
EdgeAgentReportedProperties edgeAgentReportedProperties = edgeAgentTwin.GetEdgeAgentReportedProperties();
EdgeHubReportedProperties edgeHubReportedProperties = edgeHubTwin.GetEdgeHubReportedProperties();
```
# Dependencies
[Microsoft.Azure.Devices (>= 1.37.1)](https://www.nuget.org/packages/Microsoft.Azure.Devices/1.37.1)

[StyleCop.Analyzers (>=1.1.118)](https://www.nuget.org/packages/StyleCop.Analyzers/1.1.118)

[Azure IoT Edge: 1.1, 1.2, 1.3](https://docs.microsoft.com/en-us/azure/iot-edge/version-history?view=iotedge-2020-11)

# References
[iot-edge-object-model](https://github.com/Azure/iot-edge-object-model)

[Properties of the IoT Edge agent and IoT Edge hub module twins](https://docs.microsoft.com/en-us/azure/iot-edge/module-edgeagent-edgehub?view=iotedge-2020-11)
