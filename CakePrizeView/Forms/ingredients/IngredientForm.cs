using CakePrizeCore.libs.DBUtils;
using CakePrizeDB;
using CakePrizeDB.Services;

namespace CakePrizeView.Forms.ingredients
{
    public partial class IngredientForm: Form
    {
        private Form previousForm;
        private UnitTypeService unitTypeService;
        private IngredientService ingredientService;

        public IngredientForm(Form previousForm)
        {
            InitializeComponent();
            this.previousForm = previousForm;
            unitTypeService = new UnitTypeService(DBUtils.OpenDBConnection());
            ingredientService = new IngredientService(DBUtils.OpenDBConnection());
        }

        private void FrmIngredients_Load(object sender, EventArgs e)
        {
            List<CakePrizeDB.Models.UnitTypeModel> t = unitTypeService.GetAllUnitTypes();
            foreach (var item in unitTypeService.GetAllUnitTypes())
            {
                cmbUnits.Items.Add($"{item.Name}-{item.Acronym}");
            }
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
            List<CakePrizeDB.Models.IngredientModel>  test = ingredientService.GetAllIngredients();
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
            CheckBox currentCheckBox = sender as CheckBox;
            if (currentCheckBox != null && currentCheckBox.Checked)
            {
                // Iterate through all other checkboxes in the group (e.g., on the same panel or form)
                foreach (Control control in this.Controls) // Or a specific container like a Panel
                {
                    if (control is CheckBox otherCheckBox && otherCheckBox != currentCheckBox)
                    {
                        // Uncheck the other checkboxes
                        currentCheckBox.Text = "Por defecto.";
                        otherCheckBox.Text = string.Empty;
                        otherCheckBox.Checked = false;
                    }
                }
            }
        }
    }
}
