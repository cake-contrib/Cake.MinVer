using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Cake.MinVer
{
    /// <summary>
    /// The version returned by MinVer
    /// </summary>
    public class MinVerVersion : IComparable<MinVerVersion>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinVerVersion" /> class.
        /// </summary>
        /// <param name="version">The version string returned by MinVer</param>
        public MinVerVersion(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(version));
            }

            var metaSplit = version.Split(new[] { '+' }, count: 2);
            var preSplit = metaSplit[0].Split(new[] { '-' }, count: 2);
            var rtmSplit = preSplit[0].Split('.');

            var major = rtmSplit[0];
            var minor = rtmSplit[1];
            var patch = rtmSplit[2];
            var preRelease = preSplit.ElementAtOrDefault(1);
            var buildMetadata = metaSplit.ElementAtOrDefault(1);

            Major = int.Parse(major, CultureInfo.InvariantCulture);
            Minor = int.Parse(minor, CultureInfo.InvariantCulture);
            Patch = int.Parse(patch, CultureInfo.InvariantCulture);

            PreRelease = preRelease;
            BuildMetadata = buildMetadata;

            AssemblyVersion = $"{Major}.0.0.0";
            FileVersion = $"{Major}.{Minor}.{Patch}.0";
            Version = version;
        }

        /// <summary>
        /// The original, non-normalized version string
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// The Major version number
        /// </summary>
        public int Major { get; }

        /// <summary>
        /// The Minor version number
        /// </summary>
        public int Minor { get; }

        /// <summary>
        /// The Patch version number
        /// </summary>
        public int Patch { get; }

        /// <summary>
        /// The Pre-release extension
        /// </summary>
        public string PreRelease { get; }

        /// <summary>
        /// The Build metadata extension
        /// </summary>
        public string BuildMetadata { get; }

        /// <summary>
        /// <see cref="Major" />.0.0.0
        /// </summary>
        public string AssemblyVersion { get; }

        /// <summary>
        /// <see cref="Major" />.<see cref="Minor" />.<see cref="Patch" />.0
        /// </summary>
        public string FileVersion { get; }

        /// <summary>
        /// The original, non-normalized version string. Same as <see cref="Version" />
        /// </summary>
        public string InformationalVersion => Version;

        /// <summary>
        /// The original, non-normalized version string. Same as <see cref="Version" />
        /// </summary>
        public string PackageVersion => Version;

        /// <inheritdoc />
        public override string ToString() => Version;

        /// <inheritdoc />
        public override int GetHashCode() => Version.GetHashCode();

        /// <inheritdoc />
        public int CompareTo(MinVerVersion other)
        {
            if (other is null)
            {
                return 1;
            }

            var major = Major.CompareTo(other.Major);
            if (major != 0)
            {
                return major;
            }

            var minor = Minor.CompareTo(other.Minor);
            if (minor != 0)
            {
                return minor;
            }

            var patch = Patch.CompareTo(other.Patch);
            if (patch != 0)
            {
                return patch;
            }

            var preRelease = NaturalStringComparer.Instance.Compare(PreRelease, other.PreRelease);
            if (preRelease != 0)
            {
                return preRelease;
            }

            return NaturalStringComparer.Instance.Compare(BuildMetadata, other.BuildMetadata);
        }

        internal class NaturalStringComparer : IComparer<string>
        {
            private static readonly Lazy<NaturalStringComparer> _instance = new Lazy<NaturalStringComparer>(isThreadSafe: true);

            public static NaturalStringComparer Instance => _instance.Value;

            public int Compare(string strA, string strB)
            {
                if (strA is null && strB is null)
                {
                    return 0;
                }

                if (strA is null)
                {
                    return -1;
                }

                if (strB is null)
                {
                    return 1;
                }

                var lengthOfStrA = strA.Length;
                var lengthOfStrB = strB.Length;

                for (int indexStrA = 0, indexStrB = 0; indexStrA < lengthOfStrA && indexStrB < lengthOfStrB; indexStrA++, indexStrB++)
                {
                    if (char.IsDigit(strA[indexStrA]) && char.IsDigit(strB[indexStrB]))
                    {
                        long numericValueStrA = 0;
                        long numericValueStrB = 0;

                        for (; indexStrA < lengthOfStrA && char.IsDigit(strA[indexStrA]); indexStrA++)
                        {
                            numericValueStrA = numericValueStrA * 10 + strA[indexStrA] - '0';
                        }

                        for (; indexStrB < lengthOfStrB && char.IsDigit(strB[indexStrB]); indexStrB++)
                        {
                            numericValueStrB = numericValueStrB * 10 + strB[indexStrB] - '0';
                        }

                        if (numericValueStrA != numericValueStrB)
                        {
                            return numericValueStrA > numericValueStrB ? 1 : -1;
                        }
                    }

                    if (indexStrA < lengthOfStrA && indexStrB < lengthOfStrB && strA[indexStrA] != strB[indexStrB])
                    {
                        return strA[indexStrA] > strB[indexStrB] ? 1 : -1;
                    }
                }

                return lengthOfStrA - lengthOfStrB;
            }
        }
    }
}
