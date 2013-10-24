using System;
using System.IO;
using CommandLine;
using Sitecore.Serialization.Core;

namespace Sitecore.Serialization.Checker
{
    public class SerializedItemChecker
    {
        private readonly IItemValidator _itemValidator;
        private readonly IItemFileWriter _itemFileWriter;
        private readonly IOutputWriter _outputWriter;

        private int _filesChecked;
        private int _filesWithErrors;
        private bool _fixFilesRequested;
        private int _filesFixed;

        public SerializedItemChecker(IItemValidator itemValidator, IItemFileWriter itemFileWriter, IOutputWriter outputWriter)
        {
            _itemValidator = itemValidator;
            _itemFileWriter = itemFileWriter;
            _outputWriter = outputWriter;
        }

        public void Execute(string[] args)
        {
            try
            {
                var options = new Options();
                if (Parser.Default.ParseArguments(args, options))
                {
                    var fullPath = Path.GetFullPath(options.Path);
                    _fixFilesRequested = options.FixFiles;

                    _outputWriter.Info(string.Format("Processing : {0}", fullPath));

                    ResetStatistics();

                    ProcessDirectory(fullPath);
                }

                DisplayStatistics();
            }
            catch (InvalidOperationException exception)
            {
                _outputWriter.Fail(string.Format("Fail: {0}", exception.Message));
            }
        }

        private void DisplayStatistics()
        {
            _outputWriter.WriteLine();
            _outputWriter.WriteFormatLine(MessageType.Info, "Checked {0} file{1}, {2} validation error{3} found. {4} fixed", 
                              _filesChecked, Pluralise(_filesChecked), 
                              _filesWithErrors, Pluralise(_filesWithErrors), 
                              GetFilesFixed());

            _outputWriter.WriteLine();
        }

        private string GetFilesFixed()
        {
            if (_filesFixed == _filesWithErrors)
            {
                return "All files";
            }

            if (_filesFixed == 1)
            {
                return "1 file";
            }

            return string.Format("{0} files", _filesFixed);
        }

        private string Pluralise(int count)
        {
            return count > 1 ? "s" : "";
        }

        private void ResetStatistics()
        {
            _filesChecked = 0;
            _filesWithErrors = 0;
            _filesFixed = 0;
        }   

        private void ProcessDirectory(string path)
        {
            if (! Directory.Exists(path))
            {
                throw new InvalidOperationException(string.Format("{0} does not exist", path));
            }

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
                _outputWriter.WriteFormatLine(MessageType.Warn, "Invalid - {0}", filePath);

                if (_fixFilesRequested)
                {
                    _itemFileWriter.Fix(filePath);

                    isValid = _itemValidator.IsValid(filePath);
                    
                    if (isValid)
                    {
                        _outputWriter.Success("Fixed");
                        _filesFixed++;
                    }
                    else
                    {
                        _outputWriter.Fail("Fix Failed");                    
                    }
                }

                _filesWithErrors++;
            }

            _filesChecked++;
        }
    }
}