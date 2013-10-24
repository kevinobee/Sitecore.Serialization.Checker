using System;
using System.IO;
using CommandLine;
using Sitecore.Serialization.Core;

namespace Sitecore.Serialization.Checker
{
    internal class SerializedItemChecker
    {
        private readonly IItemValidator _itemValidator;
        private int _filesChecked;
        private int _filesWithErrors;

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

                ResetStatistics();

                ProcessDirectory(fullPath);
            }

            DisplayStatistics();
        }

        private void DisplayStatistics()
        {
            Console.WriteLine();
            Console.WriteLine("Checked {0} file{1}, {2} validation error{3} found.", _filesChecked, Pluralise(_filesChecked), _filesWithErrors, Pluralise(_filesWithErrors));
            Console.WriteLine();
        }

        private string Pluralise(int count)
        {
            return count > 1 ? "s" : "";
        }

        private void ResetStatistics()
        {
            _filesChecked = 0;
            _filesWithErrors = 0;
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

            if (!isValid)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid - {0}", filePath);   
                Console.ResetColor();

                _filesWithErrors++;
            }

            _filesChecked++;
        }
    }
}