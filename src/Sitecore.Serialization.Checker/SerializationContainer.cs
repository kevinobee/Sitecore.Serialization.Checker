using Sitecore.Serialization.Core;
using Sitecore.Serialization.Infrastructure;

namespace Sitecore.Serialization.Checker
{
    internal class SerializationContainer
    {
        public SerializedItemChecker ResolveSerializationParser()
        {
            IItemValidator validator = new ItemValidator();
            IItemFileWriter fileWriter =  new SyncItemFileWriter();

            IOutputWriter outputWriter = new ConsoleOutputWriter();

            return new SerializedItemChecker(validator, fileWriter, outputWriter);
        }
    }
}