using Microsoft.Data.SqlClient;

namespace CakePrizeView.Forms.Menu.Products
{
    public partial class ProductPromosForm : Form
    {
        private Form previousForm;
        private SqlConnection sqlConnection;

        public ProductPromosForm(Form previousForm, SqlConnection sqlConnection)
        {
            this.previousForm = previousForm;
            this.sqlConnection = sqlConnection;
            InitializeComponent();
        }
    }
}
