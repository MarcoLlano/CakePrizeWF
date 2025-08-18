using CakePrizeDB;

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
            var list = cakePrizeDBQueries.GetUnitType();
            foreach (var item in list)
            {
                cmbUnits.Items.Add(item);
            }
        }
    }
}
