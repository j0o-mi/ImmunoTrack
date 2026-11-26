// Folder: Forms/LoginForm.Designer.cs
// (This is the layout file.)
#nullable disable
using System.Drawing;
using System.Windows.Forms;

namespace ImmunoTrack.Forms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblTitle = new Label();
            lblSubtitle = new Label();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnLogin = new Button();
            btnExit = new Button();
            pnlHeader.SuspendLayout();
            SuspendLayout();

            // pnlHeader
            pnlHeader.BackColor = Color.FromArgb(0, 105, 92);
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(420, 110);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 22);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "IMMUNOTRACK";

            // lblSubtitle
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(200, 230, 225);
            lblSubtitle.Location = new Point(23, 72);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Text = "Barangay Immunization and Defaulter Tracker";

            // lblUsername / txtUsername
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(40, 135);
            lblUsername.Name = "lblUsername";
            lblUsername.Text = "Username";
            txtUsername.Font = new Font("Segoe UI", 11F);
            txtUsername.Location = new Point(40, 158);
            txtUsername.MaxLength = 30;
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(340, 27);
            txtUsername.TabIndex = 0;

            // lblPassword / txtPassword
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(40, 200);
            lblPassword.Name = "lblPassword";
            lblPassword.Text = "Password";
            txtPassword.Font = new Font("Segoe UI", 11F);
            txtPassword.Location = new Point(40, 223);
            txtPassword.MaxLength = 50;
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(340, 27);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;

            // Buttons
            StyleButton(btnLogin, "btnLogin", "Log In", Color.FromArgb(0, 121, 107), new Point(40, 275), new Size(340, 42), 2);
            StyleButton(btnExit, "btnExit", "Exit", Color.FromArgb(96, 125, 139), new Point(40, 327), new Size(340, 34), 3);

            // LoginForm
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(420, 385);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login - IMMUNOTRACK";
            Controls.Add(pnlHeader);
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(btnLogin);
            Controls.Add(btnExit);

            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private static void StyleButton(Button button, string name, string text, Color color,
                                        Point location, Size size, int tabIndex)
        {
            button.BackColor = color;
            button.Cursor = Cursors.Hand;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button.ForeColor = Color.White;
            button.Location = location;
            button.Name = name;
            button.Size = size;
            button.TabIndex = tabIndex;
            button.Text = text;
            button.UseVisualStyleBackColor = false;
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnExit;
    }
}