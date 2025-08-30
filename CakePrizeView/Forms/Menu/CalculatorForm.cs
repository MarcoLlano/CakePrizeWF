using CakePrizeDB.Services;
using CakePrizeView.Utils;
using CakePrizeCore.libs.DBUtils;
using CakePrizeCore.libs.Configuration;
using Microsoft.Data.SqlClient;

namespace CakePrizeView
{
    public partial class CalculatorForm : BaseForm
    {
        private Form previousForm;
        private ProductService productService;
        private ProductIngredientService productIngredientService;
        private IngredientService ingredientService;
        private UnitTypeService unitTypeService;
        private ProductPhotoService productPhotoService;
        private LogsService logsService;



        // Store initial form size for relative positioning
        private Size initialFormSize;
        private Dictionary<Control, Rectangle> initialControlBounds;

        public CalculatorForm(Form previousForm, SqlConnection? sqlConnection = null)
        {
            initialControlBounds = new Dictionary<Control, Rectangle>();

            InitializeComponent();
            this.previousForm = previousForm;
            
            // Initialize services after InitializeComponent to ensure proper connection state
            InitializeServices();

            
            // Add resize event handler
            this.Resize += CalculatorForm_Resize;
            
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
                // Clear connection cache and reset environment to force re-reading
                EnvironmentConfig.Reset();
                DatabaseConnectionManager.ClearCache();
                
                // Create services using centralized connection management
                productService = new ProductService();
                productIngredientService = new ProductIngredientService();
                ingredientService = new IngredientService();
                unitTypeService = new UnitTypeService();
                productPhotoService = new ProductPhotoService();
                logsService = new LogsService();
                
                // Log successful initialization
                logsService.CreateLog("CalculatorForm services initialized successfully", "Info", "System");
            }
            catch (Exception ex)
            {
                // If we can't create services, show error but don't crash the form
                MessageBox.Show($"Failed to initialize database services: {ex.Message}", "Initialization Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // Create a minimal logs service for error reporting
                try
                {
                    logsService = new LogsService();
                    logsService.CreateLog($"Failed to initialize CalculatorForm services: {ex.Message}", "Error", "System");
                }
                catch
                {
                    // If even logging fails, just continue without services
                }
            }
        }

        private void UpdateTotalLabel()
        {
            /*var flour = Convert.ToInt32(TxtFlour.Text);
            var milk = Convert.ToInt32(TxtFlour.Text);
            var total = 0.0;
            if (rbtnMinFlour.Checked)
            {
                total += PrizeCalculation.GetMinFlourCostProfit(flour);
            }
            if (rbtnMaxFlour.Checked)
            {
                total += PrizeCalculation.GetMaxFlourCostProfit(flour);
            }
            if (rbtnMaxMilk.Checked)
            {
                total += PrizeCalculation.GetMaxMilkCostProfit(flour);
            }
            if (rbtnMinMilk.Checked)
            {
                total += PrizeCalculation.GetMinMilkCostProfit(flour);
            }
            lblTotalAmount.Text = total.ToString();*/
        }

        private void CreateIngredientRows(List<Guid> ingredientsAmount)
        {
            int lblIngredientNamePosX = 0;
            int lblIngredientNamePosY = 0;
            int lblIngredientNameSizeX = 99;
            int lblIngredientNameSizeY = 19;

            int lblUnitAcronymPosX = 299;
            int lblUnitAcronymPosY = 0;
            int lblUnitAcronymSizeX = 5;
            int lblUnitAcronymSizeY = 19;

            int txtIngredientPosX = 110;
            int txtIngredientPosY = 0;
            int txtIngredientSizeX = 130;
            int txtIngredientSizeY = 19;


            int ingredientRetailPosX = 350;
            int ingredientRetailPosY = 0;
            int ingredientRetailSizeX = 94;
            int ingredientRetailSizeY = 19;

            int ingredientWholesalePosX = 450;
            int ingredientWholesalePosY = 0;
            int ingredientWholesaleSizeX = 94;
            int ingredientWholesaleSizeY = 19;

            int ingredientPanelRowPosX = -2;
            int ingredientPanelRowPosY = 0;
            int ingredientPanelRowSizeX = 1168;
            int ingredientPanelRowSizeY = 1191;
            int incrementPanelRowY = 31;

            TextBox TxtIngredientAmount;
            Label LblingredientName;
            Label LblunitPrefix;
            RadioButton RbtnRetailPrize;
            RadioButton RbtnWholesalePrize;

            int numRow = 0;

            foreach (var ingrId in ingredientsAmount)
            {
                var ingredient = ingredientService.GetIngredientById(ingrId);
                var dfaultSelectedRetail = ingredient.DefaultPrice == "Retail";
                var dfaultSelectedWhole = ingredient.DefaultPrice == "Wholesale";
                var unitType = unitTypeService.GetUnitTypeById(ingredient.UnitTypeId);

                LblingredientName = FormUtils.CreateLabel($"{ingredient.Name}", numRow, lblIngredientNamePosX, lblIngredientNamePosY,
                    lblIngredientNameSizeX, lblIngredientNameSizeY);

                LblunitPrefix = FormUtils.CreateLabel($"{unitType.Acronym}", numRow, lblUnitAcronymPosX, lblUnitAcronymPosY,
                    lblUnitAcronymSizeX, lblUnitAcronymSizeY);

                TxtIngredientAmount = FormUtils.CreateTextBox(ingredient.Name, numRow, txtIngredientPosX, txtIngredientPosY,
                    txtIngredientSizeX, txtIngredientSizeY);

                RbtnRetailPrize = FormUtils.CreateRadioButton("Menor", numRow, ingredientRetailPosX, ingredientRetailPosY,
                    ingredientRetailSizeX, ingredientRetailSizeY, dfaultSelectedRetail);

                RbtnWholesalePrize = FormUtils.CreateRadioButton("Mayor", numRow, ingredientWholesalePosX, ingredientWholesalePosY,
                    ingredientWholesaleSizeX, ingredientWholesaleSizeY, dfaultSelectedWhole);

                PnlIngredients.Controls.Add(
                    FormUtils.CreateIngredientPanelRow(numRow, LblingredientName, TxtIngredientAmount, LblunitPrefix, RbtnRetailPrize,
                    RbtnWholesalePrize, ingredientPanelRowPosX, ingredientPanelRowPosY, ingredientPanelRowSizeX, ingredientPanelRowSizeY));

                ingredientPanelRowPosY += incrementPanelRowY;
                numRow++;
                FormUtils.EnsureMinimumSpacing(LblingredientName, TxtIngredientAmount, LblunitPrefix, RbtnWholesalePrize);
                float scaleX = (float)this.Width / initialFormSize.Width;
                float scaleY = (float)this.Height / initialFormSize.Height;
                StoreControlBounds(TxtIngredientAmount);
                AdjustControlLayout(PnlIngredients, scaleX, scaleY);
                /*AdjustControlLayout(LblunitPrefix, scaleX, scaleY);
                AdjustControlLayout(TxtIngredientAmount, scaleX, scaleY);
                AdjustControlLayout(RbtnRetailPrize, scaleX, scaleY);
                AdjustControlLayout(RbtnWholesalePrize, scaleX, scaleY);*/
            }
        }

        private void FrmCupcake_Load(object sender, EventArgs e)
        {
            RefreshProductList();
        }

        /// <summary>
        /// Refreshes the product list from the current environment database
        /// This ensures the TSCmbProductList always shows data from the correct environment
        /// </summary>
        private void RefreshProductList()
        {
            try
            {
                // Clear connection cache and reset environment to force re-reading
                EnvironmentConfig.Reset();
                DatabaseConnectionManager.ClearCache();
                
                TSCmbProductList.Items.Clear();
                var products = productService.GetAllProducts();
                
                foreach (var item in products)
                {
                    TSCmbProductList.Items.Add(item.Name);
                }
                
                // Log how many products were loaded
                logsService.CreateLog($"Loaded {products.Count()} products", "Info", "System");
            }
            catch (Exception ex)
            {
                logsService.CreateLog($"Failed to refresh product list: {ex.Message}", "Error", "System");
                MessageBox.Show($"Failed to load products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Public method to refresh the product list when environment changes
        /// This can be called from parent forms or other components
        /// </summary>
        public void RefreshProductListFromCurrentEnvironment()
        {
            RefreshProductList();
        }

        /// <summary>
        /// Forces a complete refresh of all services and product list with fresh connections
        /// This should be called when environment changes to ensure all data comes from the correct database
        /// </summary>
        public void ForceRefreshAllWithFreshConnections()
        {
            try
            {
                // Dispose existing services to free up connections
                productService = null;
                productIngredientService = null;
                ingredientService = null;
                unitTypeService = null;
                productPhotoService = null;
                logsService = null;

                // Force environment to Testing and clear all caches
                EnvironmentConfig.ForceEnvironment(EnvironmentConfig.Environment.Testing);
                DatabaseConnectionManager.ClearCache();

                // Reinitialize all services with fresh connections
                InitializeServices();

                // Refresh the product list
                RefreshProductList();

                MessageBox.Show("All services and product list refreshed with fresh connections from current environment.", 
                    "Refresh Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to refresh services: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Forces a refresh of the product list with a completely fresh connection
        /// This ensures we're using the current environment configuration
        /// </summary>
        public void ForceRefreshProductListWithFreshConnection()
        {
            try
            {
                // Create a fresh service to ensure we're using current environment
                var freshProductService = new ProductService();
                
                // Get connection info for debugging
                var connectionInfo = DatabaseConnectionManager.GetConnectionInfo();
                var currentEnvironment = EnvironmentConfig.CurrentEnvironment;
                
                // Log the current environment and connection details
                logsService.CreateLog(
                    $"Force refreshing product list with fresh connection - Environment: {currentEnvironment}, " +
                    $"Database: {connectionInfo.Database}, Server: {connectionInfo.Server}", 
                    "Info", "System");
                
                TSCmbProductList.Items.Clear();
                var products = freshProductService.GetAllProducts();
                
                foreach (var item in products)
                {
                    TSCmbProductList.Items.Add(item.Name);
                }
                
                // Log how many products were loaded
                logsService.CreateLog(
                    $"Force refresh loaded {products.Count()} products from {connectionInfo.Database} database", 
                    "Info", "System");
            }
            catch (Exception ex)
            {
                logsService.CreateLog($"Failed to force refresh product list: {ex.Message}", "Error", "System");
                MessageBox.Show($"Failed to force refresh products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PnlIngredients_Click(object sender, EventArgs e)
        {
            UpdateTotalLabel();
        }

        private void rbtnMin_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTotalLabel();
        }

        private void rgbtnMilk_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTotalLabel();
        }

        private void LoadImage(object sender, EventArgs e, Guid productId)
        {
            try
            {
                var photo = productPhotoService.GetProductPhotoByProductId(productId);
                if (photo != null)
                {
                    imgProduct.Image = photo != null ? photo.Image : Properties.Resources.PhotoNotFound;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CmbCupcakeList_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                PnlIngredients.Controls.Clear();
                var productId = productService.GetAllProducts()
                    .Where(product => product.Name == TSCmbProductList.Text)
                    .Select(item => item.Id).First();
                var test = productIngredientService.GetProductIngredientByProductId(productId);

                var prodIngredients = productIngredientService.GetProductIngredientByProductId(productId)
                    .Where(prodId => prodId.ProductId == productId).Select(item => item.IngredientId).ToList();

                CreateIngredientRows(prodIngredients);
                UpdateTotalLabel();

                LblFormTitle.Text = TSCmbProductList.Text;
                LoadImage(sender, e, productId);
            }
            catch (Exception ex)
            {
                logsService.CreateLog(ex.Message, "Error", "Marco Llano");
                throw;
            }
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



        private void TSMenuItem_Click(object sender, EventArgs e)
        {
            // Refresh the product list to ensure it's using the current environment
            RefreshProductList();
            TSLblSelectedCake.Text = sender.ToString();
        }

        /// <summary>
        /// Stores initial positions and sizes of controls for relative positioning
        /// </summary>
        private void StoreInitialPositions()
        {
            initialFormSize = this.Size;
            initialControlBounds = new Dictionary<Control, Rectangle>();
            
            // Store initial bounds for all controls that need responsive positioning
            StoreControlBounds(imgProduct);
            StoreControlBounds(LblFormTitle);
            StoreControlBounds(BtnBack);
            StoreControlBounds(BtnClose);
            StoreControlBounds(PnlIngredients);
            StoreControlBounds(pnlTotalPrices);
            StoreControlBounds(LblSelectPriceTitle);
            StoreControlBounds(lblIngredientsTitle);
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
        private void CalculatorForm_Resize(object? sender, EventArgs e)
        {
            if (initialControlBounds == null || initialFormSize.Width == 0 || initialFormSize.Height == 0)
                return;

            // Calculate scaling factors
            float scaleX = (float)this.Width / initialFormSize.Width;
            float scaleY = (float)this.Height / initialFormSize.Height;

            // Adjust control positions and sizes
            AdjustControlLayout(imgProduct, scaleX, scaleY);
            AdjustControlLayout(LblFormTitle, scaleX, scaleY);
            AdjustControlLayout(BtnBack, scaleX, scaleY);
            AdjustControlLayout(BtnClose, scaleX, scaleY);
            AdjustControlLayout(PnlIngredients, scaleX, scaleY);
            AdjustControlLayout(pnlTotalPrices, scaleX, scaleY);
            AdjustControlLayout(LblSelectPriceTitle, scaleX, scaleY);
            AdjustControlLayout(lblIngredientsTitle, scaleX, scaleY);
            
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
            
            /*// Ensure minimum spacing between back and close buttons
            if (BtnBack.Right + minSpacing > BtnClose.Left)
            {
                BtnClose.Left = BtnBack.Right + minSpacing;
            }*/

            // Ensure minimum spacing between picture and ingredients panel
            if (imgProduct.Right + minSpacing > PnlIngredients.Left)
            {
                PnlIngredients.Left = imgProduct.Right + minSpacing;
            }            
        }
    }

}

