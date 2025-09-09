using CakePrizeCore.libs.DBUtils;
using CakePrizeDB.Models;
using CakePrizeDB.Services;
using CakePrizeView.Utils;
using Microsoft.Data.SqlClient;
using System.Timers;

namespace CakePrizeView.Forms.ingredients
{
    public partial class BrandForm : Form
    {
        private Form previousForm;
        private BrandService brandService;
        private BrandModel brandUpdate;

        // Store initial form size for relative positioning
        private Size initialFormSize;
        private Dictionary<Control, Rectangle> initialControlBounds;

        public BrandForm(Form previousForm, SqlConnection? sqlConnection = null)
        {
            brandService = new BrandService();
            brandUpdate = new BrandModel();
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

        private void GetAllBrands(object sender, EventArgs e)
        {
            TSCmbBrandList.Items.Clear();
            foreach (var item in FormUtils.SortItems(brandService.GetAllBrands(), b => b.Name))
            {
                TSCmbBrandList.Items.Add($"{item}");
            }
        }

        private void FrmBrands_Load(object sender, EventArgs e)
        {
            GetAllBrands(sender, e);
        }

        private void TSCmbBrandList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TSCmbBrandList.Text != string.Empty)
            {
                btnClearBrandTexts.Hide();
                btnSaveBrand.Hide();
                btnEditBrand.Visible = true;
                btnCancelEditBrand.Visible = true;

                brandUpdate = brandService.GetAllBrands()
                        .Where(brandName => brandName != null && brandName.Name == TSCmbBrandList.Text)
                        .Select(brand => brand)
                        .First();

                txtBrandName.Text = brandUpdate.Name;
                richTBBrandComments.Text = brandUpdate.Comments;
            }
        }

        private void ClearFields()
        {
            txtBrandName.Text = string.Empty;
            richTBBrandComments.Text = string.Empty;
            btnCancelEditBrand.Visible = false;
            btnEditBrand.Visible = false;
            btnSaveBrand.Visible = true;
            btnClearBrandTexts.Visible = true;
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
            string newBrandName = txtBrandName.Text;
            var brand = brandService.CreateBrand(newBrandName, richTBBrandComments.Text, UserSession.GetCurrentUsername(), UserSession.GetCurrentUsername());
            GetAllBrands(sender, e);
            TSCmbBrandList.Text = newBrandName;
            ClearFields();
            lblSaveStatus.Text = $"La marca {brand.Name} se registró correctamente!";
        }

        private void btnEditBrand_Click(object sender, EventArgs e)
        {
            string updatedName = txtBrandName.Text;
            DateTime brandModifiedDate = DateTime.Now;
            var brand = brandService.UpdateBrand(brandUpdate.Id, txtBrandName.Text, richTBBrandComments.Text, UserSession.GetCurrentUsername(), brandModifiedDate);
            lblSaveStatus.Text = $"La marca {brand.Name} se actualizó correctamente!";
            GetAllBrands(sender, e);
            TSCmbBrandList.Text = updatedName;
            ClearFields();
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
            StoreControlBounds(LblBrandTitle);
            StoreControlBounds(btnClose);
            StoreControlBounds(btnBack);
            StoreControlBounds(richTBBrandComments);
            StoreControlBounds(btnSaveBrand);
            StoreControlBounds(lblIngredientName);
            StoreControlBounds(txtBrandName);
            StoreControlBounds(lblSaveStatus);
            StoreControlBounds(btnClearBrandTexts);
            StoreControlBounds(btnEditBrand);
            StoreControlBounds(btnCancelEditBrand);
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
            AdjustControlLayout(LblBrandTitle, scaleX, scaleY);
            AdjustControlLayout(btnClose, scaleX, scaleY);
            AdjustControlLayout(btnBack, scaleX, scaleY);
            AdjustControlLayout(lblIngredientName, scaleX, scaleY);
            AdjustControlLayout(txtBrandName, scaleX, scaleY);
            AdjustControlLayout(lblSaveStatus, scaleX, scaleY);
            AdjustControlLayout(richTBBrandComments, scaleX, scaleY);
            AdjustControlLayout(btnSaveBrand, scaleX, scaleY);
            AdjustControlLayout(btnClearBrandTexts, scaleX, scaleY);
            AdjustControlLayout(btnEditBrand, scaleX, scaleY);
            AdjustControlLayout(btnCancelEditBrand, scaleX, scaleY);

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

        private void btnCancelEditAdd_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
    }
}

