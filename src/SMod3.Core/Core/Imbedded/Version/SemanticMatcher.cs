using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

namespace SMod3.Core.Imbedded.Version
{
    /// <summary>
    ///     Parser for <see cref="SemanticVersion"/>.
    /// </summary>
    public sealed class SemanticMatcher
    {
        /// <summary>
        ///     Matcher by default.
        /// </summary>
        /// <remarks>
        ///     We use a modified version of Semantic Versioning 2.0.0,
        ///     it doesn't include the group 'buildmeta' and renames the group 'prerelease' to 'suffix'.
        /// </remarks>
        public static readonly SemanticMatcher Default = new SemanticMatcher(new Regex(@"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)))?",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ECMAScript));

        public Regex Regex { get; }

        public SemanticMatcher(Regex regex)
        {
            Regex = regex;
        }

        /// <summary>
        ///     Parses a raw string containing a version and returns a sematic version.
        /// </summary>
        /// <param name="raw">
        ///     Raw version string.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Raw string is null or empty.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Raw string is not a semantic version string.
        /// </exception>
        public SemanticVersion Parse(string raw)
        {
            if (string.IsNullOrEmpty(raw))
                throw new ArgumentException("Raw string cannot be null or empty");

            var match = Regex.Match(raw);
            if (!match.Success || !match.Groups[1].Success || !match.Groups[2].Success || !match.Groups[3].Success)
                throw new InvalidOperationException("Raw string isn't a semantic version string");

            return new SemanticVersion(ushort.Parse(match.Groups[1].Value), ushort.Parse(match.Groups[2].Value), ushort.Parse(match.Groups[3].Value), match.Groups[4].Value);
        }
    }
}
