using System;
using System.Text;

namespace SMod3.Core.Misc
{
    public static class StringMisc
    {
        /// <summary>
        ///     The type of interval that is inserted in the gap.
        /// </summary>
        public enum IntervalType
        {
            /// <summary>
            ///     A type of register that uses '_' between.
            /// </summary>
            SnakeInterval,
            /// <summary>
            ///     A type of register that uses '.' between.
            /// </summary>
            DotInterval
        }

        /// <summary>
        ///     The case type is followed by the next letter after the interval.
        /// </summary>
        public enum CaseType
        {
            UpperCase,
            LowerCase,
            /// <summary>
            ///     The next letter after the interval will not be aligned.
            /// </summary>
            SourceCase
        }

        #region High-level

        /// <summary>
        ///     Returns a uppercase string as a snake.
        /// </summary>
        /// <remarks>
        ///     Not completely uppercase,
        ///     but only the next letter after the interval.
        /// </remarks>
        public static string ToUpperSnakeCase(string source)
        {
            return ToSnakeCase(source, CaseType.LowerCase);
        }

        /// <summary>
        ///     Returns string as snake with full uppercase.
        /// </summary>
        public static string ToFullyUpperSnakeCase(string source)
        {
            return ToSnakeCase(source, CaseType.SourceCase).ToUpperInvariant();
        }

        /// <summary>
        ///     Returns a string in the specified case as a snake.
        /// </summary>
        /// <remarks>
        ///     Not completely lowercase,
        ///     but only the next letter after the interval.
        /// </remarks>
        public static string ToLowerSnakeCase(string source)
        {
            return ToSnakeCase(source, CaseType.LowerCase);
        }

        /// <summary>
        ///     Returns a string as snake in the specified case.
        /// </summary>
        public static string ToSnakeCase(string source, CaseType caseType)
        {
            return ToCustomInterval(source, IntervalType.SnakeInterval, caseType);
        }

        #endregion

        #region Low-level

        /// <summary>
        ///     Processes a string according to specified requirements.
        /// </summary>
        public static string ToCustomInterval(string source, IntervalType intervalType, CaseType caseType = CaseType.SourceCase)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException("String must not be empty or null", nameof(source));

            static void ProcessStep(StringBuilder @string, char interval, char source)
            {
                @string.Append(interval);
                @string.Append(source);
            }

            var curInterval = GetInterval(intervalType);
            var needInterval = false;
            // StringBuilderPool should be here
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                var lastNeedInterval = needInterval;
                needInterval = false;

                var curChar = source[i];
                // Skip start with curInterval
                if (i == 0 && curChar == curInterval)
                    continue;

                if (lastNeedInterval)
                    ProcessStep(builder, curInterval, ProcessCase(curChar, caseType));
                else if (curChar == curInterval)
                    needInterval = true;
                else if (i > 0 && char.IsUpper(curChar) && source[i - 1] != curInterval)
                    ProcessStep(builder, curInterval, ProcessCase(curChar, caseType));
            }

            return builder.ToString();
        }

        /// <summary>
        ///     Returns the interval as char.
        /// </summary>
        public static char GetInterval(IntervalType intervalType)
        {
            return intervalType switch
            {
                IntervalType.SnakeInterval => '_',
                IntervalType.DotInterval => '.',
                // It will never happen, yes,
                // therefore don't document the exception
                _ => throw new InvalidOperationException("This interval was not found"),
            };
        }

        /// <summary>
        ///     Processes a letter according to case type.
        /// </summary>
        public static char ProcessCase(char letter, CaseType caseType)
        {
            return caseType switch
            {
                CaseType.SourceCase => letter,
                CaseType.LowerCase => char.ToLowerInvariant(letter),
                CaseType.UpperCase => char.ToUpperInvariant(letter),
                _ => throw new InvalidOperationException("This case type was not found"),
            };
        }

        #endregion
    }
}
