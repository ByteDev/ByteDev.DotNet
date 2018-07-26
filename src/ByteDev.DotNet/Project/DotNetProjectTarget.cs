using System;
using ByteDev.DotNet.Project;

namespace ByteDev.DotNet
{
    public class DotNetProjectTarget
    {
        public DotNetProjectTarget(string targetValue)
        {
            if(string.IsNullOrEmpty(targetValue))
                throw new ArgumentException("Target framework was null or empty.", nameof(targetValue));

            TargetValue = targetValue;
            Type = GetTargetType(targetValue);
            Version = GetVersion(targetValue);
        }

        public string TargetValue { get; }

        public TargetType Type { get; }

        public string Version { get; }

        private TargetType GetTargetType(string targetValue)
        {
            if (targetValue.IsCore())
                return TargetType.Core;

            if (targetValue.IsStandard())
                return TargetType.Standard;

            if (targetValue.IsFramework())
                return TargetType.Framework;

            throw new InvalidDotNetProjectException($"Could not determine {nameof(Type)} from '{targetValue}'.");
        }

        private string GetVersion(string targetValue)
        {
            if (targetValue.IsCore())
                return targetValue.Substring(10);

            if (targetValue.IsStandard())
                return targetValue.Substring(11);

            if (targetValue.IsFramework())
                return targetValue.Substring(1);

            throw new InvalidDotNetProjectException($"Could not determine version from '{targetValue}'.");
        }
    }
}