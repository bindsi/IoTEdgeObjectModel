using System.Collections.Generic;

namespace Microsoft.Azure.Devices
{
    public class EdgeAgentConfig
    {
        public string SchemaVersion { get; } = "1.1";
        public string IoTEdgeModuleVersion { get; set; } = "1.2";
        public List<RegistryCredential> RegistryCredentials { get; set; }
        public List<ModuleSpecification> ModuleSpecifications { get; set; }
    }
}