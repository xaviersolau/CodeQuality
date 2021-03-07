using System.IO;

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

            if (Overwrite)
            {
                File.WriteAllText(OutputFilename, txt);
            }
            else
            {
                File.AppendAllText(OutputFilename, txt);
            }
        }
    }
}
