using CakePrizeCore.libs.DBUtils;
using Microsoft.Data.SqlClient;

namespace CakePrizeView.Forms
{
    public partial class FrmLoginForm : Form
    {
        private SqlConnection sqlConnection;
        public FrmLoginForm()
        {
            InitializeComponent();
            sqlConnection = DBUtils.OpenDBConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string missingCredentialsText = "Usuario y contraseña son requeridos.";

            if (ValidateCredentials(TxtUsername.Text, TxtPassword.Text))
            {
                Hide();
                FormHomePageForm formMainForm = new FormHomePageForm(FindForm() ?? new FrmLoginForm(), sqlConnection);
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
