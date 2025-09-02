using CakePrizeDB.Services;
using Microsoft.Data.SqlClient;
using CakePrizeView.Utils;
using CakePrizeCore.libs.DBUtils;
using System.Text.RegularExpressions;

namespace CakePrizeView.Forms.ingredients
{
    public partial class IngredientForm : BaseForm
    {
        private Form previousForm;
        private UnitTypeService unitTypeService;
        private BrandService brandService;
        private IngredientService ingredientService;

        public IngredientForm(Form previousForm, SqlConnection? sqlConnection = null)
        {
            unitTypeService = new UnitTypeService();
            brandService = new BrandService();
            ingredientService = new IngredientService();
            InitializeComponent();
            this.previousForm = previousForm;

            // Initialize services after InitializeComponent to ensure proper connection state
            InitializeServices();

            // Set up auto-maximize
            FormMaximizeHelper.SetupAutoMaximize(this);
        }

        /// <summary>
        /// Initializes all services with fresh connections
        /// </summary>
        private void InitializeServices()
        {
            try
            {
                // Create fresh connections for each service to ensure they're open and available
                var connection = DatabaseConnectionManager.OpenConnection();

                unitTypeService = new UnitTypeService();
                brandService = new BrandService();
                ingredientService = new IngredientService();
            }
            catch (Exception ex)
            {
                // If we can't create services, show error but don't crash the form
                MessageBox.Show($"Failed to initialize database services: {ex.Message}", "Initialization Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            SelectDefaultPrice(sender, e);
        }

        private void chkbDefaultRetail_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkbDefaultRetail.Checked)
            {
                chkbDefaultRetail.Text = string.Empty;
            }
            SelectDefaultPrice(sender, e);
        }

        private void SelectDefaultPrice(object sender, EventArgs e)
        {
            CheckBox? currentCheckBox = sender as CheckBox;
            if (currentCheckBox != null && currentCheckBox.Checked)
            {
                // Limit search to the same container (panel/group) as the current checkbox
                Control container = currentCheckBox.Parent ?? this;

                currentCheckBox.Text = "Por defecto.";
                foreach (Control control in container.Controls)
                {
                    if (control is CheckBox otherCheckBox && !ReferenceEquals(otherCheckBox, currentCheckBox))
                    {
                        otherCheckBox.Text = string.Empty;
                        otherCheckBox.Checked = false;
                    }
                }
            }
        }

        private bool ValidateFields()
        {
            bool pass = false;
            pass = Regex.IsMatch(@"^(\w+ ?)*$", txtIngredientName.Text);
            return pass;
        }

        private void btnSaveIngredient_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                string selectedPrice = chkbDefaultRetail.CheckState == 0 ? "Wholesale" : "Retail";
                string createdModifiedUser = "Marco Llano";

                // Handle ingredientBrandId - set to null if no brand is selected or brand doesn't exist
                Guid? ingredientBrandId = null;
                if (!string.IsNullOrEmpty(cmbIngredientBrand.Text))
                {
                    var brandMatch = GetAllBrands().FirstOrDefault(d => d.Value == cmbIngredientBrand.Text);
                    if (brandMatch.Key != Guid.Empty)
                    {
                        ingredientBrandId = brandMatch.Key;
                    }
                }

                ingredientService.CreateIngredient(
                    txtIngredientName.Text,
                    Guid.Parse(GetAllUnitTypes().Where(d => d.Value == cmbIngredientUnits.Text)
                    .Select(t => t.Key).First().ToString() ?? string.Empty),
                    ingredientBrandId,
                    float.Parse(txtRetailPrice.Text),
                    float.Parse(txtWholesalePrice.Text),
                    selectedPrice,
                    int.Parse(txtPackQty.Text),
                    richTBIngredientComments.Text,
                    createdModifiedUser,
                    createdModifiedUser);
            }

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

