using System;
using System.Configuration;
using System.Windows.Forms;

namespace FolderRestrictor
{
    public partial class ChangePasswordForm : Form
    {
        // Declare UI controls
        private Label lblCurrent;
        private Label lblNew;
        private Label lblConfirm;
        private TextBox txtCurrent;
        private TextBox txtNew;
        private TextBox txtConfirm;
        private Button btnSave;
        private Button btnCancel;

        public ChangePasswordForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string current = ConfigurationManager.AppSettings["AdminPassword"];
            if (txtCurrent.Text != current)
            {
                MessageBox.Show("Current password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtNew.Text != txtConfirm.Text)
            {
                MessageBox.Show("New passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["AdminPassword"].Value = txtNew.Text;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("Password changed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();

        private void InitializeComponent()
        {
            lblCurrent = new Label { Text = "Current Password:", Location = new System.Drawing.Point(12, 15) };
            txtCurrent = new TextBox { Location = new System.Drawing.Point(140, 12), Width = 200, PasswordChar = '●' };

            lblNew = new Label { Text = "New Password:", Location = new System.Drawing.Point(12, 50) };
            txtNew = new TextBox { Location = new System.Drawing.Point(140, 47), Width = 200, PasswordChar = '●' };

            lblConfirm = new Label { Text = "Confirm New Password:", Location = new System.Drawing.Point(12, 85) };
            txtConfirm = new TextBox { Location = new System.Drawing.Point(140, 82), Width = 200, PasswordChar = '●' };

            btnSave = new Button { Text = "Save", Location = new System.Drawing.Point(140, 120) };
            btnCancel = new Button { Text = "Cancel", Location = new System.Drawing.Point(225, 120) };

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            Controls.AddRange(new Control[] {
                lblCurrent, txtCurrent,
                lblNew, txtNew,
                lblConfirm, txtConfirm,
                btnSave, btnCancel
            });

            Text = "Change Admin Password";
            ClientSize = new System.Drawing.Size(360, 170);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
        }
    }
}
