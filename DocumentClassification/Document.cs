using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentClassification
{
    public class Document: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string documentClass;
        public string DocumentClass
        {
            get { return documentClass; }
            set
            {
                documentClass = value;
                NotifyChange("DocumentClass");
            }
        }
        public string DocumentContent { get; set; }

        public Document(string docClass, string docContent)
        {
            DocumentClass = docClass;
            DocumentContent = docContent;
        }
        protected virtual void NotifyChange(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ClassifiedDocument: Document
    {
        private string documentFileName;
        public string DocumentFileName
        {
            get { return documentFileName; }
            set
            {
                documentFileName = value;
                NotifyChange("DocumentFileName");
            }
        }

        public ClassifiedDocument(string docClass, string docContent, string docName): base(docClass, docContent)
        {
            DocumentFileName = docName;
        }
    }
}
