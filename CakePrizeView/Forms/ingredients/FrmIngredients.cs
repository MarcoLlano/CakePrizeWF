using CakePrizeDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CakePrizeView.Forms.ingredients
{
    public partial class FrmIngredients: Form
    {
        private Form previousForm;

        public FrmIngredients(Form previousForm)
        {
            InitializeComponent();
            this.previousForm = previousForm;
        }

        private void FrmIngredients_Load(object sender, EventArgs e)
        {
            CakePrizeDBQueries cakePrizeDBQueries = new CakePrizeDBQueries();
            var con = cakePrizeDBQueries.StartConnection();
            cakePrizeDBQueries.GetUnitType(con);
        }
    }
}
