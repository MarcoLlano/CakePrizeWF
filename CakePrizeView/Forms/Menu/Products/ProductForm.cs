using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using CakePrizeView.Utils;

namespace CakePrizeView.Forms.Menu.Products
{
    public partial class ProductForm : Form
    {
        private Form previousForm;
        private SqlConnection sqlConnection;

        // Store initial form size for relative positioning
        private Size initialFormSize;
        private Dictionary<Control, Rectangle> initialControlBounds;

        public ProductForm(Form previousForm, SqlConnection sqlConnection)
        {
            this.previousForm = previousForm;
            this.sqlConnection = sqlConnection;
            InitializeComponent();

            // Add resize event handler
            this.Resize += ProductForm_Resize;

            // Store initial positions for relative positioning
            StoreInitialPositions();

            // Set up auto-maximize
            FormMaximizeHelper.SetupAutoMaximize(this);
        }

        private void FrmProduct_Load(object sender, EventArgs e)
        {
            //GetAllBrands(sender, e);
        }

        private void btnBackProductUnitForm_Click(object sender, EventArgs e)
        {
            Close();
            previousForm.Show();
        }

        private void btnCloseProductUnitForm_Click(object sender, EventArgs e)
        {
            Close();
            previousForm.Close();
        }

        private void btnProdAddIngredientToList_Click(object sender, EventArgs e)
        {
            int rowIndex = gvProductIngredientList.Rows.Add();
            gvProductIngredientList.Rows[rowIndex].Cells[0].Value = cbProductIngredient.Text;
            gvProductIngredientList.Rows[rowIndex].Cells[1].Value = txtProductIngrQty.Text;
        }

        private void txtProductIngredientImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult dialogResult = ofd.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {

                //TODO: Update this
                txtProductImage.Text = ofd.FileName;
                string filePath = ofd.Filter;
                int filePath1 = ofd.FilterIndex;
                string filePath21 = ofd.SafeFileName;
                string filePath2s1 = ofd.Title;
            }
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
            StoreControlBounds(LblProductTitle);
            StoreControlBounds(btnCloseProduct);
            StoreControlBounds(btnBackProduct);
            StoreControlBounds(gvProductIngredientList);
            StoreControlBounds(richTBProdComments);
            StoreControlBounds(btnSaveProduct);
            StoreControlBounds(btnClearProductTexts);
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
        private void ProductForm_Resize(object sender, EventArgs e)
        {
            if (initialControlBounds == null || initialFormSize.Width == 0 || initialFormSize.Height == 0)
                return;

            // Calculate scaling factors
            float scaleX = (float)this.Width / initialFormSize.Width;
            float scaleY = (float)this.Height / initialFormSize.Height;

            // Adjust control positions and sizes
            AdjustControlLayout(lblPortionsPerPrep, scaleX, scaleY);
            AdjustControlLayout(lblProdComments, scaleX, scaleY);
            AdjustControlLayout(lblProdIngreQty, scaleX, scaleY);
            AdjustControlLayout(lblProductImage, scaleX, scaleY);
            AdjustControlLayout(lblProductIngredient, scaleX, scaleY);
            AdjustControlLayout(lblProductName, scaleX, scaleY);
            AdjustControlLayout(lblProductSize, scaleX, scaleY);
            AdjustControlLayout(lblProductType, scaleX, scaleY);
            AdjustControlLayout(txtProductImage, scaleX, scaleY);
            AdjustControlLayout(txtProductIngrQty, scaleX, scaleY);
            AdjustControlLayout(txtProductName, scaleX, scaleY);
            AdjustControlLayout(txtProductPortionsPerPrep, scaleX, scaleY);
            AdjustControlLayout(cbProductIngredient, scaleX, scaleY);
            AdjustControlLayout(cbProductSize, scaleX, scaleY);
            AdjustControlLayout(cbProductType, scaleX, scaleY);
            AdjustControlLayout(btnProdAddIngredientToList, scaleX, scaleY);
            AdjustControlLayout(panel1, scaleX, scaleY);
            AdjustControlLayout(LblProductTitle, scaleX, scaleY);
            AdjustControlLayout(btnCloseProduct, scaleX, scaleY);
            AdjustControlLayout(btnBackProduct, scaleX, scaleY);
            AdjustControlLayout(gvProductIngredientList, scaleX, scaleY);
            AdjustControlLayout(richTBProdComments, scaleX, scaleY);
            AdjustControlLayout(btnSaveProduct, scaleX, scaleY);
            AdjustControlLayout(btnClearProductTexts, scaleX, scaleY);

            // Ensure minimum spacing between controls
            EnsureMinimumSpacing();

            // Update DataGridView column widths proportionally
            UpdateDataGridViewColumns();
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
            FormUtils.EnsureMinimumSpacing(btnBackProduct, btnCloseProduct, richTBProdComments, btnSaveProduct);
        }

        /// <summary>
        /// Updates DataGridView column widths proportionally
        /// </summary>
        private void UpdateDataGridViewColumns()
        {
            if (gvProductIngredientList != null && gvProductIngredientList.Columns.Count > 0)
            {
                int totalWidth = gvProductIngredientList.Width - 20; // Account for scrollbar

                // Set proportional widths (60% for ingredient, 40% for quantity)
                if (gvProductIngredientList.Columns.Count >= 2)
                {
                    gvProductIngredientList.Columns[0].Width = (int)(totalWidth * 0.6); // Ingredient column
                    gvProductIngredientList.Columns[1].Width = (int)(totalWidth * 0.4); // Quantity column
                }
            }
        }

        private void btnSaveProduct_Click(object sender, EventArgs e)
        {

        }
    }
}
