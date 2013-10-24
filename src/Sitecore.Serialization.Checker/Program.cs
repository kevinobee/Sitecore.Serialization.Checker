namespace Sitecore.Serialization.Checker
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new SerializationContainer();
            container.ResolveSerializationParser()
                     .Execute(args);
        }
    }
}
