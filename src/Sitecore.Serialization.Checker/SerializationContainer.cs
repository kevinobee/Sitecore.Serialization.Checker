using Sitecore.Serialization.Infrastructure;

namespace Sitecore.Serialization.Checker
{
    internal class SerializationContainer
    {
        public SerializedItemChecker ResolveSerializationParser()
        {
            var validator = new ItemValidator();
            return new SerializedItemChecker(validator);
        }
    }
}