using Microsoft.Data.SqlClient;

namespace CakePrizeView.Forms.Menu.Products
{
    public partial class ProductUnitForm : Form
    {
        private Form previousForm;
        private SqlConnection sqlConnection;

        public ProductUnitForm(Form previousForm, SqlConnection sqlConnection)
        {
            this.previousForm = previousForm;
            this.sqlConnection = sqlConnection;
            InitializeComponent();
        }
    }
}
