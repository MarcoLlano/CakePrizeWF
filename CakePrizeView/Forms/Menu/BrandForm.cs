using CakePrizeDB.Services;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using CakePrizeView.Utils;

namespace CakePrizeView.Forms.ingredients
{
    public partial class BrandForm : Form
    {
        private Form previousForm;
        private BrandService brandService;
        
        // Store initial form size for relative positioning
        private Size initialFormSize;
        private Dictionary<Control, Rectangle> initialControlBounds;

        public BrandForm(Form previousForm, SqlConnection sqlConnection)
        {
            InitializeComponent();
            this.previousForm = previousForm;
            brandService = new BrandService(sqlConnection);
            
            // Add resize event handler
            this.Resize += BrandForm_Resize;
            
            // Store initial positions for relative positioning
            StoreInitialPositions();
            
            // Set up auto-maximize
            FormMaximizeHelper.SetupAutoMaximize(this);
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
            if (control != null)
            {
                initialControlBounds[control] = control.Bounds;
                
                // Store bounds for child controls
                foreach (Control child in control.Controls)
                {
                    StoreControlBounds(child);
                }
            }
        }

        /// <summary>
        /// Handles form resize to adjust control positions and sizes
        /// </summary>
        private void BrandForm_Resize(object sender, EventArgs e)
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
            if (control != null && initialControlBounds.ContainsKey(control))
            {
                Rectangle initialBounds = initialControlBounds[control];
                
                // Calculate new position and size
                int newX = (int)(initialBounds.X * scaleX);
                int newY = (int)(initialBounds.Y * scaleY);
                int newWidth = (int)(initialBounds.Width * scaleX);
                int newHeight = (int)(initialBounds.Height * scaleY);
                
                // Apply new bounds
                control.Bounds = new Rectangle(newX, newY, newWidth, newHeight);
            }
        }

        /// <summary>
        /// Ensures minimum spacing between controls
        /// </summary>
        private void EnsureMinimumSpacing()
        {
            const int minSpacing = 10;
            
            // Ensure minimum spacing between back and close buttons
            if (btnBack.Right + minSpacing > btnClose.Left)
            {
                btnClose.Left = btnBack.Right + minSpacing;
            }
            
            // Ensure minimum spacing between save and clear buttons
            if (btnSaveBrand.Right + minSpacing > btnClearBrandTexts.Left)
            {
                btnClearBrandTexts.Left = btnSaveBrand.Right + minSpacing;
            }
        }
    }
}
