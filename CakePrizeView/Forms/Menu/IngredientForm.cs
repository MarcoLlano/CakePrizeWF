using CakePrizeCore.libs.DBUtils;
using CakePrizeDB.Models;
using CakePrizeDB.Services;
using CakePrizeView.Forms.Popups;
using CakePrizeView.Utils;
using Microsoft.Data.SqlClient;

namespace CakePrizeView.Forms.ingredients
{
    public partial class IngredientForm : BaseForm
    {
        private Form previousForm;
        private UnitTypeService unitTypeService;
        private BrandService brandService;
        private IngredientService ingredientService;
        private Dictionary<Control, Rectangle> initialControlBounds;
        private Size initialFormSize;
        private IngredientModel ingredientEditModel;

        public IngredientForm(Form previousForm, SqlConnection? sqlConnection = null)
        {
            initialControlBounds = new Dictionary<Control, Rectangle>();
            ingredientEditModel = new IngredientModel();
            unitTypeService = new UnitTypeService();
            brandService = new BrandService();
            ingredientService = new IngredientService();
            InitializeComponent();
            this.previousForm = previousForm;

            // Initialize services after InitializeComponent to ensure proper connection state
            InitializeServices();

            Resize += IngredientForm_Resize;
            StoreInitialPositions();

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
            GetAllIngredients(sender, e);
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
            TSCmbIngredientsList.Items.Clear();
            var ingredients = ingredientService.GetAllIngredients();
            foreach (var ing in ingredients)
            {
                var brandName = ing.BrandId.HasValue ? (brandService.GetBrandById(ing.BrandId)?.Name ?? string.Empty) : string.Empty;
                TSCmbIngredientsList.Items.Add($"{ing.Name} - {brandName}");
            }
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
            //TODO: FIX THIS
            bool pass = false;
            if (txtIngredientName.Text != string.Empty && txtPackQty.Text != string.Empty && txtRetailPrice.Text != string.Empty &&
                txtWholesalePrice.Text != string.Empty && cmbIngredientBrand.Text != string.Empty)
            {
                pass = true;
            }
            return pass;
        }

        private void btnEditIngredient_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                string tempIngredientName = txtIngredientName.Text;
                string selectedPrice = chkbDefaultRetail.CheckState == 0 ? "Wholesale" : "Retail";

                // Handle ingredientBrandId - set to null if no brand is selected or brand doesn't exist
                Guid? ingredientBrandId = null;
                if (!string.IsNullOrEmpty(cmbIngredientBrand.Text))
                {
                    var brandEntry = GetAllBrands().FirstOrDefault(d => d.Value == cmbIngredientBrand.Text);
                    if (!brandEntry.Equals(default(KeyValuePair<Guid, string>)))
                    {
                        ingredientBrandId = brandEntry.Key;
                    }
                }
                Guid unitTypeId = GetAllUnitTypes().FirstOrDefault(d => d.Value == cmbIngredientUnits.Text).Key;

                try
                {
                    ingredientService.UpdateIngredient(
                        ingredientEditModel.Id,
                        txtIngredientName.Text,
                        unitTypeId,
                        ingredientBrandId,
                        float.Parse(txtRetailPrice.Text),
                        float.Parse(txtWholesalePrice.Text),
                        selectedPrice,
                        int.Parse(txtPackQty.Text),
                        richTBIngredientComments.Text,
                        UserSession.GetCurrentUsername());

                    ClearFields();
                    lblStatusCheck.ForeColor = Color.Green;
                    lblStatusCheck.Text = $"Ingrediente {tempIngredientName} actualizado exitosamente.";
                }
                catch (Exception)
                {
                    lblStatusCheck.ForeColor = Color.Red;
                    lblStatusCheck.Text = "Error: revise nuevamente y llene los campos requeridos e intente nuevamente.";
                }
            }
            else
            {
                lblStatusCheck.ForeColor = Color.Red;
                lblStatusCheck.Text = "Error: revise nuevamente y llene los campos requeridos e intente nuevamente.";
            }
        }

        private void btnSaveIngredient_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                string tempIngredientName = txtIngredientName.Text;
                string selectedPrice = chkbDefaultRetail.CheckState == 0 ? "Wholesale" : "Retail";

                // Handle ingredientBrandId - set to null if no brand is selected or brand doesn't exist
                Guid? ingredientBrandId = null;
                if (!string.IsNullOrEmpty(cmbIngredientBrand.Text))
                {
                    var brandEntry = GetAllBrands().FirstOrDefault(d => d.Value == cmbIngredientBrand.Text);
                    if (!brandEntry.Equals(default(KeyValuePair<Guid, string>)))
                    {
                        ingredientBrandId = brandEntry.Key;
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
                    UserSession.GetCurrentUsername(),
                    UserSession.GetCurrentUsername());
                ClearFields();
                lblStatusCheck.ForeColor = Color.Green;
                lblStatusCheck.Text = $"Ingrediente {tempIngredientName} creado exitosamente.";
            }
            else
            {
                lblStatusCheck.ForeColor = Color.Red;
                lblStatusCheck.Text = "Error: revise nuevamente y llene los campos requeridos e intente nuevamente.";
            }
        }

        private Dictionary<Guid, string> GetAllUnitTypes()
        {
            Dictionary<Guid, string> units = new Dictionary<Guid, string>();
            unitTypeService.GetAllUnitTypes().ForEach(unitType =>
            {
                units.Add(unitType.Id, unitType.Acronym.Replace(" ", string.Empty));
            });
            return units;
        }

        private Dictionary<Guid, string> GetAllBrands()
        {
            Dictionary<Guid, string> brandMap = new Dictionary<Guid, string>();
            brandService.GetAllBrands().ForEach(brand =>
            {
                if (brand.Id.HasValue && brand.Id.Value != Guid.Empty)
                {
                    brandMap[brand.Id.Value] = brand.Name;
                }
            });
            return brandMap;
        }

        private void LoadUnitTypes()
        {
            List<string> unitTypeList = new List<string>();
            foreach (var unit in GetAllUnitTypes())
            {
                cmbIngredientUnits.Items.Add(unit.Value);
                unitTypeList.Add(unit.Value.Replace(" ", string.Empty));
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

        private void TSCmbIngredientList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TSCmbIngredientsList.Text != string.Empty)
            {
                btnClearIngredientTexts.Hide();
                btnSaveIngredient.Hide();
                btnEditIngredient.Visible = true;
                btnCancelEditIngredient.Visible = true;

                var selectedText = TSCmbIngredientsList.Text;
                var parts = selectedText.Split(" - ");
                var selectedName = parts.Length > 0 ? parts[0] : string.Empty;
                var selectedBrandName = parts.Length > 1 ? parts[1] : string.Empty;

                Guid? selectedBrandId = null;
                if (!string.IsNullOrWhiteSpace(selectedBrandName))
                {
                    var brandMatch = brandService.GetAllBrands().FirstOrDefault(b => b.Name == selectedBrandName);
                    selectedBrandId = brandMatch?.Id;
                }


                ingredientEditModel = ingredientService.GetAllIngredients()
                    .First(ing => ing.Name == selectedName &&
                                  ((ing.BrandId == null && selectedBrandId == null) ||
                                   (ing.BrandId.HasValue && selectedBrandId.HasValue && ing.BrandId.Value == selectedBrandId.Value)));

                txtIngredientName.Text = ingredientEditModel.Name;
                txtPackQty.Text = ingredientEditModel.PackQty.ToString();
                txtRetailPrice.Text = ingredientEditModel.RetailPrice.ToString();
                txtWholesalePrice.Text = ingredientEditModel.WholesalePrice.ToString();
                var brandName = ingredientEditModel.BrandId.HasValue ? (brandService.GetBrandById(ingredientEditModel.BrandId)?.Name ?? string.Empty) : string.Empty;
                cmbIngredientBrand.Text = brandName;
                cmbIngredientUnits.Text = unitTypeService.GetUnitTypeById(ingredientEditModel.UnitTypeId).Acronym.Replace(" ", string.Empty);
                richTBIngredientComments.Text = ingredientEditModel.Comments;
                chkbDefaultRetail.Checked = ingredientEditModel.DefaultPrice.Equals("Retail");
                chkbDefaultWholesale.Checked = ingredientEditModel.DefaultPrice.Equals("Wholesale");
            }
        }

        private void ClearFields()
        {
            txtIngredientName.Text = string.Empty;
            txtRetailPrice.Text = string.Empty;
            txtWholesalePrice.Text = string.Empty;
            txtPackQty.Text = string.Empty;
            cmbIngredientBrand.Text = string.Empty;
            cmbIngredientUnits.Text = string.Empty;
            richTBIngredientComments.Text = string.Empty;
            btnCancelEditIngredient.Visible = false;
            btnEditIngredient.Visible = false;
            btnSaveIngredient.Visible = true;
            btnClearIngredientTexts.Visible = true;
            lblStatusCheck.Text = string.Empty;
        }

        private void CalculateWholeRetailSalePrices_Click(object sender, EventArgs e)
        {
            using (var dialog = new CalculateWholeRetailSalesPrice())
            {
                var result = dialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    txtWholesalePrice.Text = dialog.CalculatedWholesalePrice;
                    txtRetailPrice.Text = dialog.CalculatedRetailPrice;
                }
            }
        }
        /// <summary>
        /// Recursively stores bounds for a control and its children
        /// </summary>
        private void StoreControlBounds(Control control)
        {
            FormUtils.StoreControlBounds(control, initialControlBounds);
        }

        /// <summary>
        /// Stores initial positions and sizes of controls for relative positioning
        /// </summary>
        private void StoreInitialPositions()
        {
            initialFormSize = this.Size;
            initialControlBounds = new Dictionary<Control, Rectangle>();

            // Store initial bounds for all controls that need responsive positioning
            StoreControlBounds(pnlIngredientForm);
            StoreControlBounds(LblIngredientTitle);
            StoreControlBounds(btnSaveIngredient);
            StoreControlBounds(btnCloseIngredients);
            StoreControlBounds(lblIngredientName);
            StoreControlBounds(btnBackIngredients);
            StoreControlBounds(chkbDefaultWholesale);
            StoreControlBounds(lblWholesalePrice);
            StoreControlBounds(richTBIngredientComments);
            StoreControlBounds(btnClearIngredientTexts);
            StoreControlBounds(lblRetailPrice);
            StoreControlBounds(lblUnit);
            StoreControlBounds(txtIngredientName);
            StoreControlBounds(txtRetailPrice);
            StoreControlBounds(txtWholesalePrice);
            StoreControlBounds(txtPackQty);
            StoreControlBounds(cmbIngredientUnits);
            StoreControlBounds(lblBrand);
            StoreControlBounds(lblQtyExample);
            StoreControlBounds(lblComments);
            StoreControlBounds(btnCalculator);
            StoreControlBounds(cmbIngredientBrand);
            StoreControlBounds(chkbDefaultRetail);
            StoreControlBounds(btnEditIngredient);
            StoreControlBounds(btnCancelEditIngredient);
            StoreControlBounds(lblQtyXPack);
            StoreControlBounds(lblStatusCheck);

        }

        /// <summary>
        /// Handles form resize to adjust control positions and sizes
        /// </summary>
        private void IngredientForm_Resize(object? sender, EventArgs e)
        {
            if (initialControlBounds == null || initialFormSize.Width == 0 || initialFormSize.Height == 0)
                return;

            // Calculate scaling factors
            float scaleX = (float)this.Width / initialFormSize.Width;
            float scaleY = (float)this.Height / initialFormSize.Height;

            // Adjust control positions and sizes
            AdjustControlLayout(pnlIngredientForm, scaleX, scaleY);
            AdjustControlLayout(LblIngredientTitle, scaleX, scaleY);
            AdjustControlLayout(btnSaveIngredient, scaleX, scaleY);
            AdjustControlLayout(btnCloseIngredients, scaleX, scaleY);
            AdjustControlLayout(lblIngredientName, scaleX, scaleY);
            AdjustControlLayout(btnBackIngredients, scaleX, scaleY);
            AdjustControlLayout(chkbDefaultWholesale, scaleX, scaleY);
            AdjustControlLayout(lblWholesalePrice, scaleX, scaleY);
            AdjustControlLayout(richTBIngredientComments, scaleX, scaleY);
            AdjustControlLayout(btnClearIngredientTexts, scaleX, scaleY);
            AdjustControlLayout(lblRetailPrice, scaleX, scaleY);
            AdjustControlLayout(lblUnit, scaleX, scaleY);
            AdjustControlLayout(txtIngredientName, scaleX, scaleY);
            AdjustControlLayout(txtRetailPrice, scaleX, scaleY);
            AdjustControlLayout(txtWholesalePrice, scaleX, scaleY);
            AdjustControlLayout(txtPackQty, scaleX, scaleY);
            AdjustControlLayout(cmbIngredientUnits, scaleX, scaleY);
            AdjustControlLayout(lblBrand, scaleX, scaleY);
            AdjustControlLayout(lblQtyExample, scaleX, scaleY);
            AdjustControlLayout(lblComments, scaleX, scaleY);
            AdjustControlLayout(btnCalculator, scaleX, scaleY);
            AdjustControlLayout(cmbIngredientBrand, scaleX, scaleY);
            AdjustControlLayout(chkbDefaultRetail, scaleX, scaleY);
            AdjustControlLayout(btnEditIngredient, scaleX, scaleY);
            AdjustControlLayout(btnCancelEditIngredient, scaleX, scaleY);
            AdjustControlLayout(lblQtyXPack, scaleX, scaleY);
            AdjustControlLayout(lblStatusCheck, scaleX, scaleY);

            // Ensure minimum spacing between controls
            EnsureMinimumSpacing();
        }

        /// <summary>
        /// Adjusts a control's position and size based on scaling factors
        /// </summary>
        private void AdjustControlLayout(Control control, float scaleX, float scaleY)
        {
            FormUtils.AdjustControlLayout(control, scaleX, scaleY, initialControlBounds);
        }

        private void EnsureMinimumSpacing()
        {
            FormUtils.EnsureMinimumSpacing(lblIngredientName, txtIngredientName, btnSaveIngredient, btnClearIngredientTexts);
        }

        private void btnCancelEditIngredient_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnClearIngredientTexts_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
    }
}

