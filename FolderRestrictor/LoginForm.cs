using System;
using System.Windows.Forms;
using System.Configuration;

namespace FolderRestrictor
{
    public partial class LoginForm : Form
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblPrompt;
        private TextBox txtPassword;
        private Button btnOK;
        private Button btnCancel;
        private Button btnChangePassword;

        public LoginForm()
        {
            InitializeComponent();
            btnChangePassword.Enabled = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string storedPassword = ConfigurationManager.AppSettings["AdminPassword"];

            if (txtPassword.Text == storedPassword)
            {
                this.Hide();
                var main = new MainForm();
                main.FormClosed += (s, args) => this.Close();
                main.Show();
            }
            else
            {
                MessageBox.Show("Incorrect password.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            var changeForm = new ChangePasswordForm();
            changeForm.ShowDialog();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            string storedPassword = ConfigurationManager.AppSettings["AdminPassword"];
            btnChangePassword.Enabled = (txtPassword.Text == storedPassword);
        }

        private void InitializeComponent()
        {
            lblPrompt = new Label();
            txtPassword = new TextBox();
            btnOK = new Button();
            btnCancel = new Button();
            btnChangePassword = new Button();
            SuspendLayout();
            lblPrompt.AutoSize = true;
            lblPrompt.Location = new Point(12, 18);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new Size(127, 15);
            lblPrompt.TabIndex = 0;
            lblPrompt.Text = "Enter admin password:";
            txtPassword.Location = new Point(15, 44);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.Size = new Size(257, 23);
            txtPassword.TabIndex = 1;
            txtPassword.TextChanged += txtPassword_TextChanged;
            btnOK.Location = new Point(15, 80);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            btnCancel.Location = new Point(107, 80);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            btnChangePassword.Location = new Point(198, 73);
            btnChangePassword.Name = "btnChangePassword";
            btnChangePassword.Size = new Size(100, 39);
            btnChangePassword.TabIndex = 4;
            btnChangePassword.Text = "Change Password";
            btnChangePassword.UseVisualStyleBackColor = true;
            btnChangePassword.Click += btnChangePassword_Click;
            ClientSize = new Size(310, 121);
            Controls.Add(btnChangePassword);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(txtPassword);
            Controls.Add(lblPrompt);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Admin Login";
            ResumeLayout(false);
            PerformLayout();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }
    }
}
