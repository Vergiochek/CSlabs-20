using System;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TochnoLab1
{
    public partial class Form1 : Form
    { 
        private string filePath = null;
        private string currentFileName = "";
        private string copyPath = "";
        private string oldFileName = "";
        private bool isFile = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDrives();
            FileNameText.Hide();
        }

        private void ButtonAction()
        {
            filePath = FilePathTextBox.Text;
            LoadFilesAndDirectories();
            isFile = false;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (filePath.Length == 3)
                {
                    LoadDrives();
                    FilePathTextBox.Text = filePath = "";
                }
                else
                {
                    filePath = filePath.Substring(0, filePath.LastIndexOf("/"));
                    FilePathTextBox.Text = filePath;
                    currentFileName = "";
                    LoadFilesAndDirectories();
                }
            }
            catch (NullReferenceException range)
            {
                return;
            }
            catch (Exception exp)
            {
                return;
            }
        }

        private void LoadDrives()
        {
            listView1.Items.Clear();

            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (var drive in drives)
                if (drive.IsReady)
                    listView1.Items.Add(drive.Name, 7);
        }

        private void LoadFilesAndDirectories()
        {
            DirectoryInfo fileList;
            string tempFilePath = "";
            try
            {
                if (isFile)
                {

                    tempFilePath = filePath + "/" + currentFileName;
                    FileInfo info = new FileInfo(tempFilePath);
                    FileNameLabel.Text = info.Name;
                    FileTypeLabel.Text = info.Extension;
                    Process.Start(tempFilePath);
                }
                else
                {
                    string fileExtension = "";
                    fileList = new DirectoryInfo(filePath);
                    FileInfo[] files = fileList.GetFiles();
                    DirectoryInfo[] directories = fileList.GetDirectories();

                    listView1.Items.Clear();

                    foreach (var file in files)
                    {
                        fileExtension = file.Extension.ToUpper();
                        switch (fileExtension)
                        {
                            case ".DOCX":
                                listView1.Items.Add(file.Name, 0);
                                break;
                            case ".PDF":
                                listView1.Items.Add(file.Name, 1);
                                break;
                            case ".JPG":
                                listView1.Items.Add(file.Name, 2);
                                break;
                            case ".SLN":
                                listView1.Items.Add(file.Name, 3);
                                break;
                            case ".ASM":
                                listView1.Items.Add(file.Name, 4);
                                break;
                            case ".EXE":
                                listView1.Items.Add(file.Name, 5);
                                break;
                            case ".MP4":
                                listView1.Items.Add(file.Name, 6);
                                break;
                            case ".RAR":
                                listView1.Items.Add(file.Name, 10);
                                break;
                            default:
                                listView1.Items.Add(file.Name, 9);
                                break;
                        }
                    }

                    foreach (var directory in directories)
                        listView1.Items.Add(directory.Name, 8);
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ButtonAction();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            currentFileName = e.Item.Text;
            FileAttributes fileAttr;

            try
            {
                if (string.IsNullOrEmpty(filePath) || string.Compare(filePath, currentFileName) == 0)
                    FilePathTextBox.Text = currentFileName;
                else
                {
                    fileAttr = File.GetAttributes(filePath + "/" + currentFileName);
                    if (fileAttr == FileAttributes.Directory)
                    {
                        isFile = false;
                        FilePathTextBox.Text = filePath + "/" + currentFileName;
                    }
                    else
                        isFile = true;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            ButtonAction();
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileName = "";

            try
            {
                fileName = FileNameText.Text;
                FileInfo file = new FileInfo(filePath + "/" + fileName + ".txt");
 
                if (!file.Exists)
                {
                    file.Create().Close();
                }

                FileNameText.Clear();
                FileNameText.Hide();
                LoadFilesAndDirectories();

            }
            catch (Exception)
            {
                return;
            }
        }

        private void папкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileName = "";

            try
            {
                fileName = FileNameText.Text;
           

                DirectoryInfo directory = new DirectoryInfo(filePath + "/" + fileName);

                if (!directory.Exists)
                {
                    directory.Create();
                }

                FileNameText.Clear();
                FileNameText.Hide();
                LoadFilesAndDirectories();

            }
            catch(Exception)
            {
                return;
            }
        }

        private void FileNameText_TextChanged(object sender, EventArgs e)
        {

        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileNameText.Show();
        }

        private void DeleteDirectory(DirectoryInfo directory)
        {
            if (directory.Exists)
            {
                foreach (var file in directory.GetFiles())
                    file.Delete();
                foreach (var drPapki in directory.GetDirectories())
                    DeleteDirectory(drPapki);

                directory.Delete();
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var path = filePath + "/" + currentFileName;
            try
            {
                if (!isFile)
                    DeleteDirectory(new DirectoryInfo(path));
                else
                    File.Delete(path);

                isFile = false;
                LoadFilesAndDirectories();
            }
            catch(Exception)
            {
                return;
            }
        }

        private void переименоватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newFileName = "";
            FileNameText.Show();
            newFileName = FileNameText.Text;
            while (true)
            {
                try
                {
                    if (!isFile)
                        Directory.Move(filePath + currentFileName, filePath + "/" + newFileName);
                    else
                        File.Move(filePath + currentFileName, filePath + "/" + newFileName);
                    break;
                }
                catch (Exception)
                {
                    break;
                }
            }
            isFile = false;
            LoadFilesAndDirectories();
            FileNameText.Clear();
            FileNameText.Hide();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(filePath + "/" + currentFileName);
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyPath = filePath + "/" + currentFileName;

            oldFileName = currentFileName;
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var oldName = filePath + "/" + oldFileName;

            FileAttributes fileAttr = File.GetAttributes(copyPath);
            if (fileAttr == FileAttributes.Directory)
                isFile = false;
            else
                isFile = true;

            int count;
            
            if (!isFile)
            {
                for (count = 1; Directory.Exists(oldName) && Directory.Exists(oldName + "(" + count + ")"); count++) ;
                Directory.CreateDirectory(oldName + "(" + count + ")");
            }
            else
            {
                for (count = 1; File.Exists(oldName + ".txt") && File.Exists(oldName + "(" + count + ").txt"); count++) ;

                File.Copy(copyPath, oldName + "(" + count + ").txt");
                isFile = false;
            }

            LoadFilesAndDirectories();
        }

        private void создатьАрхивToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if(!isFile)
                {
                    ZipFile.CreateFromDirectory(filePath + currentFileName, filePath + currentFileName + ".rar");
                    LoadFilesAndDirectories();
                }
            }
            catch(Exception)
            {
                return;
            }
        }

        private void разархивироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ZipFile.ExtractToDirectory(filePath + currentFileName, filePath + currentFileName.Substring(0,currentFileName.LastIndexOf(".")));
                isFile = false;
                LoadFilesAndDirectories();
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
