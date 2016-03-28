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
    public partial class Form1 : Form
    {
        public static int startStatus = 0;
        public static int finalStatus = 3;       
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = this.textBox1.Text;
            char[] Temp = text.ToCharArray();
            DFA(Temp);
        }

        public void DFA(char[] Temp)
        {
            int start = startStatus;
            for(int i=0;i<Temp.Length;i++)
            {
                Format temp = new Format(start, Temp[i]);
                int result = temp.GetTransitionNextStatus(temp);
                if(result!=-1)
                {
                    start = result;
                }
            }
            if(start==finalStatus)
            {
                MessageBox.Show("符合规范！");
            }
            else
            {
                MessageBox.Show("不符合规范！");
            }
        }
    }
}
