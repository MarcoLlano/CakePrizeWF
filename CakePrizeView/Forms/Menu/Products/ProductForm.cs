using CakePrizeDB.Models;
using CakePrizeDB.Services;
using CakePrizeView.Utils;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CakePrizeView.Forms.Menu.Products
{
    public partial class ProductForm : Form
    {
        private Form previousForm;
        private ProductTypeService prodTypeService;
        private IngredientService ingredientService;
        private ProductService productService;
        private ProductIngredientService productIngredientService;
        private ProductPhotoService productPhotoService;
        private ProductSizeService productSizeService;
        private string fileName;
        private string fullFileName;
        private Dictionary<Guid, string> ingredientList;

        // Store initial form size for relative positioning
        private Size initialFormSize;
        private Dictionary<Control, Rectangle> initialControlBounds;

        public ProductForm(Form previousForm, SqlConnection sqlConnection)
        {
            this.previousForm = previousForm;
            prodTypeService = new ProductTypeService(sqlConnection);
            ingredientService = new IngredientService(sqlConnection);
            productService = new ProductService(sqlConnection);
            productIngredientService = new ProductIngredientService(sqlConnection);
            productPhotoService = new ProductPhotoService(sqlConnection);
            productSizeService = new ProductSizeService(sqlConnection);
            initialControlBounds = new Dictionary<Control, Rectangle>();
            ingredientList = new Dictionary<Guid, string>();
            fileName = string.Empty;
            fullFileName = string.Empty;
            InitializeComponent();

            // Add resize event handler
            this.Resize += ProductForm_Resize;

            // Store initial positions for relative positioning
            StoreInitialPositions();

            // Set up auto-maximize
            FormMaximizeHelper.SetupAutoMaximize(this);

            // Set up ComboBox functionality
            SetupComboBoxes();
        }

        /// <summary>
        /// Sets up basic ComboBox functionality
        /// </summary>
        private void SetupComboBoxes()
        {
            try
            {
                // Set up ProductType ComboBox
                cbProductType.DropDownStyle = ComboBoxStyle.DropDown;
                cbProductType.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cbProductType.AutoCompleteSource = AutoCompleteSource.ListItems;
                cbProductType.Click += CbProductType_Click;

                // Set up Ingredient ComboBox
                cbProductIngredient.DropDownStyle = ComboBoxStyle.DropDown;
                cbProductIngredient.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cbProductIngredient.AutoCompleteSource = AutoCompleteSource.ListItems;
                cbProductIngredient.Click += CbProductIngredient_Click;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting up ComboBoxes: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Handles click event for ProductType ComboBox
        /// </summary>
        private void CbProductType_Click(object sender, EventArgs e)
        {
            try
            {
                // Show dropdown when clicked
                if (!cbProductType.DroppedDown)
                {
                    cbProductType.DroppedDown = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error with ProductType ComboBox: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Handles click event for Ingredient ComboBox
        /// </summary>
        private void CbProductIngredient_Click(object sender, EventArgs e)
        {
            try
            {
                // Show dropdown when clicked
                if (!cbProductIngredient.DroppedDown)
                {
                    cbProductIngredient.DroppedDown = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error with Ingredient ComboBox: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GetAllProductTypes(object sender, EventArgs e)
        {
            foreach (var item in prodTypeService.GetAllProductTypes())
            {
                cbProductType.Items.Add($"{item.Name}");
            }
        }

        private void GetAllIngredients(object sender, EventArgs e)
        {
            foreach (var item in ingredientService.GetAllIngredients())
            {
                cbProductIngredient.Items.Add($"{item.Name}");
            }
        }

        private void FrmProduct_Load(object sender, EventArgs e)
        {
            GetAllProductTypes(sender, e);
            GetAllIngredients(sender, e);
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
                fullFileName = ofd.FileName;
                fileName = ofd.SafeFileName;
                txtProductImage.Text = fileName;
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
        private void ProductForm_Resize(object? sender, EventArgs e)
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
            FormUtils.EnsureMinimumSpacing(btnBackProduct, btnCloseProduct, btnClearProductTexts, btnSaveProduct);
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
            try
            {
                var newProd = CreateNewProduct(sender, e);
                if (newProd != null)
                {
                    var newProdPhoto = LinkProductAndPhoto(sender, e, newProd.Id);
                    LinkProductAndIngredient(sender, e, newProd.Id);
                    
                    if (newProdPhoto != null)
                    {
                        MessageBox.Show("Product saved successfully with photo!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Product saved successfully, but photo could not be processed.", "Partial Success", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Failed to create product.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private ProductModel CreateNewProduct(object sender, EventArgs e)
        {
            var productTypeId = Guid.Parse(prodTypeService.GetAllProductTypes().Where(d => d.Name == cbProductType.Text)
                .Select(t => t.Id).First().ToString() ?? string.Empty);
            return productService.CreateProduct(productTypeId, txtProductName.Text + cbProductSize, "Marco Llano", "Marco Llano");
        }

        /// <summary>
        /// Links a product with its ingredients by iterating through the DataGridView rows
        /// </summary>
        private void LinkProductAndIngredient(object sender, EventArgs e, Guid productId)
        {
            try
            {
                // Iterate through all rows in the DataGridView
                for (int rowIndex = 0; rowIndex < gvProductIngredientList.Rows.Count; rowIndex++)
                {
                    // Get the ingredient name from the first column (index 0)
                    string ingredientName = gvProductIngredientList.Rows[rowIndex].Cells[0].Value?.ToString();
                    
                    // Get the quantity from the second column (index 1)
                    string quantityText = gvProductIngredientList.Rows[rowIndex].Cells[1].Value?.ToString();
                    
                    if (!string.IsNullOrEmpty(ingredientName) && !string.IsNullOrEmpty(quantityText))
                    {
                        // Find the ingredient ID by name using the local ingredientList dictionary
                        var ingredientId = ingredientService.GetAllIngredients()
                            .Where(d => d.Name == ingredientName)
                            .Select(t => t.Id)
                            .FirstOrDefault();
                        
                        if (ingredientId != Guid.Empty)
                        {
                            // Parse the quantity
                            if (float.TryParse(quantityText, out float quantity))
                            {
                                // Create the product-ingredient relationship
                                productIngredientService.CreateProductIngredient(
                                    productId,
                                    ingredientId,
                                    quantity,
                                    "Marco Llano",
                                    "Marco Llano");
                            }
                            else
                            {
                                MessageBox.Show($"Invalid quantity format for ingredient '{ingredientName}': {quantityText}", 
                                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Ingredient '{ingredientName}' not found in the database.", 
                                "Ingredient Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error linking product and ingredients: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private ProductPhotoModel? LinkProductAndPhoto(object sender, EventArgs e, Guid productId)
        {
            try
            {
                if (string.IsNullOrEmpty(fullFileName) || !File.Exists(fullFileName))
                {
                    MessageBox.Show("No valid image file selected.", "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }

                Image image = Image.FromFile(fullFileName);
                return productPhotoService.CreateProductPhoto(
                    productId,
                    txtProductName.Text.Replace(" ", string.Empty),
                    fullFileName,
                    image,
                    "Marco Llano",
                    "Marco Llano");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing image: {ex.Message}", "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private ProductSizeModel LinkProductAndSize(object sender, EventArgs e)
        {
            return productSizeService.CreateProductSize(
                Guid.Parse(productService.GetAllProducts().Where(d => d.Name == txtProductName.Text)
                .Select(t => t.Id).First().ToString() ?? string.Empty),
                txtProductPortionsPerPrep.Text,
                cbProductSize.Text,
                richTBProdComments.Text,
                "Marco Llano",
                "Marco Llano");
        }

        private void ClearFields(object sender, EventArgs e)
        {

        }
    }
}
