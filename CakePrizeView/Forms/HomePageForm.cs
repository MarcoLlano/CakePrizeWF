using CakePrizeView.forms;
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

        private void BtnIngredients_Click(object sender, EventArgs e)
        {
            Hide();
            IngredientForm formCupcake = new IngredientForm(FindForm(), sqlConnection);
            formCupcake.Show();
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
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }

        private void btnBrands_Click(object sender, EventArgs e)
        {
            Hide();
            BrandForm formCupcake = new BrandForm(FindForm(), sqlConnection);
            formCupcake.Show();
        }
    }
}
