using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using COMInterfaceWrapper;
using System.IO;

namespace SerializableChecker
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FolderSelectButton_Click(object sender, RoutedEventArgs e)
        {
            FolderSelectDialog folderSelectDialog = new FolderSelectDialog();

            if (folderSelectDialog.ShowDialog())
            {
                DirectoryTextBox.Text = folderSelectDialog.Path;
            }
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string directory = DirectoryTextBox.Text;

            FileListBox.Items.Clear();

            //存在しない場合終了
            if (!Directory.Exists(directory))
            {
                FileListBox.Items.Add("フォルダが存在しません。");
                return;
            }

            string[] files = Directory.EnumerateFiles(directory, "*" + extCombo.Text, SearchOption.TopDirectoryOnly).ToArray();

            foreach (var fileName in files)
            {
                using(var reader = new StreamReader(fileName))
                {
                    string fileDetail = await reader.ReadToEndAsync();
                    if (!fileDetail.Contains("[Serializable]"))
                    {
                        FileListBox.Items.Add(System.IO.Path.GetFileName(fileName));
                    }
                }
            }
        }
    }
}
