using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexColorHelperCli.Verbs
{
    [Verb("hover", HelpText = "Generates hover colors for given array of colors")]
    public class HoverColors
    {
        [Option('c', "colors", Required = true, HelpText = "Comma separated list of hashes")]
        public string ColorHashes { get; set; }

        [Option('d', "darker", Required = false, HelpText = "Make hover colors darker")]
        public bool Darker { get; set; }

        [Option('f', "fileName", Required = false, HelpText = "Name of the png file to generate. If not givem, png file will not be generated.")]
        public string? FileName { get; set; }

        [Option('o', "openFile", Required = false, HelpText = "If true (which is default) generated file will be opened at the end", Default = true)]
        public bool OpenFile { get; set; }

        [Option('s', "strength", Required = false, HelpText = "Strength of making the hover color (darker or lighter depends on -d option). Takes values between 0 and 1.", Default = 0.5)]
        public double Strength { get; set; }
    }
}
