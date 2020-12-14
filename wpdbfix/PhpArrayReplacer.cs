using Fusi.Tools.Text;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace WpDbFix
{
    public sealed class PhpArrayReplacer
    {
        private const string PREFIX = "'a:";

        private readonly Regex _strRegex;
        private readonly TextReplacer _replacer;

        public int Count { get; private set; }

        public PhpArrayReplacer()
        {
            _strRegex = new Regex(@"s:\d+:\\""(.*?)\\""(;?)");
            _replacer = new TextReplacer(false);
        }

        public void LoadReplacements(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            _replacer.Load(reader);
        }

        private static int FindLimit(string text, int start)
        {
            // find end (next ' not preceded by \)
            int limit = start + 3;
            while (limit < text.Length)
            {
                if (text[limit] == '\'' && text[limit - 1] != '\\')
                    return limit + 1;
                limit++;
            }
            return -1;
        }

        private string ReplaceSerialized(string text)
        {
            return _strRegex.Replace(text, match =>
            {
                string value = match.Groups[1].Value;

                string newValue = _replacer.Replace(value);
                if (newValue != value) Count++;

                return $"s:{newValue.Length}:\\\"{newValue}\\\"" +
                    match.Groups[2].Value;
            });
        }

        public string Replace(string text)
        {
            Count = 0;

            if (string.IsNullOrEmpty(text)) return text;
            int prev = 0, start = text.IndexOf(PREFIX);
            if (start == -1) return text;

            StringBuilder sb = new StringBuilder();
            while (start > -1)
            {
                // copy any previous text unchanged
                while (prev < start) sb.Append(text[prev++]);

                // find extent of string in dump
                int limit = FindLimit(text, start);
                if (limit == -1)
                {
                    throw new ApplicationException(
                        $"String literal not closed at \"{text[start..]}\"");
                }

                // replace it
                string chunk = text[start..limit];
                sb.Append(ReplaceSerialized(chunk));

                // next
                prev = limit;
                start = text.IndexOf(PREFIX, limit);
            }

            // copy any following text unchanged
            while (prev < text.Length) sb.Append(text[prev++]);

            return sb.ToString();
        }
    }
}
