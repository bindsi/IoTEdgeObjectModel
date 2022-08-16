﻿// <copyright file="Samples.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
// </copyright>

namespace Microsoft.Azure.Devices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices.Shared;

    /// <summary>
    /// Samples.
    /// </summary>
    public class Samples
    {
        /// <summary>
        /// GenerateManifestSample.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task GenerateManifestSample()
        {
            RegistryManager registryManager = RegistryManager.CreateFromConnectionString("<ioTHubConnectionString>");
            EdgeAgentDesiredProperties edgeAgentDesiredProperties = new EdgeAgentDesiredProperties()
            {
                SystemModuleVersion = "1.3",
                RegistryCredentials = new List<RegistryCredential>()
                {
                    new RegistryCredential("<containerRegistryName>", "<address>", "<userName>", "<password>"),
                },
                EdgeModuleSpecifications = new List<EdgeModuleSpecification>()
                {
                    new EdgeModuleSpecification("simulator", "mcr.microsoft.com/azureiotedge-simulated-temperature-sensor:1.0"),
                },
            };
            EdgeHubDesiredProperties edgeHubConfig = new EdgeHubDesiredProperties()
            {
                Routes = new List<Route>() { new Route("sensorToUpstream", "FROM /messages/modules/tempSensor/outputs/temperatureOutput INTO $upstream") },
            };
            ModuleSpecificationDesiredProperties customModule = new ModuleSpecificationDesiredProperties()
            {
                Name = "simulator",
                DesiredProperties = new { customObject = "custom properties" },
            };
            ConfigurationContent configurationContent = new ConfigurationContent()
                            .SetEdgeHub(edgeHubConfig)
                            .SetEdgeAgent(edgeAgentDesiredProperties)
                            .SetModuleDesiredProperty(customModule);

            await registryManager.ApplyConfigurationContentOnDeviceAsync("IoTEdgeId", configurationContent).ConfigureAwait(false);
        }

        /// <summary>
        /// GetModuleTwinSample.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task GetModuleTwinSample()
        {
            RegistryManager registryManager = RegistryManager.CreateFromConnectionString("ioTHubConnectionString");

            // Get edgeAgent reported properties.
            Twin edgeAgentTwin = await registryManager.GetTwinAsync("IoTEdgeId", "$edgeAgent").ConfigureAwait(false);
            EdgeAgentReportedProperties edgeAgentReportedProperties = edgeAgentTwin.GetEdgeAgentReportedProperties();

            // Get edgeHub reported properties.
            Twin edgeHubTwin = await registryManager.GetTwinAsync("IoTEdgeId", "$edgeHub").ConfigureAwait(false);
            EdgeHubReportedProperties edgeHubReportedProperties = edgeHubTwin.GetEdgeHubReportedProperties();
        }
    }
}