using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DocumentClassification
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ClassifiedDocument> ClassifiedDocuments { get; set; }
        public Classifier Classifier { get; set; }
        public string RootPath { get; set; }

        public MainWindow()
        {
            ClassifiedDocuments = new ObservableCollection<ClassifiedDocument>();
            InitializeComponent();
        }


        private void LoadRootButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                RootPath = Directory.Exists(dialog.FileName) ? dialog.FileName : Path.GetDirectoryName(dialog.FileName);
        }
        private void TrainModelButton_Click(object sender, RoutedEventArgs e)
        {
            List<Document> trainingSet = DatasetLoader.LoadDatasets(RootPath);
            Classifier = new Classifier(trainingSet);
            Classifier.TrainClassifier();
        }
        private void ClassifyDocButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter += "All files (*.*) | *.*";
            while (true)
            {
                bool? dr = ofd.ShowDialog();
                if (dr.HasValue && dr.Value)
                {
                    var filePath = ofd.FileName;
                    string text = File.ReadAllText(filePath);
                    var docClass = Classifier.ClassifyDocument(text);
                    ClassifiedDocuments.Add(new ClassifiedDocument(docClass, text, Path.GetFileName(filePath)));
                    return;
                }
                else
                {
                    return;
                }
            }
        }

    }
}
