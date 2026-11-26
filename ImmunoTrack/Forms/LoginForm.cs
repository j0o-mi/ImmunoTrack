// Folder: Forms/LoginForm.cs
using System;
using System.Windows.Forms;
using ImmunoTrack.Models;
using ImmunoTrack.Services;

namespace ImmunoTrack.Forms
{
    /// <summary>
    /// The first screen. It only collects the username and password and asks
    /// AuthService to check them. Closing with DialogResult.OK means "logged in".
    /// </summary>
    public partial class LoginForm : Form
    {
        private readonly AuthService _authService = new AuthService();

        public LoginForm()
        {
            InitializeComponent();

            AcceptButton = btnLogin;   // pressing Enter clicks Log In
            CancelButton = btnExit;    // pressing Esc clicks Exit

            btnLogin.Click += btnLogin_Click;
            btnExit.Click += btnExit_Click;
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            try
            {
                HealthWorker worker = _authService.Login(txtUsername.Text, txtPassword.Text);
                UserSession.Start(worker);
                DialogResult = DialogResult.OK;    // closes this form and tells Program.cs it worked
            }
            catch (ArgumentException ex)           // empty fields
            {
                MessageBox.Show(ex.Message, "Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
            }
            catch (InvalidOperationException ex)   // wrong username or password
            {
                MessageBox.Show(ex.Message, "Login failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Clear();
                txtPassword.Focus();
            }
            catch (Exception ex)                   // e.g. the database is not reachable
            {
                MessageBox.Show("Could not log in.\n\nDetails: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}