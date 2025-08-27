using CakePrize.libs.utils;
using CakePrizeView.Utils;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace CakePrizeView
{
    public partial class CalculatorForm : Form
    {
        private Form previousForm;
        private SqlConnection sqlConnection;
        private int data;
        
        // Store initial form size for relative positioning
        private Size initialFormSize;
        private Dictionary<Control, Rectangle> initialControlBounds;

        public CalculatorForm(Form previousForm, SqlConnection sqlConnection)
        {
            InitializeComponent();
            this.previousForm = previousForm;
            this.sqlConnection = sqlConnection;
            
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

        private void CreateIngredientRows(int data)
        {
            string ingredientName = "Harina";
            string unitPrefix = "grs";

            int ingredientNamePosX = 6;
            int ingredientNamePosY = 6;
            int ingredientNameSizeX = 55;
            int ingredientNameSizeY = 17;

            int unitPrefixSizeX = 186;
            int unitPrefixSizeY = 6;
            int unitPrefixPosX = 186;
            int unitPrefixPosY = 6;

            int ingredientTxtSizeX = 91;
            int ingredientTxtSizeY = 22;
            int ingredientTxtPosX = 89;
            int ingredientTxtPosY = 2;


            int ingredientRetailPosX = 278;
            int ingredientRetailPosY = 5;
            int ingredientRetailSizeX = 60;
            int ingredientRetailSizeY = 19;

            int ingredientWholesalePosX = 381;
            int ingredientWholesalePosY = 5;
            int ingredientWholesaleSizeX = 59;
            int ingredientWholesaleSizeY = 19;

            int ingredientPanelRowPosX = 3;
            int ingredientPanelRowPosY = 3;
            int incrementPanelRowY = 31;

            TextBox TxtIngredientAmount;
            Label LblingredientName;
            Label LblunitPrefix;
            RadioButton RbtnRetailPrize;
            RadioButton RbtnWholesalePrize;

            for (int i = 0; i < data; i++)
            {
                LblingredientName = FormUtils.CreateLabel($"{ingredientName}", i, ingredientNamePosX, ingredientNamePosY,
                    ingredientNameSizeX, ingredientNameSizeY);

                LblunitPrefix = FormUtils.CreateLabel($"{unitPrefix}", i, unitPrefixPosX, unitPrefixPosY,
                    unitPrefixSizeX, unitPrefixSizeY);

                TxtIngredientAmount = FormUtils.CreateTextBox(ingredientName, i, ingredientTxtPosX, ingredientTxtPosY,
                    ingredientTxtSizeX, ingredientTxtSizeY);

                RbtnRetailPrize = FormUtils.CreateRadioButton("Menor", i, ingredientRetailPosX, ingredientRetailPosY,
                    ingredientRetailSizeX, ingredientRetailSizeY, false);

                RbtnWholesalePrize = FormUtils.CreateRadioButton("Mayor", i, ingredientWholesalePosX, ingredientWholesalePosY,
                    ingredientWholesaleSizeX, ingredientWholesaleSizeY, true);
                
                PnlIngredients.Controls.Add(
                    FormUtils.CreateIngredientPanelRow(i, LblingredientName, TxtIngredientAmount, LblunitPrefix, RbtnRetailPrize,
                    RbtnWholesalePrize, ingredientPanelRowPosX, ingredientPanelRowPosY));

                ingredientPanelRowPosY += incrementPanelRowY;
            }
        }

        private void FrmCupcake_Load(object sender, EventArgs e)
        {
            TSCmbCakeList.Items.Add(string.Empty);
            TSCmbCakeList.Items.Add("Chocolate");
            TSCmbCakeList.Items.Add("Chirimoya");
            TSCmbCakeList.Items.Add("Vainilla");
            TSCmbCakeList.Items.Add("Frutilla");
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

        private void CmbCupcakeList_SelectedValueChanged(object sender, EventArgs e)
        {
            PnlIngredients.Controls.Clear();

            if (TSCmbCakeList.Text.Equals("Chocolate"))
            {
                data = 3;
                CreateIngredientRows(data);
                UpdateTotalLabel();
            }
            if (TSCmbCakeList.Text.Equals("Vainilla"))
            {
                data = 8;
                CreateIngredientRows(data);
                UpdateTotalLabel();
            }
            if (TSCmbCakeList.Text.Equals("Chirimoya"))
            {
                data = 19;
                CreateIngredientRows(data);
                UpdateTotalLabel();
            }

            LblCakeTitle.Text = TSCmbCakeList.Text;
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
            TSCmbCakeList.Items.Clear();
            TSLblSelectedCake.Text = sender.ToString();
            switch (sender.ToString())
            {
                case "Tortas":
                    TSCmbCakeList.Items.Add(string.Empty);
                    TSCmbCakeList.Items.Add("Torta de mora");
                    TSCmbCakeList.Items.Add("Torta tres leches");
                    TSCmbCakeList.Items.Add("Torta selva negra");
                    TSCmbCakeList.Items.Add("Frutilla con relleno de chocolate");
                    break;
                case "Cupcakes":
                    TSCmbCakeList.Items.Add(string.Empty);
                    TSCmbCakeList.Items.Add("Chocolate");
                    TSCmbCakeList.Items.Add("Chirimoya");
                    TSCmbCakeList.Items.Add("Vainilla");
                    TSCmbCakeList.Items.Add("Zanahoria");
                    break;
                case "Mush":
                    TSCmbCakeList.Items.Add(string.Empty);
                    TSCmbCakeList.Items.Add("Limon");
                    TSCmbCakeList.Items.Add("Chirimoya");
                    TSCmbCakeList.Items.Add("Mora");
                    TSCmbCakeList.Items.Add("Frutilla");
                    break;
                case "Postres":
                    TSCmbCakeList.Items.Add(string.Empty);
                    TSCmbCakeList.Items.Add("Brownies");
                    TSCmbCakeList.Items.Add("Tarta de Manzana");
                    break;
                default:
                    break;
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
            StoreControlBounds(picCupcake);
            StoreControlBounds(LblCakeTitle);
            StoreControlBounds(BtnBack);
            StoreControlBounds(BtnClose);
            StoreControlBounds(PnlIngredients);
            StoreControlBounds(panel4);
            StoreControlBounds(LblPrizeTypeTitle);
            StoreControlBounds(label3);
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
        private void CalculatorForm_Resize(object sender, EventArgs e)
        {
            if (initialControlBounds == null || initialFormSize.Width == 0 || initialFormSize.Height == 0)
                return;

            // Calculate scaling factors
            float scaleX = (float)this.Width / initialFormSize.Width;
            float scaleY = (float)this.Height / initialFormSize.Height;

            // Adjust control positions and sizes
            AdjustControlLayout(picCupcake, scaleX, scaleY);
            AdjustControlLayout(LblCakeTitle, scaleX, scaleY);
            AdjustControlLayout(BtnBack, scaleX, scaleY);
            AdjustControlLayout(BtnClose, scaleX, scaleY);
            AdjustControlLayout(PnlIngredients, scaleX, scaleY);
            AdjustControlLayout(panel4, scaleX, scaleY);
            AdjustControlLayout(LblPrizeTypeTitle, scaleX, scaleY);
            AdjustControlLayout(label3, scaleX, scaleY);
            
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
            if (BtnBack.Right + minSpacing > BtnClose.Left)
            {
                BtnClose.Left = BtnBack.Right + minSpacing;
            }
            
            // Ensure minimum spacing between picture and ingredients panel
            if (picCupcake.Right + minSpacing > PnlIngredients.Left)
            {
                PnlIngredients.Left = picCupcake.Right + minSpacing;
            }
            
            // Ensure minimum spacing between ingredients panel and price panel
            if (PnlIngredients.Right + minSpacing > panel4.Left)
            {
                panel4.Left = PnlIngredients.Right + minSpacing;
            }
        }
    }

}
