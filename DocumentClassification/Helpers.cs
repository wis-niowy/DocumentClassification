using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DocumentClassification
{
    public static class Helpers
    {
        public static List<String> ExtractFeatures(this String text)
        {
            return Regex.Replace(text, @"[^\w+]", " ").Split(' ').ToList();
        }
    }

    class DatasetLoader
    {
        public static List<Document> LoadDatasets(string @rootPath)
        {
            List<string> datasetsPaths = Directory.EnumerateDirectories(rootPath).ToList();

            List<Document> trainingSet = new List<Document>();
            foreach (var datasetPath in datasetsPaths)
            {
                var classLabel = Path.GetFileName(datasetPath);
                foreach (string file in Directory.EnumerateFiles(datasetPath))
                {
                    string contents = File.ReadAllText(file);
                    trainingSet.Add(new Document(classLabel, contents));
                }
            }
            return trainingSet;
        }
    }
}
