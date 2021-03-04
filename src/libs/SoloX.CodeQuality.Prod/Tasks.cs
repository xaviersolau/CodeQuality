using System.IO;

namespace SoloX.CodeQuality.Prod
{
    public class Tasks
    {
        public string InputFilename { get; set; }
        public string OutputFilename { get; set; }
        public bool Overwrite { get; set; }
        public void CopyTextFile()
        {
            if (Overwrite)
            {
                File.WriteAllText(OutputFilename, File.ReadAllText(InputFilename));
            }
            else
            {
                File.AppendAllText(OutputFilename, File.ReadAllText(InputFilename));
            }
        }
    }
}
