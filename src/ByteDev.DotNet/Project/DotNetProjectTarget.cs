using System;
using System.Globalization;

namespace ByteDev.DotNet.Project
{
    public class DotNetProjectTarget
    {
        private bool _isOldStyleFormat;

        public DotNetProjectTarget(string targetValue)
        {
            if(string.IsNullOrEmpty(targetValue))
                throw new ArgumentException("Target value was null or empty.", nameof(targetValue));

            TargetValue = targetValue;

            SetTypeAndVersion(targetValue);
            SetDescription();
        }

        /// <summary>
        /// Raw target value as read from the project XML.
        /// </summary>
        public string TargetValue { get; }

        /// <summary>
        /// The type of target (Framework, Core or Standard).
        /// </summary>
        public TargetType Type { get; private set; }

        /// <summary>
        /// Version information for the target.
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// A readable description of the target.
        /// </summary>
        public string Description { get; private set; }

        public override string ToString()
        {
            return TargetValue;
        }
        
        private void SetTypeAndVersion(string targetValue)
        {
            if (targetValue.StartsWith(TargetValuePrefix.Core, true, CultureInfo.InvariantCulture))
            {
                Type = TargetType.Core;
                Version = targetValue.Substring(TargetValuePrefix.Core.Length);
                return;
            }

            if (targetValue.StartsWith(TargetValuePrefix.Standard, true, CultureInfo.InvariantCulture))
            {
                Type = TargetType.Standard;
                Version = targetValue.Substring(TargetValuePrefix.Standard.Length);
                return;
            }

            if(targetValue.StartsWith(TargetValuePrefix.FrameworkNew, true, CultureInfo.InvariantCulture))
            {
                Type = TargetType.Framework;
                Version = targetValue.Substring(TargetValuePrefix.FrameworkNew.Length);
                return;
            }

            if (targetValue.StartsWith(TargetValuePrefix.FrameworkOld, true, CultureInfo.InvariantCulture))
            {
                Type = TargetType.Framework;
                Version = targetValue.Substring(TargetValuePrefix.FrameworkOld.Length);
                _isOldStyleFormat = true;
                return;
            }

            throw new InvalidDotNetProjectException($"Could not determine {nameof(Type)} from '{targetValue}'.");
        }

        private void SetDescription()
        {
            switch (Type)
            {
                case TargetType.Framework:
                    Description = _isOldStyleFormat ? 
                        $".NET Framework {TargetValue.Substring(TargetValuePrefix.FrameworkOld.Length)}" : 
                        $".NET Framework {VersionNumberFormatter.Format(TargetValue.Substring(TargetValuePrefix.FrameworkNew.Length))}";
                    break;

                case TargetType.Core:
                    Description = $".NET Core {TargetValue.Substring(TargetValuePrefix.Core.Length)}"; 
                    break;

                case TargetType.Standard:
                    Description = $".NET Standard {TargetValue.Substring(TargetValuePrefix.Standard.Length)}";
                    break;

                default:
                    throw new InvalidOperationException($"Unhandled {nameof(TargetType)}: {Type}.");
            }
        }
    }
}