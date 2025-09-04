using CakePrizeView.Forms;
using CakePrizeView.Forms.ingredients;
using CakePrizeView.Forms.Menu;
using CakePrizeView.Forms.Menu.Products;
using CakePrizeView.Utils;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace CakePrizeView
{
    public partial class FormHomePageForm : BaseForm
    {
        private Form previousForm;

        // Store initial form size for relative positioning
        private Size initialFormSize;
        private Dictionary<Control, Rectangle> initialControlBounds;

        public FormHomePageForm(Form previousForm, SqlConnection? sqlConnection = null)
        {
            InitializeComponent();
            this.previousForm = previousForm;

            // Add resize event handler
            this.Resize += HomePageForm_Resize;

            // Store initial positions for relative positioning
            StoreInitialPositions();

            // Configure menu visibility based on user permissions
            ConfigureMenuPermissions();

            // Update form title with user information
            UpdateFormTitle();

            // Set up auto-maximize
            FormMaximizeHelper.SetupAutoMaximize(this);
        }

        private void FormMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            // Clear the user session
            UserSession.ClearSession();

            Hide();
            previousForm = new FrmLoginForm();
            previousForm.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
            previousForm.Close();
        }

        private void ingredientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            IngredientForm formCupcake = new IngredientForm(FindForm() ?? new ErrorNotFoundForm(), null);
            formCupcake.Show();
        }

        private void marcasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            BrandForm formCupcake = new BrandForm(FindForm() ?? new ErrorNotFoundForm(), null);
            formCupcake.Show();
        }

        private void calculadoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            CalculatorForm calculatorForm = new CalculatorForm(FindForm() ?? new ErrorNotFoundForm(), null);
            calculatorForm.Show();
        }

        private void unidadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            ProductForm productUnitForm = new ProductForm(FindForm() ?? new ErrorNotFoundForm(), null);
            productUnitForm.Show();
        }

        private void armarCombosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            ProductSetForm productSetForm = new ProductSetForm(FindForm() ?? new ErrorNotFoundForm(), null);
            productSetForm.Show();
        }

        private void combosEspecialesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            ProductSpecialComboForm productSpecialComboForm = new ProductSpecialComboForm(FindForm() ?? new ErrorNotFoundForm(), null);
            productSpecialComboForm.Show();
        }

        private void promocionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            ProductPromosForm productPromosForm = new ProductPromosForm(FindForm() ?? new ErrorNotFoundForm(), null);
            productPromosForm.Show();
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            UserManagementForm userManagementForm = new UserManagementForm(FindForm() ?? new ErrorNotFoundForm(), null);
            userManagementForm.Show();
        }

        private void tiposDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            ProductTypeForm prodTypeFrm = new ProductTypeForm(FindForm() ?? new ErrorNotFoundForm(), null);
            prodTypeFrm.Show();
        }

        /// <summary>
        /// Configures menu visibility based on user permissions
        /// </summary>
        private void ConfigureMenuPermissions()
        {
            // Ingredients management - PastryChef and Admin only
            tsBtnIngredients.Visible = UserSession.CanManageIngredients();

            // Brand management - PastryChef and Admin only
            tsBtnBrands.Visible = UserSession.CanManageBrands();

            // Product management - PastryChef and Admin only
            tsDdnProductItem.Visible = UserSession.CanManageProducts();
            tsDdnComboItem.Visible = UserSession.CanManageProducts();
            tsDdnPromosItem.Visible = UserSession.CanManageProducts();

            // Calculator - Available to all users
            tsBtnCalculator.Visible = true;

            // User Management - Admin only
            // Note: You'll need to add a menu item for user management and configure it here
            // usuariosToolStripMenuItem.Visible = UserSession.IsAdmin();

            // Reports - Sales, PastryChef, and Admin only
            // Note: You'll need to add report menu items and configure them here
        }

        /// <summary>
        /// Updates the form title with user information
        /// </summary>
        private void UpdateFormTitle()
        {
            var userName = UserSession.GetCurrentUserName();
            var userRole = UserSession.GetRoleDisplayName(UserSession.GetCurrentUserRole());
            this.Text = $"CakePrize - {userName} ({userRole})";
        }

        /// <summary>
        /// Shows an access denied message
        /// </summary>
        private void ShowAccessDeniedMessage()
        {
            MessageBox.Show(
                "No tiene permisos para acceder a esta funci�n.",
                "Acceso Denegado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Stores initial positions and sizes of controls for relative positioning
        /// </summary>
        private void StoreInitialPositions()
        {
            initialFormSize = this.Size;
            initialControlBounds = new Dictionary<Control, Rectangle>();

            // Store initial bounds for all controls that need responsive positioning
            StoreControlBounds(btnClose);
            StoreControlBounds(btnLogout);
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
        private void HomePageForm_Resize(object sender, EventArgs e)
        {
            if (initialControlBounds == null || initialFormSize.Width == 0 || initialFormSize.Height == 0)
                return;

            // Calculate scaling factors
            float scaleX = (float)this.Width / initialFormSize.Width;
            float scaleY = (float)this.Height / initialFormSize.Height;

            // Adjust control positions and sizes
            AdjustControlLayout(btnClose, scaleX, scaleY);
            AdjustControlLayout(btnLogout, scaleX, scaleY);

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

            // Ensure minimum spacing between logout and close buttons
            if (btnLogout.Right + minSpacing > btnClose.Left)
            {
                btnClose.Left = btnLogout.Right + minSpacing;
            }
        }

        private void tsBtnCalculator_Click(object sender, EventArgs e)
        {
            Hide();
            CalculatorForm calculatorForm = new CalculatorForm(FindForm() ?? new ErrorNotFoundForm(), null);
            calculatorForm.Show();
        }

        private void tsBtnBrands_Click(object sender, EventArgs e)
        {
            Hide();
            BrandForm brandForm = new BrandForm(FindForm() ?? new ErrorNotFoundForm(), null);
            brandForm.Show();
        }

        private void tsDdnProductTypeItem_Click(object sender, EventArgs e)
        {
            Hide();
            ProductTypeForm productTypeForm = new ProductTypeForm(FindForm() ?? new ErrorNotFoundForm(), null);
            productTypeForm.Show();
        }

        private void tsDdnProductItem_Click(object sender, EventArgs e)
        {
            Hide();
            ProductForm productForm = new ProductForm(FindForm() ?? new ErrorNotFoundForm(), null);
            productForm.Show();
        }

        private void tsDdnComboItem_Click(object sender, EventArgs e)
        {
            Hide();
            ProductSpecialComboForm productSpecialComboForm = new ProductSpecialComboForm(FindForm() ?? new ErrorNotFoundForm(), null);
            productSpecialComboForm.Show();
        }
    }
}

