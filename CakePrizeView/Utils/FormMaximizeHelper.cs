using System;
using System.Windows.Forms;

namespace CakePrizeView.Utils
{
    /// <summary>
    /// Utility class to handle form maximization functionality
    /// </summary>
    public static class FormMaximizeHelper
    {
        /// <summary>
        /// Maximizes a form and ensures it's visible on screen
        /// </summary>
        /// <param name="form">The form to maximize</param>
        public static void MaximizeForm(Form form)
        {
            if (form == null) return;

            // Set the form to maximize
            form.WindowState = FormWindowState.Maximized;
            
            // Ensure the form is visible and brought to front
            form.Show();
            form.BringToFront();
            form.Focus();
        }

        /// <summary>
        /// Sets up a form to automatically maximize when shown
        /// </summary>
        /// <param name="form">The form to set up</param>
        public static void SetupAutoMaximize(Form form)
        {
            if (form == null) return;

            // Subscribe to the Load event to maximize when the form loads
            form.Load += (sender, e) => MaximizeForm(form);
        }
    }
}

