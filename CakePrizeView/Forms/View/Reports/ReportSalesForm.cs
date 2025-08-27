using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CakePrizeView.Utils;

namespace CakePrizeView.Forms.View.Reports
{
    public partial class ReportSalesForm : Form
    {
        public ReportSalesForm()
        {
            InitializeComponent();
            
            // Set up auto-maximize
            FormMaximizeHelper.SetupAutoMaximize(this);
        }
    }
}
