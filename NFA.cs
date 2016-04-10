using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DFA
{
    public partial class NFAResult : Form
    {
        string Show=string.Empty;
        public NFAResult(string finalResult)
        {
            InitializeComponent();
            Show=finalResult;
        }

        private void NFAResult_Load(object sender, EventArgs e)
        {
            textBox1.Text = Show;
        }
    }
}
