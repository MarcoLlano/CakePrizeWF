using CakePrize.libs.utils;
using CakePrizeCore.libs.Configuration;
using CakePrizeCore.libs.DBUtils;
using CakePrizeCore.libs.utils;
using CakePrizeDB.Models;
using CakePrizeDB.Services;
using CakePrizeView.Utils;
using Microsoft.Data.SqlClient;
using static CakePrizeDB.Constants.DatabaseQueries;

namespace CakePrizeView
{
    public partial class CalculatorForm : BaseForm
    {
        private Form previousForm;
        private ProductService productService;
        private ProductIngredientService productIngredientService;
        private IngredientService ingredientService;
        private BrandService brandService;
        private UnitTypeService unitTypeService;
        private ProductPhotoService productPhotoService;
        private LogsService logsService;
        private bool isUpdatingGrid;
        private bool isRepopulating;
        private Guid? lastSelectedProductId;



        // Store initial form size for relative positioning
        private Size initialFormSize;
        private Dictionary<Control, Rectangle> initialControlBounds;

        public CalculatorForm(Form previousForm, SqlConnection? sqlConnection = null)
        {
            initialControlBounds = new Dictionary<Control, Rectangle>();
            brandService = new BrandService();

            InitializeComponent();
            this.previousForm = previousForm;
            isUpdatingGrid = false;
            isRepopulating = false;
            lastSelectedProductId = null;

            // Initialize services after InitializeComponent to ensure proper connection state
            InitializeServices();

            AttachGridEvents();
            gvIngredientsInfo.ReadOnly = false;
            gvIngredientsInfo.StandardTab = true;
            gvIngredientsInfo.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            gvIngredientsInfo.AllowUserToAddRows = false;
            if (gvIngredientsInfo.Columns.Count > 5)
            {
                gvIngredientsInfo.Columns[5].ReadOnly = false;
                gvIngredientsInfo.Columns[5].ValueType = typeof(bool);
                if (gvIngredientsInfo.Columns[5] is DataGridViewCheckBoxColumn chkCol)
                {
                    chkCol.ThreeState = false;
                    chkCol.TrueValue = true;
                    chkCol.FalseValue = false;
                }
            }

            // Add resize event handler
            this.Resize += CalculatorForm_Resize;

            // Store initial positions for relative positioning
            StoreInitialPositions();

            // Set up auto-maximize
            FormMaximizeHelper.SetupAutoMaximize(this);
        }

        private void AttachGridEvents()
        {
            gvIngredientsInfo.CurrentCellDirtyStateChanged += gvIngredientsInfo_CurrentCellDirtyStateChanged;
            gvIngredientsInfo.CellValueChanged += gvIngredientsInfo_CellValueChanged;
        }

        private void DetachGridEvents()
        {
            gvIngredientsInfo.CurrentCellDirtyStateChanged -= gvIngredientsInfo_CurrentCellDirtyStateChanged;
            gvIngredientsInfo.CellValueChanged -= gvIngredientsInfo_CellValueChanged;
        }

        private void gvIngredientsInfo_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            if (isUpdatingGrid)
            {
                return;
            }
            if (gvIngredientsInfo.IsCurrentCellDirty && gvIngredientsInfo.CurrentCell is DataGridViewCheckBoxCell)
            {
                gvIngredientsInfo.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void gvIngredientsInfo_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (isUpdatingGrid)
            {
                return;
            }
            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.ColumnIndex == 5)
            {
                var row = gvIngredientsInfo.Rows[e.RowIndex];
                var ingredient = row.Tag as IngredientModel;
                if (ingredient != null)
                {
                    UpdatePrizeInGrid(e.RowIndex, ingredient);
                    UpdateTotalLabelFromGrid();
                }
            }
        }

        private void UpdateTotalLabelFromGrid()
        {
            double total = 0.0;
            for (int rowIndex = 0; rowIndex < gvIngredientsInfo.Rows.Count; rowIndex++)
            {
                var row = gvIngredientsInfo.Rows[rowIndex];
                if (row.IsNewRow) continue;
                var ingredient = row.Tag as IngredientModel;
                if (ingredient == null) continue;
                var prizeObj = row.Cells[4].Value;
                bool useWholesale = Convert.ToBoolean(row.Cells[5].Value);
                var ingredientPriceObj = useWholesale ? ingredient.WholesalePrice : ingredient.RetailPrice;
                double prizeVal = Convert.ToDouble(prizeObj);
                double ingredientPriceVal = Convert.ToDouble(ingredientPriceObj);
                total += PrizeCalculation.CalculateWeightVolCost(50, prizeVal, ingredientPriceVal);
            }
            lblTotalAmount.Text = total.ToString();
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

        private void UpdateTotalLabel(List<Guid> prodIngrId)
        {
            double total = 0.0;
            int rowIndex = 0;

            foreach (var id in prodIngrId)
            {
                var prizeObj = gvIngredientsInfo.Rows[rowIndex].Cells[4].Value;
                var dfltPrizeObj = gvIngredientsInfo.Rows[rowIndex].Cells[5].Value;
                var prodIng = productIngredientService.GetProductIngredientById(id);

                bool useWholesale = Convert.ToBoolean(dfltPrizeObj);
                var ingredientEntity = ingredientService.GetIngredientById(prodIng.IngredientId);
                var ingredientPriceObj = useWholesale ? ingredientEntity.WholesalePrice : ingredientEntity.RetailPrice;

                double prizeVal = Convert.ToDouble(prizeObj);
                double ingredientPriceVal = Convert.ToDouble(ingredientPriceObj);

                total += PrizeCalculation.CalculateWeightVolCost(50, prizeVal, ingredientPriceVal);
                rowIndex++;
            }
            /*
            var flour = Convert.ToInt32(TxtFlour.Text);
            var milk = Convert.ToInt32(TxtFlour.Text);
            if (rbtnMinFlour.Checked)
            {
                
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
            }*/
            lblTotalAmount.Text = total.ToString();
        }

        private void UpdatePrizeInGrid(int rowIndex, IngredientModel ingredient)
        {
            var qtyRetail = ingredient.RetailPrice;
            var qtyWholesale = ingredient.WholesalePrice;
            bool useWholesale = Convert.ToBoolean(gvIngredientsInfo.Rows[rowIndex].Cells[5].Value);
            try
            {
                isUpdatingGrid = true;
                gvIngredientsInfo.Rows[rowIndex].Cells[4].Value = useWholesale ? qtyWholesale : qtyRetail;
            }
            finally
            {
                isUpdatingGrid = false;
            }
        }

        private void CreateIngredientRows(List<Guid> prodIngrId)
        {

            foreach (var id in prodIngrId)
            {
                int rowIndex = gvIngredientsInfo.Rows.Add();
                var prodIngId = productIngredientService.GetProductIngredientById(id);
                var ingredient = ingredientService.GetIngredientById(prodIngId.IngredientId);
                var brand = ingredient.BrandId.HasValue ? brandService.GetBrandById((Guid)ingredient.BrandId).Name : string.Empty;
                var unitType = unitTypeService.GetUnitTypeById((Guid)ingredient.UnitTypeId);

                var qtyRetail = ingredient.RetailPrice;
                var qtyWholesale = ingredient.WholesalePrice;

                var dfaultSelectedRetail = ingredient.DefaultPrice == "Retail";
                var dfaultSelectedWhole = ingredient.DefaultPrice == "Wholesale";


                //ingrediente
                gvIngredientsInfo.Rows[rowIndex].Cells[0].Value = ingredient.Name;
                //marca
                gvIngredientsInfo.Rows[rowIndex].Cells[1].Value = brand;
                //cantida
                gvIngredientsInfo.Rows[rowIndex].Cells[2].Value = prodIngId.IngredientQtyPerPrep;
                //unidad
                gvIngredientsInfo.Rows[rowIndex].Cells[3].Value = unitType.Acronym;
                //default precio
                gvIngredientsInfo.Rows[rowIndex].Cells[5].ReadOnly = false;
                var defaultIsWholesale = ingredient.DefaultPrice == "Retail" ? false : true;
                gvIngredientsInfo.Rows[rowIndex].Cells[5].Value = defaultIsWholesale;
                gvIngredientsInfo.CommitEdit(DataGridViewDataErrorContexts.Commit);
                // store ingredient for later updates
                gvIngredientsInfo.Rows[rowIndex].Tag = ingredient;
                //precio
                UpdatePrizeInGrid(rowIndex, ingredient);
                rowIndex++;
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
                var productId = productService.GetAllProducts()
                    .Where(product => product.Name == TSCmbProductList.Text)
                    .Select(item => item.Id).First();
                var test = productIngredientService.GetProductIngredientByProductId(productId);
                if (isRepopulating)
                {
                    return;
                }
                // Skip repopulate if same product selected (prevents double refresh)
                if (lastSelectedProductId.HasValue && lastSelectedProductId.Value == productId)
                {
                    return;
                }
                lastSelectedProductId = productId;
                isRepopulating = true;
                // Defer repopulation to avoid running during DataGridView internal notifications
                BeginInvoke(new Action(() =>
                {
                    try
                    {
                        RepopulateGrid(productId);
                    }
                    finally
                    {
                        isRepopulating = false;
                    }
                }));
            }
            catch (Exception ex)
            {
                logsService.CreateLog(ex.Message, "Error", "Marco Llano");
                throw;
            }
        }

        private void RepopulateGrid(Guid productId)
        {
            bool prevAllowAdd = gvIngredientsInfo.AllowUserToAddRows;
            bool prevEnabled = gvIngredientsInfo.Enabled;
            try
            {
                isUpdatingGrid = true;
                DetachGridEvents();
                if (gvIngredientsInfo.IsCurrentCellInEditMode)
                {
                    gvIngredientsInfo.EndEdit();
                }
                gvIngredientsInfo.Enabled = false;
                gvIngredientsInfo.AllowUserToAddRows = false;
                gvIngredientsInfo.SuspendLayout();
                gvIngredientsInfo.ClearSelection();
                gvIngredientsInfo.Rows.Clear();
                gvIngredientsInfo.Refresh();

                var productIngredientList = productIngredientService.GetProductIngredientByProductId(productId);
                if (productIngredientList == null)
                {
                    try { logsService.CreateLog($"Product '{TSCmbProductList.Text}' ({productId}) missing ingredients (null list).", "Warning", "System"); } catch { }
                    MessageBox.Show("The selected product is missing required data (ingredients).", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var prodIngredients = productIngredientList
                    .Where(prodId => prodId != null && prodId.ProductId == productId)
                    .Select(item => item.Id)
                    .ToList();
                if (prodIngredients == null || prodIngredients.Count == 0)
                {
                    try { logsService.CreateLog($"Product '{TSCmbProductList.Text}' ({productId}) has no ingredients configured.", "Warning", "System"); } catch { }
                    MessageBox.Show("The selected product has no ingredients configured.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                CreateIngredientRows(prodIngredients);
                UpdateTotalLabel(prodIngredients);

                LblFormTitle.Text = TSCmbProductList.Text;
                LoadImage(this, EventArgs.Empty, productId);
            }
            finally
            {
                gvIngredientsInfo.AllowUserToAddRows = prevAllowAdd;
                gvIngredientsInfo.Enabled = prevEnabled || true;
                gvIngredientsInfo.ResumeLayout();
                isUpdatingGrid = false;
                AttachGridEvents();
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
            StoreControlBounds(pnlTotalPrices);
            StoreControlBounds(gvIngredientsInfo);
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
            AdjustControlLayout(gvIngredientsInfo, scaleX, scaleY);
            AdjustControlLayout(pnlTotalPrices, scaleX, scaleY);

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
            if (imgProduct.Right + minSpacing > gvIngredientsInfo.Left)
            {
                gvIngredientsInfo.Left = imgProduct.Right + minSpacing;
            }
        }
    }

}

