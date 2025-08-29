using CakePrizeDB.Services;
using CakePrizeView.Utils;
using Microsoft.Data.SqlClient;

namespace CakePrizeView
{
    public partial class CalculatorForm : Form
    {
        private Form previousForm;
        private ProductService productService;
        private ProductIngredientService productIngredientService;
        private IngredientService ingredientService;
        private UnitTypeService unitTypeService;
        private ProductPhotoService productPhotoService;


        // Store initial form size for relative positioning
        private Size initialFormSize;
        private Dictionary<Control, Rectangle> initialControlBounds;

        public CalculatorForm(Form previousForm, SqlConnection sqlConnection)
        {
            productService = new ProductService(sqlConnection);
            productIngredientService = new ProductIngredientService(sqlConnection);
            ingredientService = new IngredientService(sqlConnection);
            unitTypeService = new UnitTypeService(sqlConnection);
            productPhotoService = new ProductPhotoService(sqlConnection);
            initialControlBounds = new Dictionary<Control, Rectangle>();

            InitializeComponent();
            this.previousForm = previousForm;

            
            // Add resize event handler
            this.Resize += CalculatorForm_Resize;
            
            // Store initial positions for relative positioning
            StoreInitialPositions();
            
            // Set up auto-maximize
            FormMaximizeHelper.SetupAutoMaximize(this);
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

            int lblUnitAcronymPosX = 230;
            int lblUnitAcronymPosY = 0;
            int lblUnitAcronymSizeX = 15;
            int lblUnitAcronymSizeY = 19;

            int txtIngredientPosX = 100;
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
                var unitType = unitTypeService.GetUnitTypeById(ingredient.UnitTypeId);

                LblingredientName = FormUtils.CreateLabel($"{ingredient.Name}", numRow, lblIngredientNamePosX, lblIngredientNamePosY,
                    lblIngredientNameSizeX, lblIngredientNameSizeY);

                LblunitPrefix = FormUtils.CreateLabel($"{unitType.Acronym}", numRow, lblUnitAcronymPosX, lblUnitAcronymPosY,
                    lblUnitAcronymSizeX, lblUnitAcronymSizeY);

                TxtIngredientAmount = FormUtils.CreateTextBox(ingredient.Name, numRow, txtIngredientPosX, txtIngredientPosY,
                    txtIngredientSizeX, txtIngredientSizeY);

                RbtnRetailPrize = FormUtils.CreateRadioButton("Menor", numRow, ingredientRetailPosX, ingredientRetailPosY,
                    ingredientRetailSizeX, ingredientRetailSizeY, false);

                RbtnWholesalePrize = FormUtils.CreateRadioButton("Mayor", numRow, ingredientWholesalePosX, ingredientWholesalePosY,
                    ingredientWholesaleSizeX, ingredientWholesaleSizeY, true);

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
            foreach (var item in productService.GetAllProducts())
            {
                TSCmbProductList.Items.Add(item.Name); 
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
            TSCmbProductList.Items.Clear();
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
