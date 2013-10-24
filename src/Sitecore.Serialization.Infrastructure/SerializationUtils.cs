using System;
using System.Collections.Generic;
using System.IO;
using Sitecore.Data.Serialization.ObjectModel;

namespace Sitecore.Serialization.Infrastructure
{
    public static class SerializationUtils
    {
        private static string BeginReadHeaders(Tokenizer reader)
        {
            var str = reader.NextLine();
            while (str.StartsWith("----", StringComparison.InvariantCulture))
            {
                str = reader.NextLine();
            }
            return str;
        }

        private static string GetSplitter(string key)
        {
            return string.Format("----{0}----", key);
        }

        public static bool IsItemSerialization(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (TextReader reader = new StreamReader(filePath))
                {
                    return (string.Compare(reader.ReadLine(), GetSplitter("item"), StringComparison.InvariantCultureIgnoreCase) == 0);
                }
            }
            return false;
        }

        internal static Dictionary<string, string> ReadHeaders(Tokenizer reader)
        {
            var s = BeginReadHeaders(reader);
            var result = new Dictionary<string, string>();
            ReadHeaders(reader, s, (key, value) => result[key] = value.TrimStart(new char[0]));
            return result;
        }

        private static void ReadHeaders(Tokenizer reader, string s, Action<string, string> updateValue)
        {
            while (!string.IsNullOrEmpty(s))
            {
                var strArray = s.Split(new[] { ':' }, 2);
                if (strArray.Length == 2)
                {
                    updateValue(strArray[0], strArray[1]);
                }
                s = reader.NextLine();
            }
        }

        internal static Dictionary<string, List<string>> ReadMultiHeaders(Tokenizer reader)
        {
            var s = BeginReadHeaders(reader);
            var result = new Dictionary<string, List<string>>();
            ReadHeaders(reader, s, delegate(string key, string value)
                {
                    if (!result.ContainsKey(key))
                    {
                        result[key] = new List<string>();
                    }
                    result[key].Add(value.TrimStart(new char[0]));
                });
            return result;
        }

        internal static void WriteHeader(string header, object value, TextWriter writer)
        {
            writer.WriteLine("{0}: {1}", header, value);
        }

        internal static void WriteNewLine(TextWriter writer)
        {
            writer.WriteLine();
        }

        internal static void WriteSplitter(string key, TextWriter writer)
        {
            writer.WriteLine(GetSplitter(key));
        }

        internal static void WriteText(string text, TextWriter writer)
        {
            writer.WriteLine(text);
        }
    }
}