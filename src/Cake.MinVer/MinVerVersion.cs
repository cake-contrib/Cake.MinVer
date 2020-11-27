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
        /// <param name="versionString">The version string returned by MinVer</param>
        public MinVerVersion(string versionString)
        {
            if (string.IsNullOrWhiteSpace(versionString))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(versionString));
            }

            if (!TryParseAndFill(versionString, this))
            {
                throw new FormatException($"The string '{versionString}' was not recognized as a valid MinVer version");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinVerVersion" /> class.
        /// </summary>
        protected internal MinVerVersion()
        {
        }

        /// <summary>
        /// The original, non-normalized version string
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// The Major version number
        /// </summary>
        public int Major { get; private set; }

        /// <summary>
        /// The Minor version number
        /// </summary>
        public int Minor { get; private set; }

        /// <summary>
        /// The Patch version number
        /// </summary>
        public int Patch { get; private set; }

        /// <summary>
        /// The Pre-release extension
        /// </summary>
        public string PreRelease { get; private set; }

        /// <summary>
        /// Returns <see langword="true"/> when <see cref="PreRelease" /> is not null or empty
        /// otherwise <see langword="false"/>
        /// </summary>
        public bool IsPreRelease => !string.IsNullOrWhiteSpace(PreRelease);

        /// <summary>
        /// The Build metadata extension
        /// </summary>
        public string BuildMetadata { get; private set; }

        /// <summary>
        /// <see cref="Major" />.0.0.0
        /// </summary>
        public string AssemblyVersion { get; private set; }

        /// <summary>
        /// <see cref="Major" />.<see cref="Minor" />.<see cref="Patch" />.0
        /// </summary>
        public string FileVersion { get; private set; }

        /// <summary>
        /// The original, non-normalized version string. Same as <see cref="Version" />
        /// </summary>
        public string InformationalVersion => Version;

        /// <summary>
        /// The original, non-normalized version string. Same as <see cref="Version" />
        /// </summary>
        public string PackageVersion => Version;

        /// <summary>
        /// Converts the specified string representation of MinVer-compatible version to its <see cref="MinVerVersion" /> equivalent.
        /// </summary>
        /// <param name="versionString">A string containing a string version to convert.</param>
        /// <returns>An object that is equivalent to the version contained in <paramref name="versionString">versionString</paramref>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="versionString">versionString</paramref> is null or empty.</exception>
        /// <exception cref="T:System.FormatException"><paramref name="versionString">versionString</paramref> does not contain a valid string representation of MinVer-compatible version.</exception>
        public static MinVerVersion Parse(string versionString)
        {
            if (!TryParse(versionString, out var version))
            {
                throw new FormatException($"The string '{versionString}' was not recognized as a valid MinVer version");
            }

            return version;
        }

        /// <summary>
        /// Converts the specified string representation of MinVer-compatible version to its <see cref="MinVerVersion" /> equivalent and returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="versionString">A string containing a string version to convert.</param>
        /// <param name="version">When this method returns <see langword="true" />, contains the <see cref="MinVerVersion" /> value equivalent to the version contained in <paramref name="versionString">versionString</paramref>, if the conversion succeeded, or <see langword="null" /> if the conversion failed. The conversion fails if the <paramref name="versionString">versionString</paramref> parameter is null, is an empty string (""), or does not contain a valid string representation of a MinVer-compatible version.</param>
        /// <returns><see langword="true" /> if the <paramref name="versionString">versionString</paramref> parameter was converted successfully; otherwise, <see langword="false" />.</returns>
        public static bool TryParse(string versionString, out MinVerVersion version)
        {
            version = new MinVerVersion();

            if (!TryParseAndFill(versionString, version))
            {
                version = null;
                return false;
            }

            return true;
        }

        private static bool TryParseAndFill(string versionString, MinVerVersion version)
        {
            if (string.IsNullOrWhiteSpace(versionString))
            {
                return false;
            }

            var metaSplit = versionString.Split(new[] { '+' }, count: 2);
            var preSplit = metaSplit[0].Split(new[] { '-' }, count: 2);
            var rtmSplit = preSplit[0].Split('.');

            if (rtmSplit.Length != 3)
            {
                return false;
            }

            var major = rtmSplit[0];
            var minor = rtmSplit[1];
            var patch = rtmSplit[2];
            var preRelease = preSplit.ElementAtOrDefault(1);
            var buildMetadata = metaSplit.ElementAtOrDefault(1);

            if (!int.TryParse(major, NumberStyles.None, CultureInfo.InvariantCulture, out var majorInt)
                || !int.TryParse(minor, NumberStyles.None, CultureInfo.InvariantCulture, out var minorInt)
                || !int.TryParse(patch, NumberStyles.None, CultureInfo.InvariantCulture, out var patchInt))
            {
                return false;
            }

            version.Major = majorInt;
            version.Minor = minorInt;
            version.Patch = patchInt;
            version.PreRelease = preRelease;
            version.BuildMetadata = buildMetadata;

            version.AssemblyVersion = FormattableString.Invariant($"{majorInt}.0.0.0");
            version.FileVersion = FormattableString.Invariant($"{majorInt}.{minorInt}.{patchInt}.0");
            version.Version = versionString;

            return true;
        }

        /// <inheritdoc />
        public override string ToString() => Version;

        /// <inheritdoc />
        // ReSharper disable once NonReadonlyMemberInGetHashCode
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

        /// <summary>
        /// Implicit conversion from <see cref="MinVerVersion"/> to <see cref="string"/> to simplify use in build scripts.
        /// </summary>
        /// <param name="version">The MinVer version.</param>
        /// <returns>The original, non-normalized version string.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="version"/> is <see langword="null"/>.</exception>
        public static implicit operator string(MinVerVersion version)
        {
            if (version is null) throw new ArgumentNullException(nameof(version));
            return version.ToString();
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
