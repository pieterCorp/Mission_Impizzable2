using System.Collections.Generic;
using System.IO;

namespace Mission_Impizzable2
{
    internal class Filemanager
    {
        public void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void CreateFile(string file)
        {
            if (!File.Exists(file))
            {
                FileStream fileStream = File.Create(file);
                fileStream.Close();
            }
        }

        public void DeleteFile(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        public void WriteDataToFile(string textToWriteToFile, string path)
        {
            using StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(textToWriteToFile);
        }

        public string[] LoadAllFiles(string path)
        {
            using StreamReader reader = new StreamReader(path);
            string line = string.Empty;
            List<string> lines = new List<string>();

            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }
            string[] loadedData = lines.ToArray();
            return loadedData;
        }

        public int CountLinesFile(string path)
        {
            CreateFile(path);            
            using StreamReader reader = new StreamReader(path);
            
            int counter = 0;

            while (reader.ReadLine() != null)
            {
                counter++;
            }
            
            return counter;
        }
    }
}