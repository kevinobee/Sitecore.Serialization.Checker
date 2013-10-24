using Sitecore.Serialization.Infrastructure;

namespace Sitecore.Serialization.Checker
{
    internal class SerializationContainer
    {
        public SerializedItemChecker ResolveSerializationParser()
        {
            var validator = new ItemValidator();
            var fileWriter = new ItemFileWriter();
            return new SerializedItemChecker(validator, fileWriter);
        }
    }
}