using System.Collections.Generic;

namespace Microsoft.Azure.Devices
{
    public class EdgeHubConfig
    {
        public List<Route> Routes = new List<Route>();
        public string SchemaVersion { get; } = "1.1";
        public int StoreAndForwardConfigurationTimeToLiveSecs { get; set; } = 7200;
    }
}