using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentClassification
{
    public class DocumentClassInfo
    {
        public string Name { get; set; }
        public List<Document> DocumentList { get; set; }
        public Dictionary<string, int> WordCount { get; set; }

        private int TotalWordsInClass { get; set; }
        private int TotalUniqueWordsInClass { get; set; }

        public DocumentClassInfo(string name, List<Document> trainDocs)
        {
            Name = name;
            DocumentList = trainDocs;
            var features = GetAllDocumentsConcatenated();
            WordCount = features.GroupBy(d => d).ToDictionary(d => d.Key, d => d.Count());
            TotalWordsInClass = features.Count();
            TotalUniqueWordsInClass = WordCount.Keys.Count();
        }
        
        public IEnumerable<string> GetAllDocumentsConcatenated()
        {
            var trainDocsContents = DocumentList.Select(d => d.DocumentContent);
            var features = trainDocsContents.SelectMany(d => d.ExtractFeatures());
            return features;
        }
        /// <summary>
        /// Calculates prior probability: Nc/N
        /// </summary>
        /// <param name="totalNumberOfDocuemnts"></param>
        /// <returns></returns>
        public double GetPriorProbability(int totalNumberOfDocuemnts)
        {
            return DocumentList.Count / (double)totalNumberOfDocuemnts;
        }
        /// <summary>
        /// Calculates probability: P(word|class)
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public double GetConditinalProbability(string word)
        {
            return (GetWordOccurancesNumber(word) + 1) / (TotalWordsInClass + TotalUniqueWordsInClass);
        }
        public int GetWordOccurancesNumber(string word)
        {
            return WordCount.Keys.Contains(word) ? WordCount[word] : 0;
        }

    }
}
