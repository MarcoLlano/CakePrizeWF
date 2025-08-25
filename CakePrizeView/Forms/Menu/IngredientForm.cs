using CakePrizeDB.Services;
using Microsoft.Data.SqlClient;
using CakePrizeView.Utils;

namespace CakePrizeView.Forms.ingredients
{
    public partial class IngredientForm : Form
    {
        private Form previousForm;
        private UnitTypeService unitTypeService;
        private BrandService brandService;
        private IngredientService ingredientService;

        public IngredientForm(Form previousForm, SqlConnection sqlConnection)
        {
            InitializeComponent();
            this.previousForm = previousForm;
            unitTypeService = new UnitTypeService(sqlConnection);
            brandService = new BrandService(sqlConnection);
            ingredientService = new IngredientService(sqlConnection);
            
            // Set up auto-maximize
            FormMaximizeHelper.SetupAutoMaximize(this);
        }

        private void FrmIngredients_Load(object sender, EventArgs e)
        {
            LoadUnitTypes();
            LoadBrands();
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

        private void GetAllIngredients(object sender, EventArgs e)
        {
            List<CakePrizeDB.Models.IngredientModel> test = ingredientService.GetAllIngredients();
        }

        private void chkbDefaultWholesale_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkbDefaultWholesale.Checked)
            {
                chkbDefaultWholesale.Text = string.Empty;
            }
            SelectDefaultPrice(sender);
        }

        private void chkbDefaultRetail_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkbDefaultRetail.Checked)
            {
                chkbDefaultRetail.Text = string.Empty;
            }
            SelectDefaultPrice(sender);
        }

        private void SelectDefaultPrice(object sender)
        {
            CheckBox? currentCheckBox = sender as CheckBox;
            if (currentCheckBox != null && currentCheckBox.Checked)
            {
                // Iterate through all other checkboxes in the group (e.g., on the same panel or form)
                foreach (Control control in Controls) // Or a specific container like a Panel
                {
                    if (control is CheckBox otherCheckBox && otherCheckBox != currentCheckBox)
                    {
                        currentCheckBox.Text = "Por defecto.";
                        otherCheckBox.Text = string.Empty;
                        otherCheckBox.Checked = false;
                    }
                }
            }
        }

        private void btnSaveIngredient_Click(object sender, EventArgs e)
        {
            string selectedPrice = chkbDefaultRetail.CheckState == 0 ? "Wholesale" : "Retail";
            string createdModifiedUser = "Marco Llano";
            var unitSelected = cmbIngredientBrand.Text;
            ingredientService.CreateIngredient(
                txtIngredientName.Text,
                Guid.Parse(GetAllUnitTypes().Where(d => d.Value == cmbIngredientUnits.Text)
                .Select(t => t.Key).First().ToString() ?? string.Empty),
                Guid.Parse(GetAllBrands().Where(d => d.Value == cmbIngredientBrand.Text)
                .Select(t => t.Key).First().ToString() ?? string.Empty),
                float.Parse(txtRetailPrice.Text),
                float.Parse(txtWholesalePrice.Text),
                selectedPrice,
                richTBIngredientComments.Text,
                createdModifiedUser,
                createdModifiedUser);
        }

        private Dictionary<Guid, string> GetAllUnitTypes()
        {
            Dictionary<Guid, string> units = new Dictionary<Guid, string>();
            unitTypeService.GetAllUnitTypes().ForEach(unitType =>
            {
                units.Add(unitType.Id, unitType.Name);
            });
            return units;
        }

        private Dictionary<Guid, string> GetAllBrands()
        {
            Dictionary<Guid, string> units = new Dictionary<Guid, string>();
            brandService.GetAllBrands().ForEach(brand =>
            {
                units.Add(brand.Id, brand.Name);
            });
            return units;
        }

        private void LoadUnitTypes()
        {
            List<string> unitTypeList = new List<string>();
            foreach (var unit in GetAllUnitTypes())
            {
                cmbIngredientUnits.Items.Add(unit.Value);
                unitTypeList.Add(unit.Value);
            }
        }

        private void LoadBrands()
        {
            List<string> brandList = new List<string>();
            foreach (var brand in GetAllBrands())
            {
                cmbIngredientBrand.Items.Add(brand.Value);
                brandList.Add(brand.Value);
            }
        }

        private void ClearFields()
        {
            txtIngredientName.Text = string.Empty;
            txtRetailPrice.Text = string.Empty;
            txtWholesalePrice.Text = string.Empty;
            cmbIngredientBrand.Text = string.Empty;
            cmbIngredientUnits.Text = string.Empty;
            richTBIngredientComments.Text = string.Empty;
        }
    }
}
