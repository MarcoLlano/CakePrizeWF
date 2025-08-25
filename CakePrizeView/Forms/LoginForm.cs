using CakePrizeCore.libs.DBUtils;
using CakePrizeDB.Services;
using CakePrizeView.Utils;
using Microsoft.Data.SqlClient;

namespace CakePrizeView.Forms
{
    public partial class FrmLoginForm : Form
    {
        private SqlConnection sqlConnection;
        private UserService userService;

        public FrmLoginForm()
        {
            InitializeComponent();
            sqlConnection = DBUtils.OpenDBConnection();
            userService = new UserService(sqlConnection);
        }

        private void Login_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void FrmLoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TxtPassword_TextChanged(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Login();
            }
        }

        private void Login()
        {
            string missingCredentialsText = "Usuario y contraseña son requeridos.";
            string invalidCredentialsText = "Usuario o contraseña incorrectos.";
            string disabledUserText = "Usuario desactivado. Contacte al administrador.";

            if (string.IsNullOrWhiteSpace(TxtUsername.Text) || string.IsNullOrWhiteSpace(TxtPassword.Text))
            {
                LblErrorUserPwd.Text = missingCredentialsText;
                return;
            }

            // First check if user exists and is active
            var user = userService.GetUserByUsername(TxtUsername.Text);
            if (user != null && !user.IsActive)
            {
                LblErrorUserPwd.Text = disabledUserText;
                return;
            }

            var authenticatedUser = userService.AuthenticateUser(TxtUsername.Text, TxtPassword.Text);

            if (authenticatedUser != null)
            {
                // Set the user session
                UserSession.SetCurrentUser(authenticatedUser);

                Hide();
                FormHomePageForm formMainForm = new FormHomePageForm(FindForm() ?? new FrmLoginForm(), sqlConnection);
                formMainForm.Show();
            }
            else
            {
                LblErrorUserPwd.Text = invalidCredentialsText;
            }
        }
    }
}
