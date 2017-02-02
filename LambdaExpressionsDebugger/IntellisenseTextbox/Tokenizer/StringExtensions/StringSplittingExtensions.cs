using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntellisenseTextbox
{
    public static class StringSplittingExtensions
    {

        public static List<string> Split(this string splittableText, IEnumerable<string> seperators)
        {
            List<string> splitResults = new List<string>();

            var seperatorsGroupedByLength = (from splitter in
                                                 (from s in seperators group s by s.Length)
                                             orderby splitter.Key descending
                                             select splitter).ToList();

            List<string> textsToSplit = new[] { splittableText }.ToList();
            foreach (var seperatorGroup in seperatorsGroupedByLength)
            {
                var fixedWidthSeperatorList = seperatorGroup.ToFixedWidthStringList();
                splitResults = textsToSplit.Split(fixedWidthSeperatorList);
                textsToSplit = splitResults;
            }

            return splitResults;
        }

        public static List<string> Split(this List<string> textsToSplit, FixedWidthStringList seperators)
        {
            List<string> results = new List<string>();
            foreach (string textToBeSplitted in textsToSplit)
            {
                results.AddRange(textToBeSplitted.Split(seperators));
            }
            return results;
        }

        public static List<string> Split(this string textToBeSplitted, FixedWidthStringList seperators)
        {
            var indices = textToBeSplitted.AllIndicesOfAny(seperators);
            if (indices.Count == 0)
                return new[] { textToBeSplitted }.ToList();
            indices.Add(textToBeSplitted.Length);
            List<string> parts = new List<string>();
            int i = 0;
            foreach (int idx in indices)
            {
                if (idx < textToBeSplitted.Length)
                    parts.Add(textToBeSplitted.Substring(idx, indices[i + 1] - idx));
                i++;
            }
            
            List<string> results = new List<string>();
            if (indices.Count > 0)
                results.Add(textToBeSplitted.Substring(0, indices[0]));
            foreach (string part in parts)
            {
                if (!string.IsNullOrEmpty(part))
                {
                    string seperator = part.Substring(0, seperators.Width);
                    if (seperator != string.Empty)
                        results.Add(seperator);
                    string text = part.Substring(seperators.Width);
                    if (text != string.Empty)
                        results.Add(text);
                }
            }
            return results;
        }

        public static List<string> Split(this string textToBeSplitted, string splitter)
        {
            List<string> splittedParts = new List<string>();
            var tokens = textToBeSplitted.Replace(splitter, "\0").Split('\0');
            if (tokens.Length > 0)
                splittedParts.Add(tokens[0]);
            for (int i = 1; i < tokens.Length; i++)
            {
                splittedParts.Add(splitter);
                splittedParts.Add(tokens[i]);
            }
            return splittedParts;
        }

        public static List<int> AllIndicesOfAny(this string text, FixedWidthStringList seperators)
        {
            var indices = (from s in seperators
                           from i in text.AllIndicesOf(s)
                           select i).ToList();
            indices.Sort();
            return indices;
        }

        public static List<int> AllIndicesOf(this string text, string seperator)
        {
            List<int> indices = new List<int>();
            int startIdx = 0;
            while (startIdx + seperator.Length < text.Length)
            {
                int idx = text.IndexOf(seperator, startIdx);
                if (idx == -1)
                    break;
                indices.Add(idx);
                startIdx = idx + seperator.Length;
            }
            return indices;
        }
    }
}
