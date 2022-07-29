

using Microsoft.Azure.Devices;
using System.Collections.Generic;

namespace Sample
{
    public class Class1
    {
        public void A()
        {
            EdgeAgentConfig edgeAgentConfig = new EdgeAgentConfig()
            {
                IoTEdgeModuleVersion = "1.2",
                RegistryCredentials = new List<RegistryCredential>()
                {
                    new RegistryCredential()
                    {
                        Name = "MyACRPull",
                        Address = "my.azurecr.io",
                        UserName = "u",
                        Password ="p",
                    }
                },
                ModuleSpecifications = new List<ModuleSpecification>()
                {
                    new ModuleSpecification()
                    {
                        Name = "rpfEdgeComms",
                        Image = "crrpfdev001.azurecr.io/rpf/khi-edgecomms:20220711.1"
                        EnvironmentVariables = new List<EnvironmentVariable>()
                        {
                            new EnvironmentVariable("Edge_ID", "f6fb9f4c-6d94-4464-9a9d-8adf9d7fd5dd")
                        },
                    }
                }
            };
            EdgeHubConfig edgeHubConfig = new EdgeHubConfig()
            {
                StoreAndForwardConfigurationTimeToLiveSecs = 7200,
                Routes = new List<Route>()
                {
                    new Route("Telemetry", "FROM /messages/modules/rpfEdgeComms/outputs/telemetry INTO $upstream"),
                    new Route("EdgeCommsToParameterUpdater", "FROM /messages/modules/rpfEdgeComms/outputs/deployrequest INTO BrokeredEndpoint(\"/modules/rpfParameterUpdater/inputs/deployresult\")"),
                }
            };
            ModuleDesiredPropertyConfig moduleDesiredPropertyConfig = new ModuleDesiredPropertyConfig();
            ConfigurationContent c = new ConfigurationContent()
                .SetEdgeAgent(edgeAgentConfig)
                .SetEdgeHub(edgeHubConfig)
                .SetModuleDesiredProperty(moduleDesiredPropertyConfig);
        }        
    }
}