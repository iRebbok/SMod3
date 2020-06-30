using System;
using System.Text.RegularExpressions;

namespace SMod3.Core.Imbedded.Version
{
    public readonly struct SemanticVersion : IComparable<SemanticVersion>, IEquatable<SemanticVersion>
    {
        #region Properties

        /// <summary>
        ///     Major version,
        ///     which increases with a large divergence of the API,
        ///     for example, for synchronization,
        ///     it's necessary to change the operation of the parts.
        /// </summary>
        public ushort Major { get; }

        /// <summary>
        ///     Minor version,
        ///     which increases with changes as an increase in functionality
        ///     and which can break the API, which require recompilation.
        /// </summary>
        public ushort Minor { get; }

        /// <summary>
        ///     Patch version,
        ///     increases with small changes,
        ///     such as fixing or adding a small API.
        /// </summary>
        public ushort Patch { get; }

        /// <summary>
        /// Suffix version, which is indicated for example for beta/alpha versions.
        /// </summary>
        public string? Suffix { get; }

        #endregion

        public static readonly Regex SuffixValidator = new Regex(@"^(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)))$");

        /// <exception cref="ArgumentException">
        ///     Suffix failed validation.
        /// </exception>
        public SemanticVersion(ushort major = 0, ushort minor = 0, ushort patch = 0, string? suffix = null)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Suffix = !string.IsNullOrWhiteSpace(suffix) ? (SuffixValidator.IsMatch(suffix) ? suffix : throw new ArgumentException("Suffix failed validation", nameof(suffix))) : null;
        }

        public int CompareTo(SemanticVersion other)
        {
            var majorCompare = Major.CompareTo(other.Major);
            if (majorCompare == 0)
            {
                var minorCompare = Minor.CompareTo(other.Minor);
                if (minorCompare == 0)
                {
                    var pathCompare = Patch.CompareTo(other.Patch);
                    if (pathCompare == 0)
                    {
                        var curIsNull = Suffix is null;
                        var otherIsNull = other.Suffix is null;

                        var suffixBoolCompare = curIsNull.CompareTo(otherIsNull);
                        if (suffixBoolCompare == 0 && !curIsNull && !otherIsNull)
                        {
                            return Suffix!.CompareTo(other.Suffix);
                        }

                        return suffixBoolCompare;
                    }
                }

                return minorCompare;
            }

            return majorCompare;
        }

        public override bool Equals(object obj)
        {
            return obj is SemanticVersion other && Equals(other);
        }

        public bool Equals(SemanticVersion other)
        {
            return CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            unchecked
            {
#pragma warning disable RCS1032 // Remove redundant parentheses.
                return ((((Major * 397) ^ Minor * 397) ^ Patch * 397) ^ Suffix?.GetHashCode() ?? 0);
#pragma warning restore RCS1032 // Remove redundant parentheses.
            }
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}" + (Suffix is null ? string.Empty : $"-{Suffix}");
        }
    }
}
