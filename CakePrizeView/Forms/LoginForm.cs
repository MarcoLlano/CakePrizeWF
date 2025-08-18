using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CakePrizeView.forms
{
    public partial class FrmLoginForm : Form
    {
        public FrmLoginForm()
        {
            InitializeComponent();
            //TxtPassword.Clear();
            //TxtUsername.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string missingCredentialsText = "Usuario y contraseña son requeridos.";

            if (ValidateCredentials(TxtUsername.Text, TxtPassword.Text))
            {
                Hide();
                FormHomePageForm formMainForm = new FormHomePageForm(FindForm());
                formMainForm.Show();
            }
            else
            {
                LblErrorUserPwd.Text = missingCredentialsText;
            }
        }

        private bool ValidateCredentials(string username, string password)
        {
            bool areValid = true;
            if (username == null | username.Equals(string.Empty))
            {
                return false;
            }
            if (password == null | password.Equals(string.Empty))
            {
                return false;
            }
            return areValid;
        }

        private void FrmLoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
