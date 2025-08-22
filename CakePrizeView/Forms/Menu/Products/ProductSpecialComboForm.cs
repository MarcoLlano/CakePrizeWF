using Microsoft.Data.SqlClient;

namespace CakePrizeView.Forms.Menu.Products
{
    public partial class ProductSpecialComboForm : Form
    {
        private Form previousForm;
        private SqlConnection sqlConnection;

        public ProductSpecialComboForm(Form previousForm, SqlConnection sqlConnection)
        {
            this.previousForm = previousForm;
            this.sqlConnection = sqlConnection;
            InitializeComponent();
        }
    }
}
