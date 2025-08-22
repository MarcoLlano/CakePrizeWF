using CakePrizeDB.Services;
using Microsoft.Data.SqlClient;

namespace CakePrizeView.Forms.ingredients
{
    public partial class BrandForm : Form
    {
        private Form previousForm;
        private BrandService brandService;

        public BrandForm(Form previousForm, SqlConnection sqlConnection)
        {
            InitializeComponent();
            this.previousForm = previousForm;
            brandService = new BrandService(sqlConnection);
        }

        private void FrmBrands_Load(object sender, EventArgs e)
        {
            GetAllBrands(sender, e);
        }

        private void btnBackIngredientForm_Click(object sender, EventArgs e)
        {
            Close();
            previousForm.Show();
        }

        private void btnCloseIngredientForm_Click(object sender, EventArgs e)
        {
            Close();
            previousForm.Close();
        }

        private void btnSaveBrand_Click(object sender, EventArgs e)
        {
            string temp = txtBrandName.Text;
            var brand = brandService.CreateBrand(txtBrandName.Text, richTBBrandComments.Text, "Marco Llano", "Marco Llano");
            txtBrandName.Text = string.Empty;
            richTBBrandComments.Text = string.Empty;
            lblSaveStatus.Text = $"La marca {temp} se registro correctamente!";
        }

        private void GetAllBrands(object sender, EventArgs e)
        {
            TSCmbBrandList.Items.Clear();
            foreach (var item in brandService.GetAllBrands())
            {
                TSCmbBrandList.Items.Add($"{item.Name}");
            }
        }

        private void ClearSaveStatusLabel(object sender, EventArgs e)
        {
            lblSaveStatus.Text = string.Empty; 
        }
    }
}
