using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Serialization.Infrastructure;


namespace Sitecore.Serialization.Checker
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new SerializationContainer();
            container.ResolveSerializationParser().Execute(args);
        }
    }

    internal class SerializationContainer
    {
        public SerializedItemChecker ResolveSerializationParser()
        {
            var validator = new ItemValidator();
            return new SerializedItemChecker(validator);
        }
    }
}
