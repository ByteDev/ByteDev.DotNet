using System;
using System.Globalization;

namespace ByteDev.DotNet.Project
{
    public class DotNetProjectTarget
    {
        public DotNetProjectTarget(string targetValue)
        {
            if(string.IsNullOrEmpty(targetValue))
                throw new ArgumentException("Target value was null or empty.", nameof(targetValue));

            TargetValue = targetValue;

            SetTypeAndVersion(targetValue);
        }

        public string TargetValue { get; }

        public TargetType Type { get; private set; }

        public string Version { get; private set; }

        private void SetTypeAndVersion(string targetValue)
        {
            if (targetValue.StartsWith("netcoreapp", true, CultureInfo.InvariantCulture))
            {
                Type = TargetType.Core;
                Version = targetValue.Substring(10);
                return;
            }

            if (targetValue.StartsWith("netstandard", true, CultureInfo.InvariantCulture))
            {
                Type = TargetType.Standard;
                Version = targetValue.Substring(11);
                return;
            }

            if(targetValue.StartsWith("net", true, CultureInfo.InvariantCulture))
            {
                Type = TargetType.Framework;
                Version = targetValue.Substring(3);
                return;
            }

            if (targetValue.StartsWith("v", true, CultureInfo.InvariantCulture))
            {
                Type = TargetType.Framework;
                Version = targetValue.Substring(1);
                return;
            }

            throw new InvalidDotNetProjectException($"Could not determine {nameof(Type)} from '{targetValue}'.");
        }

        public override string ToString()
        {
            return TargetValue;
        }
    }
}