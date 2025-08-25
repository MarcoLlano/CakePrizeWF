using CakePrizeView.Utils;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CakePrizeView.Forms.Menu.Products
{
    public partial class ProductTypeForm : Form
    {
        private Form previousForm;
        private SqlConnection sqlConnection;

        // Store initial form size for relative positioning
        private Size initialFormSize;
        private Dictionary<Control, Rectangle> initialControlBounds;

        public ProductTypeForm(Form previousForm, SqlConnection sqlConnection)
        {
            this.previousForm = previousForm;
            this.sqlConnection = sqlConnection;
            InitializeComponent();

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
            //StoreControlBounds(panel1);
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
        private void ProductTypeForm_Resize(object sender, EventArgs e)
        {
            if (initialControlBounds == null || initialFormSize.Width == 0 || initialFormSize.Height == 0)
                return;

            // Calculate scaling factors
            float scaleX = (float)this.Width / initialFormSize.Width;
            float scaleY = (float)this.Height / initialFormSize.Height;

            // Adjust control positions and sizes
            //AdjustControlLayout(lblPortionsPerPrep, scaleX, scaleY);

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
            //FormUtils.EnsureMinimumSpacing()
            /*
            const int minSpacing = 10;

            // Ensure minimum spacing between form elements
            if (btnBackProduct.Right + minSpacing > btnCloseProduct.Left)
            {
                btnCloseProduct.Left = btnBackProduct.Right + minSpacing;
            }

            // Ensure minimum spacing in panel
            if (richTBProdComments.Bottom + minSpacing > btnSaveProduct.Top)
            {
                btnSaveProduct.Top = richTBProdComments.Bottom + minSpacing;
                btnClearProductTexts.Top = btnSaveProduct.Top;
            }*/
        }
    }
}
