using CakePrizeView.Forms;
using CakePrizeView.Forms.ingredients;
using CakePrizeView.Forms.Menu.Products;
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
        }

        private void FormMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            Hide();
            previousForm = new FrmLoginForm();
            previousForm.Show();
            
            if(previousForm.Created && previousForm.Visible)
            {
            }
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
            ProductUnitForm productUnitForm = new ProductUnitForm(FindForm() ?? new ErrorNotFoundForm(), sqlConnection);
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
    }
}
