using System.Collections.Generic;

namespace Microsoft.Azure.Devices
{
    public class ModuleSpecification
    {
        public string Name { get; set; }
        public RestartPolicy RestartPolicy { get; set; } = RestartPolicy.Always;
        public string Image { get; set; }
        public string CreateOptions { get; set; }
        public ModuleStatus Status { get; set; } = ModuleStatus.Running;
        public string Version { get; set; } = "1.0";
        public string Type { get; } = "docker";
        public List<EnvironmentVariable> EnvironmentVariables { get; set; }
    }
}