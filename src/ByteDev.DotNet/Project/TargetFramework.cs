using System;
using System.Globalization;

namespace ByteDev.DotNet.Project
{
    /// <summary>
    /// Represents the .NET project target type.
    /// </summary>
    public class TargetFramework
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.DotNet.Project.DotNetProjectTarget" /> class.
        /// </summary>
        /// <param name="moniker">Target framework moniker.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="moniker" /> is null or empty.</exception>
        public TargetFramework(string moniker)
        {
            if(string.IsNullOrEmpty(moniker))
                throw new ArgumentException("Target framework moniker was null or empty.", nameof(moniker));

            Moniker = moniker;

            SetTypeAndVersion();
            SetDescription();
        }

        /// <summary>
        /// Target Framework Moniker (TFM) (e.g. netcoreapp3.1, netstandard2.0, net462 etc.).
        /// </summary>
        public string Moniker { get; }

        /// <summary>
        /// Target framework type (e.g. Framework, Core or Standard).
        /// </summary>
        public TargetFrameworkType FrameworkType { get; private set; }

        /// <summary>
        /// Version information for the target.
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// Readable description of the target.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Returns a string representation of <see cref="T:ByteDev.DotNet.Project.TargetFramework" />.
        /// </summary>
        /// <returns>String representation of <see cref="T:ByteDev.DotNet.Project.TargetFramework" />.</returns>
        public override string ToString()
        {
            return Moniker;
        }
        
        private void SetTypeAndVersion()
        {
            if (Moniker.StartsWith("net5", true, CultureInfo.InvariantCulture))
            {
                FrameworkType = TargetFrameworkType.Core;

                var hypenPos = Moniker.IndexOf('-');

                if (hypenPos < 1)
                    Version = Moniker.Substring(3);
                else
                    Version = Moniker.Substring(3, hypenPos - 3);
            }
            else if (Moniker.StartsWith("netcoreapp", true, CultureInfo.InvariantCulture))
            {
                FrameworkType = TargetFrameworkType.Core;
                Version = Moniker.Substring(10);
            }
            else if (Moniker.StartsWith("netstandard", true, CultureInfo.InvariantCulture))
            {
                FrameworkType = TargetFrameworkType.Standard;
                Version = Moniker.Substring(11);
            }
            else if (Moniker.StartsWith("netmf", true, CultureInfo.InvariantCulture))
            {
                FrameworkType = TargetFrameworkType.MicroFramework;
                Version = string.Empty;
            }
            else if (Moniker.StartsWith("net", true, CultureInfo.InvariantCulture))
            {
                FrameworkType = TargetFrameworkType.Framework;
                Version = VersionNumberFormatter.Format(Moniker.Substring(3));
            }
            else if (Moniker.StartsWith("v", true, CultureInfo.InvariantCulture))
            {
                FrameworkType = TargetFrameworkType.Framework;
                Version = Moniker.Substring(1);
            }
            else if (Moniker.StartsWith("sl", true, CultureInfo.InvariantCulture))
            {
                FrameworkType = TargetFrameworkType.Silverlight;
                Version = Moniker.Substring(2);
            }
        }

        private void SetDescription()
        {
            switch (FrameworkType)
            {
                case TargetFrameworkType.Core:
                    Description = float.Parse(Version) >= 5 ? $".NET {Version}" : $".NET Core {Version}";
                    break;

                case TargetFrameworkType.Standard:
                    Description = $".NET Standard {Version}";
                    break;

                case TargetFrameworkType.Framework:
                    Description = $".NET Framework {Version}";
                    break;

                case TargetFrameworkType.MicroFramework:
                    Description = ".NET Micro Framework";
                    break;

                case TargetFrameworkType.Silverlight:
                    Description = $"Silverlight {Version}";
                    break;
            }
        }
    }
}