﻿namespace Microsoft.Azure.Devices
{
    public class EnvironmentVariable
    {
        public EnvironmentVariable(string name, string value)
        {
            Name = name;
            Value = value;
        }
    
        public string Name { get; set; }
        public string Value { get; set; }
    }
}