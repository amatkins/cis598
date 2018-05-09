using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EquationSolver
{
    public partial class Form1 : Form
    {
        private SwarmNet.EquationSystem _system;
        private Timer _repeat;

        public Form1()
        {
            InitializeComponent();
            _repeat = new Timer();
            _repeat.Interval = 100;
            _repeat.Tick += _repeat_Tick;
        }

        private void _repeat_Tick(object sender, EventArgs e)
        {
            if (((SwarmNet.RowMaker)_system.System.Roots[0].Port).ShouldSpawn)
                _system.System.Enter(0);

            _system.System.Tick();

            if (((SwarmNet.RowMaker)_system.System.Roots[0].Port).Solved)
            {
                _repeat.Stop();
                runToolStripMenuItem.Enabled = true;
                stopToolStripMenuItem.Enabled = false;
                WriteOut();
            }
            else
                Calculating();
        }

        private void Calculating()
        {
            string cur_solution = ((SwarmNet.RowMaker)_system.System.Roots[0].Port)._solution.Aggregate("", (str, da) => string.Format("{0}{1}{2}", str, Environment.NewLine, da == null ? "no Solution" : da.Aggregate("", (str2, d) => string.Format("{0} {1}", str2, d))));
            textBox1.Text = (_system.System.Agents).Aggregate(cur_solution, (str, a) => string.Format("{0}{1}Agent: {2} {3}", str, Environment.NewLine, ((SwarmNet.SolutionRow)a).Columns.Aggregate("", (str2, v) => string.Format("{0} {1}", str2, v)), ((SwarmNet.SolutionRow)a).AdjustMag));
        }

        private void WriteOut()
        {
            textBox1.Text = ((SwarmNet.RowMaker)_system.System.Roots[0].Port)._solution.Aggregate("", (str, da) => string.Format("{0}{1}{2}", str, Environment.NewLine, da.Aggregate("", (str2, d) => string.Format("{0} {1}", str2, d))));
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_repeat.Enabled)
            {
                _repeat.Stop();
                runToolStripMenuItem.Enabled = true;
                stopToolStripMenuItem.Enabled = false;
            }
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_repeat.Enabled)
            {
                if (!((SwarmNet.RowMaker)_system.System.Roots[0].Port).Solved)
                {
                    _repeat.Start();
                    stopToolStripMenuItem.Enabled = true;
                    runToolStripMenuItem.Enabled = false;
                }
            }
        }

        private void loadSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = toolStripTextBox1.Text;
            string input2 = toolStripTextBox2.Text;

            int[][] equations = input.Split(';').Select(c => c.Split(',').Select(r => int.Parse(r)).ToArray()).ToArray();
            int[] range = input2.Split(',').Select(m => int.Parse(m)).ToArray();
            _system = new SwarmNet.EquationSystem(range[0], range[1], 2, 5, 100, equations);
        }
    }
}
