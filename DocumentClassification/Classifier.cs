﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentClassification
{
    public class Classifier
    {
        private int numberOfDocs;
        private List<string> vocabulary;
        private List<DocumentClassInfo> docClasses;
        private Dictionary<string, double> PriorProbabilities;
        private Dictionary<string,Dictionary<string,double>> ConditionalProbabilities;

        public Classifier(List<Document> trainDocs)
        {
            docClasses = trainDocs.GroupBy(d => d.DocumentClass).Select(g => new DocumentClassInfo(g.Key, g.Select(d => d).ToList())).ToList();
            vocabulary = trainDocs.Select(d => d.DocumentContent).SelectMany(c => c.ExtractFeatures()).GroupBy(w => w).Select(w => w.Key).ToList();
            numberOfDocs = trainDocs.Count;
            PriorProbabilities = new Dictionary<string, double>();
            ConditionalProbabilities = new Dictionary<string, Dictionary<string, double>>();
        }

        public void TrainClassifier()
        {
            foreach (var docClass in docClasses)
            {
                PriorProbabilities[docClass.Name] = docClass.GetPriorProbability(numberOfDocs);
                foreach (var word in vocabulary)
                {
                    if (!ConditionalProbabilities.Keys.Contains(word))
                        ConditionalProbabilities[word] = new Dictionary<string, double>();
                    ConditionalProbabilities[word][docClass.Name] = docClass.GetConditinalProbability(word);
                }
            }
        }
        
        /// <summary>
        /// Finds class which given document is most probable of belonging to
        /// </summary>
        /// <param name="newDocumentContent"></param>
        /// <returns>Class with highest probability</returns>
        public string ClassifyDocument(string newDocumentContent)
        {
            Dictionary<string, double> classProbability = new Dictionary<string, double>();
            var words = newDocumentContent.ExtractFeatures();
            foreach (var docClass in docClasses)
            {
                classProbability.Add(docClass.Name, CalculateClassBelongingness(docClass, words));
            }
            return classProbability.OrderByDescending(c => c.Value).FirstOrDefault().Key;
        }

        private double CalculateClassBelongingness(DocumentClassInfo docClass, List<string> docWords)
        {
            var result = Math.Log10(PriorProbabilities[docClass.Name]);
            foreach (var word in docWords)
            {
                result += Math.Log10(ConditionalProbabilities[word][docClass.Name]);
            }
            return result;
        }
    }
}
