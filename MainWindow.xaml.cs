using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace SanadBan
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            // Load the current user's "Documents" folder by default.
            LoadDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        }

        private void LoadDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath)) return;

            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            TreeViewItem rootItem = CreateDirectoryNode(dirInfo);
            FolderTree.Items.Clear(); // Clear existing items before loading new ones
            FolderTree.Items.Add(rootItem);
        }

        private TreeViewItem CreateDirectoryNode(DirectoryInfo dirInfo)
        {
            TreeViewItem dirNode = new TreeViewItem
            {
                Header = dirInfo.Name,
                Tag = dirInfo.FullName
            };

            // پوشه‌ها (Folders)
            try
            {
                foreach (var subDir in dirInfo.GetDirectories())
                {
                    dirNode.Items.Add(CreateDirectoryNode(subDir));
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Ignore folders we can't access
            }


            // فایل‌ها (Files)
            foreach (var file in dirInfo.GetFiles("*.pdf"))
            {
                TreeViewItem fileNode = new TreeViewItem
                {
                    Header = file.Name,
                    Tag = file.FullName
                };
                dirNode.Items.Add(fileNode);
            }

            return dirNode;
        }

        private void FolderTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (FolderTree.SelectedItem is TreeViewItem selectedNode)
            {
                string path = selectedNode.Tag.ToString();
                if (File.Exists(path) && path.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    // باز کردن PDF در WebBrowser
                    PdfViewer.Navigate(new Uri(path));
                }
            }
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    LoadDirectory(dialog.SelectedPath);
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }
    }
}