using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace PdfExplorer
{
    public partial class MainWindow : Window
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
    }
}
