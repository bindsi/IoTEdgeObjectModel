using System;
using System.Collections.Generic;

namespace Microsoft.Azure.Devices
{
    public class TwinConfig
    {
        public int ExitCode { get; set; }
        public string StatusDescription { get; set; }
        public DateTime LastStartTimeUtc { get; set; }
        public DateTime LastExitTimeUtc { get; set; }
        public int RestartCount { get; set; }
        public DateTime LastRestartTimeUtc { get; set; }
        public string RuntimeStatus { get; set; }
        public string Version { get; set; }
        public string Status { get; set; }
        public string RestartPolicy { get; set; }
        public string ImagePullPolicy { get; set; }
        public string Type { get; set; }
        public Settings Settings { get; set; }
        public List<EnvironmentVariable> EnvironmentVariables { get; set; } = new List<EnvironmentVariable>();
    }
}