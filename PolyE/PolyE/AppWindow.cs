using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolyE
{
    public partial class AppWindow : Form
    {
        private PolyEModel _model;

        public AppWindow()
        {
            InitializeComponent();

            _model = new PolyEModel(50, 8, 2, 35);
            branchesText.Text = _model.BranchesString();
            leavesText.Text = _model.LeavesString();
            agentsText.Text = _model.AgentsString();
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            _model.Tick();
            branchesText.Text = _model.BranchesString();
            leavesText.Text = _model.LeavesString();
            agentsText.Text = _model.AgentsString();
        }
    }
}
