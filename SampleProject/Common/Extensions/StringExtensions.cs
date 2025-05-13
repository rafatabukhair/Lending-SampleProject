using System.Text.RegularExpressions;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex AddSpaceBeforeCapitalLettersRegex = new Regex("(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])",
                                                                                    RegexOptions.IgnorePatternWhitespace);

        public static string AddSpaceBeforeCapitalLetter(this string text)
        {
            return string.IsNullOrEmpty(text)
                ? text
                : string.Join(" ", AddSpaceBeforeCapitalLettersRegex.Split(text));
        }
    }
}