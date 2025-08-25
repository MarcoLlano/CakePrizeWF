using Microsoft.Data.SqlClient;

namespace CakePrizeView.Forms.Menu.Products
{
    public partial class ProductForm : Form
    {
        private Form previousForm;
        private SqlConnection sqlConnection;

        public ProductForm(Form previousForm, SqlConnection sqlConnection)
        {
            this.previousForm = previousForm;
            this.sqlConnection = sqlConnection;
            InitializeComponent();
        }
        private void FrmProduct_Load(object sender, EventArgs e)
        {
            //GetAllBrands(sender, e);
        }

        private void btnBackProductUnitForm_Click(object sender, EventArgs e)
        {
            Close();
            previousForm.Show();
        }

        private void btnCloseProductUnitForm_Click(object sender, EventArgs e)
        {
            Close();
            previousForm.Close();
        }

        private void btnProdAddIngredientToList_Click(object sender, EventArgs e)
        {
            int rowIndex = gvProductIngredientList.Rows.Add();
            gvProductIngredientList.Rows[rowIndex].Cells[0].Value = cbProductIngredient.Text;
            gvProductIngredientList.Rows[rowIndex].Cells[1].Value = txtProductIngrQty.Text;
        }

        private void txtProductIngredientImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult dialogResult = ofd.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                txtProductImage.Text = ofd.FileName;
                string filePath = ofd.Filter;
                int filePath1 = ofd.FilterIndex;
                string filePath21 = ofd.SafeFileName;
                string filePath2s1 = ofd.Title;
            }
        }
    }
}
