using CakePrizeDB.Services;
using Microsoft.Data.SqlClient;
using CakePrizeView.Utils;
using CakePrizeCore.libs.DBUtils;

namespace CakePrizeView.Forms.ingredients
{
    public partial class BrandForm : Form
    {
        private Form previousForm;
        private BrandService brandService;
        
        // Store initial form size for relative positioning
        private Size initialFormSize;
        private Dictionary<Control, Rectangle> initialControlBounds;

        public BrandForm(Form previousForm, SqlConnection? sqlConnection = null)
        {
            initialControlBounds = new Dictionary<Control, Rectangle>();
            InitializeComponent();
            this.previousForm = previousForm;
            
            // Initialize services after InitializeComponent to ensure proper connection state
            InitializeServices();
            
            // Add resize event handler
            Resize += BrandForm_Resize;
            
            // Store initial positions for relative positioning
            StoreInitialPositions();
            
            // Set up auto-maximize
            FormMaximizeHelper.SetupAutoMaximize(this);
        }

        /// <summary>
        /// Initializes all services with fresh connections
        /// </summary>
        private void InitializeServices()
        {
            try
            {
                // Create fresh connections for each service to ensure they're open and available
                var connection = DatabaseConnectionManager.OpenConnection();
                
                brandService = new BrandService();
            }
            catch (Exception ex)
            {
                // If we can't create services, show error but don't crash the form
                MessageBox.Show($"Failed to initialize database services: {ex.Message}", "Initialization Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmBrands_Load(object sender, EventArgs e)
        {
            GetAllBrands(sender, e);
        }

        private void btnBackIngredientForm_Click(object sender, EventArgs e)
        {
            Close();
            previousForm.Show();
        }

        private void btnCloseIngredientForm_Click(object sender, EventArgs e)
        {
            Close();
            previousForm.Close();
        }

        private void btnSaveBrand_Click(object sender, EventArgs e)
        {
            string temp = txtBrandName.Text;
            var brand = brandService.CreateBrand(txtBrandName.Text, richTBBrandComments.Text, "Marco Llano", "Marco Llano");
            txtBrandName.Text = string.Empty;
            richTBBrandComments.Text = string.Empty;
            lblSaveStatus.Text = $"La marca {temp} se registro correctamente!";
        }

        private void GetAllBrands(object sender, EventArgs e)
        {
            TSCmbBrandList.Items.Clear();
            foreach (var item in brandService.GetAllBrands())
            {
                TSCmbBrandList.Items.Add($"{item.Name}");
            }
        }

        private void ClearSaveStatusLabel(object sender, EventArgs e)
        {
            lblSaveStatus.Text = string.Empty; 
        }

        /// <summary>
        /// Stores initial positions and sizes of controls for relative positioning
        /// </summary>
        private void StoreInitialPositions()
        {
            initialFormSize = this.Size;
            initialControlBounds = new Dictionary<Control, Rectangle>();
            
            // Store initial bounds for all controls that need responsive positioning
            StoreControlBounds(panel1);
            StoreControlBounds(LblIngredientTitle);
            StoreControlBounds(btnClose);
            StoreControlBounds(btnBack);
            StoreControlBounds(richTBBrandComments);
            StoreControlBounds(btnSaveBrand);
            StoreControlBounds(btnClearBrandTexts);
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
        private void BrandForm_Resize(object? sender, EventArgs e)
        {
            if (initialControlBounds == null || initialFormSize.Width == 0 || initialFormSize.Height == 0)
                return;

            // Calculate scaling factors
            float scaleX = (float)this.Width / initialFormSize.Width;
            float scaleY = (float)this.Height / initialFormSize.Height;

            // Adjust control positions and sizes
            AdjustControlLayout(panel1, scaleX, scaleY);
            AdjustControlLayout(LblIngredientTitle, scaleX, scaleY);
            AdjustControlLayout(btnClose, scaleX, scaleY);
            AdjustControlLayout(btnBack, scaleX, scaleY);
            AdjustControlLayout(richTBBrandComments, scaleX, scaleY);
            AdjustControlLayout(btnSaveBrand, scaleX, scaleY);
            AdjustControlLayout(btnClearBrandTexts, scaleX, scaleY);
            
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
            FormUtils.EnsureMinimumSpacing(btnBack, btnClose, btnSaveBrand, btnClearBrandTexts);
        }
    }
}

