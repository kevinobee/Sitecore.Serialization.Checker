using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sitecore.Data.Serialization.ObjectModel;
using Sitecore.Serialization.Core;

namespace Sitecore.Serialization.Infrastructure
{
    public class SyncItemFileWriter : IItemFileWriter
    {
        private const string VersionLine    = "----version----";
        private const string ItemLine       = "----item----";
        private const string FieldLine      = "----field----";

        public void Fix(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new InvalidOperationException(string.Format("{0} does not exist", filePath));
            }

            using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                using (TextReader reader = new StreamReader(fileStream))
                {
                    var syncItem = ReadItem(new Tokenizer(reader));
                    WriteResultsToFile(fileStream, syncItem);
                }
            }
        }

        private static void WriteResultsToFile(Stream stream, SyncItem syncItem)
        {
            TruncateFile(stream);

            using (TextWriter writer = new StreamWriter(stream))
            {
                syncItem.Serialize(writer);
            }
        }

        private static void TruncateFile(Stream stream)
        {
            stream.SetLength(0);
        }

        private SyncItem ReadItem(Tokenizer reader)
        {
            while ((reader.Line != null) && (reader.Line.Length == 0))
            {
                reader.NextLine();
            }
            if ((reader.Line == null) || (reader.Line != ItemLine))
            {
                throw new Exception("Format error: serialized stream does not start with ----item----");
            }
            var item = new SyncItem();
            var dictionary = SerializationUtils.ReadHeaders(reader);

            item.ID = dictionary["id"];
            item.ItemPath = dictionary["path"];

            try
            {
                item.DatabaseName = dictionary["database"];
                item.ParentID = dictionary["parent"];
                item.Name = dictionary["name"];
                item.BranchId = dictionary["master"];
                item.TemplateID = dictionary["template"];
                item.TemplateName = dictionary["templatekey"];
                reader.NextLine();
            
                while (reader.Line == FieldLine)
                {
                    var field = ReadField(reader);
                    if (field != null)
                    {
                        item.SharedFields.Add(field);
                    }
                }

                while (reader.Line == VersionLine)
                {
                    var version = ReadVersion(reader);
                    if (version != null)
                    {
                        item.Versions.Add(version);
                    }
                }
                return item;
            }
            catch (Exception exception)
            {
                throw new Exception("Error reading item: " + item.ItemPath, exception);
            }
        }

        private static SyncField ReadField(Tokenizer reader)
        {
            var field = new SyncField();
            var dictionary = SerializationUtils.ReadHeaders(reader);
            field.FieldID = dictionary["field"];
            field.FieldName = dictionary["name"];
            field.FieldKey = dictionary["key"];

            var readLines = new List<string>();

            while (true)
            {
                var line = reader.NextLine();

                if (EofFound(line) || FieldStartFound(line)) break;

                readLines.Add(line);
            }

            if (((reader.Line == null) || reader.Line.EndsWith("----", StringComparison.InvariantCulture)) || (reader.Line.Length == 0))
            {
                if (readLines.All(x => x == ""))
                {
                    field.FieldValue = "";
                }
                else
                {
                    var builder = new StringBuilder();

                    for (var index = 0; index < readLines.Count; index++)
                    {
                        var readLine = readLines[index];
                        if (index == readLines.Count - 1)
                        {
                            builder.Append(readLine); 
                        }
                        else
                        {
                            builder.AppendLine(readLine);                             
                        }
                    }

                    field.FieldValue = builder.ToString();
                }

                return field;
            }

            throw new Exception(
                string.Format(
                        "Length of field content does not match the content-length attribute. Field name: {0}, field id: {1}", 
                        field.FieldName, field.FieldID));
        }

        private static bool FieldStartFound(string line)
        {
            return line.StartsWith("----");
        }

        private static bool EofFound(string line)
        {
            return line == null;
        }

        private static SyncVersion ReadVersion(Tokenizer reader)
        {
            var version = new SyncVersion();
            try
            {
                var dictionary = SerializationUtils.ReadHeaders(reader);
                version.Language = dictionary["language"];
                version.Version = dictionary["version"];
                version.Revision = dictionary["revision"];
                reader.NextLine();

                while (reader.Line == FieldLine)
                {
                    SyncField item = ReadField(reader);
                    if (item != null)
                    {
                        version.Fields.Add(item);
                    }
                }
                return version;
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format("Failed to load version {0} for language {1}", version.Version, version.Language), exception);
            }
        }
    }
}
