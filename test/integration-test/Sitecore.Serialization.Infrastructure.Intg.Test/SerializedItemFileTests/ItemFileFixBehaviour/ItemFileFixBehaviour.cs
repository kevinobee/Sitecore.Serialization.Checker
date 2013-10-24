using System;
using System.IO;
using Sitecore.Serialization.Core;

namespace Sitecore.Serialization.Infrastructure.Intg.Test.SerializedItemFileTests.ItemFileFixBehaviour
{
    public class ItemFileFixBehaviour : SerializedItemFileTest, IDisposable
    {
        protected IItemFileWriter FileWriter;
        protected readonly ItemValidator Validator;
        protected string FilePath;

        public ItemFileFixBehaviour()
        {
            Validator = new ItemValidator();
        }

        protected static string CreateTempFile(string sourceFilePath)
        {
            var tempItemFile = Path.GetTempPath() + Guid.NewGuid() + ".item";
            File.Copy(sourceFilePath, tempItemFile);
            return tempItemFile;
        }

        public void Dispose()
        {
            if ((string.IsNullOrEmpty(FilePath)) || (!File.Exists(FilePath))) return;

            try
            {
                File.Delete(FilePath); 
            }
            catch (IOException exception)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Failed to delete {0} - {1}", FilePath, exception.Message));
            }
        }

        // TODO remove
//        public class ItemFileWriterBehaviour : ItemFileFixBehaviour
//        {
//            public ItemFileWriterBehaviour()
//            {
//                FileWriter = new ItemFileWriter(new SerializedItemParser());
//            }
//
//            [Fact]
//            public void Item_file_is_valid_after_fix_operation()
//            {
//                FilePath = CreateTempFile(TestDataPath + "Home-Corrupt.item");
//
//                FileWriter.Fix(FilePath);
//
//                var isValid = Validator.IsValid(FilePath);
//
//                isValid.ShouldBeTrue();
//            }
//        }


    }
}
