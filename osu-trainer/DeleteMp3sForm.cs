using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace osu_trainer
{
    public partial class DeleteMp3sForm : Form
    {
        public DeleteMp3sForm(List<string> filesToDelete)
        {
            InitializeComponent();
            List<string> uniqueFilesToDelete = filesToDelete.Distinct().ToList();

            // filter out files that don't exist
            Dictionary<string, string> pathMapping = new Dictionary<string, string>();
            uniqueFilesToDelete.ForEach(file => pathMapping.Add(file, JunUtils.FullPathFromSongsFolder(file)));
            List<string> relativeFileList = uniqueFilesToDelete.Where(file => File.Exists(JunUtils.FullPathFromSongsFolder(file))).ToList();

            // populate listview
            string formatFileSize(long len)
            {
                string[] sizes = { "B", "KB", "MB", "GB", "TB" };
                int order = 0;
                decimal value = (decimal)len;
                while (value >= 1024 && order < sizes.Length - 1)
                {
                    order++;
                    value = value / 1024;
                }
                return String.Format("{0:0.##} {1}", value, sizes[order]);
            }
            ListViewItem fileToListViewItem(string file)
            {
                string[] subitems = new string[3];
                FileInfo fi = new FileInfo(JunUtils.FullPathFromSongsFolder(file));

                subitems[0] = file; // path relative to songs folder
                subitems[1] = fi.CreationTime.ToString("d"); // date created
                subitems[2] = formatFileSize(fi.Length); // size
                return new ListViewItem(subitems);
            }
            fileListView.Items.Clear();
            relativeFileList
                .Select(file => fileToListViewItem(file))
                .ToList()
                .ForEach(item => fileListView.Items.Add(item));

            // update total filesize label
            long totalSize = relativeFileList
                .Select(file => new FileInfo(JunUtils.FullPathFromSongsFolder(file)).Length)
                .Sum();
            fileSizeLabel.Text = $"Total: {formatFileSize(totalSize)} to be deleted";

            confirmButton.Focus();

        }
    }
}