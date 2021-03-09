using System;
using System.IO;
using System.Linq;

namespace SoloX.CodeQuality.Prod
{
    public class Tasks
    {
        public string InputFilename { get; set; }
        public string OutputFilename { get; set; }
        public bool Overwrite { get; set; }
        public string Replace { get; set; }
        public void CopyTextFile()
        {
            var txt = File.ReadAllText(InputFilename);

            if (!string.IsNullOrEmpty(Replace))
            {
                foreach (var replaceItem in Replace.Split(';'))
                {
                    var idx = replaceItem.IndexOf('=');
                    var key = $"{{{replaceItem.Substring(0, idx).Trim()}}}";
                    var value = replaceItem.Substring(idx + 1).Trim();
                    txt = txt.Replace(key, value);
                }
            }

            var outputDirectory = Path.GetDirectoryName(OutputFilename);
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            if (Overwrite)
            {
                File.WriteAllText(OutputFilename, txt);
            }
            else
            {
                File.AppendAllText(OutputFilename, txt);
            }
        }

        public void ReplaceTextFile()
        {
            var inputTxt = File.ReadAllText(InputFilename);
            var outputTxt = File.Exists(OutputFilename) ? File.ReadAllText(OutputFilename) : string.Empty;

            if (!inputTxt.Equals(outputTxt, StringComparison.Ordinal))
            {
                File.WriteAllText(OutputFilename, inputTxt);
            }
        }

        public string Lines { get; set; }

        public void AddTextLines()
        {
            var lines = Lines.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var outputLines = File.Exists(OutputFilename) ? File.ReadAllLines(OutputFilename) : Array.Empty<string>();

            var linesToAdd = lines.Where(line => !outputLines.Contains(line)).ToArray();
            if (linesToAdd.Any())
            {
                File.AppendAllLines(OutputFilename, linesToAdd);
            }
        }
    }
}
