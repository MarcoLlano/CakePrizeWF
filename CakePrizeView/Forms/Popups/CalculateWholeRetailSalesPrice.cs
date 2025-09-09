using CakePrize.libs.utils;
using CakePrizeDB.Services;
using CakePrizeView.Forms.ingredients;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            double convertedWholesale = UnitConvertion.ConvertWeightToGram(double.Parse(txtCalcWholeSaleQty.Text), cbCalcWholesaleUnit.Text);
            lblCalculatedWholesalePrice.Text = Math.Round(double.Parse(txtCalcWholeSalePrice.Text) / convertedWholesale * 1000, 2).ToString();
        }

        private void cbCalcRetailUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            double convertedRetail = UnitConvertion.ConvertWeightToGram(double.Parse(txtCalcRetailSaleQty.Text), cbCalcRetailUnit.Text);
            lblCalculatedRetailPrice.Text = Math.Round(double.Parse(txtCalcRetailSalePrice.Text) / convertedRetail * 1000, 2).ToString();
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
