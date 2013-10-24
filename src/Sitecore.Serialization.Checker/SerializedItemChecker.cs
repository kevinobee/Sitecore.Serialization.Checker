using System;
using System.IO;
using CommandLine;
using Sitecore.Serialization.Core;

namespace Sitecore.Serialization.Checker
{
    internal class SerializedItemChecker
    {
        private readonly IItemValidator _itemValidator;

        public SerializedItemChecker(IItemValidator itemValidator)
        {
            _itemValidator = itemValidator;
        }

        public void Execute(string[] args)
        {
            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                var fullPath = Path.GetFullPath(options.Path);

                Console.WriteLine("Processing : {0}", fullPath);
                ProcessDirectory(fullPath);
            }

            Console.Write("Press a key to continue..");
            Console.ReadKey();
        }

        private void ProcessDirectory(string path)
        {
            foreach (var file in Directory.GetFiles(path, "*.item"))
            {
                ProcessFile(file);
            }

            foreach (var directory in Directory.GetDirectories(path))
            {
                ProcessDirectory(directory);
            }
        }

        private void ProcessFile(string filePath)
        {
            var isValid = _itemValidator.IsValid(filePath);
            Console.WriteLine(" - {0} - {1}", filePath, isValid);
        }
    }
}