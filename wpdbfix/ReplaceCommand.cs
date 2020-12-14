using System;
using System.IO;
using System.Text;

namespace WpDbFix
{
    public static class ReplaceCommand
    {
        public static int Execute(string repPath, string inputPath, string outputPath)
        {
            if (repPath == null)
                throw new ArgumentNullException(nameof(repPath));
            if (inputPath == null)
                throw new ArgumentNullException(nameof(inputPath));
            if (outputPath == null)
                throw new ArgumentNullException(nameof(outputPath));

            PhpArrayReplacer replacer = new PhpArrayReplacer();
            using (StreamReader reader = new StreamReader(repPath, Encoding.UTF8))
            {
                replacer.LoadReplacements(reader);
            }

            int count = 0;
            using (StreamReader reader = new StreamReader(inputPath, Encoding.UTF8))
            using (StreamWriter writer = new StreamWriter(outputPath, false,
                Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string newLine = replacer.Replace(line);
                    count += replacer.Count;
                    writer.Write(newLine);
                    // LF only for Linux
                    writer.Write('\n');
                }
                writer.Flush();
            }

            return count;
        }
    }
}
