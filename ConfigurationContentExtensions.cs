using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.Devices
{
    public static class ConfigurationContentExtensions
    {
        public static EdgeAgentSpecification SetEdgeAgent(this ConfigurationContent configurationContent, EdgeAgentConfig edgeAgentConfig)
        {
            configurationContent.ModulesContent = new Dictionary<string, IDictionary<string, object>>();
            configurationContent.ModulesContent.Add(
                "$edgeAgent",
                new Dictionary<string, object>
                {
                    ["properties.desired"] = GetEdgeAgentConfiguration(edgeAgentConfig),
                });

            return configurationContent as EdgeAgentSpecification;
        }

        private static object GetEdgeAgentConfiguration(EdgeAgentConfig edgeAgentConfig)
        {
            Dictionary<string, Dictionary<string, string>> manifestRegistryCredentials = new Dictionary<string, Dictionary<string, string>>();
            edgeAgentConfig.RegistryCredentials.ForEach(x =>
            {
                manifestRegistryCredentials.Add(x.Name, new Dictionary<string, string>()
                {
                    { "address", x.Address },
                    { "username", x.UserName },
                    { "password", x.Password },
                });
            });

            Dictionary<string, object> modules = new Dictionary<string, object>();
            edgeAgentConfig.ModuleSpecifications.ForEach(x =>
            {
                Dictionary<string, object> env = new Dictionary<string, object>();
                x.EnvironmentVariables.ForEach(e => env.Add(e.Name, new { value = e.Value }));
                modules.Add(
                    x.Name,
                    new
                    {
                        version = x.Version,
                        type = "docker",
                        status = x.Status.ToString().ToLower(),
                        restartPolicy = x.RestartPolicy.ToString().ToLower(),
                        settings = new
                        {
                            image = x.Image,
                            createOptions = x.CreateOptions,
                        },
                        env = env,
                    });
            });

            return new
            {
                schemaVersion = edgeAgentConfig.SchemaVersion,
                runtime = new
                {
                    type = "docker",
                    settings = new
                    {
                        loggingOptions = string.Empty,
                        minDockerVersion = "v1.25",
                        registryCredentials = manifestRegistryCredentials,
                    },
                },
                systemModules = new
                {
                    edgeAgent = new
                    {
                        type = "docker",
                        settings = new
                        {
                            image = $"mcr.microsoft.com/azureiotedge-agent:{edgeAgentConfig.IoTEdgeModuleVersion}",
                            createOptions = string.Empty,
                        },
                    },
                    edgeHub = new
                    {
                        type = "docker",
                        status = "running",
                        restartPolicy = "always",
                        settings = new
                        {
                            image = $"mcr.microsoft.com/azureiotedge-hub:{edgeAgentConfig.IoTEdgeModuleVersion}",
                            createOptions = "{\"HostConfig\":{\"PortBindings\":{\"443/tcp\":[{\"HostPort\":\"443\"}],\"5671/tcp\":[{\"HostPort\":\"5671\"}],\"8883/tcp\":[{\"HostPort\":\"8883\"}]}}}",
                        },
                    },
                }
            };
        }

        public static EdgeHubSpecification SetEdgeHub(this EdgeAgentSpecification edgeAgentSpecification, EdgeHubConfig edgeHubConfig)
        {
            Dictionary<string, string> routes = new Dictionary<string, string>();
            edgeHubConfig.Routes.ForEach(x =>
            {
                routes.Add(x.Name, x.Value);
            });
            edgeAgentSpecification.ModulesContent.Add(
           "$edgeHub",
           new Dictionary<string, object>
           {
               ["properties.desired"] = new
               {
                   schemaVersion = edgeHubConfig.SchemaVersion,
                   routes = routes,
                   storeAndForwardConfiguration = new
                   {
                       timeToLiveSecs = edgeHubConfig.StoreAndForwardConfigurationTimeToLiveSecs,
                   },
               },
           });

            return edgeAgentSpecification as EdgeHubSpecification;
        }

        public static ConfigurationContent SetModuleDesiredProperty(this EdgeHubSpecification edgeHubSpecification, ModuleDesiredPropertyConfig moduleDesiredPropertyConfig)
        {
            edgeHubSpecification.ModulesContent.Add(
               moduleDesiredPropertyConfig.Name, new Dictionary<string, object>
               {
                   ["properties.desired"] = moduleDesiredPropertyConfig.DesiredProperty,
               });

            return edgeHubSpecification as ConfigurationContent;
        }
    }
}
