using System.IO;

using Should;
using Xunit;

namespace Sitecore.Serialization.Infrastructure.Intg.Test.SerializedItemFileTests.ItemFileFixBehaviour
{
    public class SyncItemFileWriterBehaviour : ItemFileFixBehaviour
    {
        public SyncItemFileWriterBehaviour()
        {
            FileWriter = new SyncItemFileWriter();
        }

        public class ValidItemFileTest : SyncItemFileWriterBehaviour
        {
            [Fact]
            public void Is_still_valid_after_fix_operation()
            {
                FilePath = CreateTempFile(TestDataPath + "content.item");

                FileWriter.Fix(FilePath);

                var isValid = Validator.IsValid(FilePath);

                isValid.ShouldBeTrue();
            }

            [Fact]
            public void Is_still_same_length_as_original()
            {
                const string originalFilePath = TestDataPath + "content.item";

                FilePath = CreateTempFile(originalFilePath);

                FileWriter.Fix(FilePath);

                var originalFile = new FileInfo(originalFilePath);
                var fixedFile = new FileInfo(FilePath);

                fixedFile.Length.ShouldEqual(originalFile.Length);
            }
        }

        public class CorruptedItemFileTest : SyncItemFileWriterBehaviour
        {
            [Fact]
            public void Is_valid_after_fix_operation()
            {
                FilePath = CreateTempFile(TestDataPath + "Home-Corrupt.item");

                FileWriter.Fix(FilePath);

                var isValid = Validator.IsValid(FilePath);

                isValid.ShouldBeTrue();
            }

            [Fact]
            public void Fixed_file_should_be_same_length_as_original()
            {
                const string originalFilePath = TestDataPath + "Home-Corrupt.item";

                FilePath = CreateTempFile(originalFilePath);

                FileWriter.Fix(FilePath);

                var originalFile = new FileInfo(originalFilePath);
                var fixedFile = new FileInfo(FilePath);

                fixedFile.Length.ShouldEqual(originalFile.Length);
            }
        }
    }
}