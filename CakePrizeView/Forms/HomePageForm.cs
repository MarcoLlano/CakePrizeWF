using CakePrizeView.Forms;
using CakePrizeView.Forms.ingredients;
using CakePrizeView.Forms.Menu.Products;
using CakePrizeView.Utils;
using Microsoft.Data.SqlClient;

namespace CakePrizeView
{
    public partial class FormHomePageForm : Form
    {
        private Form previousForm;
        private SqlConnection sqlConnection;

        public FormHomePageForm(Form previousForm, SqlConnection sqlConnection)
        {
            InitializeComponent();
            this.previousForm = previousForm;
            this.sqlConnection = sqlConnection;
            
            // Configure menu visibility based on user permissions
            ConfigureMenuPermissions();
            
            // Update form title with user information
            UpdateFormTitle();
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
            IngredientForm formCupcake = new IngredientForm(FindForm() ?? new ErrorNotFoundForm(), sqlConnection);
            formCupcake.Show();
        }

        private void marcasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            BrandForm formCupcake = new BrandForm(FindForm() ?? new ErrorNotFoundForm(), sqlConnection);
            formCupcake.Show();
        }

        private void calculadoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            CalculatorForm calculatorForm = new CalculatorForm(FindForm() ?? new ErrorNotFoundForm(), sqlConnection);
            calculatorForm.Show();
        }

        private void unidadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            ProductForm productUnitForm = new ProductForm(FindForm() ?? new ErrorNotFoundForm(), sqlConnection);
            productUnitForm.Show();
        }

        private void armarCombosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            ProductSetForm productSetForm = new ProductSetForm(FindForm() ?? new ErrorNotFoundForm(), sqlConnection);
            productSetForm.Show();
        }

        private void combosEspecialesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            ProductSpecialComboForm productSpecialComboForm = new ProductSpecialComboForm(FindForm() ?? new ErrorNotFoundForm(), sqlConnection);
            productSpecialComboForm.Show();
        }

        private void promocionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            ProductPromosForm productPromosForm = new ProductPromosForm(FindForm() ?? new ErrorNotFoundForm(), sqlConnection);
            productPromosForm.Show();
        }

        /// <summary>
        /// Configures menu visibility based on user permissions
        /// </summary>
        private void ConfigureMenuPermissions()
        {
            // Ingredients management - PastryChef and Admin only
            ingredientesToolStripMenuItem.Visible = UserSession.CanManageIngredients();
            
            // Brand management - PastryChef and Admin only
            marcasToolStripMenuItem.Visible = UserSession.CanManageBrands();
            
            // Product management - PastryChef and Admin only
            unidadToolStripMenuItem.Visible = UserSession.CanManageProducts();
            armarCombosToolStripMenuItem.Visible = UserSession.CanManageProducts();
            combosEspecialesToolStripMenuItem.Visible = UserSession.CanManageProducts();
            promocionesToolStripMenuItem.Visible = UserSession.CanManageProducts();
            
            // Calculator - Available to all users
            calculadoraToolStripMenuItem.Visible = true;
            
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
                "No tiene permisos para acceder a esta función.",
                "Acceso Denegado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }
}
