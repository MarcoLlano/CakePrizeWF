using CakePrizeCore.libs.DBUtils;
using CakePrizeDB.Services;
using CakePrizeView.Utils;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace CakePrizeView.Forms
{
    public partial class FrmLoginForm : Form
    {
        private SqlConnection sqlConnection;
        private UserService userService;
        
        // Store initial form size for relative positioning
        private Size initialFormSize;
        private Dictionary<Control, Rectangle> initialControlBounds;

        public FrmLoginForm()
        {
            InitializeComponent();
            sqlConnection = DBUtils.OpenDBConnection();
            userService = new UserService(sqlConnection);
            
            // Add resize event handler
            this.Resize += LoginForm_Resize;
            
            // Store initial positions for relative positioning
            StoreInitialPositions();
            
            // Set up auto-maximize
            FormMaximizeHelper.SetupAutoMaximize(this);
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

        /// <summary>
        /// Stores initial positions and sizes of controls for relative positioning
        /// </summary>
        private void StoreInitialPositions()
        {
            initialFormSize = this.Size;
            initialControlBounds = new Dictionary<Control, Rectangle>();
            
            // Store initial bounds for all controls that need responsive positioning
            StoreControlBounds(panel1);
            StoreControlBounds(LblLoginFormTitle);
            StoreControlBounds(LblUserLabel);
            StoreControlBounds(LblPasswordLabel);
            StoreControlBounds(TxtUsername);
            StoreControlBounds(TxtPassword);
            StoreControlBounds(BtnLogin);
            StoreControlBounds(BtnExit);
            StoreControlBounds(LblErrorUserPwd);
        }

        /// <summary>
        /// Recursively stores bounds for a control and its children
        /// </summary>
        private void StoreControlBounds(Control control)
        {
            if (control != null)
            {
                initialControlBounds[control] = control.Bounds;
                
                // Store bounds for child controls
                foreach (Control child in control.Controls)
                {
                    StoreControlBounds(child);
                }
            }
        }

        /// <summary>
        /// Handles form resize to adjust control positions and sizes
        /// </summary>
        private void LoginForm_Resize(object sender, EventArgs e)
        {
            if (initialControlBounds == null || initialFormSize.Width == 0 || initialFormSize.Height == 0)
                return;

            // Calculate scaling factors
            float scaleX = (float)this.Width / initialFormSize.Width;
            float scaleY = (float)this.Height / initialFormSize.Height;

            // Adjust control positions and sizes
            AdjustControlLayout(panel1, scaleX, scaleY);
            AdjustControlLayout(LblLoginFormTitle, scaleX, scaleY);
            AdjustControlLayout(LblUserLabel, scaleX, scaleY);
            AdjustControlLayout(LblPasswordLabel, scaleX, scaleY);
            AdjustControlLayout(TxtUsername, scaleX, scaleY);
            AdjustControlLayout(TxtPassword, scaleX, scaleY);
            AdjustControlLayout(BtnLogin, scaleX, scaleY);
            AdjustControlLayout(BtnExit, scaleX, scaleY);
            AdjustControlLayout(LblErrorUserPwd, scaleX, scaleY);
            
            // Ensure minimum spacing between controls
            EnsureMinimumSpacing();
        }

        /// <summary>
        /// Adjusts a control's position and size based on scaling factors
        /// </summary>
        private void AdjustControlLayout(Control control, float scaleX, float scaleY)
        {
            if (control != null && initialControlBounds.ContainsKey(control))
            {
                Rectangle initialBounds = initialControlBounds[control];
                
                // Calculate new position and size
                int newX = (int)(initialBounds.X * scaleX);
                int newY = (int)(initialBounds.Y * scaleY);
                int newWidth = (int)(initialBounds.Width * scaleX);
                int newHeight = (int)(initialBounds.Height * scaleY);
                
                // Apply new bounds
                control.Bounds = new Rectangle(newX, newY, newWidth, newHeight);
            }
        }

        /// <summary>
        /// Ensures minimum spacing between controls
        /// </summary>
        private void EnsureMinimumSpacing()
        {
            const int minSpacing = 10;
            
            // Ensure minimum spacing between login and exit buttons
            if (BtnLogin.Right + minSpacing > BtnExit.Left)
            {
                BtnExit.Left = BtnLogin.Right + minSpacing;
            }
            
            // Ensure error label doesn't overlap with buttons
            if (LblErrorUserPwd.Bottom + minSpacing > BtnLogin.Top)
            {
                BtnLogin.Top = LblErrorUserPwd.Bottom + minSpacing;
                BtnExit.Top = BtnLogin.Top;
            }
        }
    }
}
