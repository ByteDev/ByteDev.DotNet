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
            FrameworkType = TargetFrameworkType.Unknown;
            Version = string.Empty;

            if (Moniker.StartsWith("net5", true, CultureInfo.InvariantCulture))
            {
                FrameworkType = TargetFrameworkType.Core;
                SetVersionDotNet5();
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
            else if (Moniker.StartsWith("wp", true, CultureInfo.InvariantCulture))
            {
                FrameworkType = TargetFrameworkType.WindowsPhone;
                SetVersionWindowsPhone();
            }
            else if (Moniker.StartsWith("uap", true, CultureInfo.InvariantCulture))
            {
                FrameworkType = TargetFrameworkType.UniversalWindowsPlatform;
                SetVersionUniversalWindowsPlatform();
            }
            else if (Moniker.StartsWith("netcore", true, CultureInfo.InvariantCulture))
            {
                FrameworkType = TargetFrameworkType.WindowsStore;
                SetVersionWindowsStore();
            }
            else if (Moniker.StartsWith("net", true, CultureInfo.InvariantCulture))
            {
                FrameworkType = TargetFrameworkType.Framework;
                Version = VersionNumberFormatter.Format(Moniker.Substring(3));
            }
        }

        private void SetVersionWindowsStore()
        {
            if (Moniker.StartsWith("netcore [", true, CultureInfo.InvariantCulture))
            {
                // netcore [netcore45]
                var startIndex = Moniker.IndexOf("[netcore", StringComparison.Ordinal);
                var endIndex = Moniker.IndexOf("]", StringComparison.Ordinal);

                if (endIndex > 0)
                {
                    var ver = Moniker.Substring(startIndex + 8, endIndex - startIndex - 8);
                    Version = VersionNumberFormatter.Format(ver);
                }
            }
            else
            {
                // netcore451 [win81]
                // netcore451
                var startIndex = Moniker.IndexOf("[", StringComparison.Ordinal);

                if (startIndex < 1)
                {
                    Version = VersionNumberFormatter.Format(Moniker.Substring(7));
                }
                else
                {
                    Version = VersionNumberFormatter.Format(Moniker.Substring(7, startIndex - 7));
                }
            }
        }

        private void SetVersionDotNet5()
        {
            var hyphenIndex = Moniker.IndexOf('-');

            if (hyphenIndex < 1)
                Version = Moniker.Substring(3);                     // net5.0
            else
                Version = Moniker.Substring(3, hyphenIndex - 3);    // net5.0-windows
        }

        private void SetVersionUniversalWindowsPlatform()
        {
            if (Moniker.StartsWith("uap [", true, CultureInfo.InvariantCulture))
            {
                // uap [uap10.0]
                var startIndex = Moniker.IndexOf("[uap", StringComparison.Ordinal);
                var endIndex = Moniker.IndexOf("]", StringComparison.Ordinal);

                if (endIndex > 0)
                {
                    var ver = Moniker.Substring(startIndex + 4, endIndex - startIndex - 4);
                    Version = VersionNumberFormatter.Format(ver);
                }
            }
            else
            {
                // uap10.0 [win10] [netcore50]
                var startIndex = Moniker.IndexOf("[", StringComparison.Ordinal);

                if (startIndex < 1)
                {
                    Version = VersionNumberFormatter.Format(Moniker.Substring(3));
                }
                else
                {
                    Version = VersionNumberFormatter.Format(Moniker.Substring(3, startIndex - 3));
                }
            }
        }

        private void SetVersionWindowsPhone()
        {
            if (Moniker.StartsWith("wpa", true, CultureInfo.InvariantCulture))
            {
                // wpa81
                Version = VersionNumberFormatter.Format(Moniker.Substring(3));
            }
            else if (Moniker.StartsWith("wp [", true, CultureInfo.InvariantCulture))
            {
                // wp [wp71]
                var startIndex = Moniker.IndexOf("[wp", StringComparison.Ordinal);
                var endIndex = Moniker.IndexOf("]", StringComparison.Ordinal);

                if (endIndex < 1)
                {
                    Version = string.Empty;
                }
                else
                {
                    var ver = Moniker.Substring(startIndex + 3, endIndex - startIndex - 3);
                    Version = VersionNumberFormatter.Format(ver);
                }
            }
            else
            {
                Version = VersionNumberFormatter.Format(Moniker.Substring(2));
            }
        }

        private void SetDescription()
        {
            switch (FrameworkType)
            {
                case TargetFrameworkType.Core:
                    Description = VersionGreaterOrEqual(5) ? $".NET {Version}" : $".NET Core {Version}";
                    break;

                case TargetFrameworkType.Standard:
                    Description = string.IsNullOrEmpty(Version) ? ".NET Standard" : $".NET Standard {Version}";
                    break;

                case TargetFrameworkType.Framework:
                    Description = string.IsNullOrEmpty(Version) ? ".NET Framework" : $".NET Framework {Version}";
                    break;

                case TargetFrameworkType.MicroFramework:
                    Description = ".NET Micro Framework";
                    break;

                case TargetFrameworkType.Silverlight:
                    Description = string.IsNullOrEmpty(Version) ? "Silverlight" : $"Silverlight {Version}";
                    break;

                case TargetFrameworkType.WindowsPhone:
                    Description = string.IsNullOrEmpty(Version) ? "Windows Phone" : $"Windows Phone {Version}";
                    break;

                case TargetFrameworkType.UniversalWindowsPlatform:
                    Description = string.IsNullOrEmpty(Version) ? "Universal Windows Platform" : $"Universal Windows Platform {Version}";
                    break;

                case TargetFrameworkType.WindowsStore:
                    Description = string.IsNullOrEmpty(Version) ? "Windows Store" : $"Windows Store {Version}";
                    break;
            }
        }

        private bool VersionGreaterOrEqual(int value)
        {
            if (string.IsNullOrEmpty(Version))
                return false;

            return int.Parse(Version.Substring(0, 1)) >= value;
        }
    }
}