using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using IESTransformer.lib;
using IESTransformer.lib.Data;

namespace IESTransformer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void btnOpenFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();                      
            openFileDialog.Filter = "ies files (.ies)|*.ies"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = openFileDialog.ShowDialog();
            if(result == true)
            {
                // Process open file dialog box results
                IesFile file_1 = new IesFile();
                file_1.ReadFile(openFileDialog.FileName);
                file_1.Name = openFileDialog.SafeFileName;
                file_1.ExtractData();
                TestData.IesFiles.Add(file_1);
            } 
            
        }

        private void iesFile_1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
