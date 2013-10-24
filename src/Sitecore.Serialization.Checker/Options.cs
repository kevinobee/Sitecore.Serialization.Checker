using CommandLine;
using CommandLine.Text;

namespace Sitecore.Serialization.Checker
{
    internal class Options
    {
        [Option('p', "path", Required = false, DefaultValue = ".", HelpText = "Directory path to be processed")]
        public string Path { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                                      (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}