using CakePrize.libs.utils;
using CakePrizeDB.Services;

namespace CakePrizeView.Forms.Popups
{
    public partial class CalculateWholeRetailSalesPrice : Form
    {
        private UnitTypeService UnitTypeService;
        public CalculateWholeRetailSalesPrice()
        {
            UnitTypeService = new UnitTypeService();
            InitializeComponent();
        }

        public string CalculatedWholesalePrice => lblCalculatedWholesalePrice.Text;
        public string CalculatedRetailPrice => lblCalculatedRetailPrice.Text;

        private void btnCalcSelectValues_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cbCalcWholesaleUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCalcWholesaleUnit.Text.Replace(" ", string.Empty) == "ud")
            {
                lblCalculatedWholesalePrice.Text = UnitConvertion.CalculateUnitPrice(float.Parse(txtCalcWholeSaleQty.Text), float.Parse(txtCalcWholeSalePrice.Text)).ToString();
            }
            else
            {
                double convertedWholesale = txtCalcWholeSalePrice.Text != string.Empty && txtCalcWholeSaleQty.Text != string.Empty ?
                    UnitConvertion.ConvertWeightVol(double.Parse(txtCalcWholeSaleQty.Text), cbCalcWholesaleUnit.Text): -1;
                if(convertedWholesale > 0)
                {
                    lblCalculatedWholesalePrice.Text = Math.Round(double.Parse(txtCalcWholeSalePrice.Text) / convertedWholesale * 1000, 2).ToString();
                }
                else
                {
                    lblCalcStatusMessage.ForeColor = Color.Red;
                    lblCalcStatusMessage.Text = $"Datos invalidos, o la unidad de medida '{cbCalcWholesaleUnit.Text}' no es soportada, por favor use otra";
                }
            }
        }

        private void cbCalcRetailUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCalcStatusMessage.Text = string.Empty;
            if (cbCalcWholesaleUnit.Text == "ud")
            {
                lblCalculatedRetailPrice.Text = UnitConvertion.CalculateUnitPrice(float.Parse(txtCalcRetailSaleQty.Text), float.Parse(txtCalcRetailSalePrice.Text)).ToString();
            }
            else
            {
                double convertedRetail = txtCalcRetailSaleQty.Text != string.Empty & txtCalcRetailSalePrice.Text != string.Empty ?
                    UnitConvertion.ConvertWeightVol(double.Parse(txtCalcRetailSaleQty.Text), cbCalcRetailUnit.Text) : -1;
                if (convertedRetail > 0)
                {
                    lblCalculatedRetailPrice.Text = Math.Round(double.Parse(txtCalcRetailSalePrice.Text) / convertedRetail * 1000, 2).ToString();
                }
                else
                    lblCalcStatusMessage.ForeColor = Color.Red;
                    lblCalcStatusMessage.Text = $"Datos invalidos, o la unidad de medida '{cbCalcRetailUnit.Text}' no es soportada, por favor use otra";

            }
        }

        private void btnCalcCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadUnitTypes()
        {
            foreach (var unit in UnitTypeService.GetAllAcronyms())
            {
                cbCalcRetailUnit.Items.Add(unit);
                cbCalcWholesaleUnit.Items.Add(unit);
            }
        }

        private void CalculateWholeRetailSalesPrice_Load(object sender, EventArgs e)
        {
            LoadUnitTypes();
        }
    }
}
