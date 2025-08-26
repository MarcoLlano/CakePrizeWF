using CakePrizeDB.Services;
using CakePrizeView.Utils;
using Microsoft.Data.SqlClient;

namespace CakePrizeView.Forms.Menu.Products
{
    public partial class ProductTypeForm : Form
    {
        private Form previousForm;
        private SqlConnection sqlConnection;
        private ProductTypeService prodTypeService;

        // Store initial form size for relative positioning
        private Size initialFormSize;
        private Dictionary<Control, Rectangle> initialControlBounds;

        public ProductTypeForm(Form previousForm, SqlConnection sqlConnection)
        {
            initialControlBounds = new Dictionary<Control, Rectangle>();
            InitializeComponent();
            this.previousForm = previousForm;
            prodTypeService = new ProductTypeService(sqlConnection);

            // Add resize event handler
            this.Resize += ProductTypeForm_Resize;

            // Store initial positions for relative positioning
            StoreInitialPositions();

            // Set up auto-maximize
            FormMaximizeHelper.SetupAutoMaximize(this);
        }

        /// <summary>
        /// Stores initial positions and sizes of controls for relative positioning
        /// </summary>
        private void StoreInitialPositions()
        {
            initialFormSize = this.Size;
            initialControlBounds = new Dictionary<Control, Rectangle>();

            // Store initial bounds for all controls that need responsive positioning
            StoreControlBounds(pnlProdType);
            StoreControlBounds(btnClearProdType);
            StoreControlBounds(btnSaveProdType);
            StoreControlBounds(btnClose);
            StoreControlBounds(btnBack);
            StoreControlBounds(lblProdTypeName);
            StoreControlBounds(txtProdTypeName);
            StoreControlBounds(LblProdTypeTitle);
            StoreControlBounds(lblSaveProdTypeStatus);
        }

        /// <summary>
        /// Recursively stores bounds for a control and its children
        /// </summary>
        private void StoreControlBounds(Control control)
        {
            FormUtils.StoreControlBounds(control, initialControlBounds);
        }

        /// <summary>
        /// Handles form resize to adjust control positions and sizes
        /// </summary>
        private void ProductTypeForm_Resize(object? sender, EventArgs e)
        {
            if (initialControlBounds == null || initialFormSize.Width == 0 || initialFormSize.Height == 0)
                return;

            // Calculate scaling factors
            float scaleX = (float)this.Width / initialFormSize.Width;
            float scaleY = (float)this.Height / initialFormSize.Height;

            // Adjust control positions and sizes
            AdjustControlLayout(pnlProdType, scaleX, scaleY);
            AdjustControlLayout(btnClearProdType, scaleX, scaleY);
            AdjustControlLayout(btnSaveProdType, scaleX, scaleY);
            AdjustControlLayout(btnClose, scaleX, scaleY);
            AdjustControlLayout(btnBack, scaleX, scaleY);
            AdjustControlLayout(lblProdTypeName, scaleX, scaleY);
            AdjustControlLayout(txtProdTypeName, scaleX, scaleY);
            AdjustControlLayout(LblProdTypeTitle, scaleX, scaleY);
            AdjustControlLayout(lblSaveProdTypeStatus, scaleX, scaleY);

            // Ensure minimum spacing between controls
            EnsureMinimumSpacing();
        }

        /// <summary>
        /// Adjusts a control's position and size based on scaling factors
        /// </summary>
        private void AdjustControlLayout(Control control, float scaleX, float scaleY)
        {
            FormUtils.AdjustControlLayout(control, scaleX, scaleY, initialControlBounds);
        }

        /// <summary>
        /// Ensures minimum spacing between controls
        /// </summary>
        private void EnsureMinimumSpacing()
        {
            FormUtils.EnsureMinimumSpacing(btnBack, btnClose, btnSaveProdType, btnClearProdType);
        }

        private void GetAllProductTypes(object sender, EventArgs e)
        {
            TSCbProdTypeList.Items.Clear();
            foreach (var item in prodTypeService.GetAllProductTypes())
            {
                TSCbProdTypeList.Items.Add($"{item.Name}");
            }
        }

        private void FrmProdType_Load(object sender, EventArgs e)
        {
            GetAllProductTypes(sender, e);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
            previousForm.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
            previousForm.Close();
        }

        private void btnSaveProdType_Click(object sender, EventArgs e)
        {
            string temp = txtProdTypeName.Text;
            var brand = prodTypeService.CreateProductType(txtProdTypeName.Text, "Marco Llano", "Marco Llano");
            txtProdTypeName.Text = string.Empty;
            lblSaveProdTypeStatus.Text = $"El producto {temp} se registro correctamente!";
        }
    }
}
