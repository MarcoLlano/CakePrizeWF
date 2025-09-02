using Microsoft.Data.SqlClient;
using CakePrizeView.Utils;

namespace CakePrizeView.Forms.Menu.Products
{
    public partial class ProductSetForm : Form
    {
        private Form previousForm;
        private SqlConnection sqlConnection;

        public ProductSetForm(Form previousForm, SqlConnection sqlConnection)
        {
            this.previousForm = previousForm;
            this.sqlConnection = sqlConnection;
            InitializeComponent();
            
            // Set up auto-maximize
            FormMaximizeHelper.SetupAutoMaximize(this);
        }
    }
}

