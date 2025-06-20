using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderRestrictor
{
    public partial class MainForm : Form
    {
        private FileSystemWatcher watcher;
        private readonly Regex validNameRegex = new Regex(@"^[A-Za-z]+,\s*[A-Za-z]+ - \d+$", RegexOptions.Compiled);

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select folder to monitor";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtFolderPath.Text = dlg.SelectedPath;
                }
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (watcher == null)
            {
                if (string.IsNullOrWhiteSpace(txtFolderPath.Text) || !Directory.Exists(txtFolderPath.Text))
                {
                    MessageBox.Show("Please choose a valid folder first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show(
                    "Reminder: Any file or folder you create or paste into this folder must be named exactly:\n\n" +
                    "  LAST NAME, FIRST NAME - ACCOUNT NUMBER\n\n" +
                    "Otherwise it will be deleted automatically.",
                    "Naming Convention Reminder",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                StartWatching(txtFolderPath.Text);
                btnStartStop.Text = "Stop Monitoring";
                txtFolderPath.Enabled = false;
                btnBrowse.Enabled = false;
                Log($"Started monitoring: {txtFolderPath.Text}");
            }
            else
            {
                StopWatching();
                btnStartStop.Text = "Start Monitoring";
                txtFolderPath.Enabled = true;
                btnBrowse.Enabled = true;
                Log("Stopped monitoring.");
            }
        }

        private void StartWatching(string path)
        {
            watcher = new FileSystemWatcher
            {
                Path = path,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*",
                IncludeSubdirectories = false,
                EnableRaisingEvents = false
            };

            watcher.Created += OnCreatedOrRenamed;
            watcher.Renamed += OnCreatedOrRenamed;

            watcher.EnableRaisingEvents = true;
        }

        private void StopWatching()
        {
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Created -= OnCreatedOrRenamed;
                watcher.Renamed -= OnCreatedOrRenamed;
                watcher.Dispose();
                watcher = null;
            }
        }

        private void OnCreatedOrRenamed(object sender, FileSystemEventArgs e)
        {
            _ = HandleItemAsync(e.FullPath, e.Name);
        }

        private async Task HandleItemAsync(string fullPath, string name)
        {
            await Task.Delay(10000);

            bool isDirectory = Directory.Exists(fullPath);
            bool isFile = File.Exists(fullPath);

            if (!isDirectory && !isFile) return;

            if (!validNameRegex.IsMatch(name))
            {
                try
                {
                    if (isFile)
                    {
                        File.Delete(fullPath);
                        Log($"Deleted invalid file: {name}");
                    }
                    else if (isDirectory)
                    {
                        Directory.Delete(fullPath, true);
                        Log($"Deleted invalid folder: {name}");
                    }

                    ShowInvalidNameAlert(name);
                }
                catch (Exception ex)
                {
                    Log($"Error deleting '{name}': {ex.Message}");
                }
            }
            else
            {
                Log($"Allowed: {name}");
            }
        }

        private void ShowInvalidNameAlert(string name)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ShowInvalidNameAlert(name)));
                return;
            }

            MessageBox.Show(
                $"‘{name}’ does not match:\n\nLAST NAME, FIRST NAME - ACCOUNT NUMBER\n\nThe item has been removed.",
                "Invalid Name",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        private void Log(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Log(message)));
                return;
            }

            lstLog.Items.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopWatching();
        }

        #region Designer code
        private System.ComponentModel.IContainer components = null;
        private Label lblFolder;
        private TextBox txtFolderPath;
        private Button btnBrowse;
        private Button btnStartStop;
        private ListBox lstLog;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblFolder = new Label();
            this.txtFolderPath = new TextBox();
            this.btnBrowse = new Button();
            this.btnStartStop = new Button();
            this.lstLog = new ListBox();
            this.SuspendLayout();

            this.lblFolder.AutoSize = true;
            this.lblFolder.Location = new System.Drawing.Point(12, 15);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(76, 13);
            this.lblFolder.TabIndex = 0;
            this.lblFolder.Text = "Folder to watch:";

            this.txtFolderPath.Location = new System.Drawing.Point(94, 12);
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.ReadOnly = true;
            this.txtFolderPath.Size = new System.Drawing.Size(366, 20);
            this.txtFolderPath.TabIndex = 1;

            this.btnBrowse.Location = new System.Drawing.Point(466, 10);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse…";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);

            this.btnStartStop.Location = new System.Drawing.Point(547, 10);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(111, 23);
            this.btnStartStop.TabIndex = 3;
            this.btnStartStop.Text = "Start Monitoring";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new EventHandler(this.btnStartStop_Click);

            this.lstLog.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom)
                                  | AnchorStyles.Left)
                                  | AnchorStyles.Right;
            this.lstLog.FormattingEnabled = true;
            this.lstLog.HorizontalScrollbar = true;
            this.lstLog.Location = new System.Drawing.Point(15, 50);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(643, 238);
            this.lstLog.TabIndex = 4;

            this.ClientSize = new System.Drawing.Size(670, 310);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFolderPath);
            this.Controls.Add(this.lblFolder);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Folder Restrictor - Admin Portal";
            this.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
    }
}
