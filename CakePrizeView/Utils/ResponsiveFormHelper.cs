using System.Collections.Generic;

namespace CakePrizeView.Utils
{
    /// <summary>
    /// Utility class to handle responsive form functionality
    /// </summary>
    public static class ResponsiveFormHelper
    {
        /// <summary>
        /// Stores initial positions and sizes of controls for relative positioning
        /// </summary>
        public static void StoreInitialPositions(Form form, Dictionary<Control, Rectangle> initialControlBounds, params Control[] controls)
        {
            if (initialControlBounds == null)
                initialControlBounds = new Dictionary<Control, Rectangle>();

            foreach (var control in controls)
            {
                if (control != null)
                {
                    initialControlBounds[control] = control.Bounds;
                    
                    // Store bounds for child controls
                    foreach (Control child in control.Controls)
                    {
                        StoreControlBounds(child, initialControlBounds);
                    }
                }
            }
        }

        /// <summary>
        /// Recursively stores bounds for a control and its children
        /// </summary>
        public static void StoreControlBounds(Control control, Dictionary<Control, Rectangle> initialControlBounds)
        {
            if (control != null)
            {
                initialControlBounds[control] = control.Bounds;
                
                // Store bounds for child controls
                foreach (Control child in control.Controls)
                {
                    StoreControlBounds(child, initialControlBounds);
                }
            }
        }

        /// <summary>
        /// Handles form resize to adjust control positions and sizes
        /// </summary>
        public static void HandleFormResize(Form form, Size initialFormSize, Dictionary<Control, Rectangle> initialControlBounds, params Control[] controls)
        {
            if (initialControlBounds == null || initialFormSize.Width == 0 || initialFormSize.Height == 0)
                return;

            // Calculate scaling factors
            float scaleX = (float)form.Width / initialFormSize.Width;
            float scaleY = (float)form.Height / initialFormSize.Height;

            // Adjust control positions and sizes
            foreach (var control in controls)
            {
                AdjustControlLayout(control, scaleX, scaleY, initialControlBounds);
            }
        }

        /// <summary>
        /// Adjusts a control's position and size based on scaling factors
        /// </summary>
        public static void AdjustControlLayout(Control control, float scaleX, float scaleY, Dictionary<Control, Rectangle> initialControlBounds)
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
        /// Ensures minimum spacing between two controls
        /// </summary>
        public static void EnsureMinimumSpacing(Control control1, Control control2, int minSpacing = 10)
        {
            if (control1 != null && control2 != null)
            {
                // Ensure minimum spacing between controls
                if (control1.Right + minSpacing > control2.Left)
                {
                    control2.Left = control1.Right + minSpacing;
                }
            }
        }

        /// <summary>
        /// Updates DataGridView column widths proportionally
        /// </summary>
        public static void UpdateDataGridViewColumns(DataGridView dataGridView, params float[] columnRatios)
        {
            if (dataGridView != null && dataGridView.Columns.Count > 0 && columnRatios.Length > 0)
            {
                int totalWidth = dataGridView.Width - 20; // Account for scrollbar
                
                for (int i = 0; i < Math.Min(dataGridView.Columns.Count, columnRatios.Length); i++)
                {
                    dataGridView.Columns[i].Width = (int)(totalWidth * columnRatios[i]);
                }
            }
        }

        /// <summary>
        /// Sets up responsive form with common controls
        /// </summary>
        public static void SetupResponsiveForm(Form form, Dictionary<Control, Rectangle> initialControlBounds, params Control[] controls)
        {
            // Store initial positions
            StoreInitialPositions(form, initialControlBounds, controls);
            
            // Add resize event handler
            form.Resize += (sender, e) => HandleFormResize(form, form.Size, initialControlBounds, controls);
        }
    }
}
