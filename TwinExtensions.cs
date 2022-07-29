using Microsoft.Azure.Devices.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.Devices
{
    public static class TwinExtensions
    {
        public static TwinConfig GetTwinConfig(this Twin twin, string moduleName)
        {
            if (!twin.Properties.Reported["modules"].Contains(moduleName))
            {
                return default(TwinConfig);
            }

            dynamic module = twin.Properties.Reported["modules"][moduleName];
            TwinConfig twinConfig = new TwinConfig()
            {
                ExitCode = module["exitCode"],
                LastExitTimeUtc = DateTime.Parse(module["lastExitTimeUtc"]),
                Status = Enum.Parse(typeof(ModuleStatus), module["status"], true),
            };

            // TODO:Env
            // twinConfig.EnvironmentVariables
            return twinConfig;
        }
    }
}
