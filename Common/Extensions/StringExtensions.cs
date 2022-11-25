using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex EmailRegexPattern = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

        private static readonly Regex TrimRegexPattern = new Regex(@"^\s+|\s+$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

        private static readonly Regex SpecialCharacterRegexPattern = new Regex(@"[^0-9A-Za-z ]",
            RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

        private static readonly Regex SpecialCharacterRegexPatternWithSpace = new Regex(@"[^\w\d]",
            RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

        private static readonly Regex UrlRegexPattern = new Regex(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

        private const string TinyUrlLink = "http://tinyurl.com/api-create.php?url=";

        public static bool IsValidEmailAddress(this string input)
        {
            return !input.IsNullOrEmpty() && EmailRegexPattern.IsMatch(input.Trim());
        }

        public static string TrimAll(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : TrimRegexPattern.Replace(value, string.Empty);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool HasValue(this string input)
        {
            return !input.IsNullOrEmpty();
        }

        public static string RemoveSpecialCharacters(this string value, bool ignoreSpace = false)
        {
            if (ignoreSpace)
                return string.IsNullOrWhiteSpace(value) ? string.Empty : SpecialCharacterRegexPattern.Replace(value, string.Empty);

            return string.IsNullOrWhiteSpace(value) ? string.Empty : SpecialCharacterRegexPatternWithSpace.Replace(value, string.Empty);
        }

        public static string GetValueOrDefault(this string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : value;
        }

        public static string ToTitleCase(this string value)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static string RemoveSpaces(this string value)
        {
            return IsNullOrEmpty(value) ? string.Empty : value.Replace(" ", string.Empty);
        }

        public static bool IsNumeric(this string value, bool isFloatingPoint = false)
        {
            return long.TryParse(value.RemoveSpaces(), NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out _);
        }

        public static string Copy(this string input)
        {
            return input == null ? null : new StringBuilder(input).ToString(); //string.Copy(input);
        }

        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static string JoinWith<T>(this IEnumerable<T> values, string separator)
        {
            return string.Join(separator, values.EmptyIfNull());
        }

        public static string JoinWithComma<T>(this IEnumerable<T> values, StringJoinOptions options = StringJoinOptions.None)
        {
            var suffix = GetStringJoinSeparatorSuffix(options);
            return values.JoinWith(",".Append(suffix));
        }

        public static string JoinWithSemiColon<T>(this IEnumerable<T> values, StringJoinOptions options = StringJoinOptions.None)
        {
            var suffix = GetStringJoinSeparatorSuffix(options);
            return values.JoinWith(";".Append(suffix));
        }

        private static string GetStringJoinSeparatorSuffix(StringJoinOptions options)
        {
            var shouldAddSpace = (options & StringJoinOptions.AddSpaceSuffix) == StringJoinOptions.AddSpaceSuffix;
            return shouldAddSpace ? " " : string.Empty;
        }

        public static string JoinWithNewLine<T>(this IEnumerable<T> values)
        {
            return values.JoinWith(Environment.NewLine);
        }

        

        public static string Append(this string input, IEnumerable<string> values)
        {
            return input.Append(values.ToArray());
        }

        public static string Append(this string input, params string[] values)
        {
            return input.Append(values.JoinWith(string.Empty));
        }

        public static bool ContainsAny(this string input, params string[] contains)
        {
            return input != null && contains.Any(input.Contains);
        }

        public static bool ContainsAll(this string input, params string[] contains)
        {
            if (input == null) return false;
            return contains.All(input.Contains);
        }

        public static IEnumerable<string> SplitBy(this string value, string delimiter, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            var splitValues = value.Split(new[] { delimiter }, System.StringSplitOptions.None);

            if (options.HasFlag(StringSplitOptions.TrimWhiteSpaceFromEntries) ||
                options.HasFlag(StringSplitOptions.TrimWhiteSpaceAndRemoveEmptyEntries))
                splitValues = splitValues.Select(s => s.Trim()).ToArray();

            if (options.HasFlag(StringSplitOptions.RemoveEmptyEntries) ||
                options.HasFlag(StringSplitOptions.TrimWhiteSpaceAndRemoveEmptyEntries))
                splitValues = splitValues.Where(s => !s.IsNullOrEmpty()).ToArray();

            return splitValues;
        }

        public static IEnumerable<string> SplitByComma(this string value, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return value.SplitBy(",", options);
        }

        public static IEnumerable<string> SplitBySemiColon(this string value, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return value.SplitBy(";", options);
        }

        public static IEnumerable<string> SplitByNewLine(this string value, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return value.SplitBy(Environment.NewLine, options);
        }

        public static bool ToBoolean(this string input)
        {
            if(input.IsNullOrWhiteSpace())
                throw new ArgumentException("Input value can not be empty");

            var trueValues = new[] { "true", "on", "1", "y", "yes" };
            var falseValues = new[] { "false", "off", "0", "n", "no" };

            if (trueValues.Contains(input.ToLower()))
                return true;

            return !falseValues.Contains(input.ToLower()) && Convert.ToBoolean(input);
        }
 
        public static bool Contains(this string inputValue, string comparisonValue, StringComparison comparisonType)
        {
            return inputValue.IndexOf(comparisonValue, comparisonType) != -1;
        }

        public static bool CaseInsensitiveContains(this string text, string comparisonValue, StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            return text.IndexOf(comparisonValue, stringComparison) >= 0;
        }

        public static XDocument ToXDocument(this string xml)
        {
            return XDocument.Parse(xml);
        }

        public static string ToBase64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData.Replace("-", "/"));
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string ToBase64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes).Replace("/", "-");
        }

        public static string TakeCharacters(this string input, int numberOfCharactersToTake)
        {
            return input.IsNullOrEmpty() ? input : new string(input.Take(numberOfCharactersToTake).ToArray());
        }

        public static string TakeCharacters(this string input, int numberOfCharactersToSkip, int numberOfCharactersToTake)
        {
            return input.IsNullOrEmpty()
                ? input
                : new string(input.Skip(numberOfCharactersToSkip).Take(numberOfCharactersToTake).ToArray());
        }

        public static string ToEllipsis(this string input, int numberOfCharactersToDisplay)
        {
            var numberOfCharactersToTake = numberOfCharactersToDisplay - 3;
            return input.IsNullOrEmpty() ? input : input.Length <= numberOfCharactersToDisplay ? input : new string(input.Take(numberOfCharactersToTake).ToArray()) + "...";
        }

        public static byte[] ToByteArray(this string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }

        public static bool Like(this string value, string search)
        {
            if (value.IsNullOrEmpty())
                return false;

            return value.Contains(search) || value.StartsWith(search) || value.EndsWith(search);
        }

        public static int WordCount(this string sourceString, System.StringSplitOptions options = System.StringSplitOptions.RemoveEmptyEntries)
        {
            return sourceString.Split(new char[] { ' ', '.', '?' }, options).Length;
        }

        public static string ToTinyUrl(this string url)
        {
            if (!IsValidUrl(url))
                throw new ArgumentException("Invalid Url");

            var address = new Uri($"{TinyUrlLink}{url}");
            using var client = new WebClient();
            return client.DownloadString(address);
        }

        public static bool IsValidUrl(this string url)
        {
            return UrlRegexPattern.IsMatch(url);
        }
    }


    [Flags]
    public enum StringSplitOptions
    {
        None = 0,
        RemoveEmptyEntries = 1,
        TrimWhiteSpaceFromEntries = 2,
        TrimWhiteSpaceAndRemoveEmptyEntries = 4
    }

    [Flags]
    public enum StringJoinOptions
    {
        None = 0,
        AddSpaceSuffix = 1
    }
}
